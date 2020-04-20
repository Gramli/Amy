using Amy.Exceptions;
using Amy.Grammars.EBNF.EBNFItems;
using Amy.Grammars.EBNF.EBNFItems.ProductionRuleElements;
using Amy.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Amy.Grammars.EBNF
{
    /// <summary>
    /// Implementation of EBNF Grammar parser
    /// </summary>
    public class EBNFGrammarParserCustom : IFormalGrammarParser
    {
        private EBNFGrammarDefinition _actualDefinition;
        private const string _termination = ";";
        private readonly int _cacheLength;

        public EBNFGrammarParserCustom(int cacheLength)
        {
            this._cacheLength = cacheLength;
        }

        public IStartSymbol Parse(IFormalGrammarDefinition definition)
        {
            this._actualDefinition = (EBNFGrammarDefinition)definition;

            var productionRules = new List<NonTerminal>();

            for (var i = this._actualDefinition.ProductionRules.Length - 1; i > 0; i--)
            {
                if (string.IsNullOrEmpty(this._actualDefinition.ProductionRules[i])) continue;
                var rule = RemoveSpecialChars(this._actualDefinition.ProductionRules[i]);
                var nonTerminal = GetNonTerminal(rule, productionRules);
                productionRules.Add(nonTerminal);
            }

            var startSymbolRule = RemoveSpecialChars(this._actualDefinition.ProductionRules[0]);
            var startSymbolNonTerminal = GetNonTerminal(startSymbolRule, productionRules);
            productionRules.Add(startSymbolNonTerminal);
            var startSymbol = this._actualDefinition.GetStartSymbol(startSymbolNonTerminal, productionRules);
            return startSymbol;
        }

        private string RemoveSpecialChars(string rule)
        {
            return rule.RemoveSpaces().RemoveNewLines();
        }

        private NonTerminal GetNonTerminal(string productionRule, List<NonTerminal> listOfExistedTerminals)
        {
            var splittedProductionRule = SplitByDefinition(productionRule);
            var nonTerminalRule = GetEBNFItem(splittedProductionRule[1], listOfExistedTerminals);
            var result = this._actualDefinition.GetNewNonTerminalInstance(splittedProductionRule[0]);
            result.SetRightSide(nonTerminalRule);
            return result;

        }

        private string[] SplitByDefinition(string productionRule)
        {
            var result = new string[2];
            var definitionIndex = productionRule.IndexOf(NonTerminal.Definition, StringComparison.InvariantCulture);
            result[0] = productionRule.Substring(0, definitionIndex);
            definitionIndex++;
            result[1] = productionRule.Substring(definitionIndex, productionRule.Length - definitionIndex);
            return result;
        }

        /// <summary>
        /// Recursive function for finding sequence of Terminals and NonTerminals by rule. Result is IEBNFItem which containst sequence
        /// </summary>
        private IEBNFItem GetEBNFItem(string rule, List<NonTerminal> listOfExistedTerminals, string endNotation = null)
        {
            IEBNFItem result = null;
            var left = GetStartEBNFItem(rule, listOfExistedTerminals);
            var lengthOfLeftRule = left.Rebuild().Length;
            var restOfRule = rule.Substring(lengthOfLeftRule, rule.Length - lengthOfLeftRule);
            if (string.IsNullOrEmpty(restOfRule))
                throw new GrammarParseException("Can't find IEBNFItem, rest of rule is null or empty. Check termination charatcter.");
            var firstChar = restOfRule[0].ToString();
            if (!string.IsNullOrEmpty(endNotation) && firstChar.Equals(endNotation))
                result = left;
            else if (IsTermination(firstChar))
                result = left;
            else
            {
                var newRule = restOfRule.Substring(1, restOfRule.Length - 1);
                var right = GetEBNFItem(newRule, listOfExistedTerminals, endNotation);
                switch (firstChar)
                {
                    case Alternation.notation: result = new Alternation(left, right); break;
                    case Concatenation.notation: result = new Concatenation(left, right, this._cacheLength); break;
                }
            }
            return result;
        }

        /// <summary>
        /// Try to find EBNFItem which can be on right side
        /// Its part of recursion it calls GetEBNFItem
        /// </summary>
        /// <returns></returns>
        private IEBNFItem GetStartEBNFItem(string rule, List<NonTerminal> listOfExistedTerminals)
        {
            IEBNFItem result = null;

            switch (rule[0].ToString())
            {
                case "\"":
                    {
                        var builder = new StringBuilder();
                        for (var i = 1; i < rule.Length; i++)
                        {
                            if (rule[i].Equals('"'))
                                break;
                            builder.Append(rule[i]);
                        }
                        result = new Terminal(builder.ToString());
                    }
                    break;
                case var endItem when endItem.Equals(EndRecursion.Current.Notation):
                    result = EndRecursion.Current;
                    break;
                case var nonItem when Regex.IsMatch(nonItem.ToString(), "[a-zA-Z]"):
                    {
                        var builder = new StringBuilder();
                        foreach (var t in rule)
                        {
                            if (Regex.IsMatch(t.ToString(), @"[,;|\[\]\{\}\(\)]"))
                                break;
                            builder.Append(t);
                        }
                        result = (from item in listOfExistedTerminals where item.Name.Equals(builder.ToString()) select item).SingleOrDefault();
                        if (result == null)
                        {
                            var emptyNonTerm = this._actualDefinition.GetNewNonTerminalInstance(builder.ToString());
                            result = emptyNonTerm;
                        }
                    }
                    break;
                case var groupItem when Regex.IsMatch(groupItem, @"[\[\{\(]"):
                    {
                        var restOfRepRule = rule.Substring(1, rule.Length - 1);
                        switch (groupItem)
                        {
                            case Repetition.notation:
                                var repItem = GetEBNFItem(restOfRepRule, listOfExistedTerminals, Repetition.endNotation);
                                result = new Repetition(repItem, this._cacheLength);
                                break;
                            case Optional.notation:
                                var opItem = GetEBNFItem(restOfRepRule, listOfExistedTerminals, Optional.endNotation);
                                result = new Optional(opItem);
                                break;
                            case Grouping.notation:
                                var grItem = GetEBNFItem(restOfRepRule, listOfExistedTerminals, Grouping.endNotation);
                                result = new Grouping(grItem);
                                break;
                        }
                    }
                    break;
                default:
                    throw new GrammarParseException($"Grammar parse error. Can't recognize character: {rule[0]}. Check missing rules characters.", new ArgumentException());
            }
            return result;
        }

        /// <summary>
        /// Determines termination
        /// </summary>
        private bool IsTermination(string item)
        {
            return item.Equals(EBNFGrammarParserCustom._termination);
        }
    }
}

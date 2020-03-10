using Amy.Exceptions;
using Amy.Grammars.EBNF.EBNFItems;
using Amy.Grammars.EBNF.EBNFItems.ProductionRuleElements;
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
        private readonly List<NonTerminal> _emptyRules;
        private EBNFGrammarDefinition actualDefinition;
        private const string _termination = ";";

        public EBNFGrammarParserCustom()
        {
            this._emptyRules = new List<NonTerminal>();
        }

        public IStartSymbol Parse(IFormalGrammarDefinition definition)
        {
            this.actualDefinition = (EBNFGrammarDefinition)definition;
            this._emptyRules.Clear();

            var productionRules = new List<NonTerminal>();

            //var productionRulesStrings = SplitByTermination(grammar).Reverse().ToArray();
            for (var i = this.actualDefinition.ProductionRules.Length - 1; i > 0; i--)
            {
                if (string.IsNullOrEmpty(this.actualDefinition.ProductionRules[i])) continue;
                string rule = RemoveSpecialChars(this.actualDefinition.ProductionRules[i]);
                var nonTerminal = GetNonTerminal(rule, productionRules);
                productionRules.Add(nonTerminal);
            }

            string startSymbolRule = RemoveSpecialChars(this.actualDefinition.ProductionRules[0]);
            var startSymbolNonTerminal = GetNonTerminal(startSymbolRule, productionRules);
            productionRules.Add(startSymbolNonTerminal);
            var startSymbol = new EBNFStartSymbol(startSymbolNonTerminal, productionRules);
            SetEmptyRules(startSymbol);
            return startSymbol;
        }

        private string RemoveSpecialChars(string grammar)
        {
            return grammar.Replace(" ", string.Empty).Replace(Environment.NewLine, string.Empty);
        }

        private void SetEmptyRules(IStartSymbol startSymbol)
        {
            foreach (var rule in this._emptyRules)
            {
                IEBNFItem item = (IEBNFItem)startSymbol.GetNonTerminal(rule.Expression);
                rule.SetRightSide(item);
            }
        }

        private NonTerminal GetNonTerminal(string productionRule, List<NonTerminal> listOfExistedTerminals)
        {
            var splittedProductionRule = SplitByDefinition(productionRule);
            var nonTerminalRule = GetEBNFItem(splittedProductionRule[1], listOfExistedTerminals);
            var result = this.actualDefinition.GetNewNonTerminalInstance(splittedProductionRule[0]);
            result.SetRightSide(nonTerminalRule);//new NonTerminal(splittedProductionRule[0], nonTerminalRule);
            return result;

        }

        private string[] SplitByTermination(string productionRules)
        {
            return Regex.Split(productionRules, $"(?<=[{EBNFGrammarParserCustom._termination}])");
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
                    case Alternation.notation: result = new Alternation(left, right, 20); break;
                    case Concatenation.notation: result = new Concatenation(left, right, 20); break;
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
                        result = (from item in listOfExistedTerminals where item.Expression.Equals(builder.ToString()) select item).SingleOrDefault();
                        if (result == null)
                        {
                            var emptyNonTerm = this.actualDefinition.GetNewNonTerminalInstance(builder.ToString());
                            this._emptyRules.Add(emptyNonTerm);
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
                                result = new Repetition(repItem, 20);
                                break;
                            case Optional.notation:
                                var opItem = GetEBNFItem(restOfRepRule, listOfExistedTerminals, Optional.endNotation);
                                result = new Optional(opItem, 20);
                                break;
                            case Grouping.notation:
                                var grItem = GetEBNFItem(restOfRepRule, listOfExistedTerminals, Grouping.endNotation);
                                result = new Grouping(grItem, 20);
                                break;
                        }
                    }
                    break;
                default:
                    throw new GrammarParseException($"Grammar parse error. Can't recognize character: {rule[0]}", new ArgumentException());
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

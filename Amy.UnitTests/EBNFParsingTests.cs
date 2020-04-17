using Amy.Exceptions;
using Amy.Grammars.EBNF;
using Amy.Grammars.EBNF.EBNFItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Amy.UnitTests
{
    [TestClass]
    public class EBNFParsingTests
    {
        private IFormalGrammarParser parser;
        private TestGrammarDefinition definition;

        public EBNFParsingTests()
        {
            this.parser = new EBNFGrammarParserCustom(50);
            this.definition = new TestGrammarDefinition();
        }

        [TestMethod]
        public void ParsingTest_ParseGrammar()
        {

            var definitionMock = new Mock<EBNFGrammarDefinition>();

            definitionMock.Setup(exp => exp.ProductionRules).Returns(
                new string[]
                {
                    this.definition.Termination,
                    this.definition.Space,
                    this.definition.Character, 
                    this.definition.Digit, 
                    this.definition.IntType, 
                    this.definition.BoolType, 
                    this.definition.Type, 
                    this.definition.Name,
                    this.definition.BoolValue,
                    this.definition.BoolVar,
                    this.definition.IntValue,
                    this.definition.IntVar,
                    this.definition.Variable,
                    this.definition.Function,
                    this.definition.Program
                });
            definitionMock.Setup(exp => exp.GetNewNonTerminalInstance(It.IsAny<string>())).Returns((string str) =>
            {
                return new Moq.Mock<NonTerminal>(MockBehavior.Strict, str).Object;
            });

            definitionMock.Setup(exp => exp.GetStartSymbol(It.IsAny<NonTerminal>(), It.IsAny<List<NonTerminal>>())).Returns(
            (NonTerminal nonTerminal, List<NonTerminal> nonTerminals) =>
            {
                return new Moq.Mock<EBNFStartSymbol>(MockBehavior.Strict, nonTerminal, nonTerminals).Object;
            });

            this.parser.Parse(definitionMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(GrammarParseException))]
        public void ParsingTest_MisingTerminationCharacter()
        {

            var definitionMock = new Mock<EBNFGrammarDefinition>();

            definitionMock.Setup(exp => exp.ProductionRules).Returns(
                new string[]
                {
                    this.definition.Character.Substring(0, this.definition.Character.Length-1)
                });
            definitionMock.Setup(exp => exp.GetStartSymbol(It.IsAny<NonTerminal>(), It.IsAny<List<NonTerminal>>())).Returns(
            (NonTerminal nonTerminal, List<NonTerminal> nonTerminals) =>
            {
                return new Moq.Mock<EBNFStartSymbol>(MockBehavior.Strict, nonTerminal, nonTerminals).Object;
            });

            this.parser.Parse(definitionMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(GrammarParseException))]
        public void ParsingTest_MisingRuleCharacter()
        {

            var definitionMock = new Mock<EBNFGrammarDefinition>();

            definitionMock.Setup(exp => exp.ProductionRules).Returns(
                new string[]
                {
                    this.definition.Digit.Replace("|", string.Empty)
                });
            definitionMock.Setup(exp => exp.GetStartSymbol(It.IsAny<NonTerminal>(), It.IsAny<List<NonTerminal>>())).Returns(
            (NonTerminal nonTerminal, List<NonTerminal> nonTerminals) =>
            {
                return new Moq.Mock<EBNFStartSymbol>(MockBehavior.Strict, nonTerminal, nonTerminals).Object;
            });

            this.parser.Parse(definitionMock.Object);
        }
    }
}

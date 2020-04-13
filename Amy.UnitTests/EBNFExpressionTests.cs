using Amy.Grammars.EBNF;
using Amy.Grammars.EBNF.EBNFItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Amy.UnitTests
{
    [TestClass]
    public class EBNFExpressionTests
    {
        private readonly IFormalGrammarParser parser;
        private readonly TestGrammarDefinition definition;

        public EBNFExpressionTests()
        {
            this.parser = new EBNFGrammarParserCustom(50);
            this.definition = new TestGrammarDefinition();
        }

        [TestMethod]
        public void ExpressionTest_IsVarsAndFunction()
        {

            var definitionMock = new Mock<EBNFGrammarDefinition>();

            definitionMock.Setup(exp => exp.ProductionRules).Returns(
                (new string[]
                {
                    this.definition.Space,
                    this.definition.Termination,
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
                }).Reverse().ToArray());
            definitionMock.Setup(exp => exp.GetNewNonTerminalInstance(It.IsAny<string>())).Returns((string str) =>
            {
                return new Moq.Mock<NonTerminal>(MockBehavior.Strict, str, 20).Object;
            });

            definitionMock.Setup(exp => exp.GetStartSymbol(It.IsAny<NonTerminal>(), It.IsAny<List<NonTerminal>>())).Returns(
            (NonTerminal nonTerminal, List<NonTerminal> nonTerminals) =>
            {
                return new Moq.Mock<EBNFStartSymbol>(MockBehavior.Strict, nonTerminal, nonTerminals).Object;
            });

            var symbol = (EBNFStartSymbol)this.parser.Parse(definitionMock.Object);

            var variableExp = "int a=12;";
            var variableExp1 = "bool b=true;";
            var funcExp1 = $"private int ab{{{variableExp}{variableExp1}}};";
            Assert.IsTrue(symbol.IsExpression(variableExp));
            Assert.IsTrue(symbol.IsExpression(variableExp1));
            Assert.IsTrue(symbol.IsExpression(funcExp1));
        }

        [TestMethod]
        public void ExpressionTest_ExpressionStructure()
        {

            var definitionMock = new Mock<EBNFGrammarDefinition>();

            definitionMock.Setup(exp => exp.ProductionRules).Returns(
                (new string[]
                {
                    this.definition.Space,
                    this.definition.Termination,
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
                }).Reverse().ToArray());
            definitionMock.Setup(exp => exp.GetNewNonTerminalInstance(It.IsAny<string>())).Returns((string str) =>
            {
                return new Moq.Mock<NonTerminal>(MockBehavior.Strict, str, 20).Object;
            });

            definitionMock.Setup(exp => exp.GetStartSymbol(It.IsAny<NonTerminal>(), It.IsAny<List<NonTerminal>>())).Returns(
                (NonTerminal nonTerminal, List<NonTerminal> nonTerminals) =>
                {
                    return new Moq.Mock<EBNFStartSymbol>(MockBehavior.Strict, nonTerminal, nonTerminals).Object;
                });

            var symbol = this.parser.Parse(definitionMock.Object);

            var variableExp = "int a=12;";
            var variableExp1 = "bool b=true;";
            var funcExp1 = $"private int ab{{{variableExp}{variableExp1}}};";

            var prgStructure = symbol.ExpressionStructure(funcExp1);
            Assert.AreEqual(1, prgStructure.Count());
            var prgItem = prgStructure.Single();
            Assert.AreEqual(prgItem.Expression, funcExp1);
            Assert.IsInstanceOfType(prgItem.Item, typeof(NonTerminal));

            var varStructure = symbol.ExpressionStructure($"{variableExp1}{variableExp}");
            Assert.AreEqual(1, varStructure.Count());
            foreach(var item in varStructure.Single().Childs)
            {
                Assert.IsInstanceOfType(item.Item, typeof(NonTerminal));
                Assert.AreEqual("variable", ((NonTerminal)item.Item).Name);
            }
        }

        [TestMethod]
        public void ExpressionTest_Performace()
        {
            var definitionMock = new Mock<EBNFGrammarDefinition>();

            definitionMock.Setup(exp => exp.ProductionRules).Returns(
                (new string[]
                {
                    this.definition.Space,
                    this.definition.Termination,
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
                }).Reverse().ToArray());
            definitionMock.Setup(exp => exp.GetNewNonTerminalInstance(It.IsAny<string>())).Returns((string str) =>
            {
                return new Moq.Mock<NonTerminal>(MockBehavior.Strict, str, 20).Object;
            });

            definitionMock.Setup(exp => exp.GetStartSymbol(It.IsAny<NonTerminal>(), It.IsAny<List<NonTerminal>>())).Returns(
            (NonTerminal nonTerminal, List<NonTerminal> nonTerminals) =>
            {
                return new Moq.Mock<EBNFStartSymbol>(MockBehavior.Strict, nonTerminal, nonTerminals).Object;
            });

            var symbol = this.parser.Parse(definitionMock.Object);

            TimeSpan timeResult = Time(() =>
            {
                var variableExp = "int a=12;";
                var variableExp1 = "bool b=true;";
                var variableExp2 = "bool b=false;";
                var funcExp1 = $"private int ab{{{variableExp}{variableExp1}{variableExp2}}};";

                var prgStructure = symbol.ExpressionStructure(funcExp1);
            });

            Assert.IsTrue(timeResult.TotalMilliseconds < 170);
        }

        private TimeSpan Time(Action toTime)
        {
            var timer = Stopwatch.StartNew();
            toTime();
            timer.Stop();
            return timer.Elapsed;
        }
    }
}

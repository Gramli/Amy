using Amy.Grammars.EBNF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amy.UnitTests
{
    [TestClass]
    public class EBNFTests
    {
        string character = "char = \"a\"|\"b\";";
        string digit = "digit = \"1\"|\"2\";";
        string intType = "intType = \"int\";";
        string boolType = "boolType = \"bool\";";
        string type = "type = intType | boolType";
        string name = "name = char, { digit | char };";
        string boolValue = "boolValue = \"true\" | \"false\";";
        string boolVar = "boolVar = boolType,name, \"=\", boolValue;";
        string intValue = "intValue = {digit};";
        string intVar = "intVar = intType, name, \"=\", intValue;";
        string variable = "variable = intVar | boolVar;";
        string function = "function = [private],type, name , \"{\",{variable, \";\"},\"}\";";


        [TestMethod]
        public void ParsingTest()
        {
            //EBNFGrammarDefinition definition = new EBNFGrammarDefinition()

            //EBNFGrammarParserCustom parser = new EBNFGrammarParserCustom(50);
            //parser.Parse()
        }
    }
}

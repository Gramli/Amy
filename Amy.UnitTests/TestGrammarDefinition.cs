namespace Amy.UnitTests
{
    /// <summary>
    /// Its testing grammar
    /// Defines two types int and bool,
    /// Result of grammar its program - variable or function
    /// </summary>
    public class TestGrammarDefinition
    {
        public string Space { get { return "space = \" \" ;"; } }
        public string Termination { get { return "termination = \";\";"; } }
        public string Character { get { return "char = \"a\"|\"b\";"; } }
        public string Digit { get { return "digit = \"1\"|\"2\";"; } }
        public string IntType { get { return "intType = \"int\";"; } }
        public string BoolType { get { return "boolType = \"bool\";"; } }
        public string Type { get { return "type = intType | boolType;"; } }
        public string Name { get { return "name = char, { digit | char };"; } }
        public string BoolValue { get { return "boolValue = \"true\" | \"false\";"; } }
        public string BoolVar { get { return "boolVar = boolType, space, name, \"=\", boolValue;"; } }
        public string IntValue { get { return "intValue = {digit};"; } }
        public string IntVar { get { return "intVar = intType,space, name, \"=\", intValue;"; } }
        public string Variable { get { return "variable = (intVar | boolVar), termination ;"; } }
        public string Function { get { return "function = [\"private\"],space, type,space, name , \"{\",{variable},\"}\", termination ;"; } }
        public string Program { get { return "program = function | (variable, [variable]);"; } }
    }
}

namespace Amy.UnitTests
{
    /// <summary>
    /// Its testing grammar
    /// Defines two types int and bool,
    /// Result of grammar its program - variable or function
    /// </summary>
    public class TestGrammarDefinition
    {
        public string Space => "space = \" \" ;";
        public string Termination => "termination = \";\";";
        public string Character => "char = \"a\"|\"b\";";
        public string Digit => "digit = \"1\"|\"2\";";
        public string IntType => "intType = \"int\";";
        public string BoolType => "boolType = \"bool\";";
        public string Type => "type = intType | boolType;";
        public string Name => "name = char, { digit | char };";
        public string BoolValue => "boolValue = \"true\" | \"false\";";
        public string BoolVar => "boolVar = boolType, space, name, \"=\", boolValue;";
        public string IntValue => "intValue = {digit};";
        public string IntVar => "intVar = intType,space, name, \"=\", intValue;";
        public string Variable => "variable = (intVar | boolVar), termination ;";
        public string Function => "function = [\"private\"],space, type,space, name , \"{\",{variable},\"}\", termination ;";
        public string Program => "program = function | (variable, [variable]);";
    }
}

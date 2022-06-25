using Antlr4.Runtime;
using CritLang;
using CritLang.Content;

var fileName = "";
foreach (var arg in args)
{
    if (new[] { "-h", "--help" }.Contains(arg))
    {
        Console.WriteLine("Usage: critlang [file]");
        Environment.Exit(0);
    }
    else if (arg.StartsWith("-"))
    {
        Console.WriteLine("Unknown option: " + arg);
        Environment.Exit(1);
    }
    else
    {
        fileName = arg;
    }
}
if (fileName.Length == 0)
{
    Console.WriteLine("No file specified");
    Environment.Exit(1);
}

var fileContents = "";
try
{
    fileContents = File.ReadAllText(fileName);
}
catch (FileNotFoundException)
{
    Console.WriteLine("File not found: " + fileName);
    Environment.Exit(1);
}


var inputStream = new AntlrInputStream(fileContents);
var critLexer = new CritLexer(inputStream);
var commonTokenStream = new CommonTokenStream(critLexer);
var critParser = new CritParser(commonTokenStream);
var chatContext = critParser.program();
var visitor = new CritVisitor();

visitor.Visit(chatContext);

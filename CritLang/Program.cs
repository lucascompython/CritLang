using Antlr4.Runtime;
using CritLang;
using CritLang.Content;

const string VERSION = "v0.1.8-beta";

string fileName = CommandLineArgs.Parse(args, VERSION);



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
var critContext = critParser.program();
var visitor = new CritVisitor();

visitor.Visit(critContext);

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CritLang;

const string VERSION = "v0.2.1-beta";

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
var visitor = new CritMain(VERSION);

visitor.Visit(critContext);


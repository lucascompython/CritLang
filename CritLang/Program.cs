using Antlr4.Runtime;
using CritLang;
using CritLang.Content;

string VERSION = "v0.1.6-beta";

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
var chatContext = critParser.program();
var visitor = new CritVisitor();

visitor.Visit(chatContext);

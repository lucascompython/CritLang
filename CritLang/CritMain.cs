using System.Globalization;
using System.Numerics;



namespace CritLang;

public sealed class CritMain : CritBaseVisitor<object?>
{
    private Dictionary<string, object?> Variables { get; } = new();



    public CritMain(string version)
    {
        string _version = version;
        //Math Functions
        Variables["Pow"] = new Func<object?[], object?>(Pow);
        Variables["PI"] = Math.PI;
        Variables["Sqrt"] = new Func<object?[], object?>(Sqrt);

        //Console Functions
        Variables["Write"] = new Func<object?[], object?>(Write);
        Variables["WriteLine"] = new Func<object?[], object?>(WriteLine);
        Variables["ReadLine"] = new Func<object?[], object?>(ReadLine);

        //Array Functions
        Variables["Sum"] = new Func<object?[], object?>(SumArr);
        Variables["Add"] = new Func<object?[], object?>(AddArr);
        Variables["Remove"] = new Func<object?[], object?>(RemoveArr);
        Variables["Split"] = new Func<object?[], object?>(Split);
        Variables["Len"] = new Func<object?[], object?>(Len);

        //OS Functions
        Variables["ReadText"] = new Func<object?[], object?>(ReadText);
        Variables["Time"] = new Func<object?[], object?>(Time);

        //Gerenal Functions
        Variables["Convert"] = new Func<object?[], object?>(ConvertTo);
        Variables["Delay"] = new Func<object?[], object?>(Delay);
        Variables["CritVersion"] = _version;
        Variables["Type"] = new Func<object?[], object?>(GetTypeOf);

    }

    private static object? GetTypeOf(object?[] args)
    {
        if (args.Length == 0 || args.Length > 1)
            throw new Exception("Type only takes 1 argument.");

        return args[0]!.GetType();
    }

    private static object? Split(object?[] args)
    {
        if (args.Length != 2) throw new Exception("Split takes 2 arguments.");
        object[]? ol = args[0]?.ToString()?.Split(args[1]!.ToString()!.ToCharArray());
        return ol?.ToList();
    }

    private static object? Delay(object?[] args)
    {
        if (args.Length != 1) throw new Exception("Delay takes 1 argument that is the number of milliseconds to delay.");
        Task.Delay((int)args[0]!).Wait();
        return null;

    }


    private static object? Time(object?[] args)
    {
        if (args.Length != 0) throw new Exception("Time takes no arguments");

        TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
        double timestamp = t.TotalMilliseconds;
        return Math.Round(timestamp / 1000, 2);
    }

    private static object? ReadText(object?[] args)
    {
        if (args.Length != 1) throw new Exception("ReadText expects 1 argument");

        try
        {
            return File.ReadAllText(args[0]!.ToString()!);
        }
        catch (Exception e)
        {
            throw new Exception("ReadText failed: " + e.Message);
        }
    }


    private static object? ConvertTo(object?[] args)
    {
        if (args.Length != 2)
            throw new Exception("ConvertTo expects 2 arguments, first one the variable to convert to and the second one being the type.");


        string typeToConvertTo = args[1]!.ToString()!;

        string[] typeOptions = { "int", "float", "string", "bool" };
        if (!typeOptions.Contains(typeToConvertTo))
            throw new Exception($"Invalid type...\nMust be one of the following types: {string.Join(", ", typeOptions)}");

        object? valueToConvert = args[0]!;

        return valueToConvert switch
        {
            string => Convert.ToString(valueToConvert),
            bool => Convert.ToBoolean(valueToConvert),
            _ => TypeDispatcher(valueToConvert)
        };
    }


    private static object? ReadLine(object?[] args)
    {
        if (args.Length != 1)
        {
            throw new Exception("ReadLine expects 1 arguments, being the text to prompt to the user.");
        }

        string text = args[0]!.ToString()!;
        Console.Write(text);
        return Console.ReadLine();

    }


    private static object? RemoveArr(object?[] args)
    {
        if (args.Length != 2)
        {
            throw new Exception("Remove expects 2 arguments, first one the array and the second ono the index.");
        }

        if (args[0] is List<object> objArr)
        {
            objArr.RemoveAt((int)args[1]!);
        }

        return null;
    }


    private static object? Pow(object?[] args) => args.Length is not 2
        ? throw new Exception("Pow expects 2 arguments.")
        : (float)(Math.Pow(Convert.ToSingle(args[0]!), Convert.ToSingle(args[1]!)));

    private static object? Len(object?[] args)
    {
        if (args.Length is not 1)
            throw new Exception("Len expects 1 argument");

        return args[0] switch
        {
            List<object> arr => arr.Count,
            string str => str.Length,
            _ => new Exception("Len expects an array or string")
        };
    }


    private static object? AddArr(object?[] args)
    {
        if (args.Length is not 2)
            throw new Exception("Add expects 2 argument, the first one being the array and the second one being the index.");

        if (args[0] is List<object> objArr)
        {
            objArr.Add(args[1]!);
        }

        return null;
    }


    private static object? SumArr(object?[] args)
    {
        if (args.Length is not 1)
            throw new Exception("Sum expects 1 argument");

        if (args[0] is List<object> objArr)
            return (float)objArr.Sum(Convert.ToDouble);

        throw new Exception("Sum: Argument is not a valid array.");
    }



    private static object? Sqrt(object?[] arg)
    {

        if (arg.Length != 1)
            throw new Exception("Sqrt takes one argument");

        return arg[0] switch
        {
            int d => Convert.ToInt32(Math.Sqrt(Convert.ToDouble(d))),
            float f => Convert.ToSingle(Math.Sqrt(f)),
            _ => throw new Exception("Sqrt takes one integer ot float argument")
        };
    }



    private static object? Write(object?[] args)
    {
        foreach (var arg in args)
        {
            if (arg is List<object> objArr)
            {
                foreach (var obj in objArr)
                    Console.Write(obj);
                continue;
            }
            Console.Write(arg);
        }
        return null;
    }

    private static object? WriteLine(object?[] args)
    {
        foreach (var arg in args)
        {
            if (arg is List<object> objArr)
            {
                foreach (var obj in objArr)
                    Console.WriteLine(obj);
                continue;
            }
            Console.WriteLine(arg);
        }
        return null;
    }

    //TODO Array indexing
    //public override object? VisitExpression(CritParser.ExpressionContext context)
    //{
    //    throw new NotImplementedException("Array indexing is not implemented");
    //}

    //public override object? VisitConstantExpression(CritParser.ConstantExpressionContext context)
    //{
    //    return base.VisitConstantExpression(context);
    //}




    public override object? VisitFunctionCall(CritParser.FunctionCallContext context)
    {

        var name = context.IDENTIFIER().GetText();
        var args = context.expression().Select(Visit).ToArray();

        if (!Variables.ContainsKey(name))
            throw new Exception($"Function {name} not found");



        if (Variables[name] is not Func<object?[], object?> func)
            throw new Exception($"Function {name} is not a function");


        return func(args);



    }


    public override object? VisitAssignment(CritParser.AssignmentContext context)
    {
        var varName = context.IDENTIFIER().GetText();

        var value = Visit(context.expression());

        var op = context.assignmentOp().GetText();

        if (varName.Contains('[') && varName.Contains(']'))
        {
            string[] variableHelper = varName.Replace("]", string.Empty).Split('[');
            string varWithoutIndex = variableHelper[0];
            string index = variableHelper[1];
            //Variables[varWithoutIndex[..^1]]?[int.Parse(index.ToString())] = value;
            //Console.WriteLine(Variables[varWithoutIndex[..^1]]?[int.Parse(index.ToString())]);
            var variable = Variables[varWithoutIndex];
            if (variable is not List<object> vO) return null;
            //foreach (var ola in vO)
            //{
            //    Console.WriteLine(ola);
            //}
            try
            {
                if (int.TryParse(index, out int intIndex))
                {
                    vO[intIndex] = value!;
                }
                else if (Variables.ContainsKey(varWithoutIndex))
                {
                    var varValue = Variables[index];
                    return varValue is not null ? vO[(int)Math.Round(Convert.ToSingle(varValue), 0)] = varValue : throw new Exception("Index not valid.");
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                vO.Add(value!);
            }

        }

        else switch (op)
        {
                case "*=":
                    {
                        var varValue = Convert.ToSingle(Variables[varName]!);
                        Variables[varName] = varValue * Convert.ToSingle(value!);
                        break;
                    }
                case "/=":
                    {
                        var varValue = Convert.ToSingle(Variables[varName]!);
                        Variables[varName] = varValue / Convert.ToSingle(value!);
                        break;
                    }
                case "%=":
                    {
                        var varValue = Convert.ToSingle(Variables[varName]!);
                        Variables[varName] = varValue % Convert.ToSingle(value!);
                        break;
                    }
                case "+=":
                    {
                        var varValue = Convert.ToSingle(Variables[varName]!);
                        Variables[varName] = varValue + Convert.ToSingle(value!);
                        break;
                    }
                case "-=":
                    {
                        var varValue = Convert.ToSingle(Variables[varName]!);
                        Variables[varName] = varValue - Convert.ToSingle(value!);
                        break;
                    }
                default:
                    Variables[varName] = value;
                    break;
            }


        return null;
    }


    public override object? VisitIdentifierExpression(CritParser.IdentifierExpressionContext context)
    {
        var varName = context.IDENTIFIER().GetText();
        if (varName.Contains('[') && varName.Contains(']'))
        {
            string[] variableHelper = varName.Replace("]", string.Empty).Split('[');
            string varWithoutIndex = variableHelper[0];
            string index = variableHelper[1];


            var variable = Variables[varWithoutIndex];
            if (int.TryParse(index, out _))
            {
                if (variable is List<object> vO)
                    return vO[int.Parse(index)];
            }
            else if (Variables.ContainsKey(varWithoutIndex))
            {
                var value = Variables[index];
                if (variable is List<object> vO)
                    return vO[int.Parse(value!.ToString() ?? throw new Exception("Index is not a number"))];
            }
            else
            {
                throw new Exception($"Variable {varWithoutIndex} not found");
            }
        }


        //TODO ADD SOMETHING LIKE THIS
        //if (varName.Contains('.'))
        //{az
        //    string[] variableHelper = varName.Split('.');
        //    string varWithoutIndex = variableHelper[0];
        //    string index = variableHelper[1];
        //    var variable = Variables[varWithoutIndex];
        //    if (variable is not null)
        //    {
        //        var value = variable.GetType().GetProperty(index).GetValue(variable);
        //        return value;
        //    }
        //    else
        //    {
        //        throw new Exception($"Variable {varWithoutIndex} not found");
        //    }
        //}

        if (!Variables.ContainsKey(varName))
            throw new Exception($"Variable '{varName}' is not defined");


        return Variables[varName];

    }


    public override object? VisitConstant(CritParser.ConstantContext context)
    {

        if (context.INTEGER() is { } i)
        {
            string text = i.GetText();

            if (int.TryParse(text, out int inT))
                return inT;

            if (long.TryParse(text, out long lo))
                return lo;

            if (BigInteger.TryParse(text, out var biggie))
                return biggie;
        }

        if (context.FLOAT() is { } f)
        {
            string text = f.GetText();

            return text.Split('.')[0].Length >= 40 ? double.Parse(text, CultureInfo.InvariantCulture) : float.Parse(text, CultureInfo.InvariantCulture);
        }

        if (context.STRING() is { } s)
            return s.GetText()[1..^1];

        if (context.BOOL() is { } b)
            return b.GetText() == "true";


        if (context.array() is { } a)
        {
            string[] strArr = a.GetText()[1..^1].Split(',');
            var anyLst = new List<object>();
            if (strArr.Length <= 1)
                return anyLst;




            foreach (string element in strArr)
            {
                if (element.StartsWith('"') && element.EndsWith('"'))
                    anyLst.Add(element[1..^1]);

                else
                    anyLst.Add(int.TryParse(element, out int outi) ? outi : float.Parse(element, CultureInfo.InvariantCulture));
            }
            return anyLst;
        }

        if (context.NULL() is { })
            return null;

        throw new NotImplementedException();

    }


    public override object? VisitAdditiveExpression(CritParser.AdditiveExpressionContext context)
    {
        var left = Visit(context.expression(0))!;
        var right = Visit(context.expression(1))!;


        var op = context.addOp().GetText();



        string leftString = left.ToString()!;
        string rightString = right.ToString()!;

        return op switch
        {
            "+" => Add(left, right),
            "-" => (double.TryParse(leftString, out double newLeft) && double.TryParse(rightString, out double newRight)) ? Subtract(newLeft, newRight) : throw new Exception($"Cant subtract {leftString} with {rightString}."),
            _ => throw new NotImplementedException()
        };

    }

    public override object? VisitMultiplicativeExpression(CritParser.MultiplicativeExpressionContext context)
    {
        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));


        //left = TypeDispatcher(left!);
        //right = TypeDispatcher(right!);


        var op = context.multOp().GetText();
        var newLeft = double.Parse(left!.ToString()!);
        var newRight = double.Parse(right!.ToString()!);
        return op switch
        {
            "*" => Multiply(newLeft, newRight),
            "/" => Divide(newLeft, newRight),
            "%" => Modulus(newLeft, newRight),
            _ => throw new NotImplementedException()
        };
    }


    private Type NumberType(object? left, object? right)
    {
        if (left is int || right is int)
            return typeof(int);

        if (left is long || right is long)
            return typeof(long);

        if (left is float || right is float)
            return typeof(float);

        if (left is double || right is double)
            return typeof(double);

        if (left is BigInteger || right is BigInteger)
            return typeof(BigInteger);

        throw new Exception("No number type found");
    }



    private static object? Add(object left, object right) => (left, right) switch
    {
        (string leftString, string rightString) => leftString + rightString,
        (int or long or float or double, int or long or float or double) => Convert.ToDouble(left.ToString()) + Convert.ToDouble(right.ToString()),
        (_, string) => left.ToString() + right.ToString(),
        (string, _) => left.ToString() + right.ToString(),
        _ => throw new NotImplementedException("Something went wrong.")
    };

    private static double ol(object left, object right) {
        Console.WriteLine($"Left: {left} Right: {right}");
        return (double)left + (double)right;
    }
    private static object? Subtract<T>(T left, T right) where T : INumber<T> => left - right;

    private static object? Multiply<T>(T left, T right) where T : INumber<T> => left * right;

    private static object? Divide<T>(T left, T right) where T : INumber<T> => left / right;

    private static object? Modulus<T>(T left, T right) where T : INumber<T> => left % right;




    public override object? VisitIfBlock(CritParser.IfBlockContext context)
    {
        var condition = Visit(context.expression());

        if (condition is bool b)
        {
            if (b)
            {
                Visit(context.block());
            }
            else
            {
                try
                {
                    Visit(context.elseIfBlock());
                }
                catch (NullReferenceException)
                {
                    // do nothing
                }
            }


        }
        else
        {
            throw new Exception("Condition must be a boolean");
        }

        return null;
    }




    public override object? VisitWhileBlock(CritParser.WhileBlockContext context)
    {

        Func<object?, bool> condition = context.WHILE().GetText() == "while"
            ? IsTrue
            : IsFalse
        ;

        if (condition(Visit(context.expression())))
        {

            do
            {
                Visit(context.block());
            } while (condition(Visit(context.expression())));

        }
        else
        {
            try
            {
                Visit(context.elseIfBlock());
            }
            catch (NullReferenceException)
            {
                // do nothing
            }
        }

        return null;


    }

    //TODO OPTIMIZE THIS SHIT
    private static object? TypeDispatcher(object? variable)
    {
        if (variable == null) throw new ArgumentNullException(nameof(variable));

        string variableText = variable.ToString()!;

        if (variableText.Contains('.') || variableText.Contains(','))
        {
            return variableText.Split('.')[0].Length >= 40
                ? double.Parse(variableText, CultureInfo.InvariantCulture)
                : float.Parse(variableText, CultureInfo.InvariantCulture);
        }


        if (int.TryParse(variableText, out int variableTextInt))
            return variableTextInt;

        if (long.TryParse(variableText, out long variableTextLong))
            return variableTextLong;

        return BigInteger.TryParse(variableText, out var variableTextBiggie) ? variableTextBiggie : variable;

        //TODO MAKE A ERROR MESSAGE HERE OF THE TYPE OF VARIABLE
        //throw new Exception($"Something when wrong when trying to figure out the type of {variableText}.");
    }


    public override object? VisitComparisonExpression(CritParser.ComparisonExpressionContext context)
    {
        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));

        if (left is null || right is null) return null;


        //Console.WriteLine(left.GetType());

        //left = TypeDispatcher(left);
        //right = TypeDispatcher(right);




        //if(!double.TryParse(left.ToString()!, out double newLeft))
        //{
            //string newLeftString = left.ToString()!;    
        //}
        //if(!double.TryParse(right.ToString()!, out double newRight))
        //{
            //string newRightString = right.ToString()!;
        //}

        

        

        //if (left is BigInteger && right is float or double)
        //{
        //    object obj = left; BigInteger biggieLeft = (BigInteger)obj; left = (double)biggieLeft;
        //}

        //else if (right is BigInteger && left is float or double)
        //{
        //    object obj = right; BigInteger biggieLeft = (BigInteger)obj; right = (double)biggieLeft;
        //}

        string leftString = left.ToString()!;
        string rightString = right.ToString()!;
        var op = context.compareOp().GetText();
        return op switch
        {
            "==" => (double.TryParse(leftString, out double newLeft) && double.TryParse(rightString, out double newRight)) ? IsEquals(newLeft, newRight) : IsEquals(leftString, rightString),
            "!=" => (double.TryParse(leftString, out double newLeft) && double.TryParse(rightString, out double newRight)) ? NotEquals(newLeft, newRight) : IsEquals(leftString, rightString),
            ">" => (double.TryParse(leftString, out double newLeft) && double.TryParse(rightString, out double newRight)) ? GreaterThan(newLeft, newRight) : throw new Exception($"Can't compare {leftString} and {rightString} with >"),
            "<" => (double.TryParse(leftString, out double newLeft) && double.TryParse(rightString, out double newRight)) ? LessThan(newLeft, newRight) : throw new Exception($"Can't compare {leftString} and {rightString} with <"),
            ">=" => (double.TryParse(leftString, out double newLeft) && double.TryParse(rightString, out double newRight)) ? GreaterThanOrEqual(newLeft, newRight) : throw new Exception($"Can't compare {leftString} and {rightString} with >="),
            "<=" => (double.TryParse(leftString, out double newLeft) && double.TryParse(rightString, out double newRight)) ? LessThanOrEqual(newLeft, newRight) : throw new Exception($"Can't compare {leftString} and {rightString} with <="),
            _ => throw new NotImplementedException("Something went wrong")

        };
    }

    private static bool IsEquals<T>(T left, T right) where T : IComparable<T> => left.ToString() == right.ToString();

    private static bool NotEquals<T>(T left, T right) => !EqualityComparer<T>.Default.Equals(left, right);
    
    private static bool GreaterThanOrEqual<T>(T left, T right) where T : INumber<T> => left >= right;

    private static bool LessThanOrEqual<T>(T left, T right) where T : INumber<T> => left <= right;

    private static bool GreaterThan<T>(T left, T right) where T : INumber<T> => left > right;

    private static bool LessThan<T>(T left, T right) where T : INumber<T> => left < right;

    private static bool IsTrue(object? value) => value switch
    {
        bool b => b,
        int i => i != 0,
        float f => f != 0,
        long l => l != 0,
        double d => d != 0,
        BigInteger bi => bi != 0,
        string s => s.Length != 0,
        _ => throw new Exception($"Value is not boolean.")
    };

    private static bool IsFalse(object? value) => !IsTrue(value);

}
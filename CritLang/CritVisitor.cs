using System.Globalization;
using CritLang.Content;

namespace CritLang;

public class CritVisitor: CritBaseVisitor<object?>
{
    private Dictionary<string, object?> Variables { get; } = new();



    public CritVisitor()
    {
        //GLOBAL FUNCTIONs / VARIABLES
        Variables["PI"] = Math.PI;
        Variables["Sqrt"] = new Func<object?[], object?>(Sqrt);
        Variables["Pow"] = new Func<object?[], object?>(Pow);

        Variables["Write"] = new Func<object?[], object?>(Write);
        Variables["WriteLine"] = new Func<object?[], object?>(WriteLine);
        Variables["Sum"] = new Func<object?[], object?>(SumArr);
        Variables["Add"] = new Func<object?[], object?>(AddArr);
        Variables["Remove"] = new Func<object?[], object?>(RemoveArr);
        Variables["Len"] = new Func<object?[], object?>(LenArr);
    }


    private object? RemoveArr(object?[] args)
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
    
    private static object? LenArr(object?[] args)
    {
        if (args.Length is not 1)
            throw new Exception("Len expects 1 argument");

        if (args[0] is List<object> arr)
            return arr.Count;

        return new Exception("Len expects an array");

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
        else
        {
            Variables[varName] = value;
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

        if (!Variables.ContainsKey(varName))
            throw new Exception($"Variable '{varName}' is not defined");
        

        return Variables[varName];

    }


    public override object? VisitConstant(CritParser.ConstantContext context)
    {
        
        if (context.INTEGER() is { } i)
            return int.Parse((i.GetText()));

        if (context.FLOAT() is { } f)
            return float.Parse(f.GetText(), CultureInfo.InvariantCulture);
        
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
        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));


        var op = context.addOp().GetText();

        return op switch
        {
            "+" => Add(left, right),
            "-" => Subtract(left, right),
            _ => throw new NotImplementedException()
        };

    }

    public override object? VisitMultiplicativeExpression(CritParser.MultiplicativeExpressionContext context)
    {
        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));


        var op = context.multOp().GetText();

        return op switch
        {
            "*" => Multiply(left, right),
            "/" => Divide(left, right),
            "%" => Modulus(left, right),
            _ => throw new NotImplementedException()
        };
    }


    private static object? Add(object? left, object? right) => (left, right) switch
    {
        (int l, int r) => l + r,
        (float lf, float rf) => lf + rf,
        (int lInt, float rf) => lInt + rf,
        (float lf, int rInt) => lf + rInt,
        (string, _) => $"{left}{right}",
        (_, string) => $"{left}{right}",
        (_, _) => throw new Exception($"Cannot add values of type {left?.GetType()} and {right?.GetType()}.")
    };
    

    private static object? Subtract(object? left, object? right) => (left, right) switch
    {
        (int l, int r) => l - r,
        (float lf, float rf) => lf - rf,
        (int lInt, float rf) => lInt - rf,
        (float lf, int rInt) => lf - rInt,
        (_, _) => throw new Exception($"Cannot subtract values of type {left?.GetType()} and {right?.GetType()}.")
    };


    private static object? Multiply(object? left, object? right) => (left, right) switch
    {
        (int l, int r) => l * r,
        (float lf, float rf) => lf * rf,
        (int lInt, float rf) => lInt * rf,
        (float lf, int rInt) => lf * rInt,
        (_, _) => throw new Exception($"Cannot multiply values of type {left?.GetType()} and {right?.GetType()}.")
    };

    private static object? Divide(object? left, object? right) => (left, right) switch
    {
        (int l, int r) => l / r,
        (float lf, float rf) => lf / rf,
        (int lInt, float rf) => lInt / rf,
        (float lf, int rInt) => lf / rInt,
        (_, _) => throw new Exception($"Cannot divide values of type {left?.GetType()} and {right?.GetType()}.")
    };

    private static object? Modulus(object? left, object? right) => (left, right) switch
    {
        (int l, int r) => l % r,
        (float lf, float rf) => lf % rf,
        (int lInt, float rf) => lInt % rf,
        (float lf, int rInt) => lf % rInt,
        (_, _) => throw new Exception($"Cannot modulus values of type {left?.GetType()} and {right?.GetType()}.")
    };




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
            throw new Exception($"Condition must be a boolean");
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


    public override object? VisitComparisonExpression(CritParser.ComparisonExpressionContext context)
    {
        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));

        var op = context.compareOp().GetText();

        return op switch
        {
            "==" => IsEquals(left, right),
            "!=" => NotEquals(left, right),
            ">" => GreaterThan(left, right),
            "<" => LessThan(left, right),
            ">=" => GreaterThanOrEqual(left, right),
            "<=" => LessThanOrEqual(left, right),
            _ => throw new NotImplementedException()

        };
    }




    private static bool IsEquals(object? left, object? right)
    {
        switch (left, right)
        {
            case (int l, int r):
                return l == r;
            case (float lf, float rf):
                return lf == rf;
            case (int lInt, float rFloat):
                return lInt == rFloat;
            case (float lFloat, int rInt):
                return lFloat == rInt;
        }
        if (left is string || right is string)
            return left?.ToString() == right?.ToString();

        if (left is bool || right is bool)
            return left?.ToString() == right?.ToString();

        throw new Exception($"Cannot compare values of type {left?.GetType()} and {right?.GetType()}.");
    }


    private static bool NotEquals(object? left, object? right)
    {
        switch (left, right)
        {
            case (int l, int r):
                return l != r;
            case (float lf, float rf):
                return lf != rf;
            case (int lInt, float rFloat):
                return lInt != rFloat;
            case (float lFloat, int rInt):
                return lFloat != rInt;
        }
        
        if (left is string || right is string)
            return left?.ToString() != right?.ToString();

        throw new Exception($"Cannot compare values of type {left?.GetType()} and {right?.GetType()}.");
    }

    private static bool GreaterThanOrEqual(object? left, object? right) => (left, right) switch
    {
        (int l, int r) => l >= r,
        (float lf, float rf) => lf >= rf,
        (int lInt, float rFloat) => lInt >= rFloat,
        (float lFloat, int rInt) => lFloat >= rInt,
        _ => throw new Exception($"Cannot compare values of type {left?.GetType()} and {right?.GetType()}."),
    };


    private static bool LessThanOrEqual(object? left, object? right) => (left, right) switch
    {
        (int l, int r) => l <= r,
        (float lf, float rf) => lf <= rf,
        (int lInt, float rFloat) => lInt <= rFloat,
        (float lFloat, int rInt) => lFloat <= rInt,
        _ => throw new Exception($"Cannot compare values of type {left?.GetType()} and {right?.GetType()}."),
    };



    private static bool GreaterThan(object? left, object? right) => (left, right) switch
    {
        (int l, int r) => l > r,
        (float lf, float rf) => lf > rf,
        (int lInt, float rFloat) => lInt > rFloat,
        (float lFloat, int rInt) => lFloat > rInt,
        _ => throw new Exception($"Cannot compare values of type {left?.GetType()} and {right?.GetType()}."),
    };


    private static bool LessThan(object? left, object? right) => (left, right) switch
    {
        (int l, int r) => l < r,
        (float lf, float rf) => lf < rf,
        (int lInt, float rFloat) => lInt < rFloat,
        (float lFloat, int rInt) => lFloat < rInt,
        _ => throw new Exception($"Cannot compare values of type {left?.GetType()} and {right?.GetType()}.")
    };
    
    private static bool IsTrue(object? value) => value switch
    {
        bool b => b,
        int i => i != 0,
        float f => f != 0,
        string s => s.Length != 0,
        _ => throw new Exception($"Value is not boolean.")
    };

    private static bool IsFalse(object? value) => !IsTrue(value);

}
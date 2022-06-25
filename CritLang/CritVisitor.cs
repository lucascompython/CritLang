using System.Drawing;
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

        Variables["Write"] = new Func<object?[], object?>(Write);
        Variables["WriteLine"] = new Func<object?[], object?>(WriteLine);
    }



    private object? Sqrt(object?[] arg)
    {

        if (arg.Length != 1)
        {
            throw new Exception("Sqrt takes one argument");
        }


        if (arg[0] is int d)
        {
            return Math.Sqrt(Convert.ToDouble(d));
        }
        if (arg[0] is float f)
        {
            return Math.Sqrt(f);
        }

        throw new Exception("Sqrt takes one integer ot float argument");
    }



    private object? Write(object?[] args)
    {
        foreach (var arg in args)
        {
            Console.Write(arg);
        }

        return null;
    }

    private object? WriteLine(object?[] args)
    {
        foreach (var arg in args)
        {
            Console.WriteLine(arg);
        }

        return null;
    }


    public override object? VisitFunctionCall(CritParser.FunctionCallContext context)
    {
        var name = context.IDENTIFIER().GetText();
        var args = context.expression().Select(Visit).ToArray();

        if (!Variables.ContainsKey(name))
            throw new Exception($"Function {name} not found");
        

        
        if (!(Variables[name] is Func<object?[], object?> func))
            throw new Exception($"Function {name} is not a function");


        return func(args);



    }


    public override object? VisitAssignment(CritParser.AssignmentContext context)
    {
        var varName = context.IDENTIFIER().GetText();

        var value = Visit(context.expression());


        Variables[varName] = value;
        
        
        return null;
    }


    public override object? VisitIdentifierExpression(CritParser.IdentifierExpressionContext context)
    {
        var varName = context.IDENTIFIER().GetText();

        if (!Variables.ContainsKey(varName))
        {
            throw new Exception($"Variable '{varName}' is not defined");
        }

        return Variables[varName];

    }


    public override object? VisitConstant(CritParser.ConstantContext context)
    {
        if (context.INTEGER() is { } i)
            return int.Parse((i.GetText()));

        if (context.FLOAT() is { } f)
            return float.Parse(f.GetText());

        if (context.STRING() is { } s)
            return s.GetText()[1..^1];

        if (context.BOOL() is { } b)
            return b.GetText() == "true";


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


    private object? Add(object? left, object? right)
    {
        if (left is int l && right is int r)
            return l + r;

        if (left is float lf && right is float rf)
            return lf + rf;


        if (left is int lInt && right is float rFloat)
            return lInt + rFloat;

        if (left is float lFloat && right is int rInt)
            return lFloat + rInt;

        if (left is string || right is string)
            return $"{left}{right}";
        

        throw new Exception($"Cannot add values of type {left?.GetType()} and {right?.GetType()}.");
    }

    private object? Subtract(object? left, object? right)
    {
        if (left is int l && right is int r)
            return l - r;

        if (left is float lf && right is float rf)
            return lf - rf;

        if (left is int lInt && right is float rFloat)
            return lInt - rFloat;

        if (left is float lFloat && right is int rInt)
            return lFloat - rInt;

        throw new Exception($"Cannot subtract values of type {left?.GetType()} and {right?.GetType()}.");
    }


    private object? Multiply(object? left, object? right)
    {
        if (left is int l && right is int r)
            return l * r;

        if (left is float lf && right is float rf)
            return lf * rf;

        if (left is int lInt && right is float rFloat)
            return lInt * rFloat;

        if (left is float lFloat && right is int rInt)
            return lFloat * rInt;

        throw new Exception($"Cannot multiply values of type {left?.GetType()} and {right?.GetType()}.");
    }

    private object? Divide(object? left, object? right)
    {
        if (left is int l && right is int r)
            return l / r;

        if (left is float lf && right is float rf)
            return lf / rf;

        if (left is int lInt && right is float rFloat)
            return lInt / rFloat;

        if (left is float lFloat && right is int rInt)
            return lFloat / rInt;

        throw new Exception($"Cannot divide values of type {left?.GetType()} and {right?.GetType()}.");
    }

    private object? Modulus(object? left, object? right)
    {
        if (left is int l && right is int r)
            return l % r;

        if (left is float lf && right is float rf)
            return lf % rf;

        if (left is int lInt && right is float rFloat)
            return lInt % rFloat;

        if (left is float lFloat && right is int rInt)
            return lFloat % rInt;

        throw new Exception($"Cannot mod values of type {left?.GetType()} and {right?.GetType()}.");
    }

    


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
            
            //{
            //    var hasElse = Visit(context.elseIfBlock());
            //    if (hasElse is not null)
            //        Console.WriteLine("elseDASDSADSADNASDON3");
            //    Visit(context.elseIfBlock());
            //}
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
            //">=" => GreaterThanOrEqual(left, right),
            //"<=" => LessThanOrEqual(left, right),
            _ => throw new NotImplementedException()

        };
    }




    private bool IsEquals(object? left, object? right)
    {
        if (left is int l && right is int r)
            return l == r;

        if (left is float lf && right is float rf)
            return lf == rf;

        if (left is int lInt && right is float rFloat)
            return lInt == rFloat;

        if (left is float lFloat && right is int rInt)
            return lFloat == rInt;

        if (left is string || right is string)
            return left?.ToString() == right?.ToString();

        if (left is bool || right is bool)
            return left?.ToString() == right?.ToString();


        throw new Exception($"Cannot compare values of type {left?.GetType()} and {right?.GetType()}.");
    }


    private bool NotEquals(object? left, object? right)
    {
        if (left is int l && right is int r)
            return l != r;

        if (left is float lf && right is float rf)
            return lf != rf;

        if (left is int lInt && right is float rFloat)
            return lInt != rFloat;

        if (left is float lFloat && right is int rInt)
            return lFloat != rInt;

        if (left is string || right is string)
            return left?.ToString() != right?.ToString();

        throw new Exception($"Cannot compare values of type {left?.GetType()} and {right?.GetType()}.");
    }



    private bool GreaterThan(object? left, object? right)
    {
        
        if (left is int l && right is int r)
            return l > r;
        
        if (left is float lf && right is float rf)
            return lf > rf;
        
        if (left is int lInt && right is float rFloat)
            return lInt > rFloat;
        
        if (left is float lFloat && right is int rInt)
            return lFloat > rInt;

        
        throw new Exception($"Cannot compare values of type {left?.GetType()} and {right?.GetType()}.");

    }



    private bool LessThan(object? left, object? right)
    {
        if (left is int l && right is int r)
            return l < r;

        if (left is float lf && right is float rf)
            return lf < rf;

        if (left is int lInt && right is float rFloat)
            return lInt < rFloat;

        if (left is float lFloat && right is int rInt)
            return lFloat < rInt;

        throw new Exception($"Cannot compare values of type {left?.GetType()} and {right?.GetType()}.");
    }




    private bool IsTrue(object? value)
    {
        if (value is bool b)
            return b;

        if (value is int i)
            return i != 0;

        if (value is float f)
            return f != 0;

        if (value is string s)
            return s.Length != 0;


        throw new Exception($"Value is not boolean.");
    }
    private bool IsFalse(object? value) => !IsTrue(value);






}
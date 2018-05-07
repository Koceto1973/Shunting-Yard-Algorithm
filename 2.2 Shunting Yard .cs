// Shunting Yard algorithm  https://en.wikipedia.org/wiki/Shunting-yard_algorithm
// Inorder/infix expression to Postorder/postfix expression convertion
// re: Binary Expression tree

using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        string infix = "3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3";
        Console.WriteLine(infix.ToPostfix());
    }
}

public static class ShuntingYard
{
	class Operator
	{
	    public string symbol;
	    public int precedence;
	    public bool rightAssociative;

	    public Operator(string symbol, int precedence, bool rightAssociative )
	    {
	        this.symbol=symbol;
	        this.precedence=precedence;
	        this.rightAssociative=rightAssociative;
	    }
	}

	static readonly IDictionary<string,Operator>	operators = new[]  {
		new Operator("^",4,true),
		new Operator("*",3,false),
		new Operator("/",3,false),
		new Operator("+",2,false),
		new Operator("-",2,false) }.ToDictionary(op => op.symbol);


    public static string ToPostfix(this string infix)
    {
        string[] tokens = infix.Split(' '); // tokens separated
        var stack = new Stack<string>();    // stack is ready
        var output = new List<string>();    // list is ready

        Action<string> Print = (action) => Console.WriteLine($"{action + ":",-4}
                                                               {$"stack[ {string.Join(" ", stack.Reverse())} ]",-18}
                                                               {$"out[ {string.Join(" ", output)}]"}"   );

        // tokens processing
        foreach (string token in tokens) // while there are tokens to be read:
        {
            int obsoleteInt;
            Operator op1,op2;

            if (int.TryParse(token, out obsoleteInt )) // if the token is a number,
            {
                output.Add(token);  //  push it to the output queue.
                Print(token);
            }
            else if (operators.TryGetValue(token, out op1))
            {
                while (stack.Count > 0 && operators.TryGetValue(stack.Peek(), out op2))
                {
                    int c = op1.precedence.CompareTo(op2.precedence);
                    if (c < 0 || !op1.rightAssociative && c <= 0)
                    {
                        output.Add(stack.Pop());
                    }
                    else
                    {
                        break;
                    }
                }
                stack.Push(token);
                Print(token);
            }
            else if (token == "(")
            {
                stack.Push(token);
                Print(token);
            }
            else if (token == ")")
            {
                string top = "";
                while (stack.Count > 0 && (top = stack.Pop()) != "(")
                {
                    output.Add(top);
                }
                if (top != "(") throw new ArgumentException("No matching left parenthesis.");
                Print(token);
            }
        }

        while (stack.Count > 0)
        {
            var top = stack.Pop();
            if (!operators.ContainsKey(top)) throw new ArgumentException("No matching right parenthesis.");
            output.Add(top);
        }

        Print("pop");
        return string.Join(" ", output);

    }
}

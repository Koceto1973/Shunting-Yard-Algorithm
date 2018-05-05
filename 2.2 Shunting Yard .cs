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
 
// static class - string extension method and it's data collector
public static class ShuntingYard
{ // ToDictionary(key selector function) extension method applied over new anonymous type array
    private static readonly var
        operators = new (string symbol, int precedence, bool rightAssociative) [] // anonymous type array here
            {   ("^", 4, true), 
                ("*", 3, false),  
                ("/", 3, false),  
                ("+", 2, false),    
                ("-", 2, false)
            }.ToDictionary(op => op.symbol); // https://msdn.microsoft.com/en-us/library/bb549277(v=vs.110).aspx
            
    // finally dictionary "operators" is build with key=symbol and value=symbol,precedence,rightAssociative
 
    // custom extension method of string class
    public static string ToPostfix(this string infix)
    {
        string[] tokens = infix.Split(' '); // tokens separated
        var stack = new Stack<string>();    // stack is ready
        var output = new List<string>();    // list is ready
        
        // tokens processing
        foreach (string token in tokens) // while there are tokens to be read:
        {
            if (int.TryParse(token, out _ )) // if the token is a number,
            {
                output.Add(token);  //  push it to the output queue.
                Print(token); // ????
            } 
            else if (operators.TryGetValue(token, out var op1)) 
            {
                while (stack.Count > 0 && operators.TryGetValue(stack.Peek(), out var op2))
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
 
        //Yikes!
        void Print(string action) => 
            Console.WriteLine($"{action + ":",-4} {$"stack[ {string.Join(" ", stack.Reverse())} ]",-18} {$"out[ {string.Join(" ", output)} ]"}");
        //A little more readable?
        void Print(string action) =>
            Console.WriteLine("{0,-4} {1,-18} {2}", action + ":", $"stack[ {string.Join(" ", stack.Reverse())} ]", $"out[ {string.Join(" ", output)} ]");
    }
}
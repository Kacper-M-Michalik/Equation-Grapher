using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationInterpreter
{
    public static class RPN
    {
        public static HashSet<string> AllowedSymbols = new HashSet<string>()
        {
            "^",
            "*",
            "/",
            "+",
            "-",
            "(",
            ")"
        };

        static Dictionary<string, int> Precedences = new Dictionary<string, int>()
        {
            { "-", 1 },
            { "+", 1 },
            { "/", 2 },
            { "*", 2 },
            { "^", 3 }
        };

        public static List<string> RPNGenerator(string[] Tokens)
        {
            List<string> Result = new List<string>();
            Stack<string> OperatorStack = new Stack<string>();

            for (int i = 0; i < Tokens.Length; i++)
            {
                if (IsConstant(Tokens[i]))
                {
                    Result.Add(Tokens[i]);
                }
                else
                {
                    if (Tokens[i] != "(" && Tokens[i] != ")")
                    {

                        while (OperatorStack.Count > 0 && OperatorStack.Peek() != "(" && Precedences[OperatorStack.Peek()] >= Precedences[Tokens[i]])
                        {
                            Result.Add(OperatorStack.Pop());
                        }
                        OperatorStack.Push(Tokens[i]);

                    }
                    else if (Tokens[i] == "(")
                    {
                        OperatorStack.Push(Tokens[i]);
                    }
                    else
                    {
                        while (OperatorStack.Peek() != "(")
                        {
                            Result.Add(OperatorStack.Pop());
                        }
                        OperatorStack.Pop();
                    }
                }

            }
            while (OperatorStack.Count > 0)
            {
                Result.Add(OperatorStack.Pop());
            }

            return Result;
        }

        static bool IsConstant(string Token)
        {
            if (Token.ToUpper().Contains("X"))
            {
                return true;
            }
            return double.TryParse(Token, out double Val);
        }

        static bool IsConstant(string Token, double VariableValue, out double Constant)
        {
            if (Token.ToUpper().Contains("X"))
            {
                string Coefficient = Token.ToUpper().Replace("X", "");
                Constant = (Coefficient.Length == 0) ? VariableValue : double.Parse(Coefficient) * VariableValue;
                return true;
            }
            return double.TryParse(Token, out Constant);
        }

        public static double FormulaCalculator(List<string> Tokens, double VariableValue)
        {
            Stack<double> Stack = new Stack<double>();

            for (int i = 0; i < Tokens.Count; i++)
            {
                double Token = 0;
                if (IsConstant(Tokens[i], VariableValue, out Token))
                {
                    Stack.Push(Token);
                }
                else
                {
                    double Operand1 = Stack.Pop();
                    double Operand2 = Stack.Pop();
                    double Result = 0;

                    if (Tokens[i] == "+")
                    {
                        Result = Operand1 + Operand2;
                    }
                    else if (Tokens[i] == "-")
                    {
                        Result = Operand2 - Operand1;
                    }
                    else if (Tokens[i] == "*")
                    {
                        Result = Operand1 * Operand2;
                    }
                    else if (Tokens[i] == "/")
                    {
                        Result = Operand2 / Operand1;
                    }
                    else if (Tokens[i] == "^")
                    {
                        Result = Math.Pow(Operand2, Operand1);
                    }

                    Stack.Push(Result);

                }
            }

            return Stack.Pop();
        }
    }
}

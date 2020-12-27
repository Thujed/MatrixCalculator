using MatrixCalculator.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MatrixCalculator.Workers
{
    public enum Operator : byte { 
        Plus = (byte)'+',
        Minus = (byte)'-',
        Divide = (byte)'/',
        Multiply = (byte)'*',
        OpenBracket = (byte)'(',
        CloseBracket = (byte)')'
    }
    class PolishCowCalculator
    {
        public PolishCowCalculator() 
        {
            
        }

        public Regex GetSplittingPattern() {
            string pattern = @"(\d+";
            foreach (Operator op in Priorities.Keys)
                pattern += $@"|\{(char)op}";
            pattern += ')';
            Console.WriteLine(pattern);
            return new Regex(pattern);
        }

        public List<string> ConvertToRPN(string str) {
            List<string> splittedString = GetSplittingPattern().Split(str.Replace(" ", "")).Where((e) => e != "").ToList(); 

            var postfixNotation = new List<string>();
            var operationsStack = new Stack<string>();

            for (int i = 0; i < splittedString.Count(); ++i)
            {
                if (Priorities.ContainsKey((Operator)splittedString[i][0]))
                {
                    if (splittedString[i][0] == (char)Operator.CloseBracket)
                    {
                        var isOpenBracketExist = false;

                        while (operationsStack.Count != 0)
                        {
                            if (operationsStack.Peek()[0] == (char)Operator.OpenBracket)
                            {
                                operationsStack.Pop();
                                isOpenBracketExist = true;
                                break;
                            }

                            postfixNotation.Add(operationsStack.Pop());
                        }

                        if (!isOpenBracketExist)
                            throw new Exception("Open bracket not found!");

                        continue;
                    }

                    if (i != 0 && i + 1 < splittedString.Count && splittedString[i][0] == (char)Operator.Minus &&
                        Priorities.ContainsKey((Operator)splittedString[i - 1][0]))
                    {
                        postfixNotation.Add(string.Concat(splittedString[i], splittedString[i + 1]));
                        i += 1;
                        continue;
                    }

                    while (operationsStack.Count != 0)
                    {
                        if (Priorities[(Operator)splittedString[i][0]] <= Priorities[(Operator)operationsStack.Peek()[0]] &&
                            splittedString[i][0] != (char)Operator.OpenBracket)
                            postfixNotation.Add(operationsStack.Pop());
                        else
                            break;
                    }

                    operationsStack.Push(splittedString[i]);
                }
                else
                    postfixNotation.Add(splittedString[i]);
            }

            while (operationsStack.Count != 0)
                postfixNotation.Add(operationsStack.Pop());

            return postfixNotation;
        }

        public object Calculate(string str, MatrixVM[] matrices) {
            List<string> rpn = ConvertToRPN(str);
            Stack<object> value = new Stack<object>();
            double i;
            double j;
            foreach (string elem in rpn) {
                if (elem.Length == 1 && Priorities.ContainsKey((Operator)elem[0])) {
                    object secondOperand = value.Pop();
                    object firstOperand = value.Pop();
                    switch ((Operator)elem[0]) {
                        case Operator.Plus:
                            if (firstOperand is string && secondOperand is string && Double.TryParse((string)firstOperand, out i)  && Double.TryParse((string)secondOperand, out j)) { 
                                value.Push((i + j).ToString());
                            } else if (firstOperand is MatrixVM && Double.TryParse((string)secondOperand, out i)) {
                                value.Push((MatrixVM)firstOperand + i);
                            } else if (firstOperand is MatrixVM && secondOperand is MatrixVM && IsMatricesConsistent((MatrixVM)firstOperand, (MatrixVM)secondOperand) ) {
                                value.Push((MatrixVM)firstOperand + (MatrixVM)secondOperand);
                            }
                            break;
                        case Operator.Minus:
                            if (firstOperand is string && secondOperand is string && Double.TryParse((string)firstOperand, out i) && Double.TryParse((string)secondOperand, out j)) { 
                                value.Push((i - j).ToString());
                            } else if (firstOperand is MatrixVM && Double.TryParse((string)secondOperand, out i)) { 
                                value.Push((MatrixVM)firstOperand - i);
                            } else if (firstOperand is MatrixVM && secondOperand is MatrixVM && IsMatricesConsistent((MatrixVM)firstOperand, (MatrixVM)secondOperand)) {
                                value.Push((MatrixVM)firstOperand - (MatrixVM)secondOperand);
                            }
                            break;
                        case Operator.Divide:
                            if (firstOperand is string && secondOperand is string && Double.TryParse((string)firstOperand, out i) && Double.TryParse((string)secondOperand, out j))
                                value.Push((i / j).ToString());
                            break;
                        case Operator.Multiply:
                            if (firstOperand is string && secondOperand is string && Double.TryParse((string)firstOperand, out i) && Double.TryParse((string)secondOperand, out j)) { 
                                value.Push((i * j).ToString());
                            } else if (firstOperand is MatrixVM && secondOperand is string && Double.TryParse((string)secondOperand, out i)) { 
                                value.Push((MatrixVM)firstOperand * i);
                            } else if (firstOperand is MatrixVM && secondOperand is MatrixVM && ((MatrixVM)firstOperand).Columns == ((MatrixVM)secondOperand).Rows || ((MatrixVM)secondOperand).Rows == ((MatrixVM)firstOperand).Columns) {
                                value.Push((MatrixVM)firstOperand * (MatrixVM)secondOperand);
                            }
                            break;
                    }
                } else {
                    MatrixVM matrix = matrices.FirstOrDefault((m) => m.Name == elem);
                    if (matrix != null) {
                        value.Push(matrix);
                    } else { 
                        value.Push(elem);
                    }
                }
            }

            if (value.Count == 0) {
                value.Push("Нет результата");
                return value.Pop();
            } else { 
                return value.Pop();
            }
        }

        public bool IsMatricesConsistent(MatrixVM matrix1, MatrixVM matrix2) { 
            return matrix1.Rows == matrix2.Rows && matrix1.Columns == matrix2.Columns;
        }

        public Dictionary<Operator, int> Priorities = new Dictionary<Operator, int>() {
            { Operator.Plus, 1 },
            { Operator.Minus, 1 },
            { Operator.Divide, 2 },
            { Operator.Multiply, 2 },
            { Operator.OpenBracket, 0 },
            { Operator.CloseBracket, 0 }
        }; 
    }
}

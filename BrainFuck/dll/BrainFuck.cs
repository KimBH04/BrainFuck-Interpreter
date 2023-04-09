using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrainFuck
{
    public class BrainFuckInterpreter
    {
        private byte[] Cell;
        private int Pointer;

        public BrainFuckInterpreter(int memorySize) => Reset(memorySize);

        public int CellSize => Cell.Length;

        public void Reset() => Reset(Cell.Length);

        public void Reset(int memorySize)
        {
            Pointer = 0;
            Cell = new byte[memorySize];

            Array.Clear(Cell, 0, memorySize);
        }

        public string RunCode(string code, string input = "")
        {
            int codeLen = code.Length;
            int[] matchingBrackets = new int[codeLen];

            Stack<int> brackets = new Stack<int>();
            for (int i = 0; i < codeLen; i++)
            {
                if (code[i].Equals('['))
                {
                    brackets.Push(i);
                }
                else if (code[i].Equals(']'))
                {
                    if (brackets.Count > 0)
                    {
                        if (!brackets.Peek().Equals(']'))
                        {
                            int idx = brackets.Pop();
                            matchingBrackets[i] = idx;
                            matchingBrackets[idx] = i;
                            continue;
                        }
                    }

                    brackets.Push(i);
                }
            }

            if (brackets.Count > 0)
            {
                StringBuilder exceptionMessage = new StringBuilder();

                foreach (int invalidBracketIdx in brackets.Reverse())
                {
                    string invalidBracketPointer = "";

                    for (int i = 0; i < invalidBracketIdx; i++)
                    {
                        invalidBracketPointer += " ";
                    }

                    exceptionMessage.AppendLine(
                        $"\r\nNo brackets corresponding to : {invalidBracketIdx}" +
                        $"\r\n{code}" +
                        $"\r\n{invalidBracketPointer}^~~~");
                }

                throw new NoBracketCorrespondingException(exceptionMessage.ToString());
            }

            string output = "";
            for (int i = 0; i < codeLen; i++)
            {
                switch (code[i])
                {
                    case '>':
                        MovePointer(1);
                        break;

                    case '<':
                        MovePointer(-1);
                        break;

                    case '+':
                        Cell[Pointer]++;
                        break;

                    case '-':
                        Cell[Pointer]--;
                        break;

                    case '.':
                        output += (char)Cell[Pointer];
                        break;

                    case ',':
                        if (input.Length > 0)
                        {
                            Cell[Pointer] = (byte)input[0];
                            input = input.Substring(1);
                        }
                        else
                        {
                            Cell[Pointer] = 0;
                        }
                        break;

                    case '[':
                        if (Cell[Pointer] == 0)
                        {
                            i = matchingBrackets[i];
                        }
                        break;

                    case ']':
                        if (Cell[Pointer] != 0)
                        {
                            i = matchingBrackets[i];
                        }
                        break;

                    default:
                        break;
                }
            }

            return output;
        }

        private void MovePointer(int direction)
        {
            Pointer += direction;

            if (Pointer >= Cell.Length)
            {
                Pointer = 0;
            }
            else if (Pointer < 0)
            {
                Pointer = Cell.Length - 1;
            }
        }
    }

    public class NoBracketCorrespondingException : Exception
    {
        public NoBracketCorrespondingException() { }
        public NoBracketCorrespondingException(string message) : base(message) { }
        public NoBracketCorrespondingException(string message, Exception inner) : base(message, inner) { }
    }
}

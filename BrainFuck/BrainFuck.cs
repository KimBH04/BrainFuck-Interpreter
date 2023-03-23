namespace BrainFuck
{
    using System.Text;

    public class BrainFuckInterpreter
    {
        private byte[] Cell;
        private int Pointer;

#pragma warning disable CS8618
        public BrainFuckInterpreter(int memorySize) => Reset(memorySize);
#pragma warning restore CS8618

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

            Queue<byte> inputStr = new();
            foreach (byte c in input.Select(v => (byte)v))
            {
                inputStr.Enqueue(c);
            }

            Stack<int> brackets = new();
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
                StringBuilder exceptionMessage = new();
                exceptionMessage.AppendLine();

                while (brackets.Count > 0)
                {
                    exceptionMessage.AppendLine();

                    string invalidBracketPointer = "";
                    int invalidBracketIdx = brackets.Pop();

                    for (int i = 0; i < invalidBracketIdx; i++)
                    {
                        invalidBracketPointer += " ";
                    }

                    exceptionMessage.AppendLine(
                        $"No brackets corresponding to : {invalidBracketIdx}" +
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
                        byte zero = 0;
                        Cell[Pointer] = inputStr.Count > 0 ? inputStr.Dequeue() : zero;
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

            void MovePointer(int direction)
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

            return output;
        }
    }

    public class NoBracketCorrespondingException : Exception
    {
        public NoBracketCorrespondingException() { }
        public NoBracketCorrespondingException(string massage) : base(massage) { }
        public NoBracketCorrespondingException(string massage, Exception inner) : base(massage, inner) { }
    }
}

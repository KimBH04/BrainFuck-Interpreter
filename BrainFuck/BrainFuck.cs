namespace BrainFuck
{
    public class BrainFuckInterpreter
    {
        private byte[] Memory;
        private int Pointer;
        private long InputPointer;

#pragma warning disable CS8618
        public BrainFuckInterpreter(int memorySize) => Reset(memorySize);
#pragma warning restore CS8618

        public int MemorySize => Memory.Length;

        public void Reset() => Reset(Memory.Length);

        public void Reset(int memorySize)
        {
            Pointer = 0;
            InputPointer = 0;
            Memory = new byte[memorySize];

            Array.Clear(Memory, 0, memorySize);
        }

        public string RunCode(string code, string input = "") => RunCode(code, input.ToCharArray());
        public string RunCode(string code, char[] input)
        {
            BracketsMatchCheck();
            void BracketsMatchCheck()
            {
                List<int> brackets = new();
                for (int i = 0; i < code.Length; i++)
                {
                    if (code[i].Equals('['))
                    {
                        brackets.Add(i);
                    }
                    else if (code[i].Equals(']'))
                    {
                        if (brackets.Count > 0)
                        {
                            brackets.RemoveAt(brackets.Count - 1);
                        }
                        else
                        {
                            brackets.Add(i);
                            break;
                        }
                    }
                }

                if (brackets.Count > 0)
                {
                    string invalidBracketPointer = "";
                    int invalidBracketIdx = brackets[^1];

                    for (int i = 0; i < invalidBracketIdx; i++)
                    {
                        invalidBracketPointer += " ";
                    }

                    throw new BracketsDoNotMatchException(
                        $"Brackets do not match at : {invalidBracketIdx}" +
                        $"\r\n{code}" +
                        $"\r\n{invalidBracketPointer}^~~~");
                }
            }

            string output = "";
            Interpreter(code.ToCharArray());
            void Interpreter(char[] code)
            {
                for (int i = 0; i < code.Length; i++)
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
                            Memory[Pointer] += 1;
                            break;

                        case '-':
                            Memory[Pointer] -= 1;
                            break;

                        case '.':
                            output += (char)Memory[Pointer];
                            break;

                        case ',':
                            Memory[Pointer] = InputPointer >= input.Length ? (byte)0 : (byte)input[InputPointer++];
                            break;

                        case '[':
                            string repeatition = "";
                            int repeatCnt = 0;

                            foreach (var c in code[(i + 1)..])
                            {
                                repeatition += c;

                                if (c.Equals('['))
                                    repeatCnt++;
                                else if (c.Equals(']'))
                                {
                                    if (repeatCnt == 0)
                                    {
                                        Interpreter(repeatition.ToCharArray());
                                        break;
                                    }
                                    else
                                        repeatCnt--;
                                }

                                i++;
                            }
                            break;

                        case ']':
                            if (Memory[Pointer] != 0)
                                i = -1;
                            break;

                        default:
                            break;
                    }
                }

                void MovePointer(int direction)
                {
                    Pointer += direction;

                    if (Pointer >= Memory.Length)
                    {
                        Pointer = 0;
                    }
                    else if (Pointer < 0)
                    {
                        Pointer = Memory.Length - 1;
                    }
                }
            }

            return output;
        }
    }

    public class BracketsDoNotMatchException : Exception
    {
        public BracketsDoNotMatchException()
        {

        }

        public BracketsDoNotMatchException(string massage)
            : base(massage)
        {

        }

        public BracketsDoNotMatchException(string massage, Exception inner)
            : base(massage, inner)
        {

        }
    }
}

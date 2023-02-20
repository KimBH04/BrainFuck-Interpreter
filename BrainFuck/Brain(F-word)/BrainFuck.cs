namespace BrainFuck
{
    public class BrainFuckInterpreter
    {
        private byte[] Memory;
        private int Pointer;
        private long InputPointer;

        public BrainFuckInterpreter(long memorySize)
        {
            Reset(memorySize);
        }

        public void Reset()
        {
            Reset(Memory.Length);
        }

        public void Reset(long memorySize)
        {
            Pointer = 0;
            InputPointer = 0;
            Memory = new byte[memorySize];

            for (int i = 0; i < Memory.Length; i++)
            {
                Memory[i] = 0;
            }
        }

        public string RunCode(string code, string input = "")
        {
            string output = "";

            int invalidBracketIdx = 0;
            int brackets = 0;
            for (int i = 0; i < code.Length; i++)
            {
                if (code[i].Equals('['))
                {
                    invalidBracketIdx = i;
                    brackets++;
                }
                else if (code[i].Equals(']'))
                    brackets--;

                if (brackets < 0)
                {
                    invalidBracketIdx = i;
                    break;
                }
            }

            if (brackets != 0)
            {
                string invalidBracketPointer = "";

                for (int i = 0; i < invalidBracketIdx; i++)
                {
                    invalidBracketPointer += " ";
                }

                throw new BracketsDoNotMatchException(
                    $"Brackets do not match at : {invalidBracketIdx}" +
                    $"\r\n{code}" +
                    $"\r\n{invalidBracketPointer}^~~~");
            }

            Interpreter(input.ToCharArray(), code.ToCharArray());

            void Interpreter(char[] input, char[] code)
            {
                for (int i = 0; i < code.Length; i++)
                {
                    switch (code[i])
                    {
                        case '>':
                            MoveRight();
                            break;

                        case '<':
                            MoveLeft();
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
                                        Interpreter(input, repeatition.ToCharArray());
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

                void MoveRight()
                {
                    if (++Pointer >= Memory.Length)
                    {
                        Pointer = 0;
                    }
                }

                void MoveLeft()
                {
                    if (--Pointer < 0)
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
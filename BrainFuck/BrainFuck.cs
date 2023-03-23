//So many bugs
//Real fukking my brain lol

namespace BrainFuck
{
    public class BrainFuckInterpreter
    {
        private byte[] Memory;
        private int Pointer;

#pragma warning disable CS8618
        public BrainFuckInterpreter(int memorySize) => Reset(memorySize);
#pragma warning restore CS8618

        public int MemorySize => Memory.Length;

        public void Reset() => Reset(Memory.Length);

        public void Reset(int memorySize)
        {
            Pointer = 0;
            Memory = new byte[memorySize];

            Array.Clear(Memory, 0, memorySize);
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
                        int idx = brackets.Pop();
                        matchingBrackets[i] = idx;
                        matchingBrackets[idx] = i;
                    }
                    else
                    {
                        brackets.Push(i);
                        break;
                    }
                }
            }

            if (brackets.Count > 0)
            {
                string invalidBracketPointer = "";
                int invalidBracketIdx = brackets.Pop();

                for (int i = 0; i < invalidBracketIdx; i++)
                {
                    invalidBracketPointer += " ";
                }

                throw new NoBracketCorrespondingException(
                    $"No brackets corresponding to : {invalidBracketIdx}" +
                    $"\r\n{code}" +
                    $"\r\n{invalidBracketPointer}^~~~");
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
                        Memory[Pointer]++;
                        break;

                    case '-':
                        Memory[Pointer]--;
                        break;

                    case '.':
                        output += (char)Memory[Pointer];
                        break;

                    case ',':
                        Memory[Pointer] = inputStr.Dequeue();
                        break;

                    case '[':
                        if (Memory[Pointer] == 0)
                        {
                            i = matchingBrackets[i];
                        }
                        break;

                    case ']':
                        if (Memory[Pointer] != 0)
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

                if (Pointer >= Memory.Length)
                {
                    Pointer = 0;
                }
                else if (Pointer < 0)
                {
                    Pointer = Memory.Length - 1;
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

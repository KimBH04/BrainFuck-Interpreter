using BrainFuck;

var memory = int.Parse(Console.ReadLine()!);
var brainFuck = new BrainFuckInterpreter(memory);


//Hello, World!
var a = brainFuck.RunCode("++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++.>+.+++++++..+++.>++++++++++++++.------------.<<+++++++++++++++.>.+++.------.--------.>+.");
Console.WriteLine(a);


//reset and run again
brainFuck.Reset();
var b = brainFuck.RunCode("++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++.>+.+++++++..+++.>++++++++++++++.------------.<<+++++++++++++++.>.+++.------.--------.>+.");
Console.WriteLine(b);


//Brackets do not match
try
{
    Console.WriteLine();
    brainFuck.Reset();
    brainFuck.RunCode("++++++++++>+++++++>++++++++++>+++>+<<<<-]>++.>"/* skip */);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine();
}


//input
brainFuck.Reset();
var c = brainFuck.RunCode("+[,.]>+++++++[>+++++++++<-]>..<+++++[>------<-]>.", "Who are you");
Console.WriteLine(c); //output : Who are you??!
//If the number of characters to import is greater than the length of the input string,
//all subsequent portions will be null (represented as 0 in bytes).


//resizing memory 
brainFuck.Reset();
var d = brainFuck.RunCode(",.>>>>.", "A");
Console.WriteLine(d);

brainFuck.Reset(4);
d = brainFuck.RunCode(",.>>>>.", "A"); //memory overflow
Console.WriteLine(d);

using System;

public class HiddenText
{
    public static string readLineHidden()
    {
        bool cont = true;
        Stack<string> stack = new Stack<string>();
        while (cont)
        {
            ConsoleKeyInfo cki = Console.ReadKey(true);
            ConsoleKey ck = cki.Key;
            if (ck == ConsoleKey.Enter)
            {
                cont = false;
                if (stack.Count > 0)
                {
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    Console.Write("*");
                }
                Console.WriteLine("");
            }
            else if (ck == ConsoleKey.Backspace)
            {
                if (stack.Count == 0)
                {
                    continue;
                }
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                Console.Write(" ");
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                stack.Pop();
            }
            else
            {
                char input = cki.KeyChar;
                if (input < 32)
                {
                    continue;
                }
                if (stack.Count > 0)
                {
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    Console.Write("*");
                }
                Console.Write(input);
                stack.Push(cki.KeyChar.ToString());
            }
        }
        string[] arr = stack.ToArray();
        Array.Reverse(arr);
        return string.Join(string.Empty, arr);
    }
}

using System;

public static class FactoryScriptAPI
{
    // This function will be exposed to Lua as fs.print()
    public static void print(string text)
    {
        // Redirect the text to the in-game console
        ConsoleManager.Print(text);
    }
}

using System;

namespace Nodes_of_Yesod;

public static class Program
{
    [STAThread]
    private static void Main()
    {
        using (var game = new Yesod())
        {
            game.Run();
        }
    }
}
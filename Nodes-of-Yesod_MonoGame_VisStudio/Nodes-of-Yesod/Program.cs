using System;

namespace Nodes_of_Yesod
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Yesod())
                game.Run();
        }
    }
}

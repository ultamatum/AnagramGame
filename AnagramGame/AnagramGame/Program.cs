using System;

namespace PaisleyRangers
{
#if WINDOWS || LINUX
    
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new PaisleyRangers())
                game.Run();
        }
    }
#endif
}

using System;

namespace CNAGraphicalApplication
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new SlimeGame())
                game.Run();
        }
    }
}

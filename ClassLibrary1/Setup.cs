using Microsoft.ML;
using ML_net;
using ML_net.ModelSession_1;
using ML_net.ModelSession_2;
using ML_ASP.Utility;

namespace Setup
{
    public class Setup
    {
        public static void Main(string[] args)
        {
            //ML_net.ModelSession_1.Demo.Execute();

            Console.WriteLine("Press the spacebar to execute ML_net.ModelSession_3.Demo.Execute();");

            // Subscribe to the KeyPress event
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key == ConsoleKey.Spacebar)
                {
                    ML_net.ModelSession_3.Demo.Execute();

                    Console.WriteLine("Press the Enter to repeat and Space to execute ML_net.ModelSession_3.Demo.Execute();");
                }
            }
        }
    }
}
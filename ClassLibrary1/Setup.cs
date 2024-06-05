using Microsoft.ML;
using ML_net;
using ML_net.ModelSession_1;
using ML_net.ModelSession_2;
using ML_ASP.Utility;

namespace Setup
{
    public class Setup
    {
        public static async Task Main(string[] args)
        {
            //ML_net.ModelSession_1.Demo.Execute();
            
            await ML_net.ModelSession_3.Demo.Execute();
        }
    }
}
using Microsoft.ML.Data;

namespace ML_net.ModelSession_1
{
    public class Performance_Prediction
    {
        [ColumnName("Score")]
        public float PerformancePrediciton_Score { get; set; }
    }
}
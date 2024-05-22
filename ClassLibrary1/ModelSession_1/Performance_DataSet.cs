using Microsoft.ML.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace ML_net.ModelSession_1
{
    public class Performance_DataSet
    {
        [LoadColumn(0)]
        public float TrainingSequence { get; set; }
        [LoadColumn(1), ColumnName("Label")]
        public float CompletionTime { get; set; }
        [LoadColumn(2)]
        public float PerformanceScore { get; set; }
        [LoadColumn(3)]
        public float FeedbackScore { get; set; }
        [LoadColumn(4)]
        public float EmployeeID { get; set; }
    }
}

using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_net.ModelSession_2
{
    public class Object_DataSet
    {
        [LoadColumn(0), ColumnName("Grade")]
        public float NumberOfWords { get; set; }
        [LoadColumn(1), ColumnName("Label")]
        public float Grade { get; set; }
    }
}

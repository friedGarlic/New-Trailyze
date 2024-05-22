using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_net.ModelSession_2
{
    public class Prediction
    {
        [ColumnName("Score")]
        public float Prediciton { get; set; }
    }
}

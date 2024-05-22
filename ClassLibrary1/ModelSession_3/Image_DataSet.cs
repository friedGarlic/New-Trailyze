using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_net.ModelSession_3
{
    public class Image_DataSet
    {
        [LoadColumn(0)]
        public string? ImagePath;

        [LoadColumn(1)]
        [ColumnName("Label")]
        public string? Label;
    }
}

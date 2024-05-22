using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_net.ModelSession_1
{
    public class Demo
    {
        public static void Execute()
        {
            var context = new MLContext();

            //load data
            var trainData = context.Data.LoadFromTextFile<Performance_DataSet>("./ModelSession_1/StudentDataSet.csv",
                hasHeader: true, separatorChar: ',');

            var testTrainSplit = context.Data.TrainTestSplit(trainData, testFraction: 0.1);

            var pipeline = context.Transforms.Concatenate("Features", "PerformanceScore")
                .Append(context.Regression.Trainers.LbfgsPoissonRegression());

            var model = pipeline.Fit(testTrainSplit.TrainSet);

            var predictions = model.Transform(testTrainSplit.TrainSet);

            var metrics = context.Regression.Evaluate(predictions);

            var newdata = new Performance_DataSet
            {
                CompletionTime = 30,
                PerformanceScore = 86,
                FeedbackScore = 3.2f,
                EmployeeID = 1
            };

            //predict engine
            var predictFunc = context.Model.CreatePredictionEngine<Performance_DataSet, Performance_Prediction>(model);

            var prediction = predictFunc.Predict(newdata);

            var modelPath = "ModelSession_1/PerformancePrediction.zip";

            context.Model.Save(model, trainData.Schema, modelPath);
        }
    }
}

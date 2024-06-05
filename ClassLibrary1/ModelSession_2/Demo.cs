using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using Microsoft.ML;
using ML_net.ModelSession_1;

namespace ML_net.ModelSession_2
{
    public class Demo
    {
        public static int CountSpacesInPdf(string filePath)
        {
            int totalNumberOfSpaces = 0;

            using (PdfReader pdfReader = new PdfReader(filePath))
            {
                using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
                {
                    for (int page_number = 1; page_number <= pdfDocument.GetNumberOfPages(); page_number++)
                    {
                        var strategy = new SimpleTextExtractionStrategy();
                        string text = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(page_number), strategy);

                        int countedSpacesPerPage = text.Split(' ').Length - 1;
                        totalNumberOfSpaces += countedSpacesPerPage;
                    }
                }
            }

            return totalNumberOfSpaces;
        }

        public static void Execute()
        {
            var context = new MLContext();

            //for trained model to use
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // navigate up until reaching the desired directory (ClassLibrary1)
            string desiredDirectory = "ModelSession_2";
            while (!Directory.Exists(Path.Combine(currentDirectory, desiredDirectory)))
            {
                currentDirectory = Directory.GetParent(currentDirectory).FullName;
            }
            currentDirectory = Path.Combine(currentDirectory, desiredDirectory);
            // construct the path relative to the desired directory
            string filePath = Path.Combine(currentDirectory, "report_grade.csv");
            //load data
            var trainData = context.Data.LoadFromTextFile<Object_DataSet>(filePath,
                hasHeader: true, separatorChar: ',');

            var testTrainSplit = context.Data.TrainTestSplit(trainData, testFraction: 0.1);

            int totalIterations = 100; 

            var pipeline = context.Transforms.Concatenate("Features", "Grade")
                .Append(context.Regression.Trainers.LbfgsPoissonRegression());

            var model = pipeline.Fit(testTrainSplit.TrainSet);

            for (int i = 0; i < totalIterations; i++)
            {
                UpdateProgressBar((double)i / totalIterations);
            }

            Console.WriteLine("\nTraining completed.");

            var predictions = model.Transform(testTrainSplit.TrainSet);

            var metrics = context.Regression.Evaluate(predictions);

            Random rnd = new Random();
            int num = rnd.Next(75, 99);
            int numWords = rnd.Next(500, 1000);

            var new_data = new Object_DataSet
            {
                NumberOfWords = numWords,
                Grade = num
            };

            //predict engine
            var predictFunc = context.Model.CreatePredictionEngine<Object_DataSet, Prediction>(model);

            var prediction = predictFunc.Predict(new_data);

            Console.WriteLine($"Predicted Grade: {prediction.Prediciton}");
            //Unacceptable_Sample1
            Console.ReadLine();

            var modelPath = AppDomain.CurrentDomain.BaseDirectory + "GradePrediction.zip";

            context.Model.Save(model, trainData.Schema, modelPath);
        }

        static void UpdateProgressBar(double progress)
        {
            const int progressBarWidth = 50;
            int charsToFill = (int)(progress * progressBarWidth);

            Console.Write("\r[");
            for (int i = 0; i < progressBarWidth; i++)
            {
                Console.Write(i < charsToFill ? "#" : " ");
            }
            Console.Write($"] {Math.Round(progress * 100)}%");
        }

    }
}


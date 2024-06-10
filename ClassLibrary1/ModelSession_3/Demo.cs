using Microsoft.ML.Data;
using Microsoft.ML;
using iText.IO.Image;
using Tensorflow.Contexts;


namespace ML_net.ModelSession_3
{
    public struct InceptionSettings
    {
        public const int ImageHeight = 224;
        public const int ImageWidth = 224;
        public const float Mean = 117;
        public const float Scale = 1;
        public const bool ChannelsLast = true;
    }

    public class Demo
    {
        public static string GetAssetsPath(string relativePath)
        {
            // Get the physical path of the directory containing the web application
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Combine the application directory with the relative path to the assets directory
            string assetsPath = Path.Combine(appDirectory, relativePath);

            return assetsPath;
        }

        public static ITransformer GenerateModel(MLContext mlContext)
        {
            //for trained model to use
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string _assetsPath = GetAssetsPath("assets");

            //string _imagesFolder = Path.Combine(_assetsPath, "images");
            string _imagesFolder = Path.Combine(_assetsPath, "samples");
            string _trainTagsTsv = Path.Combine(_imagesFolder, "tags.tsv");
            string _testTagsTsv = Path.Combine(_imagesFolder, "test-tags.tsv");
            string _inceptionTensorFlowModel = Path.Combine(_assetsPath, "inception", "tensorflow_inception_graph.pb");

            IEstimator<ITransformer> pipeline = mlContext.Transforms.LoadImages(outputColumnName: "input",
                                                                                imageFolder: _imagesFolder,
                                                                                inputColumnName: nameof(Image_DataSet.ImagePath))
                .Append(mlContext.Transforms.ResizeImages(outputColumnName: "input",
                                                          imageWidth: InceptionSettings.ImageWidth,
                                                          imageHeight: InceptionSettings.ImageHeight,
                                                          inputColumnName: "input"))
                .Append(mlContext.Transforms.ExtractPixels(outputColumnName: "input",
                                                           interleavePixelColors: InceptionSettings.ChannelsLast,
                                                           offsetImage: InceptionSettings.Mean))
                .Append(mlContext.Model.LoadTensorFlowModel(_inceptionTensorFlowModel).ScoreTensorFlowModel(outputColumnNames: new[]
                                                            { "softmax2_pre_activation" },
                                                            inputColumnNames: new[] { "input" },
                                                            addBatchDimensionInput: true))
                .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "LabelKey", inputColumnName: "Label"))
                .Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "LabelKey", featureColumnName: "softmax2_pre_activation"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabelValue", "PredictedLabel"))
                .AppendCacheCheckpoint(mlContext);

            IDataView trainingData = mlContext.Data.LoadFromTextFile<Image_DataSet>(path: _trainTagsTsv, hasHeader: false);

            ITransformer model = pipeline.Fit(trainingData);

            IDataView testData = mlContext.Data.LoadFromTextFile<Image_DataSet>(path: _testTagsTsv, hasHeader: false);
            IDataView predictions = model.Transform(testData);

            // Create an IEnumerable for the predictions for displaying results
            IEnumerable<ImagePrediction> imagePredictionData = mlContext.Data.CreateEnumerable<ImagePrediction>(predictions, true);
            DisplayResults(imagePredictionData);

            MulticlassClassificationMetrics metrics =
                mlContext.MulticlassClassification.Evaluate(predictions,
                    labelColumnName: "LabelKey",
                    predictedLabelColumnName: "PredictedLabel");

            Console.WriteLine($"LogLoss is: {metrics.LogLoss}");
            Console.WriteLine($"PerClassLogLoss is: {String.Join(" , ", metrics.PerClassLogLoss.Select(c => c.ToString()))}");

            string modelPath = Path.Combine(currentDirectory, "ImageClassification.zip");
            mlContext.Model.Save(model, null, modelPath);
            return model;
        }

        public static void ClassifySingleImage(MLContext mlContext, ITransformer model)
        {
            //for trained model to use
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // navigate up until reaching the desired directory (ClassLibrary1)
            string desiredDirectory = "ModelSession_3";
            while (!Directory.Exists(Path.Combine(currentDirectory, desiredDirectory)))
            {
                currentDirectory = Directory.GetParent(currentDirectory).FullName;
            }
            currentDirectory = Path.Combine(currentDirectory, desiredDirectory);
            // construct the path relative to the desired directory
            string _assetsPath = Path.Combine(currentDirectory, "assets");
            //string _imagesFolder = Path.Combine(_assetsPath, "images");
            string _imagesFolder = Path.Combine(_assetsPath, "samples");

            string _predictSingleImage = Path.Combine(_imagesFolder, "toaster.jpg");

            var imageData = new Image_DataSet()
            {
                ImagePath = _predictSingleImage
            };

            // Make prediction function (input = Image_DataSet, output = ImagePrediction)
            var predictor = mlContext.Model.CreatePredictionEngine<Image_DataSet, ImagePrediction>(model);
            var prediction = predictor.Predict(imageData);


            string modelPath = Path.Combine(currentDirectory, "ImageClassification.zip");
            mlContext.Model.Save(model, null, modelPath);

            Console.WriteLine($"Image: {Path.GetFileName(imageData.ImagePath)} " +
                $"predicted as: {prediction.PredictedLabelValue} " +
                $"with score: {prediction.Score?.Max()} ");

        }

        public static void DisplayResults(IEnumerable<ImagePrediction> imagePredictionData)
        {
            foreach (ImagePrediction prediction in imagePredictionData)
            {
                Console.WriteLine($"Image: {Path.GetFileName(prediction.ImagePath)} " +
                    $"predicted as: {prediction.PredictedLabelValue} with score: {prediction.Score?.Max()} ");
            }

        }
        public static void Execute()
        {
            MLContext mlContext = new MLContext();

            var a = GenerateModel(mlContext);
            ClassifySingleImage(mlContext, a);
        }
    }
}
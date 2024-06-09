using Microsoft.ML.Data;
using Microsoft.ML;
using iText.IO.Image;
using System.Security.Cryptography.X509Certificates;


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
			var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

			string uploadPath = "models";
			if (!Directory.Exists(uploadPath))
			{
				Directory.CreateDirectory(uploadPath);
			}

			string combinePath2 = Path.Combine(currentDirectory, uploadPath);

			return combinePath2;
        }

        public static async Task<ITransformer> GenerateModelAsync(MLContext mlContext)
        {
			//TODO change to azure file share so it can be trained using web app

			string downloadFolder = Path.Combine(Path.GetTempPath(), "MLAssets");

			Directory.CreateDirectory(downloadFolder);

			string connectionString = "DefaultEndpointsProtocol=https;AccountName=trailyzestorage1;AccountKey=1CWwbsb1L8VGeuc+rMXOLf7U8kHJz2cYcxGXZITGQyRBgi7ML/4iUR4qzxYCq+NxQo9lf45YD6or+AStOP4c8Q==;EndpointSuffix=core.windows.net";
			string shareName = "trailyzestorage";
			string zipFilePath = "ImageClassification.zip";

			await FileShareService.DownloadFileFromShareAsync(shareName, "model/samples/tags.tsv", Path.Combine(downloadFolder, "tags.tsv"), connectionString);
			await FileShareService.DownloadFileFromShareAsync(shareName, "model/samples/test-tags.tsv", Path.Combine(downloadFolder, "test-tags.tsv"), connectionString);
			await FileShareService.DownloadFileFromShareAsync(shareName, "model/inception/tensorflow_inception_graph.pb", Path.Combine(downloadFolder, "tensorflow_inception_graph.pb"), connectionString);
			
            await FileShareService.DownloadAllFilesFromDirectoryAsync(shareName, "model/samples", Path.Combine(downloadFolder), connectionString);


			//string _imagesFolder = Path.Combine(_assetsPath, "images");
			string _trainTagsTsv = Path.Combine(downloadFolder, "tags.tsv");
			string _testTagsTsv = Path.Combine(downloadFolder, "test-tags.tsv");
			string _inceptionTensorFlowModel = Path.Combine(downloadFolder, "tensorflow_inception_graph.pb");


			IEstimator<ITransformer> pipeline = mlContext.Transforms.LoadImages(outputColumnName: "input",
                                                                                imageFolder: downloadFolder,
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

            string modelPath = Path.Combine(downloadFolder, "ImageClassification.zip");
            mlContext.Model.Save(model, null, modelPath);

			// Upload the model to Azure File Share
			await FileShareService.UploadFileAsync(modelPath, "ImageClassification.zip");

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
        public static async Task Execute()
        {
            MLContext mlContext = new MLContext();

			ITransformer model = await GenerateModelAsync(mlContext);
		}
    }
}

using Azure.Storage.Files.Shares;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Files.Shares.Models;
using System.Security.Cryptography.X509Certificates;

namespace ML_net
{
	public static class FileShareService
	{
		private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=trailyzestorage1;AccountKey=ImBu3GwcZQjzw41ZvDHXppfP+CAkI3XxPQ9Rvg1bnqr3VApqk5TnbcubukmZH3xdd8LqDA5njer++ASt/XriOg==;EndpointSuffix=core.windows.net";
		private static readonly string _shareName = "trailyzestorage1";
		private const int MaxChunkSize = 4 * 1024 * 1024; // 4 MB

		public static async Task<string> UploadFileAsync(string filePath, string fileName)
		{
			ShareClient shareClient = new ShareClient(_connectionString, _shareName);

			await shareClient.CreateIfNotExistsAsync();

			ShareDirectoryClient rootDir = shareClient.GetRootDirectoryClient();
			ShareFileClient fileClient = rootDir.GetFileClient(fileName);

			// Get file length
			long fileLength = new FileInfo(filePath).Length;

			// Create the file in Azure File Share
			await fileClient.CreateAsync(fileLength);

			// Upload file in chunks
			using (FileStream stream = File.OpenRead(filePath))
			{
				byte[] buffer = new byte[MaxChunkSize];
				long offset = 0;
				int bytesRead;

				while ((bytesRead = await stream.ReadAsync(buffer, 0, MaxChunkSize)) > 0)
				{
					using (MemoryStream chunkStream = new MemoryStream(buffer, 0, bytesRead))
					{
						await fileClient.UploadRangeAsync(new HttpRange(offset, bytesRead), chunkStream);
					}
					offset += bytesRead;
				}
			}

			return fileClient.Uri.ToString();
		}


		public static async Task<string> UploadImageAsync(string filePath, string fileName)
		{
			ShareClient shareClient = new ShareClient(_connectionString, _shareName);
			await shareClient.CreateIfNotExistsAsync();

			ShareDirectoryClient modelDir = shareClient.GetDirectoryClient("model");
			ShareDirectoryClient samplesDir = modelDir.GetSubdirectoryClient("samples");

			ShareFileClient fileClient = samplesDir.GetFileClient(fileName);

			// Upload the image file
			using (FileStream stream = File.OpenRead(filePath))
			{
				await fileClient.CreateAsync(stream.Length);
				await fileClient.UploadAsync(stream);
			}

			return fileClient.Uri.ToString();
		}

		public static async Task DownloadModelFileAsync(string connectionString, string shareName, string fileName)
		{
			ShareClient shareClient = new ShareClient(connectionString, shareName);
			ShareDirectoryClient rootDir = shareClient.GetRootDirectoryClient();
			ShareFileClient fileClient = rootDir.GetFileClient(fileName);

			// Download the file to the specified path
			await fileClient.DownloadAsync();
		}

		public static async Task DownloadFileFromShareAsync(string shareName, string filePath, string downloadPath, string connectionString)
		{
			ShareClient shareClient = new ShareClient(connectionString, shareName);
			ShareDirectoryClient directoryClient = shareClient.GetRootDirectoryClient();
			ShareFileClient fileClient = directoryClient.GetFileClient(filePath);

			ShareFileDownloadInfo download = await fileClient.DownloadAsync();
			using (FileStream stream = File.OpenWrite(downloadPath))
			{
				await download.Content.CopyToAsync(stream);
				stream.Close();
			}
		}

		public static async Task DownloadAllFilesFromDirectoryAsync(string shareName, string directoryPath, string localDirectoryPath, string connectionString)
		{
			try
			{
				ShareClient shareClient = new ShareClient(connectionString, shareName);
				ShareDirectoryClient directoryClient = shareClient.GetDirectoryClient(directoryPath);
				await foreach (ShareFileItem fileItem in directoryClient.GetFilesAndDirectoriesAsync())
				{
					if (fileItem.IsDirectory)
					{
						// Handle directories if needed
						continue;
					}

					string fileName = fileItem.Name;
					string localFilePath = Path.Combine(localDirectoryPath, fileName);
					await DownloadFileFromShareAsync(shareName, Path.Combine(directoryPath, fileName), localFilePath, connectionString);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error downloading files from Azure File Share directory: {ex.Message}");
				throw;
			}
		}
	}
}

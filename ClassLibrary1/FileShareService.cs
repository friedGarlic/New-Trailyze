using Azure.Storage.Files.Shares;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Files.Shares.Models;

namespace ML_net
{
	public static class FileShareService
	{
		private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=trailyzestorage1;AccountKey=1CWwbsb1L8VGeuc+rMXOLf7U8kHJz2cYcxGXZITGQyRBgi7ML/4iUR4qzxYCq+NxQo9lf45YD6or+AStOP4c8Q==;EndpointSuffix=core.windows.net;";
		private static readonly string _shareName = "trailyzestorage";
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

		public static async Task DownloadModelFileAsync(string connectionString, string shareName, string fileName, string downloadPath)
		{
			ShareClient shareClient = new ShareClient(connectionString, shareName);
			ShareDirectoryClient rootDir = shareClient.GetRootDirectoryClient();
			ShareFileClient fileClient = rootDir.GetFileClient(fileName);

			// Download the file to the specified path
			await fileClient.DownloadAsync();
		}
	}
}

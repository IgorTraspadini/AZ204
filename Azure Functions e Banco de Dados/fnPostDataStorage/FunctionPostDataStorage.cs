using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace fnPostDataStorage
{
    public class FunctionPostDataStorage
    {
        private readonly ILogger<FunctionPostDataStorage> _logger;

        public FunctionPostDataStorage(ILogger<FunctionPostDataStorage> logger)
        {
            _logger = logger;
        }

        [Function("dataStorage")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("Processando a Imagem no Storage...");

            if (!req.Headers.TryGetValue("file-type", out var fileTypeHeader))
            {
                return new BadRequestObjectResult("O cabe�alho 'file-type' � obrigat�rio");
            }

            var fileType = fileTypeHeader.ToString();
            var form = await req.ReadFormAsync();
            var file = req.Form.Files["file"];

            if (file == null || file.Length == 0)
            {
                return new BadRequestObjectResult("O arquivo n�o foi enviado");
            }

            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string containerName = fileType;
            BlobClient blobClient = new BlobClient(connectionString, containerName, file.FileName);
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);

            await containerClient.CreateIfNotExistsAsync();
            await containerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            string blobName = file.FileName;
            var blob = containerClient.GetBlobClient(blobName);

            using (var stream = file.OpenReadStream())
            {
                await blob.UploadAsync(stream, true);
            }

            _logger.LogInformation($"Arquivo {file.FileName} armazenado com sucesso");

            return new OkObjectResult(new
            {
                Message = "Arquivo armazenado com sucesso",
                BlobUri = blob.Uri
            });

        }
    }
}

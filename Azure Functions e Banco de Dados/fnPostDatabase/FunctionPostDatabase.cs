using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace fnPostDatabase
{
    public class FunctionPostDatabase
    {
        private readonly ILogger<FunctionPostDatabase> _logger;

        public FunctionPostDatabase(ILogger<FunctionPostDatabase> logger)
        {
            _logger = logger;
        }

        [Function("movie")]
        [CosmosDBOutput("%DatabaseName%", "movies", Connection = "CosmoDBConnection", CreateIfNotExists = true, PartitionKey = "id")]
        public async Task<object?> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            MovieRequest movie = null;

            var content = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                movie = JsonConvert.DeserializeObject<MovieRequest>(content);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Erro ao deserializar o objeto" + ex.Message);
            }

            return JsonConvert.SerializeObject(movie);
        }
    }
}
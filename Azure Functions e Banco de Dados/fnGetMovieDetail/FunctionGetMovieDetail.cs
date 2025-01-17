using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace fnGetMovieDetail
{
    public class FunctionGetMovieDetail
    {
        private readonly ILogger<FunctionGetMovieDetail> _logger;
        private readonly CosmosClient _cosmosClient;

        public FunctionGetMovieDetail(ILogger<FunctionGetMovieDetail> logger, CosmosClient cosmosClient)
        {
            _logger = logger;
            _cosmosClient = cosmosClient;
        }

        [Function("detailmovie")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var container = _cosmosClient.GetContainer("ICT_Dio_Project", "movies");
            var id = req.Query["id"];
            var query = $"SELECT * FROM c WHERE c.id = @id";
            var queryDefinition = new QueryDefinition(query).WithParameter("@id", id);
            var result = container.GetItemQueryIterator<MovieResult>(queryDefinition);
            var results = new List<MovieResult>();
            while (result.HasMoreResults)
            {
                foreach (var item in await result.ReadNextAsync())
                {
                    results.Add(item);
                }
            }
            var responseMessage = req.CreateResponse(HttpStatusCode.OK);
            await responseMessage.WriteAsJsonAsync(results.FirstOrDefault());

            return responseMessage;
        }
    }
}

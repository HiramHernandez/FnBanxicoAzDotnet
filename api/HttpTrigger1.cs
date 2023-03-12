using System;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using Company.Function;

namespace Company.Function
{
    public static class HttpTrigger1
    {
        [FunctionName("HttpTrigger1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            /*log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);*/
            string initialDate = req.Query.ContainsKey("initialDate") == true ? req.Query["initialDate"] : (DateTime.UtcNow.ToString("yyyy-MM-dd"));
            string finishDate = req.Query.ContainsKey("finishDate") == true ? req.Query["finishDate"] : (DateTime.UtcNow.ToString("yyyy-MM-dd"));
            
        
            //string date = DateTime.UtcNow.ToString("yyyy-MM-dd");
            BanxicoService banxicoService = new BanxicoService();
            var responseFixing = await banxicoService.GetFixing();
            var responseExhangeRates = await banxicoService.GetExchangeRate(initialDate, finishDate);
            
            //List<string> lista = new List<string>{"ABC", "DEF", "GHI", "JKL"};
            var response = new { fixing = responseFixing.Value, date = DateTime.UtcNow.ToString("yyyy-MM-dd"), exchangesRate = responseExhangeRates};      
            
            return new OkObjectResult(response);
        }

        public static async Task<OkObjectResult> DecrepatedFunction(HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

    }
}

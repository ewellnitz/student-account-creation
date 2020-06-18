using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Student.AccountCreation
{
    public static class StudentFunctions
    {
        private static IConfiguration GetConfiguration(ExecutionContext context)
        {
            //build configuration
            var config = new ConfigurationBuilder().SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables().Build();
            return config;
        }

        private static HttpClient CreateHttpClient(IConfiguration configuration)
        {
            string userName = configuration["User"];
            string password = configuration["Password"];

            //create http client
            string authHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{password}"));
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
            return httpClient;
        }

        [FunctionName("GetStudents")]
        public static async Task<IActionResult> GetStudents([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            //build configuration
            var configuraiton = GetConfiguration(context);
            string rootUri = configuraiton["CampusNexusUrl"];

            //create http client
            using (var httpClient = CreateHttpClient(configuraiton))
            {
                string url = $"{rootUri}ds/odata/Students?$select=Id,StudentNumber,FirstName,LastName,DateOfBirth,PhoneNumber,SchoolStatusId&$count=true";
                var top = (string)req.Query["top"] ?? "10";
                url += $"&$top={top}";
                if ((string)req.Query["skip"] != null)
                    url += $"&$skip={req.Query["skip"]}";

                log.LogInformation($"Query={url}");

                //invoke HTTP Get
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                return new OkObjectResult(await response.Content.ReadAsStringAsync());
            }
        }

        [FunctionName("UpdateEmail")]
        public static async Task<IActionResult> UpdateEmail([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
           ILogger log, ExecutionContext context)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic requestData = JsonConvert.DeserializeObject(requestBody);
            
            //validate post data
            string email = requestData?.email;
            int id = requestData?.id;
            if (string.IsNullOrEmpty(email) || id == 0)
                return new BadRequestResult();

            //build configuration
            var configuraiton = GetConfiguration(context);
            string rootUri = configuraiton["CampusNexusUrl"];

            //create http client
            using (var httpClient = CreateHttpClient(configuraiton))
            {
                string url = $"{rootUri}/api/commands/Common/Student";
                var commandModelClient = new CommandModelClient(httpClient, url, log);

                //retrieve
                log.LogInformation($"Retrieving student {id}");
                JObject result = await commandModelClient.RetrieveAsync(id);

                //update
                log.LogInformation($"Updating student {id} email '{email}'");
                var data = (JObject)result.SelectToken("payload.data");
                data["studentAddressAssociation"] = 2;
                data["emailAddress"] = email;
                result = await commandModelClient.SaveAsync(data);

                return new OkObjectResult(requestData);
            }
        }
    }
}

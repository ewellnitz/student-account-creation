using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Student.AccountCreation
{
    public class CommandModelClient
    {
        private readonly HttpClient _client;
        private readonly string _entityUrl;
        private readonly ILogger _logger;

        public CommandModelClient(HttpClient client, string entityUrl, ILogger logger)
        {
            _logger = logger;
            _client = client;
            _entityUrl = entityUrl.TrimEnd('/');
        }

        public async Task<JObject> ExecuteCommandAsync(string command, JObject value)
        {
            var response = await _client.PostAsync($"{_entityUrl}/{command}", new StringContent(value.ToString(), Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(content);
                response.EnsureSuccessStatusCode();
            }
            return JsonConvert.DeserializeObject<JObject>(content);
        }

        public async Task<JObject> CreateAsync()
        {
            return await ExecuteCommandAsync("create", new JObject());
        }

        public async Task<JObject> DeleteAsync(JObject entity)
        {
            return await ExecuteCommandAsync("delete", new JObject { ["payload"] = entity });
        }

        public async Task<JObject> SaveNewAsync(JObject entity)
        {
            return await ExecuteCommandAsync("saveNew", new JObject { ["payload"] = entity });
        }

        public async Task<JObject> SaveAsync(JObject entity)
        {
            return await ExecuteCommandAsync("save", new JObject { ["payload"] = entity });
        }

        public async Task<JObject> RetrieveAsync(int idValue)
        {
            var body = JObject.FromObject(new
            {
                payload = new
                {
                    id = idValue
                }
            });
            return await ExecuteCommandAsync("get", body);
        }
    }
}

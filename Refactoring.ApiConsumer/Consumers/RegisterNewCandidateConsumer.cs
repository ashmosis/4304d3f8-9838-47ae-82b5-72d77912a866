namespace Refactoring.ApiConsumer.Consumers
{
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using MassTransit;
    using Contracts;
    using Models;
    
    public class RegisterNewCandidateConsumer : IConsumer<RegisterNewCandidate>
    {
        private readonly ILogger<RegisterNewCandidateConsumer> _logger;
        private readonly ApiConfig _config;

        public RegisterNewCandidateConsumer(ILogger<RegisterNewCandidateConsumer> logger, IOptions<ApiConfig> configOption)
        {
            _logger = logger;
            _config = configOption.Value;
        }

        public async Task Consume(ConsumeContext<RegisterNewCandidate> context)
        {
            _logger.LogInformation("Received Candidate: {Candidate}", context.Message);

            using var httpClient = new HttpClient();

            string payload = JsonSerializer.Serialize(context.Message);

            var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync($"{_config.BaseUrl}/Candidates", httpContent);

            _logger.LogInformation($"A new candidate has been sent to endpoint, Result: {result}. Email: {context.Message.EmailAddress}");
        }
    }
}

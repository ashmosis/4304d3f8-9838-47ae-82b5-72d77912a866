namespace Refactoring.ApiConsumer.NoChangeInfrastructure
{
    using MassTransit;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Contracts;

    public class EnqueueCandidatesWorker : BackgroundService
    {
        private readonly IBus _bus;
        private readonly IFakeFeed _fakeFeed;
        private readonly ILogger<EnqueueCandidatesWorker> _logger;

        public EnqueueCandidatesWorker(ILogger<EnqueueCandidatesWorker> logger, IBus bus, IFakeFeed fakeFeed)
        {
            _logger = logger;
            _bus = bus;
            _fakeFeed = fakeFeed;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var fakeCandidate = _fakeFeed.FetchCandidate();

                    var endpoint = await _bus.GetSendEndpoint(new Uri("queue:register-new-candidate"));

                    await endpoint.Send<RegisterNewCandidate>(new
                    {
                        fakeCandidate.DateOfBirth,
                        fakeCandidate.EmailAddress,
                        fakeCandidate.Firstname,
                        fakeCandidate.Surname,
                        fakeCandidate.PositionId
                    });

                    _logger.LogInformation("A new candidate has been sent to queue. Email: {Email}", fakeCandidate.EmailAddress);

                    await Task.Delay(1000);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Failed to send item to queue");
                    throw;
                }
            }
        }
    }
}

namespace Refactoring.ApiConsumer.NoChangeInfrastructure
{
    using Refactoring.ApiConsumer.Models;
    public class FakeFeedProxy : IFakeFeed
    {
        public CandidateRequest FetchCandidate() => FakeFeed.FetchCandidate();
    }
}

namespace Refactoring.ApiConsumer.NoChangeInfrastructure
{
    using Refactoring.ApiConsumer.Models;
    public interface IFakeFeed
    {
        CandidateRequest FetchCandidate();
    }
}

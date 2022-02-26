namespace Refactoring.ApiConsumer.NoChangeInfrastructure
{
    using System;
    using FizzWare.NBuilder;
    using Refactoring.ApiConsumer.Models;

    public static class FakeFeed
    {
        private static readonly RandomGenerator _randomGenerator = new RandomGenerator();
        private static readonly int _minDays = 365 * 18;
        private static readonly int _maxDays = 365 * 70;

        public static CandidateRequest FetchCandidate()
        {
            return new CandidateRequest
            {
                Firstname = Faker.Name.First(),
                Surname = Faker.Name.Last(),
                EmailAddress = Faker.Internet.Email(),
                DateOfBirth = DateTime.Now.AddDays(-_randomGenerator.Next(_minDays, _maxDays)),
                PositionId = _randomGenerator.Next(1, 100_000)
            };
        }
    }
}

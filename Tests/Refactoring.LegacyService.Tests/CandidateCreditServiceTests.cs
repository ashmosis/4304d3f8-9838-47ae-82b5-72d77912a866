using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Xunit;
using Moq;
using Refactoring.LegacyService.Candidates;
using Refactoring.LegacyService.Candidates.Services;
using Refactoring.LegacyService.Positions.Constants;
using Refactoring.LegacyService.Positions.Models;

namespace Refactoring.LegacyService.Tests
{
    public class CandidateCreditServiceTests
    {
        private IApplicant _candidate;
        private Mock<ICandidateCreditServiceChannel> _mockCandidateCreditServiceChannel;
        private ICandidateCreditService _candidateCreditService;

        protected void Setup()
        {
            _mockCandidateCreditServiceChannel = new Mock<ICandidateCreditServiceChannel>();

            var position = new Position
            {
                Id = 1,
                Name = PositionNames.SecuritySpecialist,
                Status = PositionStatus.none
            };

            _candidate = new Candidate
            {
                Position = position,
                DateOfBirth = new DateTime(1990, 01, 01),
                EmailAddress = "test.user@example.com",
                Firstname = "Test",
                Surname = "User",
                RequireCreditCheck = false,
                Credit = 0
            };

            _mockCandidateCreditServiceChannel
                .Setup(x => x.GetCredit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(10000);

            _candidateCreditService = new CandidateCreditServiceClient(new CustomBinding(), new EndpointAddress("http://test.com.au"), _mockCandidateCreditServiceChannel.Object);
        }
        
        [Fact]
        public void RequiresCreditCheckShouldReturnTrueForSecuritySpecialist()
        {
            _candidateCreditService =
                new CandidateCreditServiceClient(new CustomBinding(), new EndpointAddress("http://test.com.au"));
            var requiresCreditCheck = _candidateCreditService.RequiresCreditCheck(PositionNames.SecuritySpecialist);

            Assert.True(requiresCreditCheck);
        }

        [Fact]
        public void RequiresCreditCheckShouldReturnTrueForFeatureDeveloper()
        {
            _candidateCreditService =
                new CandidateCreditServiceClient(new CustomBinding(), new EndpointAddress("http://test.com.au"));
            var requiresCreditCheck = _candidateCreditService.RequiresCreditCheck(PositionNames.FeatureDeveloper);

            Assert.True(requiresCreditCheck);
        }

        [Fact]
        public void RequiresCreditCheckShouldReturnFalseForOtherPositions()
        {
            _candidateCreditService =
                new CandidateCreditServiceClient(new CustomBinding(), new EndpointAddress("http://test.com.au"));
            var requiresCreditCheck = _candidateCreditService.RequiresCreditCheck("Worker");

            Assert.False(requiresCreditCheck);
        }

        [Fact]
        public void GetAdjustedCreditShouldReturnHalfScoreForSecuritySpecialist()
        {
            Setup();
            _candidate.Position.Name = PositionNames.SecuritySpecialist;
            var creditScore = _candidateCreditService.GetAdjustedCredit(_candidate);

            Assert.Equal(5000, creditScore);
        }

        [InlineData("FeatureDeveloper")]
        [InlineData("Worker")]
        [InlineData("")]
        [Theory]
        public void GetAdjustedCreditShouldReturnFullScoreForOthers(string position)
        {
            Setup();
            _candidate.Position.Name = position;
            var creditScore = _candidateCreditService.GetAdjustedCredit(_candidate);

            Assert.Equal(10000, creditScore);
        }
    }
}

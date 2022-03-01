using System;
using Xunit;
using Moq;
using Refactoring.LegacyService.Candidates;
using Refactoring.LegacyService.Candidates.Repositories;
using Refactoring.LegacyService.Candidates.Services;
using Refactoring.LegacyService.Positions.Models;

namespace Refactoring.LegacyService.Tests
{
    public class CandidateServiceTests
    {
        private Mock<ICandidateDataAccessProxy> _mockCandidateDataAccess;
        private Mock<ICandidateBuilder> _mockCandidateBuilder;
        private Mock<IExpectsPosition> _mockWithDetails;
        private Mock<IExpectsCreditCheck> _mockWithPosition;
        private IApplicant _candidate;

        protected void Setup()
        {
            _mockCandidateDataAccess = new Mock<ICandidateDataAccessProxy>();
            _mockCandidateBuilder = new Mock<ICandidateBuilder>();
            _mockWithDetails = new Mock<IExpectsPosition>();
            _mockWithPosition = new Mock<IExpectsCreditCheck>();

            var position = new Position
            {
                Id = 1,
                Name = "SecuritySpecialist",
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

            _mockWithPosition.Setup(x => x.CreditCheck()).Returns(_candidate);

            _mockWithDetails.Setup(x => x.WithPosition(It.IsAny<int>())).Returns(_mockWithPosition.Object);

            _mockCandidateBuilder.Setup(x => x.Create(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>()))
                .Returns(_mockWithDetails.Object);

            _mockCandidateDataAccess.Setup(x => x.AddCandidate(It.IsAny<Candidate>())).Verifiable();
        }
        
        [Fact]
        public void ShouldReturnTrueWhenCandidateAdded()
        {
            Setup();
            var candidateService = new CandidateService(_mockCandidateBuilder.Object, _mockCandidateDataAccess.Object);
            
            Assert.True(candidateService.AddCandidate(_candidate.Firstname, _candidate.Surname, _candidate.EmailAddress, _candidate.DateOfBirth, _candidate.Position.Id));
        }

        [Fact]
        public void ShouldReturnFalseWhenCandidateNotAdded()
        {
            Setup();
            var candidateService = new CandidateService(_mockCandidateBuilder.Object, _mockCandidateDataAccess.Object);

            _mockCandidateBuilder
                .Setup(x => x.Create(_candidate.Firstname, _candidate.Surname, _candidate.EmailAddress,
                    _candidate.DateOfBirth)).Throws(new ArgumentException());

            Assert.False(candidateService.AddCandidate(_candidate.Firstname, _candidate.Surname, _candidate.EmailAddress, _candidate.DateOfBirth, _candidate.Position.Id));
        }
    }
}

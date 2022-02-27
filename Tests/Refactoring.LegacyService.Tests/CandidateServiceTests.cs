using System;
using Xunit;
using Moq;
using Refactoring.LegacyService.Candidate.Repositories;
using Refactoring.LegacyService.Candidate.Services;
using Refactoring.LegacyService.Position.Model;
using Refactoring.LegacyService.Position.Repositories;

namespace Refactoring.LegacyService.Tests
{
    public class CandidateServiceTests
    {
        private Mock<IPositionRepository> _mockPositionRepo;
        private Mock<ICandidateCreditService> _mockCandidateCreditServiceClient;
        private Mock<ICandidateDataAccess> _mockCandidateDataAccess;
        private Position.Model.Position _position;

        protected void Setup(string firstname, string surname, string email, DateTime dob, int positionId)
        {
            _mockPositionRepo = new Mock<IPositionRepository>();
            _mockCandidateCreditServiceClient = new Mock<ICandidateCreditService>();
            _mockCandidateDataAccess = new Mock<ICandidateDataAccess>();

            _position = new Position.Model.Position
            {
                Id = positionId,
                Name = "SecuritySpecialist",
                Status = PositionStatus.none
            };

            _mockPositionRepo.Setup(x => x.GetById(positionId)).Returns(_position);
            _mockCandidateCreditServiceClient.Setup(x => x.GetCredit(firstname, surname, dob)).Returns(10000);
            _mockCandidateDataAccess.Setup(x => x.AddCandidate(It.IsAny<Candidate.Candidate>())).Verifiable();
        }

        [InlineData("Test", "User", "test.user@example.com", "1990/1/1", 1)]
        [Theory]
        public void ShouldAddCandidateWithValidData(string firstname, string surname, string email,
            string dateOfBirth, int positionId)
        {
            DateTime.TryParse(dateOfBirth, out var dob);

            Setup(firstname, surname, email, dob, positionId);
            var candidateService = new CandidateService(_mockPositionRepo.Object, _mockCandidateCreditServiceClient.Object, _mockCandidateDataAccess.Object);
            
            Assert.True(candidateService.AddCandidate(firstname, surname, email, dob, positionId));
        }
        
        [Fact]
        public void ShouldFailWithInvalidFirstName()
        {

            var firstname = "";
            var surname = "Example";
            var email = "test.user@example.com";
            var dob = new DateTime(1990, 1, 1);
            var positionId = 1;
            
            Setup(firstname, surname, email, dob, positionId);

            var candidateService = new CandidateService(_mockPositionRepo.Object, _mockCandidateCreditServiceClient.Object, _mockCandidateDataAccess.Object);

            Assert.False(candidateService.AddCandidate(firstname, surname, email, dob, positionId));
        }

        [Fact]
        public void ShouldFailWithInvalidSurname()
        {
            var firstname = "Test";
            var surname = "";
            var email = "test.user@example.com";
            var dob = new DateTime(1990, 1, 1);
            var positionId = 1;

            Setup(firstname, surname, email, dob, positionId);

            var candidateService = new CandidateService(_mockPositionRepo.Object, _mockCandidateCreditServiceClient.Object, _mockCandidateDataAccess.Object);

            Assert.False(candidateService.AddCandidate(firstname, surname, email, dob, positionId));
        }

        [InlineData("test.userexample.com")]
        [Theory]
        public void ShouldFailWithInvalidEmail(string email)
        {
            var firstname = "Test";
            var surname = "";
            var dob = new DateTime(1990, 1, 1);
            var positionId = 1;

            Setup(firstname, surname, email, dob, positionId);

            var candidateService = new CandidateService(_mockPositionRepo.Object, _mockCandidateCreditServiceClient.Object, _mockCandidateDataAccess.Object);

            Assert.False(candidateService.AddCandidate(firstname, surname, email, dob, positionId));
        }

        [InlineData(100)]
        [Theory]
        public void ShouldFailWithInvalidCreditScore(int score)
        {
            var firstname = "Test";
            var surname = "";
            var email = "test.user@example.com";
            var dob = new DateTime(1990, 1, 1);
            var positionId = 1;

            Setup(firstname, surname, email, dob, positionId);

            _mockCandidateCreditServiceClient.Setup(x => x.GetCredit(firstname, surname, dob)).Returns(score);

            var candidateService = new CandidateService(_mockPositionRepo.Object, _mockCandidateCreditServiceClient.Object, _mockCandidateDataAccess.Object);

            Assert.False(candidateService.AddCandidate(firstname, surname, email, dob, positionId));
        }
    }
}

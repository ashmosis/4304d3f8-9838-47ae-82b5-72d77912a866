using System;
using Xunit;
using Moq;
using Refactoring.LegacyService.Candidates;
using Refactoring.LegacyService.Candidates.Repositories;
using Refactoring.LegacyService.Candidates.Services;
using Refactoring.LegacyService.Positions.Models;
using Refactoring.LegacyService.Positions.Repositories;

namespace Refactoring.LegacyService.Tests
{
    public class CandidateBuilderTests
    {
        private Mock<IPositionRepository> _mockPositionRepo;
        private Mock<ICandidateCreditService> _mockCandidateCreditServiceClient;
        private Position _position;

        protected void Setup(string firstname, string surname, string email, DateTime dob, int positionId)
        {
            _mockPositionRepo = new Mock<IPositionRepository>();
            _mockCandidateCreditServiceClient = new Mock<ICandidateCreditService>();

            _position = new Position
            {
                Id = positionId,
                Name = "SecuritySpecialist",
                Status = PositionStatus.none
            };

            _mockPositionRepo.Setup(x => x.GetById(positionId)).Returns(_position);
            _mockCandidateCreditServiceClient.Setup(x => x.RequiresCreditCheck(It.IsAny<string>())).Returns(true);
            _mockCandidateCreditServiceClient.Setup(x => x.GetAdjustedCredit(It.IsAny<IApplicant>())).Returns(500);
        }

        [InlineData("Test", "User", "test.user@example.com", "1990/1/1", 1)]
        [Theory]
        public void ShouldBuildCandidateWithValidData(string firstname, string surname, string email,
            string dateOfBirth, int positionId)
        {
            DateTime.TryParse(dateOfBirth, out var dob);
            Setup(firstname, surname, email, dob, positionId);

            var candidateBuilder = new CandidateBuilder(_mockPositionRepo.Object, _mockCandidateCreditServiceClient.Object);

            var candidate = candidateBuilder.Create(firstname, surname, email, dob)
                .WithPosition(positionId)
                .CreditCheck();

            Assert.Equal(firstname, candidate.Firstname);
            Assert.Equal(surname, candidate.Surname);
            Assert.Equal(email, candidate.EmailAddress);
            Assert.Equal(dob, candidate.DateOfBirth);
            Assert.Equal(_position, candidate.Position);
            Assert.True(candidate.RequireCreditCheck);
            Assert.Equal(500, candidate.Credit);
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

            var candidateBuilder = new CandidateBuilder(_mockPositionRepo.Object, _mockCandidateCreditServiceClient.Object);

            Assert.Throws<ArgumentException>(() => candidateBuilder.Create(firstname, surname, email, dob)
                .WithPosition(positionId)
                .CreditCheck()
            );
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

            var candidateBuilder = new CandidateBuilder(_mockPositionRepo.Object, _mockCandidateCreditServiceClient.Object);

            Assert.Throws<ArgumentException>(() => candidateBuilder.Create(firstname, surname, email, dob)
                .WithPosition(positionId)
                .CreditCheck()
            );
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

            var candidateBuilder = new CandidateBuilder(_mockPositionRepo.Object, _mockCandidateCreditServiceClient.Object);

            Assert.Throws<ArgumentException>(() => candidateBuilder.Create(firstname, surname, email, dob)
                .WithPosition(positionId)
                .CreditCheck()
            );
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

            var candidateBuilder = new CandidateBuilder(_mockPositionRepo.Object, _mockCandidateCreditServiceClient.Object);

            Assert.Throws<ArgumentException>(() => candidateBuilder.Create(firstname, surname, email, dob)
                .WithPosition(positionId)
                .CreditCheck()
            );
        }
    }
}

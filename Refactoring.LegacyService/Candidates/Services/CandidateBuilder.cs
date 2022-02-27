using System;
using Refactoring.LegacyService.Candidates.Services;
using Refactoring.LegacyService.Positions.Repositories;

namespace Refactoring.LegacyService.Candidates
{
    public class CandidateBuilder : ICandidateBuilder, IExpectsPosition, IExpectsCreditCheck
    {
        private IApplicant Candidate { get; set; }
        private readonly IPositionRepository _positionRepository;
        private readonly ICandidateCreditService _candidateCreditService;

        public CandidateBuilder(IPositionRepository positionRepository, ICandidateCreditService candidateCreditService)
        {
            _positionRepository = positionRepository;
            _candidateCreditService = candidateCreditService;
        }

        public IExpectsPosition Create(string firstname, string surname, string email, DateTime dateOfBirth)
        {
            ValidateCandidate(firstname, surname, email, dateOfBirth);
            Candidate = new Candidate
            {
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firstname,
                Surname = surname,
            };
            return this;
        }

        public IExpectsCreditCheck WithPosition(int positionId)
        {
            Candidate.Position = _positionRepository.GetById(positionId);
            return this;
        }

        public IApplicant CreditCheck()
        {
            Candidate.RequireCreditCheck = _candidateCreditService.RequiresCreditCheck(Candidate.Position.Name);
            if (Candidate.RequireCreditCheck)
            {
                Candidate.Credit = _candidateCreditService.GetAdjustedCredit(Candidate);
            }
            return Candidate;
        }

        private static void ValidateCandidate(string firstname, string surname, string email, DateTime dateOfBirth)
        {
            ValidateFirstName(firstname);
            ValidateSurname(surname);
            ValidateEmail(email);
            ValidateDateOfBirth(dateOfBirth);
        }

        private static void ValidateFirstName(string firstname)
        {
            if (string.IsNullOrEmpty(firstname))
            {
                throw new ArgumentException("First name supplied is invalid. First name must not be empty");
            }
        }

        private static void ValidateSurname(string surname)
        {
            if (string.IsNullOrEmpty(surname))
            {
                throw new ArgumentException("Surname supplied is invalid. Surname must not be empty");
            }
        }

        private static void ValidateEmail(string email)
        {
            if (!email.Contains("@") || !email.Contains("."))
            {
                throw new ArgumentException("Email supplied is invalid. Please supply a valid email address.");
            }
        }

        private static void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            var age = now.Year - dateOfBirth.Year;

            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }

            if (age <= 18) throw new ArgumentException("Minimum candidate age is 18");
        }
    }
}

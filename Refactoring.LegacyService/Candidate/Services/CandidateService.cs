using System;
using Refactoring.LegacyService.Candidate.Repositories;
using Refactoring.LegacyService.Position.Constants;
using Refactoring.LegacyService.Position.Repositories;

namespace Refactoring.LegacyService.Candidate.Services
{
    public class CandidateService
    {
        private readonly IPositionRepository _positionRepository;
        private readonly ICandidateCreditService _candidateCreditService;
        private readonly ICandidateDataAccess _candidateDataAccess;

        public CandidateService(
            IPositionRepository positionRepository,
            ICandidateCreditService candidateCreditService,
            ICandidateDataAccess candidateDataAccess
            )
        {
            _positionRepository = positionRepository;
            _candidateCreditService = candidateCreditService;
            _candidateDataAccess = candidateDataAccess;
        }
        public bool AddCandidate(string firstname, string surname, string email, DateTime dateOfBirth, int positionId)
        {
            if (!ValidateCandidate(firstname, surname, email, dateOfBirth)) return false;

            var position = _positionRepository.GetById(positionId);

            var candidate = new Candidate
            {
                Position = position,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firstname,
                Surname = surname
            };
            
            _candidateDataAccess.AddCandidate(candidate);

            return true;
        }

        private static bool ValidateCandidate(string firstname, string surname, string email, DateTime dateOfBirth)
        {
            if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(surname))
            {
                return false;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                return false;
            }

            var now = DateTime.Now;
            var age = now.Year - dateOfBirth.Year;

            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }

            return age >= 18;
        }

        private bool HasSufficientCredit(int minCredit, IApplicant candidate)
        {
            switch (candidate.Position.Name)
            {
                case PositionNames.SecuritySpecialist:
                {
                    // Do credit check and half credit
                    candidate.RequireCreditCheck = true;
                    var credit = _candidateCreditService.GetCredit(candidate.Firstname, candidate.Surname, candidate.DateOfBirth);
                    credit = credit / 2;
                    candidate.Credit = credit;
                    break;
                }
                case PositionNames.FeatureDeveloper:
                {
                    // Do credit check
                    candidate.RequireCreditCheck = true;
                    var credit = _candidateCreditService.GetCredit(candidate.Firstname, candidate.Surname, candidate.DateOfBirth);
                    candidate.Credit = credit;
                    break;
                }
                default:
                    // No credit check
                    candidate.RequireCreditCheck = false;
                    break;
            }

            return !candidate.RequireCreditCheck || candidate.Credit >= 500;
        }
    }
}

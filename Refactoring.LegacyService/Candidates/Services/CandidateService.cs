using System;
using Refactoring.LegacyService.Candidates.Repositories;

namespace Refactoring.LegacyService.Candidates.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateBuilder _candidateBuilder;
        private readonly ICandidateDataAccessProxy _candidateDataAccess;

        public CandidateService(ICandidateBuilder candidateBuilder, ICandidateDataAccessProxy candidateDataAccess
            )
        {
            _candidateBuilder = candidateBuilder;
            _candidateDataAccess = candidateDataAccess;
        }
        public bool AddCandidate(string firstname, string surname, string email, DateTime dateOfBirth, int positionId)
        {
            try
            {
                var candidate = _candidateBuilder.Create(firstname, surname, email, dateOfBirth)
                    .WithPosition(positionId)
                    .CreditCheck();
                _candidateDataAccess.AddCandidate(candidate);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

using System;

namespace Refactoring.LegacyService.Candidates.Services
{
    public interface ICandidateService
    {
        bool AddCandidate(string firstname, string surname, string email, DateTime dateOfBirth, int positionId);
    }
}

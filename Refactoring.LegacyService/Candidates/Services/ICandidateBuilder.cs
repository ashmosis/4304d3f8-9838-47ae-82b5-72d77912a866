using System;

namespace Refactoring.LegacyService.Candidates
{
    public interface ICandidateBuilder
    {
        IExpectsPosition Create(string firstname, string surname, string email, DateTime dateOfBirth);
    }
}

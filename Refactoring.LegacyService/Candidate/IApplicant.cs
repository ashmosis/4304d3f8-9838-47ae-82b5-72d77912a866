using System;

namespace Refactoring.LegacyService.Candidate
{
    public interface IApplicant
    {
        DateTime DateOfBirth { get; set; }
        string EmailAddress { get; set; }
        string Firstname { get; set; }
        string Surname { get; set; }
        Position.Model.Position Position { get; set; }
        bool RequireCreditCheck { get; set; }
        int Credit { get; set; }
    }
}

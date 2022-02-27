using System;
using Refactoring.LegacyService.Positions.Models;

namespace Refactoring.LegacyService.Candidates
{
    public interface IApplicant
    {
        DateTime DateOfBirth { get; set; }
        string EmailAddress { get; set; }
        string Firstname { get; set; }
        string Surname { get; set; }
        Position Position { get; set; }
        bool RequireCreditCheck { get; set; }
        int Credit { get; set; }
    }
}

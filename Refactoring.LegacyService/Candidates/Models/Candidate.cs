using System;
using Refactoring.LegacyService.Positions.Models;

namespace Refactoring.LegacyService.Candidates
{
    public class Candidate : IApplicant
    {
        public Position Position { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public bool RequireCreditCheck { get; set; }
        public int Credit { get; set; }
    }
}

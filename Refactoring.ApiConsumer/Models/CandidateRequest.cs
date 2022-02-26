namespace Refactoring.ApiConsumer.Models
{
    using System;

    public class CandidateRequest
    {
        public DateTime DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public int PositionId { get; set; }
    }
}

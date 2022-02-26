namespace Refactoring.ApiConsumer.Contracts
{
    using System;
    public interface RegisterNewCandidate
    {
        DateTime DateOfBirth { get; set; }
        string EmailAddress { get; set; }
        string Firstname { get; set; }
        string Surname { get; set; }
        int PositionId { get; set; }
    }
}

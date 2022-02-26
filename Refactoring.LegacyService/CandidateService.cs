namespace Refactoring.LegacyService
{
    using System;

    public class CandidateService
    {
        public bool AddCandidate(string firname, string surname, string email, DateTime dateOfBirth, int positionid)
        {
            if (string.IsNullOrEmpty(firname) || string.IsNullOrEmpty(surname))
            {
                return false;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;

            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }

            if (age < 18)
            {
                return false;
            }

            var positionRepo = new PositionRepository();
            var position = positionRepo.GetById(positionid);

            var candidate = new Candidate
            {
                Position = position,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firname,
                Surname = surname
            };

            if (position.Name == "SecuritySpecialist")
            {
                // Do credit check and half credit
                candidate.RequireCreditCheck = true;
                using (var candidateCreditService = new CandidateCreditServiceClient())
                {
                    var credit = candidateCreditService.GetCredit(candidate.Firstname, candidate.Surname, candidate.DateOfBirth);
                    credit = credit / 2;
                    candidate.Credit = credit;
                }                
            }
            else if (position.Name == "FeatureDeveloper")
            {
                // Do credit check
                candidate.RequireCreditCheck = true;
                using (var candidateCreditService = new CandidateCreditServiceClient())
                {
                    var credit = candidateCreditService.GetCredit(candidate.Firstname, candidate.Surname, candidate.DateOfBirth);
                    candidate.Credit = credit;
                }
            }
            else
            {
                // No credit check
                candidate.RequireCreditCheck = false;
            }

            if (candidate.RequireCreditCheck && candidate.Credit < 500)
            {
                return false;
            }

            CandidateDataAccess.AddCandidate(candidate);

            return true;
        }
    }
}

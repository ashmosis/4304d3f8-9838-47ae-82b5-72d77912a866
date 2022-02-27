using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Refactoring.LegacyService.Candidates.Repositories
{
    public class CandidateDataAccessProxy : ICandidateDataAccessProxy
    {

        public void AddCandidate(IApplicant candidate)
        {

            CandidateDataAccess.AddCandidate(candidate);
        }
    }
}

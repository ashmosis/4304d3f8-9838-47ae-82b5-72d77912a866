using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Refactoring.LegacyService.Position.Model;

namespace Refactoring.LegacyService.Position.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        public Model.Position GetById(int id)
        {
            Model.Position position = null;
            var connectionString = ConfigurationManager.ConnectionStrings["applicationDatabase"].ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "uspGetPositionById"
                };

                var parametr = new SqlParameter("@positionId", SqlDbType.Int) { Value = id };
                command.Parameters.Add(parametr);

                connection.Open();
                var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    position = new Model.Position
                    {
                        Id = int.Parse(reader["positionId"].ToString()),
                        Name = reader["Name"].ToString(),
                        Status = (PositionStatus)int.Parse(reader["Status"].ToString())
                    };
                }
            }

            return position;
        }
    }
}

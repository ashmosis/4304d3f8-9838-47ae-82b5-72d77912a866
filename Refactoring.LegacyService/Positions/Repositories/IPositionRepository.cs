using Refactoring.LegacyService.Positions.Models;

namespace Refactoring.LegacyService.Positions.Repositories
{
    public interface IPositionRepository
    {
        Position GetById(int id);
    }
}

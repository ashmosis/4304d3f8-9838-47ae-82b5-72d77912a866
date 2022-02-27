namespace Refactoring.LegacyService.Position.Repositories
{
    public interface IPositionRepository
    {
        Model.Position GetById(int id);
    }
}

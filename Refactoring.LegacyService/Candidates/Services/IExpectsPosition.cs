namespace Refactoring.LegacyService.Candidates
{
    public interface IExpectsPosition
    {
        IExpectsCreditCheck WithPosition(int positionId);
    }
}

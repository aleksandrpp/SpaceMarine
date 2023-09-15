using AK.AStar;

namespace AK.SpaceMarine.Parts
{
    public interface IPathfinder
    {
        void Bind(IPathfinding pathfinding);
    }
}
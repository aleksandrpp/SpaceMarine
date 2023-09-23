using Priority_Queue;
using System.Collections.Generic;
using UnityEngine;

namespace AK.AStar
{
    public interface IPathfinding
    {
        IGrid Grid { get; }
        
        Status CreatePath(Vector3 sourcePosition, Vector3 targetPosition, List<Tile> path);
    }
    
    public class Pathfinding : IPathfinding
    {
        public IGrid Grid { get; }
        
        private FastPriorityQueue<Tile> _open;
        
        private Dictionary<Tile, Tile> _close;
        private Dictionary<Tile, float> _cost;
        private List<Tile> _neighbours;

        public Pathfinding(IGrid grid)
        {
            Grid = grid;

            var maxPath = (int)Mathf.Sqrt(Mathf.Pow(Grid.Dimension.x, 2) + Mathf.Pow(Grid.Dimension.y, 2)) * 2;
            _open = new FastPriorityQueue<Tile>(maxPath);
            
            _close = new Dictionary<Tile, Tile>();
            _cost = new Dictionary<Tile, float>();
            _neighbours = new List<Tile>();
        }

        public Status CreatePath(Vector3 sourcePosition, Vector3 targetPosition, List<Tile> path)
        {
            path.Clear();

            if (!Grid.TryGetNodeFromPosition(sourcePosition, out Tile start) ||
                !Grid.TryGetNodeFromPosition(targetPosition, out Tile end))
                return Status.InvalidPosition;

            if (start.Cost < 0)
                if (!Grid.TryGetAvailable(start))
                    return Status.InvalidPosition;

            if (end.Cost < 0)
                if (!Grid.TryGetAvailable(end))
                    return Status.InvalidPosition;

            if (start.Equals(end))
                return Status.StartIsEnd;

            _open.Clear();
            _open.Enqueue(start, 0f);

            _close.Clear();
            _close.Add(start, start);

            _cost.Clear();
            _cost.Add(start, 0);

            int i = 250; // under construction
            
            while (_open.Count > 0 || i > 0)
            {
                i--;
                
                var current = _open.Dequeue();
                if (current.Equals(end))
                {
                    TracePath(path, end);
                    return Status.Success;
                }

                Grid.GetNeighbours(current, _neighbours);
                foreach (Tile neighbour in _neighbours)
                {
                    if (neighbour.Cost < 0) continue;
                    if (_open.Contains(neighbour)) continue;
                    
                    var cost = _cost[current] + Math.GetNeighbourCost(current, neighbour) + neighbour.Cost;
                    if (!_cost.ContainsKey(neighbour) || cost < _cost[neighbour])
                    {
                        _cost[neighbour] = cost;
                        _close[neighbour] = current;
                        _open.Enqueue(neighbour, cost + Math.ManhattanDistance(neighbour, end, 4));
                    }

                    if (_open.Count == _open.MaxSize)
                        return Status.PathLengthOversize;
                }
            }
            
            return Status.PathNotFound;
        }

        private void TracePath(ICollection<Tile> path, Tile end)
        {
            Tile child = end;
            while (true)
            {
                Tile previous = _close[child];
                path.Add(child);
                if (!child.Equals(previous))
                    child = previous;
                else
                    break;
            }
        }
    }
}
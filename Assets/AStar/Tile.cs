using Priority_Queue;
using UnityEngine;

namespace AK.AStar
{
    public class Tile : FastPriorityQueueNode
    {
        public Vector3 Position { get; }

        public float Cost => _blocked ? -1 : _cost;
        
        public int X { get; }
        
        public int Y { get; }

        private bool _blocked;

        private float _cost;

        public Tile(Vector3 position, int x, int y, bool blocked)
        {
            _blocked = blocked;
            Position = position;
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return (obj is Tile node) && (X == node.X && Y == node.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public void Occupy(IBlocker blocker)
        {
            _cost += blocker.BlockerPower;
        }

        public void Free(IBlocker blocker)
        {
            _cost -= blocker.BlockerPower;
        }
    }
}
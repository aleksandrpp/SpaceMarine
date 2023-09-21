using System.Collections.Generic;
using UnityEngine;

namespace AK.AStar
{
    public interface IGrid
    {
        Vector2Int Dimension { get; }

        bool TryGetNode(int x, int y, out Tile tile);

        bool TryGetNodeFromPosition(Vector3 position, out Tile tile);

        void GetNeighbours(Tile tile, List<Tile> neighbours);

        bool TryGetAvailable(Tile tile);

        void Build();

        void Debug();
    }
}
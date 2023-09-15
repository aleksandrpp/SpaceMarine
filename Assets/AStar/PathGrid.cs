using System.Collections.Generic;
using UnityEngine;

namespace AK.AStar
{
    public class PathGrid : IGrid
    {
        public Vector2Int Dimension { get; private set; }

        private GridConfig _config;
        private Vector3 _worldOffset;

        private Tile[,] _grid;

        public PathGrid(GridConfig config) : this(config, Vector3.zero)
        {
        }

        public PathGrid(GridConfig config, Vector3 worldOffset)
        {
            _config = config;
            _worldOffset = worldOffset;
        }

        public void Build()
        {
            Dimension = new Vector2Int(
                Mathf.RoundToInt(_config.Size.x / _config.TileSize),
                Mathf.RoundToInt(_config.Size.y / _config.TileSize));

            Vector3 origin = _worldOffset - Vector3.right * _config.Size.x / 2 - Vector3.forward * _config.Size.y / 2;
            _grid = new Tile[Dimension.x, Dimension.y];

            for (int x = 0; x < Dimension.x; x++)
            for (int y = 0; y < Dimension.y; y++)
            {
                Vector3 worldPoint = origin + Vector3.right * (x * _config.TileSize + _config.TileSize / 2) + Vector3.forward * (y * _config.TileSize + _config.TileSize / 2);
                Vector3 checkBox = new Vector3(_config.TileSize / 2, _config.TileHeight, _config.TileSize / 2);
                bool obstacle = Physics.CheckBox(worldPoint, checkBox, Quaternion.identity, _config.ObstacleMask);
                _grid[x, y] = new Tile(worldPoint, x, y, obstacle);
            }
        }

        public bool TryGetNode(int x, int y, out Tile tile)
        {
            tile = null;

            if (x < 0 || x >= Dimension.x || y < 0 || y >= Dimension.y)
                return false;

            tile = _grid[x, y];
            return true;
        }

        public bool TryGetNodeFromPosition(Vector3 position, out Tile tile)
        {
            tile = null;

            float xNormalize = (position.x + _config.Size.x / 2) / _config.Size.x;
            float yNormalize = (position.z + _config.Size.y / 2) / _config.Size.y;

            if (!Check01(xNormalize) || !Check01(yNormalize))
                return false;

            int x = Mathf.RoundToInt((Dimension.x - 1) * xNormalize);
            int y = Mathf.RoundToInt((Dimension.y - 1) * yNormalize);

            return TryGetNode(x, y, out tile);
        }

        private bool Check01(float value)
        {
            return value is >= 0 and <= 1;
        }

        public void GetNeighbours(Tile tile, List<Tile> neighbours)
        {
            neighbours.Clear();

            for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                if (TryGetNode(tile.X + x, tile.Y + y, out Tile neighbour))
                    neighbours.Add(neighbour);
            }
        }

        public Tile GetAvailable(Tile tile)
        {
            var neighbours = new List<Tile>();
            GetNeighbours(tile, neighbours);

            Tile neighbour = null;
            foreach (var t in neighbours)
            {
                neighbour = t;
                if (t.Cost >= 0)
                    return neighbour;
            }

            return GetAvailable(neighbour);
        }

        public void Debug()
        {
            foreach (var tile in _grid)
            {
                if (tile.Cost < 0) continue;
                Gizmos.color = new Color(0, 1, 0, .25f);
                Gizmos.DrawSphere(tile.Position, 1 / (1 / _config.TileSize * 2 + tile.Cost));
            }
        }
    }
}
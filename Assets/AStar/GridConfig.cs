using UnityEngine;

namespace AK.AStar
{
    [CreateAssetMenu(fileName = "SO_GridConfig", menuName = "AK.AStar/GridConfig")]
    public class GridConfig : ScriptableObject
    {
        public Vector2 Size = new(50, 50);
        public float TileSize = .5f;
        public float TileHeight = 1;
        public LayerMask ObstacleMask;
    }
}
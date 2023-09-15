using Unity.Mathematics;
using UnityEngine;

namespace AK.SpaceMarine.Maze
{
    [CreateAssetMenu(fileName = "SO_Maze", menuName = "SpaceMarine/MazeConfig")]
    public class MazeConfig : ScriptableObject
    {
        [System.Serializable]
        public struct Connectivity
        {
            public Wfc.Axis Left;
            public Wfc.Axis Right;
            public Wfc.Axis Bottom;
            public Wfc.Axis Top;
            public Wfc.Axis Back;
            public Wfc.Axis Front;

            public static explicit operator Wfc.Connectivity(Connectivity c)
                => new Wfc.Connectivity(c.Left, c.Right,
                    c.Bottom, c.Top,
                    c.Back, c.Front);
        }

        [System.Serializable]
        public struct Module
        {
            public GameObject Object;
            public Connectivity Connectivity;
        }

        public int3 Size = new(20, 1, 20);
        public float3 Scale = new(2.5f, 4, 2.5f);
        public float3 Offset = new(0, -1.5f, 0);
        public Module[] Modules;
    }
}
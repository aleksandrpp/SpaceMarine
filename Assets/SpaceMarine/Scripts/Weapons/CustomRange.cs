using AK.SpaceMarine.Parts;
using UnityEngine;

namespace AK.SpaceMarine.Weapons
{
    public class CustomRange : IRange
    {
        public CustomRange(Vector3 position, float range)
        {
            Position = position;
            Range = range;
        }
        
        public Vector3 Position { get; }
        public float Range { get; }
    }
}
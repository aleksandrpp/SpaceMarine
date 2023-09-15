using UnityEngine;

namespace AK.SpaceMarine.Parts
{
    public interface ITransform
    {
        Transform Transform { get; }
        
        bool Equals(Transform other);
    }
}
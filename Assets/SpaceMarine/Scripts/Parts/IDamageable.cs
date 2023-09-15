using UnityEngine;

namespace AK.SpaceMarine.Parts
{
    public interface IDamageable : IPosition, ITransform
    {
        Vector3 HitPoint { get; }
        
        void ReceiveHit(float damage);
    }
}
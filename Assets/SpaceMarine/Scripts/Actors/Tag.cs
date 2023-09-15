using UnityEngine;

namespace AK.SpaceMarine.Actors
{
    public interface ITag
    {
        ActorConfig Config { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }
    }
    
    public class Tag : MonoBehaviour, ITag
    {
        [SerializeField] private ActorConfig _config;

        public ActorConfig Config => _config;
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Position, .1f);
        }
    }
}
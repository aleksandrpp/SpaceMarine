using UnityEngine;

namespace AK.SpaceMarine.Actors
{
    [DisallowMultipleComponent]
    public abstract class Actor : MonoBehaviour, System.IDisposable, System.IEquatable<Transform>
    {
        private int _lastPositionCache;
        private Vector3 _position;
        
        private int _lastRotationCache;
        private Quaternion _rotation;
        
        public Transform Transform => transform;
        
        public Vector3 Position
        {
            get
            {
                int frame = Time.frameCount;
                if (_lastPositionCache < frame)
                {
                    _position = transform.position;
                    _lastPositionCache = frame;
                }
                
                return _position;
            }
            set
            {
                transform.position = value;
                _position = value;
            }
        }
        
        public Quaternion Rotation
        {
            get
            {
                int frame = Time.frameCount;
                if (_lastRotationCache < frame)
                {
                    _rotation = transform.rotation;
                    _lastRotationCache = frame;
                }
                
                return _rotation;
            }
            set
            {
                transform.rotation = value;
                _rotation = value;
            }
        }

        public Vector3 NearPoint(float distance = 1)
        {
            Vector2 offset = Random.insideUnitCircle * distance;
            return Position + new Vector3(offset.x, 0, offset.y);
        }

        public virtual void Dispose()
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }

        public bool Equals(Transform other)
        {
            return transform == other;
        }

        public abstract void Bind(IWorld world);
    }
}
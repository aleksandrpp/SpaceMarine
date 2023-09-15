using UnityEngine;

namespace AK.SpaceMarine
{
    public static class Utils
    {
        public const float Epsilon = .05f;
        
        public static bool Contains(this LayerMask mask, int layer)
        {
            int layerMask = 1 << layer;
            return (layerMask & mask.value) != 0;
        }
    }
}

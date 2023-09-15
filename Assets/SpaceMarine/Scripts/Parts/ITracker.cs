using UnityEngine;

namespace AK.SpaceMarine.Parts
{
    public interface ITracker : IPosition
    {
        Sprite TrackerIcon { get; }
        
        bool IsTracked { get; }
    }
}
using AK.SpaceMarine.Weapons;
using UnityEngine;

namespace AK.SpaceMarine.Parts
{
    public interface IGunner : IRange
    {
        GunConfig GunConfig { get; }
        
        Transform GunRoot { get; }
        
        Gun Gun { get; }
    }
}
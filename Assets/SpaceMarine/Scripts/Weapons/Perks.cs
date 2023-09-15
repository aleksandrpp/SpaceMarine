using System;

namespace AK.SpaceMarine.Weapons
{
    [Flags]
    public enum Perks
    {
        Ricochet = 1 << 1,
        SideShot = 1 << 3,
        Bounce = 1 << 5,
    }
}
using UnityEngine;

namespace AK.SpaceMarine.Weapons
{
    public class Rifle : Gun
    {
	    public override void Fire()
	    {
		    Straight();

		    if ((Config.Perks & Perks.SideShot) != 0)
            {
                Side();
            }
	    }

        private void Straight()
		{
			var p = _muzzleRoot.position;
			var r = _muzzleRoot.rotation;
			var f = r * Vector3.forward;
			Launch(p, r, f);
		}

        private void Side()
		{
			for (float i = -90; i <= 90; i += 180)
			{
                var p = _muzzleRoot.position;
                var r = Quaternion.Euler(0, _muzzleRoot.rotation.eulerAngles.y + i, 0);
				var f = r * Vector3.forward * Random.Range(.9f, 1f);
				Launch(p, r, f);
			}
		}
    }
}
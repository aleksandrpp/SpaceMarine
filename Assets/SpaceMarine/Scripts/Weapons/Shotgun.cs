using UnityEngine;

namespace AK.SpaceMarine.Weapons
{
    public class Shotgun : Gun
    {
        public override void Fire()
        {
            Heap();

            if ((Config.Perks & Perks.SideShot) != 0)
            {
                SideHeap();
            }
        }

        private void Heap()
        {
            for (float i = -6; i <= 6; i += 3)
            {
                var p = _muzzleRoot.position;
                var r = Quaternion.Euler(0, _muzzleRoot.rotation.eulerAngles.y + i, 0);
                var f = r * Vector3.forward * Random.Range(.6f, 1f);

                Launch(p, r, f);
            }
        }

        private void SideHeap()
        {
            var p = _muzzleRoot.position;
            var r = _muzzleRoot.rotation.eulerAngles;
            Quaternion R(int i) => Quaternion.Euler(0, r.y + i, 0);
            Vector3 F(Quaternion r) => r * Vector3.forward * Random.Range(.6f, 1f);

            var r1 = R(-60);
            Launch(p, r1, F(r1));

            var r3 = R(-50);
            Launch(p, r3, F(r3));

            var r5 = R(60);
            Launch(p, r5, F(r5));

            var r7 = R(50);
            Launch(p, r7, F(r7));
        }
    }
}
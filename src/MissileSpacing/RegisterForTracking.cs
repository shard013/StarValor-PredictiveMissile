using HarmonyLib;
using UnityEngine;

namespace PredictiveMissile.MissileSpacing
{
    [HarmonyPatch(typeof(MissleWeapon), nameof(MissleWeapon.Fire))]
    public class MissleRegister
    {
        public static void Postfix(Transform target, ProjectileControl ___projControl)
        {
            if (!Plugin.Settings.EnableMissileSpacing.Value)
            {
                return;
            }

            if (target != null)
            {
                var targetSizeClass = target.GetComponent<SpaceShip>()?.sizeClass;
                //Don't try to keep missiles spaced if the target is Shuttle or Yacht as the spread can cause misses
                if (targetSizeClass <= (int)ShipClassLevel.Yacht)
                {
                    return;
                }
            }

            //Add new missiles to shared dictionary so that they can see what other missiles
            //  are near owned by the same character and can space themselves apart
            Spacing.AddMissile(___projControl);
        }

    }
}

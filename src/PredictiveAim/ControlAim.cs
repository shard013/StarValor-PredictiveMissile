using HarmonyLib;
using System;
using UnityEngine;

namespace PredictiveMissile.PredictiveAim
{
    [HarmonyPatch(typeof(ProjectileControl), "FixedUpdate")]
    public class ControlAim
    {
        private static readonly float _maxTurnSpeed = Plugin.Settings.MissileTurnSpeed.Value;
        private static readonly float _finalBurnThreshold = Plugin.Settings.FinalBurnThreshold.Value;

        public class InitialState
        {
            public Quaternion rotation;
        }

        public static void Prefix(ProjectileControl __instance, out InitialState __state)
        {
            var s = __instance.transform.rotation;

            __state = new InitialState
            {
                rotation = new Quaternion(s.x, s.y, s.z, s.w)
            };

        }

        public static void Postfix(ProjectileControl __instance, InitialState __state, ref float ___currSpeed)
        {
            if (!__instance.homing)
            {
                return;
            }

            if (!__instance.target)
            {
                return;
            }

            if (!__instance.owner)
            {
                //Untarget projectile if the owner of the projectile has been destroyed
                __instance.target = null;
                return;
            }

            if (Plugin.Settings.OnlyAimForPlayer.Value)
            {
                var playerSpaceShip = GameObject.FindGameObjectWithTag("Player").GetComponent<SpaceShip>();
                if (playerSpaceShip.transform != __instance.owner)
                {
                    return;
                }
            }

            if (Plugin.Settings.EnablePredictiveAiming.Value)
            {
                LookToPredictedTarget(__instance, __state, ref ___currSpeed);
            }

        }

        public static void LookToPredictedTarget(
            ProjectileControl __instance,
            InitialState __state,
            ref float ___currSpeed
            )
        {
            //Current velocity of target, this is a Vector3
            var targetVelocity = __instance.target.gameObject.GetComponent<Rigidbody>().velocity;

            //Current speed of target, this is a float showing units per second
            var targetSpeed = targetVelocity.magnitude;

            //Average the current speed and the maximum speed of this projectile
            //If only using the maximum speed the projectile tends to undershoot due to losses due to turning
            //If only using the current speed the predicted position is too far forward when still accelerating
            var assumeProjectileSpeed = (__instance.speed + ___currSpeed) * 0.5f;

            var projectilePosition = __instance.transform.position;

            //Set intial predictedPosition to current position of target, this will be refined in below loop
            var targetPredictedPosition = __instance.target.position;

            //Maximum times to iterate the predicted position
            const int maxIterations = 2;

            //Iterate over the estimated targeted position to trend towards more accurate solution
            for (int i = 0; i < maxIterations; i++)
            {
                //Distance from the projectile to the last predicted position
                var currentDistance = Vector3.Distance(targetPredictedPosition, projectilePosition);

                //Estimated time for projectile to travel to predicted position
                var estimatedTime = currentDistance / assumeProjectileSpeed;

                //If projectile is nearly out of fuel then lead less and tend to aim more directly at the target
                estimatedTime = Math.Min(estimatedTime, __instance.timeToDestroy * _finalBurnThreshold);

                //If the target is moving faster than the projectile maximum speed reduce the leading
                //If the target keeps moving away we will never hit no matter what, but if the
                //   target is rapidly changing direction then it will help to hit them sooner
                if (targetSpeed > __instance.speed)
                {
                    estimatedTime = Math.Min(1f, estimatedTime * 0.5f);
                }

                //Set new predicted position to where we think the target will 
                //  be at the time taken for the projectile to reach that point
                targetPredictedPosition = __instance.target.position + targetVelocity * estimatedTime;
            }

            //Relative position of the target compared to the projectile
            var relativePredictedPosition = Quaternion.LookRotation(targetPredictedPosition - projectilePosition);

            //Maximum turn speed for this update
            var turnSpeed = Time.deltaTime * __instance.turnSpeed * _maxTurnSpeed;

            //Turn projectile towards the predicted position of the target
            //Must use __state.rotation instead of the current projectile rotation as the rotation may have been modified
            //  by the base ProjectileControl already and keep the systems from intefering with each other
            __instance.transform.rotation = Quaternion.RotateTowards(__state.rotation, relativePredictedPosition, turnSpeed);
        }

    }
}

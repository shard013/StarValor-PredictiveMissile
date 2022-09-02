using BepInEx.Configuration;

namespace PredictiveMissile.Settings
{
    public class ConfigOptions
    {
        public const string PredictiveAimSection = "Section 01 - Predictive Aim";
        public ConfigEntry<bool> EnablePredictiveAiming;
        public ConfigEntry<bool> OnlyAimForPlayer;
        public ConfigEntry<float> MissileTurnSpeed;
        public ConfigEntry<float> FinalBurnThreshold;

        public const string MissileSpacingSection = "Section 02 - Missile Spacing";
        public ConfigEntry<bool> EnableMissileSpacing;
        public ConfigEntry<float> MissileSpacingDistance;
        public ConfigEntry<float> MissileSpacingTime;

        public static void LoadConfig(ConfigFile config)
        {
            // Section 01 - Predictive Aim
            Plugin.Settings.EnablePredictiveAiming = config.Bind(
                PredictiveAimSection,
                nameof(EnablePredictiveAiming),
                true,
                "Is Predictive Aiming enabled"
            );

            Plugin.Settings.OnlyAimForPlayer = config.Bind(
                PredictiveAimSection,
                nameof(OnlyAimForPlayer),
                false,
                "Predictive Aiming only allowed for the player (NPC missiles use default guidance if enabled)"
            );

            Plugin.Settings.MissileTurnSpeed = config.Bind(
                PredictiveAimSection,
                nameof(MissileTurnSpeed),
                15f,
                "Missile turn speed"
            );

            Plugin.Settings.FinalBurnThreshold = config.Bind(
                PredictiveAimSection,
                nameof(FinalBurnThreshold),
                0.9f,
                "When a missile has used this amount of fuel make a final burn directly to the target"
            );


            // Section 02 - Missile Spacing
            Plugin.Settings.EnableMissileSpacing = config.Bind(
                MissileSpacingSection,
                nameof(EnableMissileSpacing),
                true,
                "Is Projectile Spacing enabled"
            );

            Plugin.Settings.MissileSpacingDistance = config.Bind(
                MissileSpacingSection,
                nameof(MissileSpacingDistance),
                3f,
                "Minimum distance to to keep missiles with the same owner apart"
            );

            Plugin.Settings.MissileSpacingTime = config.Bind(
                MissileSpacingSection,
                nameof(MissileSpacingTime),
                1f,
                "Approximately how long in seconds will it take to move away from a fixed point"
            );

        }
    }

}

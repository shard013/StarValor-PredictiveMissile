using System.Collections.Generic;
using UnityEngine;

namespace PredictiveMissile.MissileSpacing
{
    public static class Spacing
    {
        private static readonly float _spacingDistance = Plugin.Settings.MissileSpacingDistance.Value;
        private static readonly float _spacingTime = Plugin.Settings.MissileSpacingTime.Value;
        private static readonly float _spacingForce = 1f / _spacingTime;

        public static Dictionary<Transform, List<ProjectileControl>> CharactersMissiles { get; set; } = new Dictionary<Transform, List<ProjectileControl>>();

        public static void FixedUpdate()
        {
            if (!Plugin.Settings.EnableMissileSpacing.Value)
            {
                return;
            }

            RemoveDestroyed();
            SpaceProjectiles();
        }

        public static void AddMissile(ProjectileControl missile)
        {
            if (!CharactersMissiles.ContainsKey(missile.owner))
            {
                CharactersMissiles.Add(missile.owner, new List<ProjectileControl>());
            }

            if (!CharactersMissiles[missile.owner].Contains(missile))
            {
                CharactersMissiles[missile.owner].Add(missile);
            }
        }

        public static void RemoveDestroyed()
        {
            var charactersToRemove = new List<Transform>();

            foreach (var charMissiles in CharactersMissiles)
            {
                if (charMissiles.Key == null ||
                    charMissiles.Key.transform == null ||
                    charMissiles.Key.gameObject == null)
                {
                    charactersToRemove.Add(charMissiles.Key);
                    continue;
                }

                charMissiles.Value.RemoveAll(m =>
                    m == null ||
                    m.owner == null ||
                    m.gameObject == null ||
                    m.transform == null
                );

                if (charMissiles.Value.Count == 0)
                {
                    charactersToRemove.Add(charMissiles.Key);
                    continue;
                }
            }

            foreach (var character in charactersToRemove)
            {
                CharactersMissiles.Remove(character);
            }

        }

        public static void SpaceProjectiles()
        {
            foreach (var missiles in CharactersMissiles.Values)
            {
                for (var i = 0; i < missiles.Count - 1; i++)
                {
                    for (var j = i + 1; j < missiles.Count; j++)
                    {
                        SpaceProjectilePair(missiles[i], missiles[j]);
                    }
                }
            }
        }

        public static void SpaceProjectilePair(ProjectileControl p1, ProjectileControl p2)
        {
            var p1Rigidbody = p1?.gameObject?.GetComponent<Rigidbody>();
            var p2Rigidbody = p2?.gameObject?.GetComponent<Rigidbody>();

            if (p1Rigidbody == null || p2Rigidbody == null)
            {
                return;
            }

            var distance = p1.transform.position - p2.transform.position;
            if (distance.magnitude < _spacingDistance)
            {
                var magnitude = _spacingDistance * _spacingForce;

                var p1force = p1.transform.position - p2.transform.position;
                p1force.Normalize();
                p1Rigidbody.position += p1force * magnitude * Time.deltaTime;

                var p2force = p2.transform.position - p1.transform.position;
                p2force.Normalize();
                p2Rigidbody.position += p2force * magnitude * Time.deltaTime;
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Acedia;

namespace Shmup
{
    // CONSIDER Wave: Position/Rotation variation

    [CreateAssetMenu(menuName = "Wave", order = AssetMenu.order)]
    public class Wave : ScriptableObject
    {
        public SimpleEnemy enemyPrefab;

        public Vector2 position = new Vector2();
        public float rotation = 180f;

        public float delay = 1f;
        public bool sequenced = true;

        public List<WaveEntry> entries = new List<WaveEntry>();

        public IEnumerator SpawnWave(WaveManager manager)
        {
            foreach (WaveEntry entry in entries)
            {
                bool sequenced = entry.customDelay ?
                    entry.sequenced : this.sequenced;

                float delay = entry.customDelay ?
                    entry.delay : this.delay;

                if (!sequenced) manager.StartCoroutine(SpawnEntry(entry, delay));
            }

            foreach (WaveEntry entry in entries)
            {
                bool sequenced = entry.customDelay ?
                    entry.sequenced : this.sequenced;

                float delay = entry.customDelay ?
                    entry.delay : this.delay;

                if (sequenced) yield return SpawnEntry(entry, delay);
            }

            yield break; // END

            IEnumerator SpawnEntry(WaveEntry entry, float delay)
            {
                yield return new WaitForSeconds(delay);

                Vector2 position = entry.customPosition ?
                    entry.position : this.position;

                Quaternion rotation = entry.customRotation ?
                    Quaternion.Euler(0f, 0f, entry.rotation):
                    Quaternion.Euler(0f, 0f, this.rotation);

                SimpleEnemy enemy = Instantiate(enemyPrefab, position, rotation);
                enemy.SetupShip(entry.enemyData);
            }
        }
    }

    [System.Serializable]
    public struct WaveEntry
    {
        public SimpleEnemyData enemyData;

        public bool customPosition;

        [ShowIf(nameof(customPosition))]
        public Vector2 position;

        public bool customRotation;

        [ShowIf(nameof(customRotation))]
        public float rotation;

        [Space]
        public bool customDelay;

        [ShowIf(nameof(customDelay))]
        public float delay;

        [ShowIf(nameof(customDelay))]
        public bool sequenced;
    }
}

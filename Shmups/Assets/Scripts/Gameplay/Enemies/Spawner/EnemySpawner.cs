using System.Collections;
using System.Collections.Generic;
using Acedia;
using UnityEngine;

namespace Shmup
{
    public class EnemySpawner : MonoBehaviour
    {
        [field: SerializeField]
        public List<SpawnerEntry> Entries { get; private set; }

        [field: SerializeField]
        public float MinSpawnDelay { get; private set; }

        [field: SerializeField]
        public float MaxSpawnDelay { get; private set; }

        [field: SerializeField]
        public int MaxSpawnCount { get; private set; }

        public float NextSpawnTime { get; private set; }

        // TODO: Decouple Pooling


        private void Update()
        {
            if (Entries.Count <= 0) return;
            if (Time.time < NextSpawnTime) return;

            List<SpawnerEntry> entries = new List<SpawnerEntry>(Entries);
            int count = Random.Range(1, MaxSpawnCount);

            for (int i = 0; i < count; i++)
            {
                if (entries.Count <= 0) entries.AddRange(entries);

                int rnd = Random.Range(0, entries.Count);
                SpawnerEntry entry = entries[rnd];
                entries.QuickRemoveAt(rnd);

                Spawn(entry);
            }
        }

        private void Spawn(SpawnerEntry entry)
        {
            int rnd = Random.Range(0, entry.prefabs.Length);
            GameObject prefab = entry.prefabs[rnd];

            rnd = Random.Range(0, entry.spawnAreas.Length);
            SpawnArea area = entry.spawnAreas[rnd];

            Instantiate(prefab, area.GetRandomPoint(), area.transform.rotation);
            NextSpawnTime = Time.time + Random.Range(MinSpawnDelay, MaxSpawnDelay);
        }

        private void OnValidate()
        {
            MaxSpawnCount = Mathf.Max(MaxSpawnCount, 1);
        }

        [System.Serializable]
        public struct SpawnerEntry
        {
            public SpawnArea[] spawnAreas;
            public GameObject[] prefabs;
        }
    }
}

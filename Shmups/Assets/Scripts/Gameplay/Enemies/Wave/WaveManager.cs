using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Shmup
{
    public class WaveManager : MonoBehaviour
    {
        public List<Wave> possibleWaves = new List<Wave>();

        public float minWaveDelay = 1f;
        public float maxWaveDelay = 3f;

        public float NextWaveTime { get; private set; }

        private void Awake()
        {
            NextWaveTime = Time.time + Random.Range(minWaveDelay, maxWaveDelay);
        }

        private void Update()
        {
            if (Time.time >= NextWaveTime) SpawnWave();
        }

        public void SpawnWave()
        {
            int i = Random.Range(0, possibleWaves.Count);
            Wave wave = possibleWaves[i];

            StartCoroutine(wave.SpawnWave(this));

            NextWaveTime = Time.time + Random.Range(minWaveDelay, maxWaveDelay);
        }
    }
}

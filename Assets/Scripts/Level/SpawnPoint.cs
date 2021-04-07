using System;
using System.Collections;
using System.Collections.Generic;
using CustomHelper;
using Managers;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level
{
    [Serializable]
    public struct SpawnInfo
    {
        [HideLabel]
        public GameObject gameObject;
        [MinValue(0.001f),MaxValue(100f)]
        public float rate;
    }
    
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField]
        private List<SpawnInfo> toSpawn;

        private void Awake()
        {
            GameManager.OnReplayGameReadyIn += RespawnObject;
            Refresh();
            StartCoroutine(Spawn());
        }

        private void OnDestroy()
        {
            GameManager.OnReplayGameReadyIn -= RespawnObject;
        }

        private void RespawnObject(float value)
        {
            for (int i = 0; i < transform.childCount; i++)
                Destroy(transform.GetChild(i).gameObject);

            StartCoroutine(Spawn());
        }
        
        private IEnumerator Spawn()
        {
            Instantiate(GetObjectToInstantiate(), transform.position, quaternion.identity).transform
                .SetParent(transform);
            yield return null;
        }

        private GameObject GetObjectToInstantiate()
        {
            var defaultObject = toSpawn.Count > 0 ? toSpawn[toSpawn.Count - 1].gameObject : null;
            var rateSeed = Random.Range(0.001f, 100f);
            var randomObject = toSpawn.Find(info => rateSeed <= info.rate).gameObject;

            return randomObject ? randomObject : defaultObject;
        }

        [Button(ButtonSizes.Large)]
        private void Refresh()
        {
            toSpawn.Sort((x, y) => x.rate.CompareTo(y.rate));
        }
    }
}
using System.Collections.Generic;
using CustomHelper;
using Unity.Mathematics;
using UnityEngine;

namespace Level
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> spawnables;

        private void Awake()
        {
            Instantiate(spawnables.GetRandom(), transform.position, quaternion.identity);
        }
    }
}
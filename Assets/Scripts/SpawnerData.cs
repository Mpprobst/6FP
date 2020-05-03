using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerData", menuName = "ScriptableObjects/SpawnerData", order = 0)]
public class SpawnerData : ScriptableObject
{
    public EnemyData[] enemies;

    [System.Serializable]
    public class EnemyData
    {
        public GameObject prefab;
        public int weightedProbability;
    }
}

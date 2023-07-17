using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyList", menuName = "Cardasia/EnemyList")]
public class SC_EnemyList : ScriptableObject
{
    public string ListName;
    public int MinSpawn, MaxSpawn;

    [System.Serializable]
    public class Enemy
    {
        public string name;
        public int SpawnChance;
        public int MinLevel, MaxLevel;
        public SC_Character scEnemy;
    }
    public Enemy[] EnemyList;
}

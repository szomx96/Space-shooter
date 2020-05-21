using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemyPlayerPrefab;

    public int startEnemiesCount = 20;
    public int minEnemiesCount = 5;

    //number of enemies to generate when minEnemiesCount is reached
    public int enemiesToGenerate = 10;

    private int enemiesCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        GenerateEnemies(startEnemiesCount);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesCount <= minEnemiesCount)
        {
            GenerateEnemies(enemiesToGenerate);
        }
    }

    private void GenerateEnemies(int count)
    {
        enemiesCount += count;
        //Debug.Log("enemies count: " + enemiesCount);

        for (int i = 0; i < count; i++)
        {
            GameObject enemyPlayer = Instantiate(enemyPlayerPrefab) as GameObject;

            enemyPlayer.transform.position = new Vector3(
                    Random.Range(-10.0f, 10.0f),
                    Random.Range(0.5f, 5.0f),
                    Random.Range(-10.0f, 10.0f)
                );
        }
    }

    public void decrementEnemyCount()
    {
        enemiesCount--;
    }



}

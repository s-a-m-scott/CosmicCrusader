using UnityEngine;
using System.Collections.Generic;
public class EnemySpawner : MonoBehaviour
{
    public Level currentLevel;
    public GameObject player;
    public GameManager manager;
    public static float spawnDistance = 11;
    private float levelStart;
    void Start()
    {
        GameObject managerObject = GameObject.FindWithTag("GameController");
        manager = managerObject.GetComponent<GameManager>();

        currentLevel = manager.levels[manager.currentWave];
        levelStart = Time.time;
    }


    void Update()
    {
        
        for (int i = 0; i < manager.levels[manager.currentWave].waveData.Count; i++)
        { 
            Wave wave = manager.levels[manager.currentWave].waveData[i];
            for (int j = 0; j < wave.spawnedEnemies.Count; j++)
            {
                GameObject _enemy = wave.spawnedEnemies[j];
                if (!_enemy) wave.spawnedEnemies.Remove(_enemy);
            }
            if (levelStart + wave.startTime < Time.time && levelStart + wave.endTime > Time.time)
            {
                int enemiesToSpawn = wave.maxSpawns - wave.enemiesBeingSpawned;
                
                if (wave.spawnedEnemies.Count + wave.enemiesBeingSpawned < wave.maxSpawns)
                { 
                    //Debug.Log(wave.enemiesBeingSpawned);
                    float waitTime = enemiesToSpawn * wave.spawnRate;
                    StartCoroutine(SpawnEnemyRoutine(wave, waitTime, player.transform));
                }
            }
        }
    }

    private IEnumerator<WaitForSeconds> SpawnEnemyRoutine(Wave wave, float waitTime, Transform playerPos)
    {
        wave.enemiesBeingSpawned++;
        yield return new WaitForSeconds(waitTime);
        if (wave.enemiesBeingSpawned != 0 && levelStart + wave.endTime > Time.time)//dont spawn enemy after wave is finished
        {
            wave.spawnedEnemies.Add(SpawnEnemy(wave, playerPos));
            wave.enemiesBeingSpawned--;
        }
    }

    private GameObject SpawnEnemy(Wave wave,Transform playerPos)
    {
        float pMoveDir = playerPos.rotation.eulerAngles.z;
        float angleOffset = Random.Range(-45, 45);
        pMoveDir += angleOffset;
        Quaternion spawnDir = Quaternion.AngleAxis(pMoveDir, Vector3.forward);

        Vector3 spawnOffset = spawnDir * Vector3.right * spawnDistance;
        GameObject enemySpawned = Instantiate(wave.enemy, playerPos.position + spawnOffset, new Quaternion());
        EnemyBase _enemy = enemySpawned.GetComponent<EnemyBase>();
        _enemy.scaleFactor = manager.scaleFactor;
        return enemySpawned;
    }
}

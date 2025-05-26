using UnityEngine;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int currentWave = 1;
    public int waveUnlocked = 1;
    public int currentMoney = 0;
    public float scaleFactor;
    public PlayerStats currentStats;
    public List<GameObject> enemies;
    //handles information passed between scenes
    public static GameManager Instance;

    public const int levelsCount = 20;
    public Level[] levels = new Level[levelsCount];

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        currentStats = new PlayerStats();
        GenerateLevels();
    }

    private void GenerateLevels()
    {//fill level array with levels. would be better to do this from a JSON file or something but alas, i am time restricted

        //level 1
        List<Wave> levelTemp = new List<Wave>();
        levelTemp.Add(new Wave(0, 10, enemies[0], 1, 5));
        levelTemp.Add(new Wave(10, 20, enemies[0], 1f, 8));

        levels[0] = new Level(levelTemp, 20);
        levelTemp.Clear();

        //level 2
        levelTemp.Add(new Wave(0, 5, enemies[1], 1, 2));
        levelTemp.Add(new Wave(5, 12, enemies[0], 0.7f, 5));
        levelTemp.Add(new Wave(5, 25, enemies[1], 0.5f, 3));
        levelTemp.Add(new Wave(12, 25, enemies[0], 0.5f, 8));
        levels[1] = new Level(levelTemp, 25);
        levelTemp.Clear();
        
        //level 3
        levelTemp.Add(new Wave(0, 10, enemies[0], 1f, 5));
        levelTemp.Add(new Wave(10, 30, enemies[0], 0.5f, 8));
        levelTemp.Add(new Wave(5, 20, enemies[1], 0.5f, 2));
        levelTemp.Add(new Wave(20, 30, enemies[1], 0.5f, 5));
        levelTemp.Add(new Wave(0, 5, enemies[2], 1f, 3));
        levelTemp.Add(new Wave(5, 30, enemies[2], 0.5f, 4));
        levels[2] = new Level(levelTemp, 30);
        levelTemp.Clear();

        //level 4
        levelTemp.Add(new Wave(0, 28, enemies[0], 1f, 5));
        levelTemp.Add(new Wave(0, 14, enemies[1], 0.5f, 5));
        levelTemp.Add(new Wave(14, 28, enemies[1], 0.5f, 10));
        levelTemp.Add(new Wave(28, 35, enemies[2], 0.25f, 20));
        levels[3] = new Level(levelTemp, 35);
        levelTemp.Clear();

        //level 5
        levelTemp.Add(new Wave(15, 20, enemies[1], 1f, 3));
        levelTemp.Add(new Wave(25, 40, enemies[1], 0.5f, 5));
        levelTemp.Add(new Wave(20, 30, enemies[2], 1f, 3));
        levelTemp.Add(new Wave(0, 5, enemies[3], 1f, 1));
        levelTemp.Add(new Wave(5, 10, enemies[3], 1f, 2));
        levelTemp.Add(new Wave(10, 20, enemies[3], 1f, 3));
        levelTemp.Add(new Wave(20, 30, enemies[3], 1f, 4));
        levelTemp.Add(new Wave(30, 40, enemies[3], 1f, 5));
        levels[4] = new Level(levelTemp, 40);
        levelTemp.Clear();

        //level 6
        levelTemp.Add(new Wave(12, 45, enemies[0], 0.5f, 15));
        levelTemp.Add(new Wave(12, 45, enemies[1], 1f, 5));
        levelTemp.Add(new Wave(0, 1, enemies[3], 0.2f, 5));
        levelTemp.Add(new Wave(7, 8, enemies[3], 0.2f, 5));
        levelTemp.Add(new Wave(15, 45, enemies[3], 1f, 3));
        levels[5] = new Level(levelTemp, 45);
        levelTemp.Clear();

        //level 7
        levelTemp.Add(new Wave(0, 12, enemies[0], 0.5f, 10));
        levelTemp.Add(new Wave(12, 25, enemies[1], 1f, 5));
        levelTemp.Add(new Wave(25, 35, enemies[2], 1f, 8));
        levelTemp.Add(new Wave(35, 50, enemies[3], 1f, 3));
        levelTemp.Add(new Wave(0, 50, enemies[4], 0.5f, 12));
        levels[6] = new Level(levelTemp, 50);
        levelTemp.Clear();

        //level 8
        levelTemp.Add(new Wave(0, 45, enemies[0], 0.3f, 5));
        levelTemp.Add(new Wave(5, 45, enemies[1], 5f, 2));
        levelTemp.Add(new Wave(0, 45, enemies[3], 3f, 3));
        levelTemp.Add(new Wave(0, 3, enemies[4], 0.3f, 15));
        levelTemp.Add(new Wave(10, 13, enemies[4], 0.3f, 15));
        levelTemp.Add(new Wave(20, 23, enemies[4], 0.3f, 15));
        levelTemp.Add(new Wave(30, 33, enemies[4], 0.3f, 15));
        levelTemp.Add(new Wave(40, 43, enemies[4], 0.3f, 15));
        levelTemp.Add(new Wave(50, 55, enemies[4], 0.1f, 30));
        levels[7] = new Level(levelTemp, 55);
        levelTemp.Clear();

        //level 9
        levelTemp.Add(new Wave(0, 5, enemies[0], 0.1f, 30));
        levelTemp.Add(new Wave(10, 12, enemies[1], 0.1f, 10));
        levelTemp.Add(new Wave(20, 25, enemies[2], 0.1f, 20));
        levelTemp.Add(new Wave(30, 31, enemies[3], 0.1f, 8));
        levelTemp.Add(new Wave(40, 45, enemies[4], 0.1f, 15));

        levelTemp.Add(new Wave(50, 51, enemies[0], 0.1f, 10));
        levelTemp.Add(new Wave(50, 51, enemies[1], 0.1f, 10));
        levelTemp.Add(new Wave(50, 51, enemies[2], 0.1f, 10));
        levelTemp.Add(new Wave(50, 51, enemies[3], 0.1f, 10));
        levelTemp.Add(new Wave(50, 51, enemies[4], 0.1f, 10));
        levels[8] = new Level(levelTemp, 60);
        levelTemp.Clear();

        //level 10
        levelTemp.Add(new Wave(15, 30, enemies[1], 0.5f, 5));
        levelTemp.Add(new Wave(45, 60, enemies[1], 0.5f, 3));
        levelTemp.Add(new Wave(30, 45, enemies[2], 1f, 6));
        levelTemp.Add(new Wave(45, 60, enemies[3], 1f, 3));

        levelTemp.Add(new Wave(0, 15, enemies[5], 1f, 2));
        levelTemp.Add(new Wave(15, 30, enemies[5], 1f, 4));
        levelTemp.Add(new Wave(30, 45, enemies[5], 1f, 6));
        levelTemp.Add(new Wave(45, 60, enemies[5], 1f, 8));
        levels[9] = new Level(levelTemp, 60);
        levelTemp.Clear();

        //level 11
        levelTemp.Add(new Wave(0, 20, enemies[0], 0.5f, 10));
        levelTemp.Add(new Wave(0, 20, enemies[1], 0.5f, 5));
        levelTemp.Add(new Wave(20, 60, enemies[1], 0.5f, 2));
        levelTemp.Add(new Wave(20, 60, enemies[2], 1f, 6));
        levelTemp.Add(new Wave(0, 20, enemies[3], 2f, 3));
        levelTemp.Add(new Wave(20, 40, enemies[3], 2f, 6));

        levelTemp.Add(new Wave(15, 25, enemies[5], 1f, 2));
        levelTemp.Add(new Wave(30, 60, enemies[5], 1f, 5));
        levels[10] = new Level(levelTemp, 60);
        levelTemp.Clear();

        //level 12
        levelTemp.Add(new Wave(10, 30, enemies[1], 0.5f, 3));
        levelTemp.Add(new Wave(30, 60, enemies[1], 0.5f, 5));
        levelTemp.Add(new Wave(15, 60, enemies[2], 3f, 1));
        levelTemp.Add(new Wave(20, 60, enemies[3], 3f, 2));

        levelTemp.Add(new Wave(20, 21, enemies[5], 0.1f, 3));
        levelTemp.Add(new Wave(40, 41, enemies[5], 0.1f, 3));

        levelTemp.Add(new Wave(0, 60, enemies[6], 1f, 5));
        levels[11] = new Level(levelTemp, 60);
        levelTemp.Clear();

        //level 13
        levelTemp.Add(new Wave(0, 15, enemies[4], 1f, 4));
        levelTemp.Add(new Wave(15, 30, enemies[4], 1f, 8));
        levelTemp.Add(new Wave(30, 45, enemies[4], 1f, 12));
        levelTemp.Add(new Wave(45, 60, enemies[4], 1f, 16));

        levelTemp.Add(new Wave(0, 20, enemies[6], 1f, 3));
        levelTemp.Add(new Wave(20, 40, enemies[6], 1f, 6));
        levelTemp.Add(new Wave(40, 60, enemies[6], 1f, 10));
        levels[12] = new Level(levelTemp, 60);
        levelTemp.Clear();

        //level 14
        levelTemp.Add(new Wave(10, 25, enemies[0], 1f, 10));
        levelTemp.Add(new Wave(25, 40, enemies[2], 1f, 5));
        levelTemp.Add(new Wave(40, 60, enemies[2], 1f, 2));
        levelTemp.Add(new Wave(30, 31, enemies[5], 0.1f, 8));
        levelTemp.Add(new Wave(40, 60, enemies[6], 1f, 5));
        levelTemp.Add(new Wave(0, 10, enemies[7], 1f, 3));
        levelTemp.Add(new Wave(10, 40, enemies[7], 1f, 6));
        levelTemp.Add(new Wave(40, 60, enemies[7], 1f, 10));
        levels[13] = new Level(levelTemp, 60);
        levelTemp.Clear();

        //level 15
        levelTemp.Add(new Wave(0, 60, enemies[0], 2f, 3));
        levelTemp.Add(new Wave(0, 60, enemies[2], 2f, 6));
        levelTemp.Add(new Wave(0, 1, enemies[3], 0.1f, 3));
        levelTemp.Add(new Wave(20, 21, enemies[3], 0.1f, 3));
        levelTemp.Add(new Wave(30, 31, enemies[3], 0.1f, 4));
        levelTemp.Add(new Wave(40, 41, enemies[3], 0.1f, 5));
        levelTemp.Add(new Wave(50, 51, enemies[3], 0.1f, 6));
        
        levelTemp.Add(new Wave(25, 26, enemies[5], 0.1f, 5));
        levelTemp.Add(new Wave(45, 46, enemies[5], 0.1f, 5));

        levelTemp.Add(new Wave(2, 3, enemies[7], 0.1f, 3));
        levelTemp.Add(new Wave(22, 23, enemies[7], 0.1f, 3));
        levelTemp.Add(new Wave(32, 33, enemies[7], 0.1f, 4));
        levelTemp.Add(new Wave(42, 43, enemies[7], 0.1f, 5));
        levelTemp.Add(new Wave(52, 53, enemies[7], 0.1f, 6));
        levels[14] = new Level(levelTemp, 60);
        levelTemp.Clear();

        //level 16
        levelTemp.Add(new Wave(10, 40, enemies[1], 1f, 2));
        levelTemp.Add(new Wave(30, 45, enemies[3], 2f, 2));

        levelTemp.Add(new Wave(0, 20, enemies[8], 1f, 3));
        levelTemp.Add(new Wave(20, 40, enemies[8], 1f, 5));
        levelTemp.Add(new Wave(40, 60, enemies[8], 1f, 12));
        levels[15] = new Level(levelTemp, 60);
        levelTemp.Clear();

        //level 17
        levelTemp.Add(new Wave(10, 40, enemies[1], 1f, 2));
        levelTemp.Add(new Wave(30, 45, enemies[3], 2f, 2));

        levelTemp.Add(new Wave(0, 20, enemies[8], 1f, 3));
        levelTemp.Add(new Wave(20, 40, enemies[8], 1f, 5));
        levelTemp.Add(new Wave(40, 60, enemies[8], 1f, 12));
        levels[16] = new Level(levelTemp, 60);
        levelTemp.Clear();

        //level 18
        levelTemp.Add(new Wave(15, 16, enemies[0], 0.05f, 15));
        levelTemp.Add(new Wave(35, 36, enemies[0], 0.05f, 15));
        levelTemp.Add(new Wave(15, 60, enemies[1], 2f, 1));
        levelTemp.Add(new Wave(0, 60, enemies[3], 2f, 5));
        levelTemp.Add(new Wave(20, 60, enemies[4], 2f, 8));
        levelTemp.Add(new Wave(5, 6, enemies[5], 0.1f, 5));
        levelTemp.Add(new Wave(10, 35, enemies[7], 2f, 2));
        levelTemp.Add(new Wave(35, 60, enemies[7], 1f, 4));
        levelTemp.Add(new Wave(0, 60, enemies[8], 3f, 1));
        levels[17] = new Level(levelTemp, 60);
        levelTemp.Clear();

        //level 19
        levelTemp.Add(new Wave(0, 90, enemies[0], 2f, 3));
        levelTemp.Add(new Wave(0, 90, enemies[1], 2f, 2));
        levelTemp.Add(new Wave(10, 90, enemies[2], 1f, 3));
        levelTemp.Add(new Wave(51, 52, enemies[2], 0.05f, 15));
        levelTemp.Add(new Wave(81, 82, enemies[2], 0.05f, 15));
        levelTemp.Add(new Wave(20, 60, enemies[3], 1f, 4));
        levelTemp.Add(new Wave(60, 90, enemies[3], 1f, 2));
        levelTemp.Add(new Wave(25, 90, enemies[4], 1f, 5));
        levelTemp.Add(new Wave(42, 43, enemies[4], 0.05f, 15));
        levelTemp.Add(new Wave(72, 73, enemies[4], 0.05f, 15));
        levelTemp.Add(new Wave(32, 90, enemies[5], 3f, 2));
        levelTemp.Add(new Wave(35, 36, enemies[5], 0.05f, 15));
        levelTemp.Add(new Wave(65, 66, enemies[5], 0.05f, 15));
        levelTemp.Add(new Wave(60, 90, enemies[6], 1f, 6));
        levelTemp.Add(new Wave(45, 90, enemies[7], 1f, 3));
        levelTemp.Add(new Wave(40, 90, enemies[8], 1f, 1));
        levelTemp.Add(new Wave(60, 90, enemies[8], 1f, 3));
        levels[18] = new Level(levelTemp, 90);
        levelTemp.Clear();

        //level 20
        levelTemp.Add(new Wave(3, 4, enemies[9], 0.1f, 1));
        levels[19] = new Level(levelTemp, -1, true);
        levelTemp.Clear();
    }
}

public class Level
{
    public List<Wave> waveData;
    public float duration;
    public bool isBossWave;

    public Level(List<Wave> _waves, float _duration, bool _boss = false)
    {
        this.isBossWave = _boss;
        this.waveData = new List<Wave>(_waves);
        this.duration = _duration;
        this.isBossWave = _boss;
    }
}
public class Wave
{
    //waves within levels describe spawning patterns of enemies

    public float startTime;
    public float endTime;
    public GameObject enemy;
    public float spawnRate;
    public List<GameObject> spawnedEnemies;
    public int maxSpawns;
    public bool isBossWave;

    public int enemiesBeingSpawned = 0;//enemies in the process of being spawned

    public Wave(float _start, float _end, GameObject _enemy, float _rate, int _max)
    {
        this.startTime = _start;
        this.endTime = _end;
        this.enemy = _enemy;
        this.spawnRate = _rate;
        this.maxSpawns = _max;
        spawnedEnemies = new List<GameObject>();
        
    }

}

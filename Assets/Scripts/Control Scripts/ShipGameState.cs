using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ShipGameState : MonoBehaviour
{
    #region properties etc
    public GameManager manager;
    public GameObject enemySpawnObject;
    public PlayerActions player;
    //displays
    public TextMeshProUGUI waveDisplay;
    public TextMeshProUGUI healthDisplay;
    public TextMeshProUGUI gameOverDisplay;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI timeDisplay;
    public TextMeshProUGUI levelWinDisplay;
    public Slider healthBar;
    public Button upgradeMenu;//go to upgrade menu
    public Button retryLevelButton;
    public bool levelWon = false;
    public float levelTime = 60;//seconds
    public bool bossWaveWin = false;
    public bool isBossWave = false;
    double startTime;
    #endregion
    gameState state = gameState.play;
    void Start()
    {
        GameObject managerObject = GameObject.FindWithTag("GameController");
        manager = managerObject.GetComponent<GameManager>();
        manager.scaleFactor = Mathf.Pow(1.1f, manager.currentWave); //cumulative 10% per wave in enemy hp, dmg and money
        
        //Debug.Log("factor: "+manager.scaleFactor);
        enemySpawnObject.GetComponent<EnemySpawner>().currentLevel = manager.levels[manager.currentWave];
        levelTime = manager.levels[manager.currentWave].duration;
        isBossWave = manager.levels[manager.currentWave].isBossWave;

        Next(gameState.play);
    }

    void Update()
    {
        int timePassed = (int)(Time.time-startTime);
        switch (state) {
            case gameState.play: {
                //manage gui
                healthDisplay.text = "Health: " + player.currentHealth;
                healthBar.value = (float)(player.currentHealth / player.stats[(int)statNames.maxHealth]);
                scoreDisplay.text = "Money: " + Convert.ToString(manager.currentMoney);
                timeDisplay.text = Convert.ToString(levelTime - timePassed);

                //state change
                if ((levelTime-timePassed <= 0 && !isBossWave) || (bossWaveWin)) { Next(gameState.win); }
                if (player.currentHealth <= 0) { Next(gameState.lose); }
            } break;
            case gameState.win: {
                //go to upgrade screen
            } break;
            case gameState.lose: {
                //restart - on button
                
            } break;
        }
    }

    public void moveToUpgrade() {
        if (state == gameState.win) manager.waveUnlocked = Math.Max(manager.waveUnlocked, manager.currentWave + 2);//dont want to reduce unlock if replaying earlier wave
        //Debug.Log("wave:" + manager.currentWave + "     unlocked:" + manager.waveUnlocked);
        SceneManager.LoadScene("UpgradeScene");
    }

    void Next(gameState _state) {
        state = _state;
        switch (_state) {
            case gameState.win:
            {
                timeDisplay.gameObject.SetActive(false);
                healthDisplay.gameObject.SetActive(false);
                healthBar.gameObject.SetActive(false);
                scoreDisplay.gameObject.SetActive(false);
                levelWinDisplay.gameObject.SetActive(true);
                upgradeMenu.gameObject.SetActive(true);

                //player stuff
                player.alive = false;
                player.GetComponent<Collider>().enabled = false;
                player.GetComponent<SpriteRenderer>().enabled = false;
                
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = (AudioClip)Resources.Load("Sounds/level_win");
                audioSource.volume = 0.1f;
                audioSource.Play();
            } return;
            case gameState.lose:
            {
                gameOverDisplay.gameObject.SetActive(true);
                healthDisplay.gameObject.SetActive(false);
                healthBar.gameObject.SetActive(false);
                scoreDisplay.gameObject.SetActive(false);
                timeDisplay.gameObject.SetActive(false);
                retryLevelButton.gameObject.SetActive(true);

                //player stuff
                player.alive = false;
                player.GetComponent<Collider>().enabled = false;
                player.GetComponent<SpriteRenderer>().enabled = false;

                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = (AudioClip)Resources.Load("Sounds/level_lose");
                audioSource.volume = 0.1f;
                audioSource.Play();
            } return;
            case gameState.play: {
                if (isBossWave) timeDisplay.gameObject.SetActive(false); //no timer for boss wave - have to beat it
                waveDisplay.text = "LEVEL " + (manager.currentWave+1);
                startTime = Time.time;
            } return;
        }
    }

    enum gameState {
        win,lose,play
    }
}

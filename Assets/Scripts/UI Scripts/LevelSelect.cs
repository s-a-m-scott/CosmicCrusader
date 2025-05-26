using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public GameObject quitButton;
    public string gameScene = "GameScene";
    public GameObject upgradeButton;//button to go to upgrades
    public GameManager manager;
    public GameObject canvasObject;

    public TextMeshProUGUI moneyDisplay;
    public List<GameObject> levelButtons;
    public GameObject levelButtonTemplate;


    private static int columns = 5;

    void Start()
    {
        GameObject managerObject = GameObject.FindWithTag("GameController");
        manager = managerObject.GetComponent<GameManager>();   
        moneyDisplay.text = "$"+manager.currentMoney;

        RectTransform canvTrans = canvasObject.GetComponent<RectTransform>();
        float buttonSep = canvTrans.rect.width * 0.02f;//space between button is x% of screen width
        float buttonHeight = canvTrans.rect.height * 0.1f;
        float buttonWidth = canvTrans.rect.width * 0.18f;

        float startX = -canvTrans.rect.width*0.88f;//numbers are random because unity is wack
        float startY = canvTrans.rect.height * 0.3f;
        float rows = GameManager.levelsCount / columns;

        int buttonID = 0;
        Vector2 pos = new Vector2(); LevelSelectButton scriptComp;

        for (int j = 0; j < rows; j++)
        {
            for (int i = 0; i < columns; i++)
            {
                //create and position button
                pos.x = startX + (i * (buttonWidth));
                pos.y = -startY - (j * (buttonHeight + buttonSep));
                levelButtons.Add(Instantiate(levelButtonTemplate, canvasObject.transform));
                levelButtons[buttonID].transform.localPosition = pos;

                scriptComp = levelButtons[buttonID].GetComponent<LevelSelectButton>();//get script component
                

                if (buttonID >= manager.waveUnlocked) { scriptComp.upgradeButton.interactable = false; }
                //else Debug.Log("id:" + buttonID + "     unlocked:" + manager.waveUnlocked);
                scriptComp.buttonText.text = "LEVEL " + (buttonID + 1);
                scriptComp.levelID = buttonID;
                scriptComp.LScontroller = this;
                buttonID++;
            }
        }
    }


    public void GoToUpgrade()
    {
        SceneManager.LoadScene("UpgradeScene");
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void GoToLevel(int level)
    {
        manager.currentWave = level;
        SceneManager.LoadScene("GameScene");
    }
}

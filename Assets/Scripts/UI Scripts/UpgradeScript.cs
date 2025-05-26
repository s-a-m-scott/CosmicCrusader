using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
public class UpgradeScript : MonoBehaviour
{
    public GameManager manager;
    public TextMeshProUGUI moneyDisplay;
    public GameObject canvasObject;

    public List<GameObject> upgradeButtons;
    public GameObject upgradeTemplate;


    void Start() {
        RectTransform canvTrans = canvasObject.GetComponent<RectTransform>();
        Vector2 offsets = new Vector2(canvTrans.rect.width * 0.22f, -canvTrans.rect.height * 0.15f);
        float startY = canvTrans.rect.height * 0.78f;
        float startX = 0f;//0? why on earth does this one work at 0 and the other doesnt
        //find manager
        GameObject managerObject = GameObject.FindWithTag("GameController");
        manager = managerObject.GetComponent<GameManager>();

        //assign buttons
        
        Vector3 pos; UpgradeOptionScript upgrader;
        for (int i = 0; i < Enum.GetNames(typeof(statNames)).Length; i++)
        {
            pos = new Vector3(startX, canvTrans.rect.height - startY, 0);
            pos += new Vector3(offsets.x * (((i % 2) * 2) - 1), //gives -1 or +1 for x multiplier
            offsets.y * (i / 2), 0);
            upgradeButtons.Add(Instantiate(upgradeTemplate, canvasObject.transform));
            upgradeButtons[i].transform.localPosition = pos;

            upgrader = upgradeButtons[i].GetComponent<UpgradeOptionScript>();
            upgrader.statData = manager.currentStats.statValues[i];
            upgrader.upgradeManager = this.gameObject;
            upgrader.manager = this.manager;
        }
        //check what can/cant be bought
        EvaluatePrices();

    }

    void Update()
    {
        
    }

    public void GoToLevelSelect() {
        SceneManager.LoadScene("LevelSelectScene");
    }

    public void EvaluatePrices() {//check to disable buttons if cant afford
        UpgradeOptionScript upgrader;
        for (int i=0; i<Enum.GetNames(typeof(statNames)).Length; i++) {
            Console.WriteLine(i);
            upgrader = upgradeButtons[i].GetComponent<UpgradeOptionScript>();
            upgrader.Evaluate();
        }
        moneyDisplay.text = "$"+manager.currentMoney;
    }
}

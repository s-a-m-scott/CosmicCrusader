using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeOptionScript : MonoBehaviour
{
    public GameManager manager;
    public TextMeshProUGUI upgradeLabel;
    public TextMeshProUGUI upgradeLevel;
    public Button upgradeButton;
    public TextMeshProUGUI buttonText;
    public GameObject upgradeManager;

    public Stat statData;//stat that this upgrade button is for
    //public statNames stat;

    void Start() {
        
    }

    public void Upgrade() {//should be unable to call this if current_level too high, so not check here
        manager.currentMoney -= statData.upgradeLevels[statData.currentLevel+1].price;
        statData.currentLevel = statData.currentLevel+1;
        upgradeLevel.text = statData.currentLevel+"/"+(statData.upgradeLevels.Length-1);
        upgradeManager.GetComponent<UpgradeScript>().EvaluatePrices();
    }

    public void Evaluate() {//check if can be bought / update display
        upgradeLabel.text = statData.label;
        upgradeLevel.text = statData.currentLevel+"/"+(statData.upgradeLevels.Length-1); 
        if (statData.currentLevel >= statData.upgradeLevels.Length-1) {
            upgradeButton.interactable = false;
            buttonText.text = "MAX LEVEL";
            return;
        }
        buttonText.text = "$"+statData.upgradeLevels[statData.currentLevel+1].price;
        
        if (manager.currentMoney < statData.upgradeLevels[statData.currentLevel+1].price) {
            upgradeButton.interactable = false;
            //dont change text - need to see value to know cant afford
        }
        else {upgradeButton.interactable = true;}
        
    }
}

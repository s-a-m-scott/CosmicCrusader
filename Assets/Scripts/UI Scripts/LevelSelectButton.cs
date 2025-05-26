using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    public Button upgradeButton;
    public TextMeshProUGUI buttonText;
    public LevelSelect LScontroller;
    public int levelID;

    public void StartLevel()
    {
        LScontroller.GoToLevel(levelID);
    }

}

using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    public void playButtonPress() {
        SceneManager.LoadScene("LevelSelectScene");
    }
    public void HowToButtonPress() {
        SceneManager.LoadScene("HowToPlayScene");
    }
    public void quitButtonPress()
    {
        Application.Quit();
    }
}

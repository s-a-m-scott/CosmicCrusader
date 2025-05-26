using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayScript : MonoBehaviour
{
    public void quitButtonPress() {
        SceneManager.LoadScene("TitleScene");
    }
}

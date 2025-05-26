using TMPro;

using UnityEngine;
using UnityEngine.UI;

//creates a pop when collecting an item, describing the score added to total
public class ItemCollectDisplay : MonoBehaviour
{
    public float displayTime = 1.2f;
    public const float MIN_ALPHA = 0.05f;
    public float FLOAT_SPEED = 3f;
    public TextMeshPro textDisplay;

    float endTime = 0;
    void Start()
    {
        endTime = Time.time + displayTime;
    }


    void Update()
    {
        
        textDisplay.rectTransform.anchoredPosition3D += Vector3.up * FLOAT_SPEED * Time.deltaTime;
        
        if (endTime < Time.time) {
            textDisplay.alpha = Mathf.Lerp(textDisplay.alpha,0,0.05f);
            if (textDisplay.alpha < MIN_ALPHA) {
                Destroy(gameObject);
            }
        }
    }
}
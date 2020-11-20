using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class EndCinematicTelegramChoice : MonoBehaviour
{

    public TextMeshProUGUI telegramTextEnd;

    public Image telegramImage;

    public List<string> textOutcomes;
    public Sprite secondaryOutcomeImage;

    public int angryAliens;
    public int satisfiedAliens;
    public int totalAliens;

    // Start is called before the first frame update
    void Start()
    {
        angryAliens = PlayerPrefs.GetInt("satisfiedAliens");
        satisfiedAliens = PlayerPrefs.GetInt("angryAliens");
        satisfiedAliens = PlayerPrefs.GetInt("maxAliens");
        if(angryAliens+satisfiedAliens >= 6)
        {
            telegramTextEnd.text = textOutcomes[0];
        }
        else
        {
            telegramTextEnd.text = textOutcomes[1];
        }
        if(satisfiedAliens+angryAliens > (1 / 2 * totalAliens))
        {
            telegramImage.sprite = secondaryOutcomeImage;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

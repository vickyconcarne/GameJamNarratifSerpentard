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
    public GameObject secondaryOutcomeImage;

    public int angryAliens;
    public int satisfiedAliens;
    public int totalAliens;

    // Start is called before the first frame update
    void Start()
    {
        satisfiedAliens = PlayerPrefs.GetInt("satisfiedAliens",0);
        angryAliens = PlayerPrefs.GetInt("angryAliens",0);
        totalAliens = PlayerPrefs.GetInt("maxAliens",8);
        if(angryAliens+satisfiedAliens >= 6)
        {
            telegramTextEnd.text = textOutcomes[0];
        }
        else
        {
            telegramTextEnd.text = textOutcomes[1];
        }
        if(satisfiedAliens > 4)
        {
            secondaryOutcomeImage.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

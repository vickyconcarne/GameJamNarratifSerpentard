using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundsCinematic : MonoBehaviour
{

    public AudioSource audioEffects;

    public AudioClip telegramPrinting;

    public AudioClip clickSound;

    public AudioClip paperSound;

    public AudioClip oldFlashSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayTelegram()
    {
        audioEffects.PlayOneShot(telegramPrinting,0.7f);
    }

    public void PlayClick()
    {
        audioEffects.PlayOneShot(clickSound,2f);
    }
    public void PlayPaper()
    {
        audioEffects.PlayOneShot(paperSound,1.2f);
    }
    public void PlayFlash()
    {
        audioEffects.PlayOneShot(oldFlashSound,2f);
    }
}

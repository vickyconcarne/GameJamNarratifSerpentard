using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptToPlayParticularSound : MonoBehaviour
{

    public AudioClip carPassBy;
    public AudioClip birdsFly;
    public AudioClip annoucement;

    public AudioSource environmentSoundPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCarPassBy()
    {
        environmentSoundPlayer.PlayOneShot(carPassBy);
    }

    public void PlayBirdsFly()
    {
        environmentSoundPlayer.PlayOneShot(birdsFly);
    }
    public void PlayAnnouncement()
    {
        environmentSoundPlayer.PlayOneShot(annoucement);
    }
}

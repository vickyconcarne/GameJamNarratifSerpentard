using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "Alien", menuName = "Alien", order = 1)]
public class Alien : ScriptableObject
{
    [Header("Meta information")]
    public string name;
    public GameObject characterPrefab;

    public AudioClip talkSound;
    public AudioClip happySound;
    public AudioClip angrySound;

    [Header("Gameplay conversation")]
    public List<string> positiveSymbols;
    public List<string> negativeSymbols;

    public int numberOfCorrectSymbolsToBeSatisfied;

    public string introductionDialogue;
    public List<string> additionalInformationDialogue;
    public List<string> additionalQuestion;

    private int talkCost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetTalkCost()
    {
        return talkCost;
    }



}


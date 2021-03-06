﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DigitalRuby.Tween;
using TMPro;
using UnityEngine;

namespace AdVd.GlyphRecognition
{
    public class GameLoopManager : MonoBehaviour
    {
        [Header("Time of day")]
        //In hours
        public int currentTime;
        public int maxTime;
        public int minTime;
        public Slider timeSlider;
        public GameObject clock;
        public List<Sprite> clockSprites = new List<Sprite>();

        public MainMenu sceneTransitionManager;

        public TextMeshProUGUI tempsRestantText;
        public GameObject endOfTheDayText;

        //Sky color change
        public Material bg1Material;
        public Material bg2Material;
        private Color originalBg1Color;
        private Color originalBg2Color;
        public Color endBg2Color;
        public Color endBg1Color;
        public Color endColor;
        public Color selection;
        public float tColor;

        public Camera gameCamera;

        [Header("Characters In Loop")]
        public bool canInteract;
        public bool inConversation;
        public int currentDialogueCounter;
        public List<Alien> aliens;

        
        [Header("Dialogue")]
        //Text
        public Animator dialogueAnimator;
        public GameObject speechBubbleAnimator;
        public TextMeshProUGUI dialogueBox;
        public GameObject questionBox1;
        public GameObject questionBox2;
        public GameObject questionBox3;

        public Image bubbleSprite;
        public List<Sprite> randomBubbleImages;

        //To make write text progressively
        string currentDialogue;

        [Header("Telegrams")]
        //Telegrams
        public List<string> telegramPrompts;
        public TextMeshProUGUI telegramText;
        public Animator telegramAnimator;
        public List<AudioClip> radioSounds;

        public bool canShowTelegram;

        //On which alien are we
        public Alien currentAlien;
        private int alienCounter;
        public int satisfiedAliens;
        public int angryAliens;

        [Header("Sounds")]
        public AudioSource audioPlayer; 

        public AudioClip submitAnswer;
        public AudioClip correctSymbolSound;
        public AudioClip incorrectSymbolSound;
        public AudioClip indifferentSymbolSound;
        public AudioClip clockTickSound;
        public AudioClip successSound;
        public AudioClip failureSound;
        public AudioClip nextAlienSound;
        public AudioClip manualOpenSound;
        public AudioClip paperShuffleSound;
        public AudioClip telegramEject;
        public AudioClip endOfTheDaySound;

        [Header("Animation")]
        public GameObject alienParent;
        public FadeBlackBgController blackBackground;
        private float timeBeforeNextConversation = 4f;
        [Header("glyphs")]
        public GlyphSet alienSymbols;
        public Color correctSymbolColor;
        public Color incorrectSymbolColor;
        public Color indifferentSymbolColor;
        //Compile le glyphe et son index dans le glyphset en fonction de son nom, comme ça on peut ressortir les informations facilement (Item1 et Item2)
        public Dictionary<string, Tuple<int, Glyph>> glyphDictionary = new Dictionary<string, Tuple<int, Glyph>>();

        //In game
        [Header("Added symbols in convo")]
        public List<string> addedGlyphsInConvo = new List<string>();
        public List<GameObject> addedGlyphsInGrid = new List<GameObject>();

        public GameObject gridObject;
        public GameObject resultObject;
        public GameObject symbolUIElement;
        public TextMeshProUGUI numberOfSymbolsText;

        public List<GameObject> resultSymbols = new List<GameObject>();
        //Singleton
        public static GameLoopManager gameManagerInstance;
        private bool initialized = false;

        // Start is called before the first frame update
        void Start()
        {
            InitializeInfo();
            if (!gameManagerInstance)
            {
                gameManagerInstance = this.GetComponent<GameLoopManager>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (initialized)
            {
                CheckIfGoToNextAlien();
            }
            if (canShowTelegram)
            {
                CheckIfShowTelegram();
            }
            
        }

        private void InitializeInfo()
        {
            originalBg1Color = bg1Material.color;
            originalBg2Color = bg2Material.color;
            currentTime = maxTime;
            alienCounter = 0;
            inConversation = false;
            canInteract = true;
            currentDialogueCounter = 0;
            timeSlider.maxValue = maxTime;
            timeSlider.minValue = minTime;
            timeSlider.value = currentTime;
            satisfiedAliens = 0;
            angryAliens = 0;
            canShowTelegram = true;
            angryAliens = 0;
            tColor = 0;
            int i = 0;
            foreach (Glyph g in alienSymbols)
            {
                glyphDictionary.Add(g.ToString(), new Tuple<int, Glyph>(i, g));
                i += 1;
            }
            initialized = true;
        }

        public void RandomizeSpeechBubble()
        {
            int r = Random.Range(0, randomBubbleImages.Count);
            bubbleSprite.sprite = randomBubbleImages[r];
        }

        private void UpdateSymbolCount()
        {
            numberOfSymbolsText.text = addedGlyphsInConvo.Count.ToString() + "/4 SYMBOLES";
        }

        private void CheckIfGoToNextAlien()
        {
            if(!inConversation && canInteract)
            {
                if(currentTime <= minTime)
                {
                    Debug.Log("End of the line!");
                    inConversation = false;
                    canInteract = false;
                    StartCoroutine(EndOfTheLine());
                    return;
                }
                else if(alienCounter < aliens.Count)
                {
                    currentAlien = aliens[alienCounter];
                    inConversation = true;
                    Instantiate(currentAlien.characterPrefab, alienParent.transform);
                    alienParent.GetComponent<Animator>().SetTrigger("SlideIn");
                    ReduceTime();
                    
                    StartCoroutine("FadeInDialogue");
                }
                else
                {
                    Debug.Log("End of the line!");
                    inConversation = false;
                    canInteract = false;
                    StartCoroutine(EndOfTheLine());
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void CheckIfShowTelegram()
        {
            if(currentTime == 12)
            {
                canShowTelegram = false; //Pour eviter de relancer dans l'update
                ShowTelegram(telegramPrompts[0]);
            }
            if (currentTime == 8)
            {
                canShowTelegram = false; //Pour eviter de relancer dans l'update
                ShowTelegram(telegramPrompts[1]);
            }
            if (currentTime == 3)
            {
                Debug.Log("showing last telegram prompt");
                canShowTelegram = false; //Pour eviter de relancer dans l'update
                ShowTelegram(telegramPrompts[2]);
            }
        }

        private IEnumerator EndOfTheLine()
        {
            yield return new WaitForSeconds(1f);
            bg1Material.color = originalBg1Color;
            bg2Material.color = originalBg2Color;
            audioPlayer.PlayOneShot(endOfTheDaySound);
            blackBackground.ActivateBg();
            PlayerPrefs.SetInt("satisfiedAliens", satisfiedAliens);
            PlayerPrefs.SetInt("angryAliens", angryAliens);
            PlayerPrefs.SetInt("maxAliens", aliens.Count);
            endOfTheDayText.SetActive(true);
            yield return new WaitForSeconds(3.5f);
            sceneTransitionManager.PlayGame();
        }

        void OnApplicationQuit()
        {
            bg1Material.color = originalBg1Color;
            bg2Material.color = originalBg2Color;
        }

        public void ShowTelegram(string s)
        {
            
            telegramText.text = s;
            blackBackground.ActivateBg();
            Debug.Log("showing telegram");
            telegramAnimator.SetTrigger("TelegramIn");
            audioPlayer.PlayOneShot(telegramEject, 0.7f);
            StartCoroutine("PlayRandomTelegramSound");
        }

        public void HideTelegram()
        {
            StopCoroutine("PlayRandomTelegramSound");
            Debug.Log("hiding telegram");
            blackBackground.DeactivateBg();
            telegramAnimator.SetTrigger("TelegramOut");
            audioPlayer.Stop();
            audioPlayer.PlayOneShot(paperShuffleSound, 2f);
        }

        public void CanShowTelegramAgain() {
            canShowTelegram = true;
        }
        

        public void AddNewSymbolToLayout(Glyph g)
        {
            if (canInteract)
            {
                string glyphName = g.ToString();
                int count = addedGlyphsInGrid.Count;
                if (!addedGlyphsInConvo.Contains(glyphName) && count < 4)
                {
                    GameObject tile = (GameObject)Instantiate(Resources.Load("UI/AddedSymbolPrefab"), gridObject.transform);
                    addedGlyphsInGrid.Add(tile);
                    addedGlyphsInConvo.Add(glyphName);
                    AddListenersToGridObject(tile, g);
                    UpdateSymbolCount();
                }
            }
            
        }

        /// <summary>
        /// The value of c passed into the lambda is the value of c at the time the lambda is called (delegating at that point is like operating in a coroutine). 
        /// Since count iterates from 0-maxCalue over the course of the foreach loop, by the time the click listener is triggered, it is already maxValue, and will always be maxValue. 
        /// If we want to pass a different value into each lambda, we need to create a local value that captures the proper value of count for each iteration of the loop, hence changing
        /// the scope by calling it in a separate method.
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="index"></param>
        public void AddListenersToGridObject(GameObject tile, Glyph g)
        {
            //Glyph
            GlyphDisplay gDisplay = tile.transform.GetChild(0).GetComponent<GlyphDisplay>();
            gDisplay.glyph = g; 
            //Button
            Button b = tile.transform.GetChild(1).GetComponent<Button>();
            b.onClick.AddListener(delegate { DeleteGridSymbol(tile); });
        }

        public void DeleteGridSymbol(GameObject tile)
        {
            int index = GetDictionaryIndex(tile);
            int count = addedGlyphsInGrid.Count;
            if (index < count)
            {
                Destroy(addedGlyphsInGrid[index]);
                addedGlyphsInGrid.RemoveAt(index);
                addedGlyphsInConvo.RemoveAt(index);
                UpdateSymbolCount();
            }
        }

        /// <summary>
        /// We cant directly attach an index to the delegate functions, because the index of our dictionaries is subject to change if we 
        /// delete a dictionary. (dictionaries that come after in the list will have their index re-updated). As such, we iterate through our
        /// object list until we find the corresponding object, at which point we know what index theyre in.
        /// </summary>
        public int GetDictionaryIndex(GameObject tileObject)
        {
            for (int i = 0; i < addedGlyphsInGrid.Count; i++)
            {
                if (GameObject.ReferenceEquals(tileObject, addedGlyphsInGrid[i]))
                {
                    Debug.Log("Found object index is: " + i.ToString());
                    return i;
                }
            }
            //If we dont find, but this should never happen
            Debug.LogError("Couldnt find such tile object in our dictionary list");
            return 0;
        }


        public void ValidateSymbols()
        {
            if (canInteract && inConversation)
            {
                StopAllCoroutines();
                StartCoroutine("ConversationFinish");
            }
            
        }

        private IEnumerator PlayRandomTelegramSound()
        {
            yield return new WaitForSeconds(1f);
            int r = Random.Range(0, radioSounds.Count);
            audioPlayer.PlayOneShot( radioSounds[r], 0.15f);
        }

        public void PlayManualOpenSound()
        {
            audioPlayer.PlayOneShot(manualOpenSound,2f);
        }

        private IEnumerator ConversationFinish()
        {
          
            canInteract = false;
            //Hide speech bubble
            speechBubbleAnimator.SetActive(false);
            speechBubbleAnimator.transform.GetChild(0).GetComponent<Image>().enabled = false;
            //Start analysis
            audioPlayer.PlayOneShot(submitAnswer, 0.3f);
            yield return new WaitForSeconds(1f);
            //Analysis info
            int satisfaction = 0;
            Glyph currentGlyph;
            Vector3 beginningScale = Vector3.one;
            Vector3 endScale = beginningScale * 1.5f;
            //Analysis loop
            foreach (GameObject go in addedGlyphsInGrid)
            {
                
                GlyphDisplay gDisplay = go.transform.GetChild(0).GetComponent<GlyphDisplay>();
                currentGlyph = gDisplay.glyph;
                Transform gDisplayTrans = go.transform.GetChild(0);
                //TWEEN
                System.Action<ITween<Vector3>> updateSize = (t) =>
                {
                    gDisplayTrans.localScale = t.CurrentValue;
                };

                // completion defaults to null if not passed in
                gDisplayTrans.gameObject.Tween("scaleGo", gDisplayTrans.localScale, endScale, 0.4f, TweenScaleFunctions.QuadraticEaseOut, updateSize);
                
                // change color and sound based on answer
                if (currentAlien.positiveSymbols.Contains(currentGlyph.ToString()))
                {
                    satisfaction += 1;
                    go.GetComponent<Image>().color = correctSymbolColor;
                    audioPlayer.PlayOneShot(correctSymbolSound, 0.4f);
                }
                else if (currentAlien.negativeSymbols.Contains(currentGlyph.ToString()))
                {
                    satisfaction -= 1;
                    go.GetComponent<Image>().color = incorrectSymbolColor;
                    audioPlayer.PlayOneShot(incorrectSymbolSound, 0.5f);
                }
                else
                {
                    go.GetComponent<Image>().color = indifferentSymbolColor;
                    audioPlayer.PlayOneShot(indifferentSymbolSound, 0.4f);
                }
                //Add symbol to result
                GameObject superImposedSymbol = (GameObject)Instantiate(Resources.Load("UI/FinalDisplayElement"), resultObject.transform);
                superImposedSymbol.transform.GetChild(0).GetComponent<GlyphDisplay>().glyph = currentGlyph;
                resultSymbols.Add(superImposedSymbol);
                //Revert back to previous scale
                yield return new WaitForSeconds(0.1f);
                gDisplayTrans.gameObject.Tween("scaleGo", gDisplayTrans.localScale, beginningScale, 0.4f, TweenScaleFunctions.QuadraticEaseOut, updateSize);

                yield return new WaitForSeconds(0.8f);
            }
            //Start alien reaction
            yield return new WaitForSeconds(2f);
            
            if (satisfaction >= currentAlien.numberOfCorrectSymbolsToBeSatisfied)
            {
                audioPlayer.PlayOneShot(successSound, 0.5f);
                alienParent.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Content");
                satisfiedAliens += 1;
            }
            else
            {
                audioPlayer.PlayOneShot(failureSound, 0.4f);
                alienParent.transform.GetChild(0).GetComponent<Animator>().SetTrigger("PasContent");
                angryAliens += 1;
            }
            StartCoroutine(FadeOutDialogue());
            yield return new WaitForSeconds(2f);
            if (satisfaction >= currentAlien.numberOfCorrectSymbolsToBeSatisfied)
            {
                audioPlayer.PlayOneShot(currentAlien.happySound, 0.5f);
            }
            else
            {
                audioPlayer.PlayOneShot(currentAlien.angrySound, 0.5f);
            }
            yield return new WaitForSeconds(2f);
            //Start alien departure
            alienParent.GetComponent<Animator>().SetTrigger("SlideOut");
            
            //Reset info and UI
            ClearAllThatLoopInfo();
            foreach (GameObject resultSymbolObject in resultSymbols)
            {
                Destroy(resultSymbolObject);
            }
            resultSymbols.Clear();
            Debug.Log("Started waiting");
            yield return new WaitForSeconds(timeBeforeNextConversation);
            Debug.Log("finished waiting");
            Destroy(alienParent.transform.GetChild(0).gameObject);
            inConversation = false;
            canInteract = true;
            alienCounter += 1; //Go to next alien
            yield return null;
        }

        private void ClearAllThatLoopInfo()
        {
            dialogueBox.text = "";
            questionBox1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            questionBox2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            foreach (GameObject gotodelete in addedGlyphsInGrid)
            {
                Destroy(gotodelete);
            }
            addedGlyphsInGrid.Clear();
            addedGlyphsInConvo.Clear();
            UpdateSymbolCount();
        }

        private IEnumerator FadeOutDialogue()
        {
            dialogueAnimator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(2f);
            questionBox1.SetActive(false);
            questionBox2.SetActive(false);
            questionBox3.SetActive(false);
        }

        private IEnumerator FadeInDialogue()
        {
            dialogueBox.text = currentAlien.introductionDialogue;
            //StartCoroutine(PlayText(currentAlien.introductionDialogue));
            questionBox1.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentAlien.additionalQuestion[0];
            questionBox2.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentAlien.additionalQuestion[1];
            questionBox3.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentAlien.additionalQuestion[2];
            yield return new WaitForSeconds(2f);
            audioPlayer.PlayOneShot(currentAlien.talkSound, 0.5f);
            questionBox1.SetActive(true);
            questionBox2.SetActive(true);
            questionBox3.SetActive(true);
            dialogueAnimator.SetTrigger("FadeIn");
        }

        public void ShowDialogueOption1()
        {
            StopCoroutine("PlayText");
            questionBox1.SetActive(false);
            ReduceTime();
            speechBubbleAnimator.SetActive(true);
            audioPlayer.PlayOneShot(currentAlien.talkSound,0.6f);
            speechBubbleAnimator.GetComponent<Animator>().SetTrigger("Pop");
            //dialogueBox.text = currentAlien.additionalInformationDialogue[0];
            StartCoroutine(PlayText(currentAlien.additionalInformationDialogue[0]));
        }

        public void ShowDialogueOption2()
        {
            StopCoroutine("PlayText");
            questionBox2.SetActive(false);
            ReduceTime();
            speechBubbleAnimator.SetActive(true);
            audioPlayer.PlayOneShot(currentAlien.talkSound, 0.6f);
            speechBubbleAnimator.GetComponent<Animator>().SetTrigger("Pop");
            //dialogueBox.text = currentAlien.additionalInformationDialogue[1];
            StartCoroutine(PlayText(currentAlien.additionalInformationDialogue[1]));
        }

        public void ShowDialogueOption3()
        {
            StopCoroutine("PlayText");
            questionBox3.SetActive(false);
            ReduceTime();
            speechBubbleAnimator.SetActive(true);
            audioPlayer.PlayOneShot(currentAlien.talkSound, 0.6f);
            speechBubbleAnimator.GetComponent<Animator>().SetTrigger("Pop");
            //dialogueBox.text = currentAlien.additionalInformationDialogue[2];
            StartCoroutine(PlayText(currentAlien.additionalInformationDialogue[2]));
        }

        public void ReduceTime()
        {
            //StopCoroutine("HorlogeGonfle");
            if (currentTime > minTime)
            {
                currentTime -= 1;
            }
            else
            {
                currentTime = 0;
            }
            timeSlider.value = currentTime;
            StartCoroutine("HorlogeGonfle");
            
        }

        public void ChangeSkyColor()
        {
            if (tColor <= 1f)
            { // if end color not reached yet...
                tColor += (float)1/maxTime; // advance timer at the right speed
                gameCamera.backgroundColor = Color.Lerp(selection, endColor, tColor);
                bg1Material.color = Color.Lerp(originalBg1Color, endBg1Color, tColor);
                bg2Material.color = Color.Lerp(originalBg2Color, endBg2Color, tColor);
            }
        }

        public IEnumerator HorlogeGonfle()
        {
            Vector3 endScale = Vector3.one * 1.7f;
            Vector3 beginningScale = Vector3.one * 1f;
            System.Action<ITween<Vector3>> updateSize = (t) =>
            {
                clock.transform.localScale = t.CurrentValue;
            };
            yield return new WaitForSeconds(0.2f);
            // completion defaults to null if not passed in
            clock.Tween("scaleClock", clock.transform.localScale, endScale, 0.4f, TweenScaleFunctions.QuadraticEaseOut, updateSize);
            audioPlayer.PlayOneShot(clockTickSound, 0.6f);
            tempsRestantText.text = currentTime + "H \n" + (aliens.Count - alienCounter).ToString() + " Individus"; 
            clock.GetComponent<Image>().sprite = clockSprites[currentTime];
            ChangeSkyColor();
            yield return new WaitForSeconds(0.1f);
            clock.Tween("scaleClock", clock.transform.localScale, beginningScale, 0.4f, TweenScaleFunctions.QuadraticEaseOut, updateSize);
            CanShowTelegramAgain();
            yield return null;
        }

        public IEnumerator PlayText(string s)
        {
            dialogueBox.text = "";
            float timePerCharacter = 0.025f;
            int characterIndex = 0;
            while(characterIndex < s.Length)
            {
                yield return new WaitForSeconds(timePerCharacter);
                dialogueBox.text = s.Substring(0, characterIndex);
                characterIndex++;
            }
            dialogueBox.text = s;
            yield return null;
        }

        //TUPLE CLASS DECLARATION

        public class Tuple<T, U>
        {
            public T Item1 { get; private set; }
            public U Item2 { get; private set; }

            public Tuple(T item1, U item2)
            {
                Item1 = item1;
                Item2 = item2;
            }
        }

        public static class Tuple
        {
            public static Tuple<T, U> Create<T, U>(T item1, U item2)
            {
                return new Tuple<T, U>(item1, item2);
            }
        }

    }
}

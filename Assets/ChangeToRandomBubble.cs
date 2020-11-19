using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdVd.GlyphRecognition
{
    public class ChangeToRandomBubble : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangeRandomBubble()
        {
            GameLoopManager.gameManagerInstance.SelectRandomSpeechBubble();
        }
    }

}

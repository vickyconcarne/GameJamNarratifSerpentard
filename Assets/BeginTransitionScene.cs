using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginTransitionScene : MonoBehaviour
{
    public Animator animationTransition;
    public string triggerName;

    // Start is called before the first frame update
    void Start()
    {
        animationTransition.SetTrigger(triggerName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

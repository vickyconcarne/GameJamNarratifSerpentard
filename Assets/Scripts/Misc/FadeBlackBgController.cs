using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBlackBgController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeactivateObject()
    {
        Debug.Log("deactivated bg!");
        this.gameObject.SetActive(false);
    }

    public void ActivateBg()
    {
        Debug.Log("activating bg!");
        this.gameObject.SetActive(true);
        this.GetComponent<Animator>().SetTrigger("FadeIn");
    }

    public void DeactivateBg()
    {
        this.GetComponent<Animator>().SetTrigger("FadeOut");
    }
}

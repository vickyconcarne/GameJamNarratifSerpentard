using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateConstantly : MonoBehaviour
{

    private Transform objectTransform;
    public float rotationRate;
    // Start is called before the first frame update
    void Start()
    {
        objectTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        objectTransform.Rotate(0f, rotationRate, 0f, Space.Self);
    }
}

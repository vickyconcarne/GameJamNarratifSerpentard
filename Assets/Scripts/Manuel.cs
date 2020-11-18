using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "Manuel", menuName = "Manuel", order = 2)]
public class Manuel : ScriptableObject
{
    [SerializeField]
    public List<SymbolInfo> manuelList = new List<SymbolInfo>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    

}

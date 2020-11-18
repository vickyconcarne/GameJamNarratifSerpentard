using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SymbolInfo", menuName = "SymbolInfo", order = 3)]
public class SymbolInfo : ScriptableObject
{
    public string symbolName;
    public Sprite symbolIcon;
    public string description;
    public string motsClefs;
}
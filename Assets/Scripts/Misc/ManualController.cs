using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ManualController : MonoBehaviour
{
    public Manuel manualInfo;

    public Transform contentGridParent;

    public TextMeshProUGUI titleSymbol;
    public Image imageSymbol;
    public TextMeshProUGUI motsClefSymbol;
    public TextMeshProUGUI descriptionSymbol;

    // Start is called before the first frame update
    void Start()
    {
        ComposeInitialGrid();
        ShowInfo(manualInfo.manuelList[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ComposeInitialGrid()
    {
        for(int index = 0; index < manualInfo.manuelList.Count; index++ )
        {
            SymbolInfo si = manualInfo.manuelList[index];
            GameObject tile = (GameObject)Instantiate(Resources.Load("UI/ManualSymbolElement"), contentGridParent);
            tile.GetComponent<Image>().sprite = si.symbolIcon;
            AddListenersToGridObject(tile, si);
        }
    }

    public void AddListenersToGridObject(GameObject tile, SymbolInfo si)
    {
        //Button
        Button b = tile.GetComponent<Button>();
        b.onClick.AddListener(delegate { ShowInfo(si); });
    }

    public void ShowInfo(SymbolInfo si)
    {
        titleSymbol.text = si.symbolName;
        imageSymbol.sprite = si.symbolIcon;
        motsClefSymbol.text = si.motsClefs;
        descriptionSymbol.text = si.description;
    }





}


using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIWordAnswer : UIView
{
    public Image imageBg;
    public Image imageBar;
    public Text textLevel;

    public UIWordList uiWordList;

    public UILetterItem uiLetterItemPrefab;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        LayOut();
        // UpdateItem();

    }

    void LoadPrefab()
    {

    }
    public override void LayOut()
    {
        RectTransform rctan = uiWordList.GetComponent<RectTransform>();
        float oft = 40;
        rctan.offsetMax = new Vector2(-oft, -oft);
        rctan.offsetMin = new Vector2(oft, oft);
        uiWordList.LayOut();
    }

    public void UpdateItem()
    {
        // UpdateLevel();
        uiWordList.UpdateItem();
    }



}

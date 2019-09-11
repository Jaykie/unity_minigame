
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
        rctan.offsetMax = Vector2.zero;
        rctan.offsetMin = Vector2.zero;
        uiWordList.LayOut();
    }

    public void UpdateItem()
    {
        UpdateLevel();
        uiWordList.UpdateItem();
    }

    public void UpdateLevel()
    {
        string str = Language.main.GetString("GAME_LEVEL");
        int level = LevelManager.main.gameLevel + 1;
        if (str.Contains("xxx"))
        {
            str = str.Replace("xxx", level.ToString());
        }
        else
        {
            str += level.ToString();
        }
        textLevel.text = str;
    }



}

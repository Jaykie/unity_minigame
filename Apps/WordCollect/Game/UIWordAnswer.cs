
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIWordAnswer : UIWordContentBase
{
    public Image imageBg;
    public Image imageBar;
    public Image imageWord;
    public Text textLevel;

    public UIWordList uiWordList;
    public GameObject uiWordImage;

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

    public override void UpdateGuankaLevel(int level)
    {
        UpdateItem();
    }

    public void UpdateItem()
    {
        // UpdateLevel();

        WordItemInfo info = (WordItemInfo)GameGuankaParse.main.GetGuankaItemInfo(LevelManager.main.gameLevel);

        if ((info.gameType == GameRes.GAME_TYPE_WORDLIST) || info.gameType == GameRes.GAME_TYPE_POEM)
        {
            uiWordList.gameObject.SetActive(true);
        }
        else
        {
            uiWordList.gameObject.SetActive(false);
        }
        uiWordImage.SetActive(info.gameType == GameRes.GAME_TYPE_IMAGE ? true : false);
        if (info.gameType == GameRes.GAME_TYPE_IMAGE)
        {
            TextureUtil.UpdateImageTexture(imageWord, info.pic, true);
        }

        uiWordList.UpdateItem();
    }


    public override void OnTips()
    {
    }

    public override void OnAddWord(string word)
    {
    }
    public override void OnReset()
    {
    }

}

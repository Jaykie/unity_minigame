using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHowToPlayPage1 : UIView
{
    public Text textTitle;
    public Text textDetail;
    public GameObject objCell;
    UICellWord cellItemPrefab;
    int heightCell = 128;
    int oneCellNum = 2;
    int totalItem = 2;
    WordItemInfo infoGuanka;

    UICellWord cellItem0;
    UICellWord cellItem1;

    /// </summary>
    void Awake()
    {
        WordItemInfo info = (WordItemInfo)GameLevelParse.main.GetGuankaItemInfo(LevelManager.main.gameLevel);
        textTitle.color = AppRes.colorTitle;
        textDetail.color = AppRes.colorTitle;
        LevelManager.main.ParseGuanka();
        infoGuanka = GameLevelParse.main.GetGuankaItemInfo(3) as WordItemInfo;
        textTitle.text = Language.main.GetString("STRING_HOWTOPLAY_TITLE1");
        textDetail.text = Language.main.GetString("STRING_HOWTOPLAY_DETAIL1");
        LoadPrefab();
        if (info.gameType == GameRes.GAME_TYPE_WORDLIST)
        {
            cellItem0 = AddItem(0);
            cellItem1 = AddItem(1);
        }
    }

    void Start()
    {
        LayOut();
        Invoke("LayOut", 0.2f);
    }
    public override void LayOut()
    {

        RectTransform rctranCellRoot = objCell.GetComponent<RectTransform>();
        if (cellItem0 != null)
        {
            RectTransform rctran = cellItem0.GetComponent<RectTransform>();
            rctran.sizeDelta = new Vector2(rctranCellRoot.rect.width, rctran.rect.height);
            cellItem0.LayOut();
        }
        if (cellItem1 != null)
        {
            RectTransform rctran = cellItem1.GetComponent<RectTransform>();
            rctran.sizeDelta = new Vector2(rctranCellRoot.rect.width, rctran.rect.height);
            cellItem1.LayOut();
        }

        LayOutBase lay = objCell.GetComponent<LayOutBase>();
        if (lay != null)
        {
            lay.LayOut();
        }
    }
    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UICellWord");
            cellItemPrefab = obj.GetComponent<UICellWord>();
        }

    }

    UICellWord AddItem(int idx)
    {
        UICellWord item = (UICellWord)GameObject.Instantiate(cellItemPrefab);
        item.transform.SetParent(objCell.transform);
        item.transform.localScale = new Vector3(1f, 1f, 1f);
        Rect rc = (item.transform as RectTransform).rect;
        item.index = idx;
        item.UpdateItem(true);
        return item;
    }
}

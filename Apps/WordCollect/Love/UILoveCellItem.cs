﻿using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.UI.Button;
public delegate void OnUILoveCellItemDelegate(UILoveCellItem ui);
public class UILoveCellItem : UICellItemBase
{

    public Image imageBg;
    public Button btnDelete;
    public Text textTitle; 
    public float itemWidth;
    public float itemHeight;
    public Color colorSel;
    public Color colorUnSel;
    WordItemInfo infoItem;
    public OnUILoveCellItemDelegate callbackClickDelete { get; set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LevelManager.main.ParseGuanka();
        Common.SetButtonText(btnDelete, Language.main.GetString("STR_Detail_DELETE_LOVE"), 0, false);
    }


    public override void UpdateItem(List<object> list)
    {
        infoItem = list[index] as WordItemInfo;
        GameLevelParse.main.ParseIdiomItem(infoItem);
        UpdateInfo(infoItem);

    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }

    public override void LayOut()
    {


    }
    void SetSelect(bool isSel)
    {
        if (isSel)
        {
            textTitle.color = colorSel;
        }
        else
        {
            textTitle.color = colorUnSel;
        }
    }

    public void UpdateInfo(WordItemInfo info)
    {
        textTitle.text = info.id; 
        LayOut();

    }

    public void OnClickBtnDelete()
    {
        if (this.callbackClickDelete != null)
        {
            this.callbackClickDelete(this);
        }
    }

}

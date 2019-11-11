using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//word接龙
/*
Word Life
https://apps.apple.com/cn/app/id1418492982
 */

public class UIWordConnect : UIWordContentBase, IUILetterItemDelegate
{
    public Image imageBg;
    public Text textTitle;
    public UILetterItem uiLetterItemPrefab;
    public int row = 7;
    public int col = 7;
    public List<UILetterItem> listItem;

    LayOutGrid lygrid;
    int indexFillWord;
    int indexAnswer;

    void Awake()
    {
        lygrid = this.GetComponent<LayOutGrid>();
        listItem = new List<UILetterItem>();
        row = 7;
        col = 7;
        lygrid.row = row;
        lygrid.col = col;
        lygrid.enableLayout = false;
        lygrid.dispLayVertical = LayOutBase.DispLayVertical.TOP_TO_BOTTOM;
    }

    // Use this for initialization
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }

    public override void LayOut()
    {
        float x, y, w, h;
        RectTransform rctranRoot = this.GetComponent<RectTransform>();
        if (lygrid != null)
        {
            lygrid.LayOut();
            foreach (UILetterItem item in listItem)
            {
                Vector2 pos = lygrid.GetItemPostion(item.indexRow, item.indexCol);
                RectTransform rctran = item.GetComponent<RectTransform>();
                w = (rctranRoot.rect.width - (lygrid.space.x) * (col - 1)) / col;
                h = w;
                rctran.sizeDelta = new Vector2(w, h);
                rctran.anchoredPosition = pos;
                item.LayOut();
            }
        }


    }
    public override void UpdateGuankaLevel(int level)
    {
        UpdateItem();
    }
    public void UpdateItem()
    {
        WordItemInfo info = infoItem as WordItemInfo;

        row = info.listBoard.Count;
        col = info.listBoard[0].Count;

        Debug.Log("UpdateItem row = " + row + " col=" + col);

        lygrid.row = row;
        lygrid.col = col;

        for (int i = 0; i < info.listBoard.Count; i++)
        {
            List<object> ls = info.listBoard[i];
            for (int j = 0; j < ls.Count; j++)
            {
                string strword = (string)ls[j];
                UILetterItem ui = (UILetterItem)GameObject.Instantiate(uiLetterItemPrefab);
                ui.transform.SetParent(this.transform);
                ui.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                UIViewController.ClonePrefabRectTransform(uiLetterItemPrefab.gameObject, ui.gameObject);
                ui.iDelegate = this;
                ui.indexRow = i;
                ui.indexCol = j;
                ui.index = i;
                ui.isAnswerItem = false;
                ui.wordAnswer = strword;
                ui.UpdateItem(strword);
                ui.SetStatus(UILetterItem.Status.LOCK);
                if (Common.BlankString(strword))
                {
                    ui.SetStatus(UILetterItem.Status.HIDE);
                }
                listItem.Add(ui);
            }
        }
        indexAnswer = 0;
        // indexFillWord = info.listWordAnswer[indexAnswer];
        // for (int i = 0; i < info.listWordAnswer.Count; i++)
        // {
        //     int idx = info.listWordAnswer[i];
        //     UILetterItem ui = listItem[idx];
        //     ui.isAnswerItem = true;
        //     if (idx == indexFillWord)
        //     {
        //         ui.SetStatus(UILetterItem.Status.LOCK_SEL);
        //     }
        //     else
        //     {
        //         ui.SetStatus(UILetterItem.Status.LOCK_UNSEL);
        //     }

        // }

        LayOut();

    }

    public UILetterItem GetSelItem()
    {
        UILetterItem ui = listItem[indexFillWord];
        return ui;
    }

    public UILetterItem GetFistUnSelItem()
    {
        foreach (UILetterItem item in listItem)
        {
            if (item.GetStatus() == UILetterItem.Status.LOCK_UNSEL)
            {
                return item;
            }
        }
        return null;
    }
    public UILetterItem GetItem(int idx)
    {
        foreach (UILetterItem item in listItem)
        {
            if (idx == item.index)
            {
                return item;
            }
        }
        return null;
    }
    public UILetterItem GetItem(int idxRow, int idxCol)
    {
        foreach (UILetterItem item in listItem)
        {
            if ((idxRow == item.indexRow) && (idxCol == item.indexCol))
            {
                return item;
            }
        }
        return null;
    }


    public void ScanItem(int idxRow, int idxCol)
    {
        bool isAllRight = true;

        //scan row
        foreach (UILetterItem item in listItem)
        {
            if (idxRow == item.indexRow)
            {
                if (!((IsItemRightAnswer(item) || (item.GetStatus() == UILetterItem.Status.NORMAL))))
                {
                    isAllRight = false;
                }
            }
        }
        if (isAllRight)
        {
            foreach (UILetterItem item in listItem)
            {
                if (idxRow == item.indexRow)
                {
                    item.SetStatus(UILetterItem.Status.ALL_RIGHT_ANSWER);
                }
            }
        }

        //scan col
        isAllRight = true;
        foreach (UILetterItem item in listItem)
        {
            if (idxCol == item.indexCol)
            {
                if (!((IsItemRightAnswer(item) || (item.GetStatus() == UILetterItem.Status.NORMAL))))
                {
                    isAllRight = false;
                }
            }
        }
        if (isAllRight)
        {
            foreach (UILetterItem item in listItem)
            {
                if (idxCol == item.indexCol)
                {
                    item.SetStatus(UILetterItem.Status.ALL_RIGHT_ANSWER);
                }
            }
        }

    }

    bool IsItemRightAnswer(UILetterItem ui)
    {
        if (ui.GetStatus() == UILetterItem.Status.LOCK)
        {
            return false;
        }
        return true;
    }

    //判断答案是否正确
    public override bool CheckAllAnswerFinish()
    {
        bool isAllAnswer = true;
        WordItemInfo info = infoItem as WordItemInfo;

        for (int i = 0; i < info.listAnswerInfo.Count; i++)
        {
            AnswerInfo answerInfo = info.listAnswerInfo[i];
            int row = answerInfo.row;
            int col = answerInfo.col;
            foreach (UILetterItem item in listItem)
            {
                if ((row == item.indexRow) || (col == item.indexCol))
                {
                    if (!IsItemRightAnswer(item))
                    {
                        isAllAnswer = false;
                        Debug.Log("isAllAnswer false row=" + row + " col=" + col + " answerInfo=" + answerInfo.word + " status=" + item.GetStatus());
                        break;
                    }
                }
            }

            if (!isAllAnswer)
            {
                break;
            }
        }

        if (isAllAnswer)
        {
            //全部猜对 game win
            // OnGameWin();
        }
        else
        {
            //游戏失败
            //  OnGameFail();
        }
        return isAllAnswer;
    }

    int GetNextFillWord(WordItemInfo info)
    {
        int ret = -1;
        for (int i = 0; i < info.listWordAnswer.Count; i++)
        {
            int idx = info.listWordAnswer[i];
            UILetterItem ui = listItem[idx];
            if ((ui.GetStatus() == UILetterItem.Status.LOCK_UNSEL) || (ui.GetStatus() == UILetterItem.Status.ERROR_ANSWER))
            {
                ret = i;
                break;
            }

        }
        return ret;
    }


    int GetIndexAnswer(UILetterItem uiSel)
    {
        int ret = -1;
        WordItemInfo info = infoItem as WordItemInfo;
        for (int i = 0; i < info.listWordAnswer.Count; i++)
        {
            int idx = info.listWordAnswer[i];
            Debug.Log("GetIndexAnswer idx = " + idx + " listItem=" + listItem.Count + " level=" + LevelManager.main.gameLevel);
            UILetterItem ui = listItem[idx];
            if (ui.index == uiSel.index)
            {
                ret = i;
                break;
            }

        }
        return ret;
    }

    public int GetFirstUnFinishAnswer()
    {
        WordItemInfo info = infoItem as WordItemInfo;
        int ret = -1;
        for (int i = 0; i < info.listAnswerInfo.Count; i++)
        {
            AnswerInfo answerInfo = info.listAnswerInfo[i];
            int row = answerInfo.row;
            int col = answerInfo.col;
            foreach (UILetterItem item in listItem)
            {
                if ((row == item.indexRow) || (col == item.indexCol))
                {
                    if (!IsItemRightAnswer(item))
                    {
                        ret = i;
                        break;
                    }
                }
            }

            if (ret >= 0)
            {
                break;
            }

        }
        return ret;
    }
    public override void OnAddWord(string word)
    {
        // UILetterItem ui = listItem[indexFillWord];
        // WordItemInfo info = infoItem as WordItemInfo;
        // if (UILetterItem.Status.ERROR_ANSWER == ui.GetStatus())
        // {
        //     //先字符退回
        //     if (iDelegate != null)
        //     {
        //         iDelegate.UIWordContentBaseDidBackWord(this, ui.wordDisplay);
        //     }
        // }
        // ui.UpdateItem(word);
        // if (ui.wordAnswer == word)
        // {
        //     ui.SetStatus(UILetterItem.Status.RIGHT_ANSWER);
        //     ScanItem(ui.indexRow, ui.indexCol);
        //     //显示下一个
        //     indexAnswer = GetNextFillWord(info);
        //     if ((indexAnswer < info.listWordAnswer.Count) && (indexAnswer >= 0))
        //     {
        //         indexFillWord = info.listWordAnswer[indexAnswer];
        //         UILetterItem uiNext = listItem[indexFillWord];
        //         uiNext.SetStatus(UILetterItem.Status.LOCK_SEL);
        //     }

        // }
        // else
        // {
        //     ui.SetStatus(UILetterItem.Status.ERROR_ANSWER);
        // }

    }

    public override void OnTips()
    {
        WordItemInfo info = infoItem as WordItemInfo;
        int idx = GetFirstUnFinishAnswer();
        if (idx >= 0)
        {
            // indexAnswer = idx;
            // if (indexAnswer < info.listAnswerInfo.Count)
            // {
            //     indexFillWord = info.listWordAnswer[indexAnswer];
            //     string strword = info.listWord[indexFillWord];
            //     OnAddWord(strword);
            //     if (iDelegate != null)
            //     {
            //         iDelegate.UIWordContentBaseDidTipsWord(this, strword);
            //     }
            // }
            OnRightAnswer(idx);
        }

    }

    public override void OnReset()
    {
        indexAnswer = 0;
        WordItemInfo info = infoItem as WordItemInfo;
        foreach (UILetterItem item in listItem)
        {
            item.SetStatus(UILetterItem.Status.UNLOCK);
        }
    }

    public override void OnRightAnswer(int idx)
    {
        WordItemInfo info = infoItem as WordItemInfo;
        AnswerInfo answerInfo = info.listAnswerInfo[idx];
        int row = answerInfo.row;
        int col = answerInfo.col;
        foreach (UILetterItem item in listItem)
        {
            if ((row == item.indexRow) || (col == item.indexCol))
            {
                if (item.GetStatus() == UILetterItem.Status.LOCK)
                {
                    item.SetStatus(UILetterItem.Status.UNLOCK);
                }
                else if (item.GetStatus() == UILetterItem.Status.UNLOCK)
                {
                    item.SetStatus(UILetterItem.Status.DUPLICATE);
                }
            }
        }
    }


    public void OnUILetterItemDidClick(UILetterItem ui)
    {
        Debug.Log("OnUILetterItemDidClick status=" + ui.GetStatus());
        WordItemInfo info = infoItem as WordItemInfo;

    }
    public void OnClickItem()
    {
    }
}

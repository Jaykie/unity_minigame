using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMathFormulationCellItem : UICellItemBase
{
    public Image imageBg;
    // Use this for initialization 
    bool isHideChild;
    public List<UIMathFormulationDot> listItem;

    UIMathFormulationDot uiFormulationDotPrefab;
    int indexTips;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        indexTips = 1;
        listItem = new List<UIMathFormulationDot>();
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/MathFormulation/UIMathFormulationDot");
            if (obj != null)
            {
                uiFormulationDotPrefab = obj.GetComponent<UIMathFormulationDot>();
            }
        }
    }

    public override void UpdateItem(List<object> list)
    {
        indexTips = 1;
        List<string> listTmp = list[index] as List<string>;
        UpdateInfo(listTmp);
    }



    void RemoveAllItems()
    {
        foreach (UIMathFormulationDot item in listItem)
        {
            DestroyImmediate(item.gameObject);
        }
        listItem.Clear();
    }

    public void UpdateInfo(List<string> list)
    {

        float x, y, w, h;
        RemoveAllItems();
        int len = list.Count;
        RectTransform rctranCellItem = this.GetComponent<RectTransform>();
        //编号
        string word = (index + 1).ToString();
        float ratio = 0.8f;
        float w_dot = 0, h_dot = 0;
        float step = 8;
        int count = len + 1;

        w_dot = (width - (count + 1) * step) / count;
        h_dot = height;
         Debug.Log("dot:w_dot=" + w_dot+" h_dot="+h_dot+" width="+width+" height="+height);

        w_dot = Mathf.Min(w_dot, h_dot) * ratio;
        h_dot = w_dot;
       
        //head
        {
            UIMathFormulationDot item = (UIMathFormulationDot)GameObject.Instantiate(uiFormulationDotPrefab);
            item.transform.parent = objContent.transform;
            item.transform.localScale = new Vector3(1f, 1f, 1f);
            item.index = 0;
            RectTransform rctran = item.GetComponent<RectTransform>();
            w = w_dot;
            h = w;
            rctran.sizeDelta = new Vector2(w, h);
            item.UpdateItem(word, UIMathFormulationDot.DotType.HEAD);
            listItem.Add(item);
        }

        for (int i = 0; i < len; i++)
        {
            word = (string)list[i];
            UIMathFormulationDot item = (UIMathFormulationDot)GameObject.Instantiate(uiFormulationDotPrefab);
            item.transform.parent = objContent.transform;
            item.transform.localScale = new Vector3(1f, 1f, 1f);
            item.index = i + 1;
            RectTransform rctran = item.GetComponent<RectTransform>();
            w = w_dot;
            h = w;
            rctran.sizeDelta = new Vector2(w, h);
            if (i % 2 == 0)
            {
                item.UpdateItem(word, UIMathFormulationDot.DotType.IMAGE_NUM);
            }
            else
            {
                item.UpdateItem(word, UIMathFormulationDot.DotType.IMAGE_MATH);
            }

            listItem.Add(item);
        }
    }

    //运算正确
    public void OnFormulationFinish()
    {
        foreach (UIMathFormulationDot item in listItem)
        {
            if (item.index > 0)
            {
                item.UpdateType(UIMathFormulationDot.DotType.FINISH);
            }
        }
    }

    //提示
    public void OnTips()
    {

                    if(Common.gold<=0){
                        
                        return;
                    }
        foreach (UIMathFormulationDot item in listItem)
        {
            if (item.index > 0)
            {
                if (item.IsLock())
                {
                    item.UpdateType(UIMathFormulationDot.DotType.TIPS);
                    Common.gold--;
                    break;
                }
            }
        }

    }

    //所有运算完成
    public bool IsAllUnLock()
    {
        bool ret = true;
        foreach (UIMathFormulationDot item in listItem)
        {
            if (item.index > 0)
            {
                if (item.IsLock())
                {
                    ret = false;
                    break;
                }
            }
        }
        return ret;
    }
}


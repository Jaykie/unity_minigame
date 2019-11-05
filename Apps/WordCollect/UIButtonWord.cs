using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIButtonWord : UIView
{
    public Image imageBg;
    public Text textTitle;
    public int index;
    WordItemInfo infoItem; 

    // Use this for initialization
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {


    }
    public void UpdateItem(WordItemInfo info)
    {
        infoItem = info;
        textTitle.text = info.title;
    } 
    public void OnClickItem()
    {

        PopUpManager.main.Show<UIWordDetail>("App/Prefab/Game/UIWordDetail", popup =>
        {
            Debug.Log("UIWordDetail Open ");
            popup.UpdateItem(infoItem);

        }, popup =>
        {
            Debug.Log("UIWordDetail Close ");

        });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlaceCellItemWordConnect : UICellItemBase
{

    public Text textTitle;
    public Image imageBg;
    public RawImage imageIcon;
    public override void UpdateItem(List<object> list)
    {
        ItemInfo info = list[index] as ItemInfo;

        string str = Language.main.GetString("PLACE_LEVEL");
        int level = index + 1;
        if (str.Contains("xxx"))
        {
            str = str.Replace("xxx", level.ToString());
        }
        else
        {
            str += level.ToString();
        }
        textTitle.text = str;

        //textTitle.gameObject.SetActive(false);
        // TextureUtil.UpdateImageTexture(imageBg, "App/UI/Place/PlaceItemBg", true);
        imageIcon.gameObject.SetActive(info.isAd);
        LayOut();
    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }
    public override void LayOut()
    {

    }

}

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
        string str = "";
        if (Common.appKeyName == GameRes.GAME_WORDCONNECT)
        {
            str = Language.main.GetString("PLACE_LEVEL");
            int level = index + 1;
            if (str.Contains("xxx"))
            {
                str = str.Replace("xxx", level.ToString());
            }
            else
            {
                str += level.ToString();
            }

        }
        else if (Common.appKeyName == GameRes.GAME_IDIOM)
        {
            LanguageManager.main.UpdateLanguagePlace();
            str = LanguageManager.main.languagePlace.GetString("STR_PLACE_" + info.id);
        }
        else
        {

            LanguageManager.main.UpdateLanguagePlace();
            str = LanguageManager.main.languagePlace.GetString("STR_PLACE_" + info.id);
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

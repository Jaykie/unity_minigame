using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIMathFormulationDot : UIView
{
    public Image imageBg;
    public Text textTitle;

    public int index;
    public DotType type;
    public enum DotType
    {
        HEAD = 0,
        IMAGE_NUM,
        IMAGE_MATH,//运算符号
        TIPS,//string 提示 显示黄色
        FINISH//string 运算完成 显示绿色
    }

    public void UpdateItem(string str, DotType ty)
    {
        textTitle.text = str;
        UpdateType(ty);
    }

    public void UpdateType(DotType ty)
    {
        type = ty;
        switch (type)
        {
            case DotType.HEAD:
                textTitle.color = Color.white;
                textTitle.gameObject.SetActive(true);
                imageBg.gameObject.SetActive(true);
                imageBg.sprite = LoadTexture.CreateSprieFromTex(TextureCache.main.Load(AppRes.IMAGE_MATHFORMULATION_DOT_HEAD));
                break;
            case DotType.IMAGE_NUM:
                textTitle.gameObject.SetActive(false);
                imageBg.gameObject.SetActive(true);
                imageBg.sprite = LoadTexture.CreateSprieFromTex(TextureCache.main.Load(AppRes.IMAGE_MATHFORMULATION_DOT_NUM));
                break;
            case DotType.IMAGE_MATH:
                textTitle.gameObject.SetActive(false);
                imageBg.gameObject.SetActive(true);
                imageBg.sprite = LoadTexture.CreateSprieFromTex(TextureCache.main.Load(AppRes.IMAGE_MATHFORMULATION_DOT_MATH));
                break;
            case DotType.TIPS:
                textTitle.color = Color.yellow;
                textTitle.gameObject.SetActive(true);
                imageBg.gameObject.SetActive(false);
                break;
            case DotType.FINISH:
                textTitle.color = Color.green;
                textTitle.gameObject.SetActive(true);
                imageBg.gameObject.SetActive(false);
                break;

        }
    }

    //还未运算正确状态
    public bool IsLock()
    {
        bool ret = false;
        if ((type == DotType.IMAGE_NUM)||(type == DotType.IMAGE_MATH))
        {
            ret = true;
        }
        return ret;
    }
}

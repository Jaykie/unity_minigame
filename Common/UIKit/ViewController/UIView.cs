using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIView : MonoBehaviour
{
    public UIViewController controller;
    public Camera mainCam
    {
        get
        {
            if (AppSceneBase.main == null)
            {
                Debug.Log("UIView::AppSceneBase.main==null");
            }
            return AppSceneBase.main.mainCamera;
        }
    }

    public Rect frame
    {
        get
        {
            return GetFrame(this.GetComponent<RectTransform>());
        }
    }

    public Rect frameParent
    {
        get
        {
            return GetFrame(this.transform.parent.GetComponent<RectTransform>());
        }
    }

    public Rect frameMainWorld
    {
        get
        {
            return GetFrame(AppSceneBase.main.objMainWorld.GetComponent<RectTransform>());
        }
    }


    public string keyText;
    public string keyColor;

    public string keyImage;

    static public Rect GetFrame(RectTransform rctran)
    {
        Rect rc = Rect.zero;
        if (rctran != null)
        {
            rc = rctran.rect;
        }
        return rc;
    }
    public virtual void LayOut()
    {

    }

    public virtual void UpdateLanguage()
    {

    }
    public void SetController(UIViewController con)
    {
        controller = con;
        //this.transform.parent = controller.objController.transform;
        this.transform.SetParent(controller.objController.transform);
        con.view = this;
    }

    public void SetViewParent(GameObject obj)
    {
        this.transform.parent = obj.transform;
        this.transform.localScale = new Vector3(1f, 1f, 1f);
        this.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
    // 
    public Color GetKeyColor()
    {
        return GetKeyColor(Color.white);
    }
    public Color GetKeyColor(Color def)
    {
        Color ret = def;
        if (!Common.isBlankString(keyColor))
        {
            ret = ColorConfig.main.GetColor(keyColor);
        }
        return ret;
    }


    public string GetKeyText()
    {
        string ret = "";
        if (!Common.isBlankString(keyText))
        {
            ret = Language.main.GetString(keyText);
        }
        return ret;
    }
    public void OnUIDidFinish()
    {
        DoUIFinish();
    }

    void DoUIFinish()
    {
        if (this.controller != null)
        {
            if (controller.callbackUIFinish != null)
            {
                controller.callbackUIFinish();
            }
        }
    }
}

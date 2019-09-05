
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
public class UILetterItem : UIView
{
    public enum Type
    {
        Connect = 0,
    }

    public enum Status
    {
        LOCK = 0,
        UNLOCK = 1,
        DUPLICATE = 2//重复连线
    }
    public Image imageBg;
    public Image imageIcon;
    public Text textTitle;

    public int index;

    Status status;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        SetStatus(Status.LOCK);
        LayOut();
    }


    public override void LayOut()
    {

    }
    public void SetStatus(Status st)
    {
        status = st;
        if (st == Status.LOCK)
        {
            textTitle.gameObject.SetActive(false);
            imageIcon.gameObject.SetActive(true);

        }
        else
        {
            textTitle.gameObject.SetActive(true);
            imageIcon.gameObject.SetActive(false);
        }

    }

    public Status GetStatus()
    {
        return status;
    }

    public void UpdateItem(string letter)
    {
        textTitle.text = letter;
    }
}

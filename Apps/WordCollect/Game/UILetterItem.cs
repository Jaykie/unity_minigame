
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
    public Image imageBg;
    public Text textTitle;

    public int index;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LayOut();
    }


    public override void LayOut()
    {

    }

    public void UpdateItem(string letter)
    {
        textTitle.text = letter;
    }
}


using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
public class UILetterList : UIView, IPopViewControllerDelegate
{
    public Image imageBg;
    public Image imageGold;
    public Text textGold;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        UpdateGold();
        LayOut();
    }


    public override void LayOut()
    {

    }

    public void UpdateGold()
    { 
    }
    public void OnPopViewControllerDidClose(PopViewController controller)
    {
        
    }
    public void OnClickGold()
    {
        ShopViewController.main.Show(null, this);
    }


}

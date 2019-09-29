
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class LetterItem : UIView
{
    public GameObject objSpriteBg;
    public GameObject objSpriteSel;
    public TextMesh textTitle;

    public int index;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // objSpriteSel.SetActive(false);

        //全透明
        {
            SpriteRenderer rd = objSpriteSel.GetComponent<SpriteRenderer>();
            Color cr = rd.color;
            cr.a = 0f;
            rd.color = cr;
        }

        LayOut();
    }


    public override void LayOut()
    {

    }

    public void UpdateItem(string letter)
    {
        textTitle.text = letter;
    }

    public void OnItemDidSelect()
    {
        //objSpriteSel.SetActive(true);
        float duration = 1f;
        SpriteRenderer rd = objSpriteSel.GetComponent<SpriteRenderer>();
        Tween tweenAlpha = DOTween.ToAlpha(() => rd.color, x => rd.color = x, 1f, duration);
        AudioPlay.main.PlayFile(GameRes.Audio_LetterItemSel);

    }
    public void OnItemDidUnSelect()
    {
        //objSpriteSel.SetActive(false);
        float duration = 1f;
        SpriteRenderer rd = objSpriteSel.GetComponent<SpriteRenderer>();
        Tween tweenAlpha = DOTween.ToAlpha(() => rd.color, x => rd.color = x, 0f, duration);

    }
}

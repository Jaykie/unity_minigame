using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class GameRes
{
    public const string GAME_WORDCONNECT = "WordCollect";
    public const string GAME_IDIOM = "Idiom";
    public const string GAME_POEM = "Poem";

    //type
    public const string GAME_TYPE_WORDLIST = "WordList";
    public const string GAME_TYPE_IMAGE = "Image";
    public const string GAME_TYPE_TEXT = "Text";
    public const string GAME_TYPE_IMAGE_TEXT = "ImageText";
    public const string GAME_TYPE_POEM = "Poem";

    public const string GAME_TYPE_CONNECT = "Connect";//接龙
    //

    public const string PREFAB_LETTER_ITEM = "AppCommon/Prefab/Game/UILetterItem";

    public const string Audio_LetterItemSel = "AppCommon/Audio/Game/LetterItemSel";
    public const string Audio_WordDuplicate = "AppCommon/Audio/Game/WordDuplicate";
    public const string Audio_WordError = "AppCommon/Audio/Game/WordError";
    public const string Audio_WordRight = "AppCommon/Audio/Game/WordRight";

    static private GameRes _main = null;
    public static GameRes main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameRes();
            }
            return _main;
        }
    }
    public Color32 colorTitle
    {
        get
        {
            Color32 cr = new Color32(255, 255, 255, 255);
            // if (Common.appKeyName == GameGuankaParse.STR_APPKEYNAME_RIDDLE)
            {
                cr = new Color32(89, 45, 6, 255);
            }
            return cr;
        }
    }

    public Color32 colorGameText
    {
        get
        {
            Color32 cr = new Color32(0, 0, 0, 255);
            // if (Common.appKeyName == GameGuankaParse.STR_APPKEYNAME_RIDDLE)
            {
                cr = new Color32(255, 255, 255, 255);
            }
            return cr;
        }
    }

    public Color32 colorGameWinTitle
    {
        get
        {
            Color32 cr = new Color32(192, 90, 59, 255);
            // if (Common.appKeyName == GameGuankaParse.STR_APPKEYNAME_RIDDLE)
            {
                // cr = new Color32(255, 255, 255, 255);
            }
            return cr;
        }
    }
    public Color32 colorGameWinTextView
    {
        get
        {
            Color32 cr = new Color32(192, 90, 59, 255);
            //if (Common.appKeyName == GameGuankaParse.STR_APPKEYNAME_RIDDLE)
            {
                cr = new Color32(89, 45, 6, 255);
            }
            return cr;
        }
    }

}

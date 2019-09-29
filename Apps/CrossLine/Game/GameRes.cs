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


    //type
    public const string GAME_TYPE_WORDLIST = "WordList";
    public const string GAME_TYPE_IMAGE = "Image";
    public const string GAME_TYPE_TEXT = "Text";
    public const string GAME_TYPE_IMAGE_TEXT = "ImageText";

    //

    public const string PREFAB_LETTER_ITEM = "AppCommon/Prefab/Game/UILetterItem";

    public const string Audio_LetterItemSel = "AppCommon/Audio/Game/LetterItemSel";
    public const string Audio_WordDuplicate = "AppCommon/Audio/Game/WordDuplicate";
    public const string Audio_WordError = "AppCommon/Audio/Game/WordError";
    public const string Audio_WordRight = "AppCommon/Audio/Game/WordRight";
}

using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;


public class WordItemInfo : ItemInfo
{
    public List<object> listFormulation;//公式
    public string[] listLetter;
    public string[] listAnswer;//答案 LO|SOL 
    public string[] listError;
}
public class GameGuankaParse : GuankaParseBase
{
    static private GameGuankaParse _main = null;
    public static GameGuankaParse main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameGuankaParse();
            }
            return _main;
        }
    }


    public override ItemInfo GetGuankaItemInfo(int idx)
    {
        if (listGuanka == null)
        {
            return null;
        }
        if (idx >= listGuanka.Count)
        {
            return null;
        }
        ItemInfo info = listGuanka[idx] as ItemInfo;
        return info;
    }

    public WordItemInfo GetItemInfo()
    {
        int idx = LevelManager.main.gameLevel;
        return GetGuankaItemInfo(idx) as WordItemInfo;
    }

    public override int GetGuankaTotal()
    {
        ParseGuanka();
        if (listGuanka != null)
        {
            return listGuanka.Count;
        }
        return 0;
    }

    public override void CleanGuankaList()
    {
        if (listGuanka != null)
        {
            listGuanka.Clear();
        }

    }

    public override int ParseGuanka()
    {
        int count = 0;

        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            return listGuanka.Count;
        }

        listGuanka = new List<object>();
        int idx = LevelManager.main.placeLevel;
        string fileName = Common.GAME_RES_DIR + "/guanka/Chapter_" + (idx + 1) + ".txt";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["levels"];


        /*
             "n": "1",
            "o": 0,
            "l": "L|O|S",
            "r": "LO|SOL",
            "e": ""
         */
        char[] charSplit = { '|' };

        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            WordItemInfo info = new WordItemInfo();
            string str = (string)item["l"];

            info.listLetter = str.Split(charSplit);

            str = (string)item["r"];
            info.listAnswer = str.Split(charSplit);

            str = (string)item["e"];
            info.listError = str.Split(charSplit);

            listGuanka.Add(info);
        }

        count = listGuanka.Count;

        Debug.Log("ParseGame::count=" + count);
        return count;
    }

}

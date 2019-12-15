using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class CrossItemInfo : ItemInfo
{
    public List<Vector2> listDot;
    public List<Vector2> listLine;
    public string author;
}
public class GameLevelParse : LevelParseBase
{
    public string strWord3500;
    Language languageGame;
    static private GameLevelParse _main = null;
    public static GameLevelParse main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameLevelParse();
                _main.UpdateLanguage();
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

    public CrossItemInfo GetItemInfo()
    {
        int idx = LevelManager.main.gameLevel;
        return GetGuankaItemInfo(idx) as CrossItemInfo;
    }


    public void UpdateLanguage()
    {
        ItemInfo info = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        string strlan = Common.GAME_RES_DIR + "/language/" + info.language + ".csv";
        languageGame = new Language();
        languageGame.Init(strlan);
        languageGame.SetLanguage(SystemLanguage.Chinese);
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

    int GetRandomOtherLevelIndex(int level)
    {
        int total = listGuanka.Count;
        int size = total - 1;
        int[] idxTmp = new int[size];
        int idx = 0;
        for (int i = 0; i < total; i++)
        {
            if (i != level)
            {
                idxTmp[idx++] = i;
            }
        }

        int rdm = Random.Range(0, size);
        if (rdm >= size)
        {
            rdm = size - 1;
        }
        idx = idxTmp[rdm];
        return idx;
    }
    public override int ParseGuanka()
    {
        int count = 0;

        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            return listGuanka.Count;
        }

        listGuanka = new List<object>();

        for (int i = 0; i < 70; i++)
        {
            CrossItemInfo info = new CrossItemInfo();
            info.id = (i + 1).ToString();
            ParseCrossItem(info);
            listGuanka.Add(info);
        }

        count = listGuanka.Count;

        Debug.Log("ParseGame::count=" + count);
        return count;
    }


    public void ParseCrossItem(CrossItemInfo info)
    {
        int idx = LevelManager.main.gameLevel;
        string filepath = Common.GAME_RES_DIR + "/guanka/" + info.id + ".txt";
        if (!FileUtil.FileIsExistAsset(filepath))
        {
            return;
        }
        //
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);
        JsonData root = JsonMapper.ToObject(json);
        if (JsonUtil.ContainsKey(root, "test"))
        {
            //double v = (double)root["test"];
            // Debug.Log("v=" + v);
        }
        JsonData dots = root["dots"];
        info.listDot = new List<Vector2>();
        for (int i = 0; i < dots.Count; i++)
        {
            JsonData item = dots[i];
            Vector2 pt = Vector2.zero;
            //listjson 支持double 不支持float
            pt.x = (float)((double)item["x"]);
            pt.y = (float)((double)item["y"]);
            info.listDot.Add(pt);
        }

        info.listLine = new List<Vector2>();
        JsonData lines = root["lines"];
        for (int i = 0; i < lines.Count; i++)
        {
            JsonData item = lines[i];
            Vector2 pt = Vector2.zero;
            pt.x = (float)((double)item["x"]);
            pt.y = (float)((double)item["y"]);
            info.listLine.Add(pt);
        }

    }

}

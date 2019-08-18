using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
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

    public ItemInfo GetItemInfo()
    {
        int idx = LevelManager.main.gameLevel; 
        return GetGuankaItemInfo(idx);
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
        string fileName = Common.GAME_RES_DIR + "/guanka/guanka_list_place" + idx + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            WordItemInfo info = new WordItemInfo();
            info.title = (string)item["title"];
            info.row = Common.String2Int((string)item["row"]);
            info.col = Common.String2Int((string)item["col"]);

 

            listGuanka.Add(info);
        }

        count = listGuanka.Count;

        Debug.Log("ParseGame::count=" + count);
        return count;
    }

}

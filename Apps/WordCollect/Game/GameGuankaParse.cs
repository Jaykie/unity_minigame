using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class PoemContentInfo
{
    public string content;
    public string pinyin;
    public bool skip;
}
public class WordItemInfo : ItemInfo
{
    public string[] listLetter;
    public string[] listAnswer;//答案 LO|SOL 
    public List<AnswerInfo> listAnswerInfo;
    public List<List<object>> listBoard;//top t0 bottom 排列
    public string[] listError;
    public string gameType;
    public string author;
    public string year;
    public string style;
    public string album;
    public string intro;
    public string translation;
    public string appreciation;
    public string pinyin;
    public string head;
    public string end;
    public string tips;
    public string change;

    public List<PoemContentInfo> listPoemContent;

    //idiomconnet
    public List<string> listWord;
    public List<string> listIdiom;
    public List<int> listPosX;
    public List<int> listPosY;
    public List<int> listWordAnswer;

    public string date;
    public string addtime;

}
public class GameGuankaParse : GuankaParseBase
{
    public string strWord3500;
    Language languageGame;
    string[] arrayPunctuation = { "。", "？", "！", "，", "、", "；", "：" };
    static private GameGuankaParse _main = null;
    public static GameGuankaParse main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameGuankaParse();
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

    public WordItemInfo GetItemInfo()
    {
        int idx = LevelManager.main.gameLevel;
        return GetGuankaItemInfo(idx) as WordItemInfo;
    }


    public bool IsPunctuation(string str)
    {
        bool ret = false;

        foreach (string item in arrayPunctuation)
        {
            if (str == item)
            {
                ret = true;
                break;
            }
        }
        return ret;
    }

    //非标点符号文字
    public List<int> IndexListNotPunctuation(string str)
    {
        List<int> listRet = new List<int>();

        int len = str.Length;
        for (int i = 0; i < len; i++)
        {
            string word = str.Substring(i, 1);
            if (!IsPunctuation(word))
            {
                listRet.Add(i);
            }

        }
        return listRet;
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
        if (Common.appKeyName == GameRes.GAME_WORDCONNECT)
        {
            return ParseGuankaWordConnectList();
        }
        if (Common.appKeyName == GameRes.GAME_CONNECT)
        {
            return ParseGuankaConnect();
        }
        if ((Common.appKeyName == GameRes.GAME_IDIOM) || (Common.appKeyName == GameRes.GAME_POEM))
        {
            return ParseGuankaDefault();
        }
        return 0;
    }

    public int ParseGuankaDefault()
    {
        int count = 0;

        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            return listGuanka.Count;
        }
        UpdateLanguage();
        listGuanka = new List<object>();
        int idx = LevelManager.main.placeLevel;

        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        string filepath = Common.GAME_RES_DIR + "/guanka/guanka_list_place" + idx + ".json";
        if (!FileUtil.FileIsExistAsset(filepath))
        {
            filepath = Common.GAME_RES_DIR + "/guanka/item_" + infoPlace.id + ".json";
        }

        //
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);

        JsonData root = JsonMapper.ToObject(json);
        string strPlace = infoPlace.id;
        JsonData items = root["items"];

        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            WordItemInfo info = new WordItemInfo();
            info.id = JsonUtil.JsonGetString(item, "id", "");
            //string str = "aa";// = languageGame.GetString(info.id);
            //Debug.Log(i + ":ParseGame:" + str);
            info.pic = Common.GAME_RES_DIR + "/image/" + strPlace + "/" + info.id + ".png";
            info.icon = Common.GAME_RES_DIR + "/image_thumb/" + strPlace + "/" + info.id + ".png";
            if (!FileUtil.FileIsExistAsset(info.icon))
            {
                info.icon = info.pic;
            }
            string key = "xiehouyu";
            if (JsonUtil.ContainsKey(item, key))
            {
                JsonData xiehouyu = item[key];
                for (int j = 0; j < xiehouyu.Count; j++)
                {
                    JsonData item_xhy = xiehouyu[j];
                    if (j == 0)
                    {
                        info.head = (string)item_xhy["content"];
                    }
                    if (j == 1)
                    {
                        info.end = (string)item_xhy["content"];
                    }
                }

            }

            key = "head";
            if (JsonUtil.ContainsKey(item, key))
            {
                //Riddle
                info.head = (string)item["head"];
                info.end = (string)item["end"];
                info.tips = (string)item["tips"];
                info.type = (string)item["type"];
            }

            if (Common.appKeyName == GameRes.GAME_IDIOM)
            {
                info.gameType = GameRes.GAME_TYPE_IMAGE;
            }
            else if (Common.appKeyName == GameRes.GAME_POEM)
            {
                info.gameType = GameRes.GAME_TYPE_POEM;
            }
            else if (Common.appKeyName == GameRes.GAME_CONNECT)
            {
                info.gameType = GameRes.GAME_TYPE_CONNECT;
            }
            else
            {
                info.gameType = GameRes.GAME_TYPE_TEXT;
            }

            if (Common.appKeyName == GameRes.GAME_POEM)
            {
                ParsePoemItem(info);
            }
            listGuanka.Add(info);
        }

        count = listGuanka.Count;

        for (int i = 0; i < listGuanka.Count; i++)
        {
            WordItemInfo info = listGuanka[i] as WordItemInfo;
            string word0 = GameAnswer.main.GetGuankaAnswer(info, false, 0);
            int idx1 = GetRandomOtherLevelIndex(i);
            WordItemInfo info1 = listGuanka[idx1] as WordItemInfo;
            string word1 = GameAnswer.main.GetGuankaAnswer(info1, false, 0);
            // word1 = word1.Substring(0, word1.Length / 2);
            string word = word0 + word1;
            if (Common.appKeyName == GameRes.GAME_POEM)
            {
                word = word0;
            }

            Debug.Log("word = " + word);

            info.listLetter = new string[word.Length];
            for (int k = 0; k < word.Length; k++)
            {
                info.listLetter[k] = word.Substring(k, 1);
            }

            if (Common.appKeyName == GameRes.GAME_POEM)
            {
                info.listAnswer = new string[info.listPoemContent.Count];
                for (int k = 0; k < info.listPoemContent.Count; k++)
                {
                    PoemContentInfo infoPoem = info.listPoemContent[k];
                    Debug.Log("word answer = " + infoPoem.content);
                    info.listAnswer[k] = infoPoem.content;
                }
            }
            else
            {
                info.listAnswer = new string[1];
                info.listAnswer[0] = word0;
                // info.listAnswer[1] = word1;

            }
        }

        //word3500
        filepath = Common.GAME_DATA_DIR + "/words_3500.json";
        if (FileUtil.FileIsExistAsset(filepath))
        {
            json = FileUtil.ReadStringAsset(filepath);
            root = JsonMapper.ToObject(json);
            strWord3500 = (string)root["words"];
            Debug.Log(strWord3500);
        }


        Debug.Log("ParseGame::count=" + count);
        return count;
    }

    public void UpdateLetterString(int idx)
    {
        WordItemInfo info = GetItemInfo();
        if (Common.appKeyName == GameRes.GAME_WORDCONNECT)
        {
            return;
        }

        string word = GameAnswer.main.GetGuankaAnswer(info, false, idx);
        info.listLetter = new string[word.Length];
        for (int k = 0; k < word.Length; k++)
        {
            info.listLetter[k] = word.Substring(k, 1);
        }
        Debug.Log("word = UpdateLetterString =" + word);
    }

    public string GetWordFromBoard(WordItemInfo info, int idx, bool isByRow)
    {
        string ret = "";
        List<object> ls = null;
        bool isEnd = false;
        bool isError = false;
        int count = 0;
        if (isByRow)
        {
            ls = info.listBoard[idx];
            count = ls.Count;
        }
        else
        {
            count = info.listBoard.Count;
        }
        for (int i = 0; i < count; i++)
        {
            string str = "";
            if (isByRow)
            {
                str = (string)ls[i];
            }
            else
            {
                ls = info.listBoard[i];
                str = (string)ls[idx];
            }


            if (!Common.BlankString(str))
            {
                if (!isEnd)
                {
                    ret += str;
                }
                else
                {
                    //有效字符不连续为非答案
                    isError = true;
                }
            }
            else
            {
                if (!Common.BlankString(ret))
                {
                    //检测到空表示结束
                    isEnd = true;
                    break;
                }
            }
        }

        if (isError)
        {
            ret = "";
        }
        return ret;
    }
    public void ParseWordAnswer(WordItemInfo info)
    {
        int row = info.listBoard.Count;
        int col = info.listBoard[0].Count;

        info.listAnswerInfo = new List<AnswerInfo>();

        for (int i = 0; i < row; i++)
        {

            string str = GetWordFromBoard(info, i, true);
            if (!Common.BlankString(str) && (str.Length > 1))
            {
                AnswerInfo infoanswer = new AnswerInfo();
                infoanswer.word = str;
                infoanswer.row = i;
                infoanswer.col = -1;
                info.listAnswerInfo.Add(infoanswer);
            }
        }

        for (int i = 0; i < col; i++)
        {
            string str = GetWordFromBoard(info, i, false);
            if (!Common.BlankString(str) && (str.Length > 1))
            {
                AnswerInfo infoanswer = new AnswerInfo();
                infoanswer.word = str;
                infoanswer.row = -1;
                infoanswer.col = i;
                info.listAnswerInfo.Add(infoanswer);
            }
        }

        info.listAnswer = new string[info.listAnswerInfo.Count];
        for (int i = 0; i < info.listAnswerInfo.Count; i++)
        {
            AnswerInfo infoanswer = info.listAnswerInfo[i];
            info.listAnswer[i] = infoanswer.word;
        }

    }
    //接龙
    public int ParseGuankaConnect()
    {
        int count = 0;

        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            return listGuanka.Count;
        }

        listGuanka = new List<object>();
        int idx = LevelManager.main.placeLevel;
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(idx);
        string filepath = Common.GAME_RES_DIR + "/guanka/guanka_list_place" + idx + ".json";
        if (!FileUtil.FileIsExistAsset(filepath))
        {
            filepath = Common.GAME_RES_DIR + "/guanka/item_" + infoPlace.id + ".json";
        }

        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
                                                         // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);

        char[] charSplit = { '|' };

        for (int i = 0; i < root.Count; i++)
        {
            WordItemInfo info = new WordItemInfo();
            JsonData item = root[i];
            JsonData itemData = item["data"];
            JsonData letters = itemData["letters"];

            info.listLetter = new string[letters.Count];
            for (int j = 0; j < letters.Count; j++)
            {
                info.listLetter[j] = (string)letters[j];
            }


            JsonData board = itemData["board"];
            info.listBoard = new List<List<object>>();
            for (int j = 0; j < board.Count; j++)
            {
                List<object> ls = new List<object>();
                JsonData it = board[j];
                for (int k = 0; k < it.Count; k++)
                {
                    ls.Add((string)it[k]);
                }
                info.listBoard.Add(ls);

            }
            ParseWordAnswer(info);
            //  info.listAnswer = str.Split(charSplit);

            info.gameType = GameRes.GAME_TYPE_CONNECT;
            listGuanka.Add(info);
        }

        count = listGuanka.Count;

        Debug.Log("ParseGame::count=" + count);
        return count;
    }

    public int ParseGuankaWordConnectList()
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
            info.listAnswerInfo = new List<AnswerInfo>();
            for (int j = 0; j < info.listAnswer.Length; j++)
            {
                AnswerInfo infoanswer = new AnswerInfo();
                infoanswer.word = info.listAnswer[j];
                info.listAnswerInfo.Add(infoanswer);
            }

            str = (string)item["e"];
            info.listError = str.Split(charSplit);

            info.gameType = GameRes.GAME_TYPE_WORDLIST;

            listGuanka.Add(info);
        }

        count = listGuanka.Count;

        Debug.Log("ParseGame::count=" + count);
        return count;
    }


    public void ParseItem(WordItemInfo info)
    {

        if (Common.appKeyName == GameRes.GAME_IDIOM)
        {
            ParseIdiomItem(info);
        }



    }

    public void ParseIdiomItem(WordItemInfo info)
    {
        string filepath = Common.GAME_RES_DIR + "/guanka/data/" + (info.id) + ".json";
        if (!FileUtil.FileIsExistAsset(filepath))
        {
            return;
        }
        //
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);
        JsonData root = JsonMapper.ToObject(json);
        info.title = (string)root["title"];
        info.album = (string)root["album"];
        info.translation = (string)root["translation"];
        info.pinyin = (string)root["pinyin"];
    }

    //过滤标点符号 点号：句号（ 。）、问号（ ？）、感叹号（ ！）、逗号（ ，）顿号（、）、分号（；）和冒号（：）。
    public string FilterPunctuation(string str)
    {
        string ret = str;

        foreach (string item in arrayPunctuation)
        {
            ret = ret.Replace(item, "");
        }
        return ret;
    }
    //诗词
    public void ParsePoemItem(WordItemInfo info)
    {
        string filepath = Common.GAME_RES_DIR + "/guanka/poem/" + info.id + ".json";
        if (!FileUtil.FileIsExistAsset(filepath))
        {
            return;
        }
        //
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);
        JsonData root = JsonMapper.ToObject(json);
        info.title = (string)root["title"];
        info.author = (string)root["author"];
        info.year = (string)root["year"];
        info.style = (string)root["style"];
        info.album = (string)root["album"];
        info.url = (string)root["url"];
        info.intro = (string)root["intro"];
        info.translation = (string)root["translation"];
        info.appreciation = (string)root["appreciation"];

        JsonData itemPoem = root["poem"];
        info.listPoemContent = new List<PoemContentInfo>();
        for (int i = 0; i < itemPoem.Count; i++)
        {
            JsonData item = itemPoem[i];
            string str = (string)item["content"];
            string[] strlist = new string[1];
            strlist[0] = str;
            if (Common.appKeyName == GameRes.GAME_POEM)
            {
                strlist = str.Split('，');
            }
            for (int k = 0; k < strlist.Length; k++)
            {

                PoemContentInfo infoPoem = new PoemContentInfo();
                infoPoem.content = strlist[k];
                infoPoem.pinyin = (string)item["pinyin"];
                bool isSkip = JsonUtil.JsonGetBool(item, "skip", false);
                if (Common.appKeyName == GameRes.GAME_POEM)
                {
                    infoPoem.content = FilterPunctuation(infoPoem.content);
                    // isSkip = false;
                }

                if (!isSkip)
                {
                    info.listPoemContent.Add(infoPoem);
                }

            }
        }
    }
}

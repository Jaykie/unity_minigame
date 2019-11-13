
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoveDB
{
    public const string TABLE_NAME = "table_items";
    DBToolSqliteKit dbTool;
    public string dbFileName;

    public const string KEY_id = "id";
    public const string KEY_title = "title";
    public const string KEY_change = "change";
    public const string KEY_translation = "translation";
    public const string KEY_date = "date";
    public const string KEY_addtime = "addtime";


    public const string KEY_text = "text";

    string[] item_col = new string[] { KEY_id, KEY_title, KEY_translation, KEY_change};
    static public string strSaveWordShotDir//字截图保存目录
    {
        get
        {
            return Application.temporaryCachePath + "/SaveItem";
        }
    }

    public string dbFilePath
    {
        get
        {

            string appDBPath = Application.temporaryCachePath + "/" + dbFileName;
            Debug.Log("appDBPath=" + appDBPath);

            return appDBPath;
        }
    }
    static LoveDB _main = null;
    static bool isInited = false;
    public static LoveDB main
    {
        get
        {
            if (!isInited)
            {
                isInited = true;
                _main = new LoveDB();
                Debug.Log("LoveDB main init");
                _main.dbFileName = "LoveDB_" + Common.appKeyName + ".sqlite";
                _main.CreateDb();
            }
            return _main;
        }


        //ng:
        //  get
        // {
        //     if (_main==null)
        //     {
        //         _main = new LoveDB();
        //         Debug.Log("LoveDB main init");
        //         _main.CreateDb();
        //     }
        //     return _main;
        // }
    }

    void CreateDb()
    {
        dbTool = new DBToolSqliteKit();
        OpenDB();
        string[] item_coltype = new string[item_col.Length];
        for (int i = 0; i < item_coltype.Length; i++)
        {
            item_coltype[i] = KEY_text;
        }

        if (item_col.Length != item_coltype.Length)
        {
            Debug.Log("DB Table Error");
        }
        if (!dbTool.IsExitTable(TABLE_NAME))
        {
            dbTool.CreateTable(TABLE_NAME, item_col, item_coltype);
        }

        CloseDB();
    }

    void OpenDB()
    {
        dbTool.OpenDB(dbFilePath);
        //   string[] item_col = new string[] { "id,filesave,date,addtime" };
        //   string[] item_coltype = new string[] { "string,string,string,string" };

    }
    //判断是否空
    public bool DBEmpty()
    {
        bool ret = true;
        string strsql = "select id from " + TABLE_NAME;// + " order by addtime desc";
        OpenDB();
        //SqliteDataReader reader = dbTool.ReadFullTable(TABLE_NAME);//
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            ret = false;
            break;
        }

        reader.Release();

        CloseDB();
        return ret;
    }

    void CloseDB()
    {
        dbTool.CloseDB();
    }

    public void ClearDB()
    {
        //
        string dir = strSaveWordShotDir;
        //Directory.Delete(dir);
        DirectoryInfo TheFolder = new DirectoryInfo(dir);

        // //遍历文件
        // foreach (FileInfo NextFile in TheFolder.GetFiles())
        // {
        //     NextFile.Delete();

        // }
        OpenDB();
        dbTool.DeleteContents(TABLE_NAME);
        CloseDB();
    }

    //2017.1.10 to 1.10
    static public string getDateDisplay(string date)
    {
        int idx = date.IndexOf(".");
        if (idx >= 0)
        {
            string str = date.Substring(idx + 1);
            return str;
        }
        return date;
    }

    public void AddItem(WordItemInfo info)
    {
        OpenDB();
        int lengh = item_col.Length;
        string[] values = new string[lengh];
        //id,filesave,date,addtime 

        values[0] = info.id;
        //values[0] = "性";//ng

        values[1] = info.title;
        values[2] = info.translation;
        values[3] = info.change;

        // int year = System.DateTime.Now.Year;
        // int month = System.DateTime.Now.Month;
        // int day = System.DateTime.Now.Day;
        // string str = year + "." + month + "." + day;
        // Debug.Log("date:" + str);
        // values[lengh - 2] = str;
        // long time_ms = Common.GetCurrentTimeMs();//GetCurrentTimeSecond
        // values[lengh - 1] = time_ms.ToString();


        dbTool.InsertInto(TABLE_NAME, values);

        CloseDB();
    }


    public void DeleteItem(WordItemInfo info)
    {
        OpenDB();
        // string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'" + " and addtime = '" + info.addtime + "'";
        string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'";
        dbTool.ExecuteQuery(strsql, true);
        CloseDB();
    }


    public bool IsItemExist(WordItemInfo info)
    {
        bool ret = false;
        OpenDB();
        //string strsql = "SELECT count(*) FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'";
        string strsql = "SELECT * FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'";
        SQLiteQuery qr = dbTool.ExecuteQuery(strsql, false);
        int count = 0;//qr.GetCount();
        while (qr.Step())// 循环遍历数据 
        {
            count++;
        }
        qr.Release();
        Debug.Log("IsItemExist count=" + count);
        CloseDB();
        if (count > 0)
        {
            ret = true;
        }
        return ret;
    }

    void ReadInfo(WordItemInfo info, SQLiteQuery rd)
    {
        info.id = rd.GetString(KEY_id);
        info.title = rd.GetString(KEY_title);
        info.translation = rd.GetString(KEY_translation);
        info.change = rd.GetString(KEY_change);
    }

    public List<WordItemInfo> GetAllItem()
    {
        // Distinct 去掉重复
        //desc 降序 asc 升序 
        string strsql = "select DISTINCT id from " + TABLE_NAME;// + " order by addtime desc";

        List<WordItemInfo> listRet = new List<WordItemInfo>();
        OpenDB();
        //SqliteDataReader reader = dbTool.ReadFullTable(TABLE_NAME);//
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        Debug.Log("GetAllItem start read");
        while (reader.Step())// 循环遍历数据 
        {
            Debug.Log("GetAllItem reading");
            WordItemInfo info = new WordItemInfo();
            ReadInfo(info, reader);
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        Debug.Log("GetAllItem:" + listRet.Count);
        return listRet;
    }

    public List<WordItemInfo> GetAllDate()
    {
        // Distinct 去掉重复
        //desc 降序 asc 升序 
        string strsql = "select DISTINCT date from " + TABLE_NAME + " order by addtime desc";

        List<WordItemInfo> listRet = new List<WordItemInfo>();
        OpenDB();
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            WordItemInfo info = new WordItemInfo();
            info.date = reader.GetString("date");
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return listRet;
    }

    public List<WordItemInfo> GetItemByDate(string date)
    {
        string strsql = "select * from " + TABLE_NAME + " where date = '" + date + "'" + "order by addtime desc";
        List<WordItemInfo> listRet = new List<WordItemInfo>();
        OpenDB();

        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            WordItemInfo info = new WordItemInfo();
            ReadInfo(info, reader);
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return listRet;
    }


    public List<WordItemInfo> GetItemById(string id)
    {
        string strsql = "select * from " + TABLE_NAME + " where id = '" + id + "'" + "order by addtime desc";
        List<WordItemInfo> listRet = new List<WordItemInfo>();
        OpenDB();
        //"select * from %s where keyZi = \"%s\" order by addtime desc"
        SQLiteQuery reader = dbTool.ExecuteQuery(strsql, false);
        while (reader.Step())// 循环遍历数据 
        {
            WordItemInfo info = new WordItemInfo();
            ReadInfo(info, reader);
            listRet.Add(info);
        }

        reader.Release();

        CloseDB();
        return listRet;
    }

}


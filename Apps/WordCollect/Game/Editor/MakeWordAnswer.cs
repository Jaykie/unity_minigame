using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor.Build.Reporting;
using LitJson;
using System.Text;

public class WordAnswerJsonItemInfo
{
    //public string pic;//0,0,100,100
    public string id;
}

public class MakeWordAnswer
{

    static public string filepathWordAnswer = Application.streamingAssetsPath + "/" + Common.GAME_RES_DIR + "/wordanswer.json";

    [MenuItem("Custom/MakeWordAnswer")]
    static void OnMakeWordAnswer()
    {
        Debug.Log("OnMakeWordAnswer start");
        List<WordAnswerJsonItemInfo> listItemJson;
        int total_place = LevelManager.main.placeTotal;
        listItemJson = new List<WordAnswerJsonItemInfo>();
        for (int place = 0; place < total_place; place++)
        {
            LevelManager.main.placeLevel = place;
            GameLevelParse.main.CleanGuankaList();
            GameLevelParse.main.ParseGuanka();
            for (int i = 0; i < GameLevelParse.main.listGuanka.Count; i++)
            {
                WordItemInfo info = GameLevelParse.main.listGuanka[i] as WordItemInfo;
                for (int j = 0; j < info.listAnswerInfo.Count; j++)
                {
                    AnswerInfo infoanswer = info.listAnswerInfo[j];
                    WordAnswerJsonItemInfo infojson = new WordAnswerJsonItemInfo();
                    infojson.id = infoanswer.word;
                    listItemJson.Add(infojson);
                }

            }

        }

        {

            Hashtable data = new Hashtable();
            data["items"] = listItemJson;
            string strJson = JsonMapper.ToJson(data);
            //Debug.Log(strJson);

            byte[] bytes = Encoding.UTF8.GetBytes(strJson);
            System.IO.File.WriteAllBytes(filepathWordAnswer, bytes);
        }
        Debug.Log("OnMakeWordAnswer end count =" + listItemJson.Count);
    }


}
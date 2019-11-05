
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AnswerInfo
{
    public int index;
    public bool isFinish;//是否答对
    public string word;//答案
    public bool isFillWord;//是否填了字
    public string wordFill;//实际填充的字
}
public class GameAnswer
{

    public string strWordAnswer = "";

    static private GameAnswer _main = null;
    public static GameAnswer main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameAnswer();
            }
            return _main;
        }
    }


    public string GetGuankaAnswer(WordItemInfo info, bool isRandom, int idx)
    {
        string str = "";
        //真正的答案
        if ((info.gameType == GameRes.GAME_TYPE_IMAGE) || (info.gameType == GameRes.GAME_TYPE_IMAGE_TEXT))
        {
            //str = UIGameCaiCaiLe.languageWord.GetString(info.id);
            str = info.id;
            //歇后语
            if ((!Common.BlankString(info.head)) && (!Common.BlankString(info.end)))
            {
                return info.end;
            }
        }

        if (info.gameType == GameRes.GAME_TYPE_CONNECT)
        {
            for (int i = 0; i < info.listWordAnswer.Count; i++)
            {
                int idxtmp = info.listWordAnswer[i];
                string word = info.listWord[idxtmp];
                int rdm = Random.Range(0, str.Length);
                //是否打乱
                if (!isRandom)
                {
                    rdm = str.Length;
                    if (rdm < 0)
                    {
                        rdm = 0;
                    }
                }

                str = str.Insert(rdm, word);
                Debug.Log("GetGuankaAnswer rdm=" + rdm + " word=" + word + " str=" + str);
            }
        }

        if (Common.appKeyName == GameRes.GAME_POEM)
        {
            PoemContentInfo infoPoem = info.listPoemContent[idx];
            str = infoPoem.content;
        }
        return str;
    }

    public string GetWordBoardString(WordItemInfo info, int row, int col)
    {
        string ret = "";
        string answer = GetInsertToBoardAnswer(info);

        int len = answer.Length;
        int total = row * col;
        Debug.Log("UIWordBoard GetWordBoardString:" + answer + " answer.len=" + len);
        string strAllWord = GetAllWordWithoutAnswer(answer);
        string strRandom = GetRandomWordFromAllWord(total - len, strAllWord);
        ret = answer + strRandom;
        ret = Common.RandomString(ret);
        Debug.Log("UIWordBoard GetWordBoardString:" + " total=" + total + " ret=" + ret + " ret.count=" + ret.Length);
        return ret;
    }

    //从常用3500的汉字中去除答案字符 避免重复
    string GetAllWordWithoutAnswer(string answer)
    {
        string strret = "";
        string strAllWord = GameGuankaParse.main.strWord3500;
        int len = strAllWord.Length;
        for (int i = 0; i < len; i++)
        {
            string word = strAllWord.Substring(i, 1);
            if (!IsWordInString(word, answer))
            {
                strret += word;
            }

        }
        return strret;
    }


    bool IsWordInString(string word, string strAnswer)
    {
        bool ret = false;
        int len = strAnswer.Length;
        for (int i = 0; i < len; i++)
        {
            string wordAnswer = strAnswer.Substring(i, 1);
            if (word == wordAnswer)
            {
                ret = true;
                Debug.Log("word in answer:" + word);
                break;
            }

        }

        return ret;
    }



    //从allword中随机抽取count个word
    string GetRandomWordFromAllWord(int count, string strAll)
    {
        string strret = "";
        int[] indexSel = Common.RandomIndex(strAll.Length, count);
        for (int i = 0; i < indexSel.Length; i++)
        {
            int idx = indexSel[i];
            string str = strAll.Substring(idx, 1);
            strret += str;
        }
        return strret;
    }

    public int GetOtherGuankaIndex()
    {
        int idx = 0;
        int gamelevel = LevelManager.main.gameLevel;
        int total = LevelManager.main.maxGuankaNum;
        if (total > 1)
        {

            int size = total - 1;
            int[] idxTmp = new int[size];


            for (int i = 0; i < total; i++)
            {
                if (i != gamelevel)
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
        }
        return idx;
    }

    //插入Board的最终答案
    public string GetInsertToBoardAnswer(WordItemInfo info)
    {
        //真正的答案
        string str = GameAnswer.main.GetGuankaAnswer(info, true, 0);
        Debug.Log("UIWordBoard GetGuankaAnswer:" + str);
        switch (info.gameType)
        {
            case GameRes.GAME_TYPE_TEXT:
                {
                    str = GameAnswer.main.strWordAnswer;
                }
                break;
            case GameRes.GAME_TYPE_CONNECT:
                {
                    //str = GameAnswer.main.GetGuankaAnswer(info);
                }
                break;
            case GameRes.GAME_TYPE_IMAGE:
                {
                    //随机抽取其他关卡的答案
                    int total = LevelManager.main.maxGuankaNum;
                    if (total > 1)
                    {
                        int idx = GetOtherGuankaIndex();
                        WordItemInfo infoOther = GameGuankaParse.main.GetGuankaItemInfo(idx) as WordItemInfo;
                        if (infoOther != null)
                        {
                            string strOther = GameAnswer.main.GetGuankaAnswer(infoOther, true, 0);
                            string strtmp = RemoveSameWord(str, strOther);
                            str += strtmp;
                            Debug.Log("UIWordBoard other strOther=:" + strOther + " RemoveSameWord:" + strtmp);
                        }

                    }
                }
                break;

        }

        return str;
    }

    //从str2中过滤在str1重复的字
    string RemoveSameWord(string str1, string str2)
    {
        string ret = "";
        int len = str2.Length;
        for (int i = 0; i < len; i++)
        {
            string word = str2.Substring(i, 1);
            if (!IsWordInString(word, str1))
            {
                ret += word;
            }

        }
        return ret;
    }




}

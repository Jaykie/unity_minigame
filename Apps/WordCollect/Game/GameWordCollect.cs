using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/*
参考游戏： Word Collect: Word Games
https://apps.apple.com/cn/app/id1299956969
https://www.taptap.com/app/72589
 */
public class GameWordCollect : GameBase
{
    public const float RATIO_RECT = 0.9f;
    public LetterConnect letterConnectPrefab;
    public LetterConnect letterConnect;

    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {


    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        LayOut();
    }
    public override void LayOut()
    {
        float x, y, z, w, h;
        Vector2 sizeWorld = Common.GetWorldSize(mainCam);
        Vector2 sizeCanvas = this.frame.size;
        float ratio = 1f;
        if (sizeCanvas.x <= 0)
        {
            return;
        }

        //letter connect
        if (letterConnect != null)
        {
            ratio = RATIO_RECT;
            RectTransform rctan = letterConnect.GetComponent<RectTransform>();
            float oft_bottom = GameManager.main.heightAdWorld;// + Common.ScreenToCanvasHeigt(sizeCanvas, Device.offsetBottom);
            if (Device.isLandscape)
            {
                w = sizeWorld.x / 2 * ratio;
                h = (sizeWorld.y - oft_bottom * 2) * ratio;
                y = 0;
                x = sizeWorld.x / 4;
            }
            else
            {
                w = sizeWorld.x * ratio;
                h = (sizeWorld.y / 2 - oft_bottom) * ratio;
                x = 0;
                y = (-sizeWorld.y / 2 + oft_bottom) / 2;
            }


            rctan.sizeDelta = new Vector2(w, h);
            z = letterConnect.transform.position.z;
            letterConnect.transform.position = new Vector3(x, y, z);
            letterConnect.LayOut();
        }


    }
    public void UpdateGuankaLevel(int level)
    {
        WordItemInfo info = (WordItemInfo)GameGuankaParse.main.GetGuankaItemInfo(level);
        letterConnect = (LetterConnect)GameObject.Instantiate(letterConnectPrefab);
        //AppSceneBase.main.AddObjToMainWorld(letterConnect.gameObject); 
        letterConnect.transform.SetParent(this.transform);
        UIViewController.ClonePrefabRectTransform(letterConnectPrefab.gameObject, letterConnect.gameObject);
        letterConnect.transform.localPosition = new Vector3(0f, 0f, -1f);

    }


}

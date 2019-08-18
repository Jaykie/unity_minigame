
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Assertions;

using GameVanilla.Core;
using GameVanilla.Game.Common;
using GameVanilla.Game.Popups;
using GameVanilla.Game.UI;


/*
参考游戏： Math Academy
https://itunes.apple.com/cn/app/id1001897231
 */
public class WordItemInfo : ItemInfo
{
    public List<object> listFormulation;//公式
    public List<string> listBox;//数字显示

}

namespace GameVanilla.Game.Scenes
{
    public class GameCandyMatch : GameScene//BaseScene
    {


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        public void Awake()
        {

            LoadPrefab();

            //@moon
            if (canvas == null)
            {
                canvas = AppSceneBase.main.canvasMain;
            }
            if (gameBoard.gameScene == null)
            {
                gameBoard.gameScene = this;
            }
            //@moon
            LayOut();
            base.Awake();
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            LayOut();
            base.Start(); 
        }

    

        void LoadPrefab()
        {
            // {
            //     GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UIGameBoxItemMathMaster");
            //     if (obj != null)
            //     {
            //         uiGameBoxItemPrefab = obj.GetComponent<UIGameBoxItemMathMaster>();
            //     }
            // }
            // {
            //     GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UIGameBoxMathMaster");
            //     if (obj != null)
            //     {
            //         uiGameBoxPrefab = obj.GetComponent<UIGameBoxMathMaster>();
            //     }
            // }

        }
        public override void LayOut()
        {


        }
    }

}



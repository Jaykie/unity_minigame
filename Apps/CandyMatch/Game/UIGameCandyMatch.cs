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

namespace GameVanilla.Game.Scenes
{
    public class UIGameCandyMatch : UIGameBase
    {
        GameCandyMatch gameCandyMatch;
        GameCandyMatch gameCandyMatchPrefab;

        public GameUi gameUi;


        [SerializeField]
        private Image ingameBoosterPanel;

        [SerializeField]
        private Text ingameBoosterText;


        public BaseScene gameScene;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            LoadPrefab();

            LayOut();

        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            LayOut();
            //LayoutChild 必须在前面执行
            UpdateGuankaLevel(LevelManager.main.gameLevel);


            float delaytime = 0.1f * 10;
            Invoke("OnUIDidFinish", delaytime);
        }


        void LoadPrefab()
        {

            {
                GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/GameCandyMatch");
                if (obj != null)
                {
                    gameCandyMatchPrefab = obj.GetComponent<GameCandyMatch>();
                }
            }
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
            float x, y, w, h;

            Vector2 sizeCanvas = this.frame.size;
            if (sizeCanvas.x <= 0)
            {
                return;
            }
            LayoutChildBase();

        }

        public override void UpdateGuankaLevel(int level)
        {
            base.UpdateGuankaLevel(level);
            WordItemInfo info = (WordItemInfo)GameGuankaParse.main.GetGuankaItemInfo(level);
            AppSceneBase.main.ClearMainWorld();
            {
                gameCandyMatch = (GameCandyMatch)GameObject.Instantiate(gameCandyMatchPrefab);
                AppSceneBase.main.AddObjToMainWorld(gameCandyMatch.gameObject);
                gameCandyMatch.transform.localPosition = new Vector3(0f, 0f, -1f);
                RectTransform rctranPrefab = gameCandyMatchPrefab.transform as RectTransform;
                RectTransform rctran = gameCandyMatch.transform as RectTransform;
                // 初始化rect
                rctran.offsetMin = rctranPrefab.offsetMin;
                rctran.offsetMax = rctranPrefab.offsetMax;

                float value_z = 1f;
                AppSceneBase.main.objSpriteBg.transform.localPosition = new Vector3(0, 0, value_z + 20);

                gameCandyMatch.transform.localPosition = new Vector3(0f, 0f, -value_z);

            }
            gameCandyMatch.gameBoard.gameUi = gameUi;

            //  gameCandyMatch.LoadLevel();
            //   gameCandyMatch.StartGame();

        }

        public void OnClickBtnPause()
        {
            if (gameScene.currentPopups.Count == 0)
            {
                gameScene.OpenPopup<InGameSettingsPopup>("Popups/InGameSettingsPopup");
            }
            else
            {
                gameScene.CloseCurrentPopup();
            }
        }


    }


}



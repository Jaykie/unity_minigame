// Copyright (C) 2017-2018 gamevanilla. All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.

using GameVanilla.Core;
using GameVanilla.Game.Common;

namespace GameVanilla.Game.Popups
{
    /// <summary>
    /// This class contains the logic associated to the popup that is shown when a player tries to exit a game.
    /// </summary>
    public class ExitGamePopup : Popup
    {
        /// <summary>
        /// Called when the close button is pressed.
        /// </summary>
        public void OnCloseButtonPressed()
        {
            Close();
        }


        public void DoExit()
        {
            NaviViewController navi = GameViewController.main.naviController;
            if (navi != null)
            {
                navi.Pop();
            }
        }
        /// <summary>
        /// Called when the exit button is pressed.
        /// </summary>
        public void OnExitButtonPressed()
        {
            PuzzleMatchManager.instance.livesSystem.RemoveLife();
            //@moon
            NaviViewController navi = GameViewController.main.naviController;
            if (navi != null)
            {
                Close();
                Loom.QueueOnMainThread(() =>
{

    DoExit();

}, 0.3f);

                return;
            }
            //@moon

            GetComponent<SceneTransition>().PerformTransition();
        }

        /// <summary>
        /// Called when the resume button is pressed.
        /// </summary>
        public void OnResumeButtonPressed()
        {
            Close();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.IAP;
using Moonma.Share;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UITypeButtonEditor : Editor
{
    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {

    }

    [MenuItem("GameObject/UI/Moonma/UITypeButton", false, 4)]
    static void CreateTypeButton()
    {
        // GameObject obj = new GameObject("UITypeButton");
        // UITypeButton ui = obj.AddComponent<UITypeButton>();

        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = PrefabCache.main.Load("Common/Prefab/UIKit/UIButton/UITypeButton");
            if (obj != null)
            {
                UITypeButton uiPrefab = obj.GetComponent<UITypeButton>();
                UITypeButton ui = (UITypeButton)GameObject.Instantiate(uiPrefab);
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
            }
        }

    }
}

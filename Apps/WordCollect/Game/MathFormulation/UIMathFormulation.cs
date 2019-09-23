using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

public class UIMathFormulation : UIView, ITableViewDataSource
{

    public GameObject objTableViewTemplate;

    public UICellBase cellPrefab;//GuankaItemCell GameObject 
    public UICellItemBase cellItemPrefab;
    public TableView tableView;
    public int numRows;
    private int numInstancesCreated = 0;

    private int oneCellNum;
    //appcenter
    private List<object> listItem;
    int heightCell;
    int totalItem;
    int indexCellItemTips;

    // Use this for initialization
    void Awake()
    {
        // listItem = new List<object>();
        LoadPrefab();
        oneCellNum = 2;
        if (Device.isLandscape)
        {
            oneCellNum = 1;
        }
        heightCell = 100;
        Rect rccell = (cellPrefab.transform as RectTransform).rect;
        Rect rctable = (tableView.transform as RectTransform).rect;
        //oneCellNum = (int)(rctable.width / rccell.height);

        totalItem = 0;

        //.Log("MoreApp Start 1");
        numRows = 0;

        tableView.dataSource = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void LoadPrefab()
    {

        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_UICELLBASE);
            cellPrefab = obj.GetComponent<UICellBase>();
        }
        {
            GameObject obj = PrefabCache.main.Load(AppRes.PREFAB_MathFormulationCellItem);
            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }
    public void UpdateItem(WordItemInfo info)
    {
        indexCellItemTips = 0;
        // listItem = info.listFormulation;
        // totalItem = listItem.Count;//cene.MAX_GUANKA_NUM;

        // numRows = totalItem / oneCellNum;
        // if (totalItem % oneCellNum != 0)
        // {
        //     numRows++;
        // }

        // tableView.ReloadData();
    }
    //运算正确
    public void OnFormulationFinish(string formulation, int idx)
    {
        UIMathFormulationCellItem item = GetMathFormulationCellItem(idx);
        if (item != null)
        {
            item.OnFormulationFinish();
        }
    }

    //提示
    public void OnTips()
    {
        UIMathFormulationCellItem item = GetMathFormulationCellItem(indexCellItemTips);
        if (item != null)
        {
            if (item.IsAllUnLock())
            {
                //go to next
                indexCellItemTips++;
                if (indexCellItemTips < totalItem)
                {
                    OnTips();
                    return;
                }

            }

            item.OnTips();
        }
    }
    public void OnCellItemDidClick(UICellItemBase item)
    {

    }

    UIMathFormulationCellItem GetMathFormulationCellItem(int idx)
    {
        int row = idx / oneCellNum;
        int indexCellItem = idx % oneCellNum;
        UIMathFormulationCellItem cellItem = null;
        UICellBase cell = tableView.GetCellAtRow(row) as UICellBase;
        if (cell != null)
        {
            cellItem = cell.GetItem(indexCellItem) as UIMathFormulationCellItem;
        }
        return cellItem;
    }

    #region ITableViewDataSource

    //Will be called by the TableView to know how many rows are in this table
    public int GetNumberOfRowsForTableView(TableView tableView)
    {
        return numRows;
    }

    //Will be called by the TableView to know what is the height of each row
    public float GetHeightForRowInTableView(TableView tableView, int row)
    {
        return heightCell;
    }

    //Will be called by the TableView when a cell needs to be created for display
    public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
    {
        UICellBase cell = tableView.GetReusableCell(cellPrefab.reuseIdentifier) as UICellBase;
        if (cell == null)
        {
            cell = (UICellBase)GameObject.Instantiate(cellPrefab);
            cell.name = "UICellBase" + (++numInstancesCreated).ToString();
            Rect rccell = (cellPrefab.transform as RectTransform).rect;
            Rect rctable = (tableView.transform as RectTransform).rect;
            Vector2 sizeCell = (cellPrefab.transform as RectTransform).sizeDelta;
            Vector2 sizeTable = (tableView.transform as RectTransform).sizeDelta;
            Vector2 sizeCellNew = sizeCell;
            sizeCellNew.x = rctable.width;
            //int i =0;
            for (int i = 0; i < oneCellNum; i++)
            {
                int itemIndex = row * oneCellNum + i;
                float cell_space = 10;
                UICellItemBase item = (UICellItemBase)GameObject.Instantiate(cellItemPrefab);
                //item.itemDelegate = this;
                Rect rcItem = (item.transform as RectTransform).rect;
                item.width = (rctable.width - cell_space * (oneCellNum - 1)) / oneCellNum;
                item.height = heightCell;
                item.transform.SetParent(cell.transform, false);
                item.index = itemIndex;
                item.totalItem = totalItem;
                item.callbackClick = OnCellItemDidClick;

                // RectTransform rectTransform = item.GetComponent<RectTransform>();
                // Vector3 pos = new Vector3(rcItem.width * i, 0, 0);

                // // rectTransform.position = pos;
                // rectTransform.anchoredPosition = pos;
                cell.AddItem(item);

            }

        }
        cell.totalItem = totalItem;
        cell.oneCellNum = oneCellNum;
        cell.rowIndex = row;
        cell.UpdateItem(listItem);
        return cell;
    }

    #endregion

    #region Table View event handlers

    //Will be called by the TableView when a cell's visibility changed
    public void TableViewCellVisibilityChanged(int row, bool isVisible)
    {
        //Debug.Log(string.Format("Row {0} visibility changed to {1}", row, isVisible));
        if (isVisible)
        {
        }
    }

    #endregion
}

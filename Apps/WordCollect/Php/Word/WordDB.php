<?php

include_once('../Common/Sql/SqlDBUtil.php');
include_once('WordItemInfo.php');

class WordDB  extends SqlDBUtil
{
	const TABLE_NAME = 'TableWord';

	const KEY_text = 'text';

	const KEY_id = 'id';
	const KEY_title = 'title';
	const KEY_change = 'change';
	const KEY_translation = 'translation';
	const KEY_example = 'example';


	public $arrayCol = array(
		self::KEY_id, self::KEY_title, self::KEY_translation, self::KEY_change,
	);
	public $arrayColType;


	function __construct()
	{ }


	public function CreateDb()
	{
		$this->OpenFile("Word.db");
		$count = count($this->arrayCol);
		$this->arrayColType = array($count);
		for ($i = 0; $i < $count; $i++) {
			$this->arrayColType[$i] = self::KEY_text;
		}
		$this->CreateTableByName(self::TABLE_NAME, $this->arrayCol, $this->arrayColType);
	}
	public function ClearDB()
	{
		$this->DeleteTable(self::TABLE_NAME);
	}
	public function IsItemExist($info)
	{
		$ret = false;
		$sql = "SELECT * FROM "  . self::TABLE_NAME . " WHERE id = '" . $info->id . "'";
		//$sql = "select * from  where type='table' and name = '" . $this->table_name . "'";
		$result = $this->query($sql);
		$count = 0;
		// foreach ($result as $row) {
		// 	$count++;
		// }
		while ($res = $result->fetchArray(SQLITE3_ASSOC)) {
			$count++;
		}
		if ($count > 0) {
			$ret = true;
		}
		return $ret;
	}

	public function AddItem($info)
	{
		$count = count($this->arrayCol);
		$values = array($count);
		//id,filesave,date,addtime  
		$values[0] = $info->id;
		$values[1] = $info->title;
		$values[2] = $info->translation;
		$values[3] = $info->change;
		$this->Insert2Table(self::TABLE_NAME, $values);
	}

	public function DeleteItem($info)
	{
		// string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'" + " and addtime = '" + info.addtime + "'";
		$sql = "DELETE FROM " . self::TABLE_NAME . " WHERE id = '" . $info . id . "'";
		$result = $this->query($sql);
	}

	public function GetAllItem()
	{
		// Distinct 去掉重复
		//desc 降序 asc 升序 
		//string strsql = "select DISTINCT id from " + TABLE_NAME + " order by addtime desc";
		$sql = "select  *  from  " . self::TABLE_NAME;
		//$sql = "select  DISTINCT id  from  " . self::TABLE_NAME;
		$result = $this->query($sql);
		$listRet = array();
		$i = 0;
		while ($item = $result->fetchArray(SQLITE3_ASSOC)) {
			echo "read  result i=" . $i . "\n";
			// $row[$i]['user_id'] = $res['NAME']; 
			$info = $this->ReadInfo($item);
			array_push($listRet, $info);
			$i++;
		}
		return $listRet;
	}



	public function ReadInfo($item)
	{
		$info = new IdiomItemInfo();

		$info->id =   $item[self::KEY_id];
		$info->title =   $item[self::KEY_title];
		$info->translation =   $item[self::KEY_translation];
		$info->change =   $item[self::KEY_change];

		echo "id=" . $info->id . "\n";
		echo "title=" . $info->title . "\n";
		return $info;
	}
}

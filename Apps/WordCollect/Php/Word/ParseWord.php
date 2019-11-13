<?php
header("Content-type: text/html; charset=utf-8");
include_once('WordDB.php');
include_once('WordItemInfo.php');
include_once('../Common/Download.php');
include_once('../Common/Common.php');
include_once('../Common/Html/simple_html_dom.php');

//疯狂猜图答案_疯狂猜图所有答案_疯狂猜图品牌__72G疯狂猜图专区
//http://fkct.72g.com/

function get_html($url)
{
    $html = new simple_html_dom();

    // // 从url中加载  
    // $html->load_file('http://www.jb51.net');  

    // // 从字符串中加载  
    // $html->load('<html><body>从字符串中加载html文档演示</body></html>');  

    //从文件中加载  
    $html->load_file($url);

    return $html;
}

// 疯狂猜图  http://www.caichengyu.com/fkct/
class ParseWord //extends Thread
{

    public $url;
    public $id;
    public $channel;
    public $listItem;
    public $listSort = array();
    public  $page_total = 10; //10 
    public $ROOT_SAVE_DIR = "Data";
    public $WEB_HOME = "http://www.iciba.com";
    public $IMAGE_DIR = "Word";
    public $PIC_DIR = "Pic";
    public $dbWord;
    public function DoPaser()
    {
        $this->InitDB();
        $save_dir = $this->ROOT_SAVE_DIR;
        if (!is_dir($save_dir)) {
            mkdir($save_dir);
        }

        $save_dir = $this->ROOT_SAVE_DIR . "/" . $this->IMAGE_DIR;
        if (!is_dir($save_dir)) {
            mkdir($save_dir);
        }


        //$info = $this->dbWord->GetItem('LO');
        $this->ParseWordList("wordanswer.json", $save_dir);
    }

    public function InitDB()
    {
        $this->dbWord = new WordDB();
        $this->dbWord->CreateDb();
    }

    public function ParseWordList($filepath, $save_dir)
    {
        $fiel_exist = file_exists($filepath);
        if ($fiel_exist) {
            $json_string = file_get_contents($filepath);
            $data = json_decode($json_string, true);
            $data_list =  $data['items'];
            foreach ($data_list as $item) {
                $word = $item['id'];
                $infoid = new WordItemInfo();
                $infoid->id = $word;
                if (!$this->dbWord->IsItemExist($infoid)) {
                    $info =  $this->PaserWordInfo($this->WEB_HOME . "/" . $word, $word);
                    $this->dbWord->AddItem($info);
                    // $this->SaveWordJson($save_dir, $info);
                }
            }
        }
    }

    public function SaveWordJson($save_dir, $info)
    {
        //save sort
        $savefilepath = $save_dir . "/" . $info->title . ".json";
        $ret = file_exists($savefilepath);
        if ($ret) {
            // return;
        } {

            $element = array(
                'title' => $info->title,
                'translation' => $info->translation,
                'change' => $info->change,

            );
            //JSON_UNESCAPED_SLASHES json去除反斜杠 JSON_UNESCAPED_UNICODE中文不用\u格式
            $jsn = urldecode(json_encode($element, JSON_UNESCAPED_SLASHES | JSON_UNESCAPED_UNICODE));

            // "[  ]"
            //$jsn = str_replace("\"[", "[", $jsn);
            //$jsn = str_replace("]\"", "]", $jsn);

            $fp = fopen($savefilepath, "w");
            if (!$fp) {
                echo "打开文件失败<br>";
                return;
            }
            $flag = fwrite($fp, $jsn);
            if (!$flag) {
                echo "写入文件失败<br>";
            }
            fclose($fp);
        }
    }

    function FormatString($str)
    {
        //网页空格
        $strtmp = str_replace("	", "", $str);
        //普通空格
        $strtmp = str_replace(" ", "", $strtmp);

        //'为DB 关键字  
        //SQL 的单引号转义字符 https://www.cnblogs.com/qiuting/p/8038316.html
        $strtmp = str_replace("'", "''", $strtmp);
        return $strtmp;
    }


    //http://www.iciba.com/word
    function PaserWordInfo($url, $word)
    {
        $info = new WordItemInfo();
        // $info->translation = array();
        $info->id = $word;
        $info->title = $word;

        $html = get_html($url);
        if (!$html) {
            echo "PaserSortList html fail\n";
            return $info;
        }
        $ul_main = $html->find('ul[class=base-list switch_part]', 0);
        if (!$ul_main) {
            echo "PaserWordInfo find ul_main fail\n";
            return $info;
        }
        $arry_li = $ul_main->find('li');
        foreach ($arry_li as $li) {
            $arry_span = $li->find('span');
            $str = "";
            foreach ($arry_span as $span) {
                if ($span->class == "prop") { }
                $str = $str . " " . $span->plaintext;
            }
            $str =   $this->FormatString($str);
            $info->translation .= "\n" . $str;
            // array_push($info->translation, $str);
        } {
            //变形 change clearfix
            $change_li = $html->find('li[class=change clearfix]', 0);
            if ($change_li == null) {
                $info->change = "";
            } else {
                $arry_span = $change_li->find('span');
                $str = "";
                foreach ($arry_span as $span) {
                    $strtmp =  $span->plaintext;
                    $strtmp =   $this->FormatString($strtmp);
                    $str = $str . " " . $strtmp;
                }
                $info->change = $str;
            }
        }

        return $info;
    }
}


$parser = new ParseWord();
$parser->DoPaser();

echo 'done<br>';

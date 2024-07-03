/*******************************************
* 本簡繁切換前導程式 JS 檔存放位置由 WFU BLOG 提供
* 欲編輯、修改本程式，記得儲存的格式要選 unicode。
*
* WFU Blog : http://wayne-fu.blogspot.com/
***********************************/

var _setTS=1 //預設 1 為繁體；0 為簡體
var BodyIsFt=1 //預設 1 為繁體；0 為簡體

function TS_Switch(){
  if(navigator.userAgent.indexOf("MSIE")<0){
    if(_setTS==1){
      tongwen_TtoS();
      _setTS=0;
	  
    }
    else{
      if(_setTS==0){
	    tongwen_StoT();
	    _setTS=1;
      }
    }
  }	
  else{
    ie_switchTS()
  }
}

function tongwen_TtoS(){
  var s=document.getElementById("tongwenlet_cn");
  if(s!=null){document.body.removeChild(s);}
  var s=document.createElement("script");
  s.language="javascript";
  s.type="text/javascript";
  s.src = "https://googledrive.com/host/0B6juSkpGol5zSlpQQ2VYWXM1QnM/tongwen-ts.js";
  s.id="tongwenlet_cn";
  document.body.appendChild(s); 
}

function tongwen_StoT(){
  var s=document.getElementById("tongwenlet_tw");
  if(s!=null){document.body.removeChild(s);}
  var s=document.createElement("script");
  s.language="javascript";
  s.type="text/javascript";
  s.src = "https://googledrive.com/host/0B6juSkpGol5zSlpQQ2VYWXM1QnM/tongwen-st.js";
  s.id="tongwenlet_tw";
  document.body.appendChild(s); 
}

function ie_switchTS(){
  var s=document.getElementById("temp_tsSwitch");
  if(s!=null){document.body.removeChild(s);}
  var s=document.createElement("script");
  s.language="javascript";
  s.type="text/javascript";
  s.src="https://googledrive.com/host/0BykclfTTti-0SlU3SDg5RUVtNlk/StranJF_WFU.js";
  s.id="temp_tsSwitch";
  document.body.appendChild(s); 
}
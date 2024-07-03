//功能：檢驗數字輸入
//參數：source通常傳入this, event照抄就好, isNegative是否允許負數, isInteger是否為整數
//範例：正整數===>NumericText(this, event, false, true);
function NumericText(source, event, isNegative, isInteger)
{
    //8 : BackSpace
    //9 : Tab
    if (((event.keyCode < "0".charCodeAt()) || (event.keyCode > "9".charCodeAt())) && ((event.keyCode < 96) || (event.keyCode > 105)) && (event.keyCode != 8) && (event.keyCode != 9))
	{
		//alert(event.keyCode);
		//alert("-".charCodeAt());
		
		if (((event.keyCode == ".".charCodeAt()) || (event.keyCode == 110) || (event.keyCode == 190)) && (!isInteger))
		{
			if (source.value.indexOf(".") != -1)
				event.returnValue = false;
				
			if (source.value == "-")
				event.returnValue = false;			            	
		}
		else if ((event.keyCode == "-".charCodeAt()) || (event.keyCode == 109) || (event.keyCode == 189))
		{
		    if (!isNegative)
		        event.returnValue = false;
		        		
			if (source.value.indexOf("-") != -1)
				event.returnValue = false;
				
			if (source.value == ".")
				event.returnValue = false;						
				
			if (source.value != "")
				event.returnValue = false;
		}
		else if ((event.keyCode >= 37) && (event.keyCode <= 40))
		{}
		else
			event.returnValue = false;
	}
}

function regInput(obj, reg, inputStr) {
    var docSel = document.selection.createRange();
    if (docSel.parentElement().tagName != "INPUT") return false;

    oSel = docSel.duplicate();
    oSel.text = "";
    var srcRange = obj.createTextRange();
    oSel.setEndPoint("StartToStart", srcRange);
    var str = oSel.text + inputStr + srcRange.text.substr(oSel.text.length);

    return reg.test(str);
}
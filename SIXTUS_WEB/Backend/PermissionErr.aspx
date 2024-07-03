<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PermissionErr.aspx.cs" Inherits="Backend_PermissionErr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            height: 79px;
        }
    </style>
</head>
<body style="background-position: top; background-image: url(Images/login_bg.jpg); background-repeat: repeat-x;">
    <form id="form1" runat="server">
    <div id="login">
    
        <table style="width:100%;">
            <tr>
                <td class="style1">
                    </td>
            </tr>
        </table>

    <table align="center" border="0" cellpadding="0" cellspacing="0" height="400" 
        width="480">
        <tr>
            <td colspan="2" height="41">
                <img alt="" height="41" 
                    src="Images/logout_05.gif" width="480" /></td>
        </tr>
        <tr>
            <td rowspan="3" width="188">
                <img alt="" height="279" src="Images/no_right_02.gif" width="188" /></td>
        </tr>
        <tr>
            <td height="38">
                <img alt="" height="46" src="Images/no_right_04.gif" width="292" /></td>
        </tr>
        <tr>
            <td bgcolor="#ffffff" height="231" align=center valign="middle">
            <a href='javascript:history.back(-1)' title="點選回上頁">回上頁</a>
              </td>
        </tr>
        <tr>
            <td colspan="2" height="80">
                <img alt="" height="80" 
                    src="Images/logout_05.gif" width="480" /></td>
        </tr>
    </table>
    
  </div>
 </form>
</body>
</html>

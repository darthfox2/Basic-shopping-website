<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginErr.aspx.cs" Inherits="Backend_LoginErr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            height: 80px;
        }
    </style>
</head>
<body style="background-position: top; background-image: url(Images/login_bg.jpg); background-repeat: repeat-x;">
    <form id="form1" runat="server">
    <div id="login">
    
    <font face="新細明體">
    <table align="center" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="2" class="style1">
                </td>
        </tr>
        <tr>
            <td colspan="2">
                <img alt="" height="41" src="Images/logout_05.gif" width="480" /></td>
        </tr>
        <tr>
            <td rowspan="2">
                <img alt="" height="276" src="Images/logout_02.gif" width="192" /></td>
            <td bgcolor="White">
                <img alt="" 
                    src="Images/no_right_04.gif" width="288" style="height: 51px" /></td>
        </tr>
        <tr>
            <td align="right" bgcolor="#ffffff" height="196" valign="top">
                <p align="left">
                    <font size="2">您沒有回應的時間已經超過系統安全設定</font></p>
                <p align="left">
                    <font size="2">為保障您的資料安全</font></p>
                <p align="left">
                    <font size="2">系統已經將您自動登出。</font></p>
                <p align="left">
                    <font id="FONT1" runat="server" size="2">
                    <asp:LinkButton ID="LinkButtonRetLogin" runat="server" Font-Size="X-Small" 
                        onclick="LinkButtonRetLogin_Click" style="font-size: small" Width="136px">請按一下這裡重新登入</asp:LinkButton>
                    <a href="#"></a>
                </p>
                <p>
                    &nbsp;</p>
                <p>
                    &nbsp;</p>
                </font>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <img alt="" height="83" src="Images/logout_05.gif" width="480" /></td>
        </tr>
    </table>
    </font>
    
    </div>
    
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Backend_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <script type="text/javascript" language="javascript">
          function setCookie() {
              var e = document.getElementById("ddlLanguage");
              var strLan = e.options[e.selectedIndex].value;
              document.cookie = "Lang=" + strLan;
          }
    </script>
    <style type="text/css">
        .style1
        {
            height: 31px;
        }
        .style2
        {
            height: 100px;
        }
    </style>
</head>
<body style="background-position: top; background-image: url(Images/login_bg.jpg); background-repeat: repeat-x;" 
    bgcolor="#ffffff">
    <form id="form1" runat="server">
    <div id="login">
    

    
        <table style="width:100%;">
            <tr>
                <td class="style2">
                    </td>
                <td class="style2">
                    </td>
                <td class="style2">
                    </td>
            </tr>
        </table>
    

    
        <table class="style1" align="center" bgcolor="White">
            <tr>
                <td colspan="2">
                    <img alt="" height="41" 
                        src="Images/LoginUser_01.gif" width="480" /></td>
            </tr>
               <td colspan="2">
            <img alt="" src="Images/LoginTitle.jpg" />
            </td>
            <tr>
                <td rowspan="3" bgcolor="White">
                    <img alt="" height="211" 
                        src="Images/LoginUser_02.gif" width="137" /></td>
                <td bgcolor="White" style="background-color: #FFFFFF">
                    &nbsp;</td>
            </tr>
            <tr>
                <td bgcolor="White">
                    <font size="2">使用者名稱<br />
                    </font>
                    <asp:TextBox ID="UserID" runat="server" Width="136px" Height="19px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUserID" runat="server" 
                    ControlToValidate="UserID" ErrorMessage="RequiredFieldValidator" 
                    SkinID="MainAlert" ValidationGroup="OK" ForeColor="Red">*</asp:RequiredFieldValidator>

                    <br />
                    <br />
                    <font size="2">密碼<br />
                    </font>
                    <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="136px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                    ControlToValidate="Password" ErrorMessage="RequiredFieldValidator" 
                    SkinID="MainAlert" ValidationGroup="OK" ForeColor="Red">*</asp:RequiredFieldValidator>
                    <br />
                    <br />
               <%--     <font size="2">語系<br />
                    </font>
                    <asp:DropDownList ID="ddlLanguage" runat="server" Width="136px">
                        <asp:ListItem Value="TC">繁體中文</asp:ListItem>
                        <asp:ListItem Value="SC">簡體中文</asp:ListItem>
                    </asp:DropDownList>--%>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Send" runat="server" onclick="Send_Click" Text="送出" 
                        Width="48px" ValidationGroup="OK" OnClientClick='setCookie();' />
                    <asp:Button ID="Cancel" runat="server" Text="清除" Width="40px" 
                        onclick="Cancel_Click" />
                    <br />
                    <asp:Label ID="lblLoginMsg" runat="server" Font-Size="Smaller" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td bgcolor="White">
                    &nbsp;</td>
            </tr>
             <tr>
                <td bgcolor="White" colspan="2">
                    <img alt="" height="41" 
                        src="Images/LoginUser_01.gif" width="480" /></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>

<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PageControl.ascx.cs" Inherits="WebControls_PageControl"  %>

<%@ Register Src="MessageBar.ascx" TagName="MessageBar" TagPrefix="uc1" %>

<table ID="ControlContainer" cellpadding="0" cellspacing="0" width="100%" class="Page_control" runat="server">
    <tr>
        <td align="left" colspan="6" class="Page_control">
            <uc1:MessageBar id="MessageBar1" runat="server">
            </uc1:MessageBar></td>
    </tr>
    <tr>
        <td align="right" class="Page_control" colspan="5" style="text-align: right; height: 26px;" valign="middle">
            &nbsp;<asp:Label ID="lblRowCount" runat="server" Text="每頁{0}筆（共{1}筆）" 
                SkinID="Page"></asp:Label> &nbsp;&nbsp; &nbsp;
            <asp:Label ID="lblPageCount" runat="server" Text="第{0}頁（共{1}頁）" 
                SkinID="Page"></asp:Label>
            &nbsp;&nbsp; &nbsp;<asp:Button 
                ID="btnFirst" runat="server" Text="首頁" CausesValidation="False" 
                OnClick="btnFirst_Click" Width="60px" SkinID="PageButton" />
            <asp:Button ID="btnPrior" runat="server" Text="上一頁" CausesValidation="False" 
                OnClick="btnPrior_Click" Width="65px" SkinID="PageButton" />
            <asp:Button ID="btnNext" runat="server" Text="下一頁" CausesValidation="False" 
                OnClick="btnNext_Click" Width="65px" SkinID="PageButton" />
            <asp:Button ID="btnLast" runat="server" Text="末頁" CausesValidation="False" 
                OnClick="btnLast_Click" Width="65px" SkinID="PageButton" />
            &nbsp;
            
            <asp:Label ID="lblPage1" runat="server" Text="快速到第" 
                SkinID="Page"></asp:Label>
            <asp:TextBox ID="txtPageJumper" runat="server" Width="20px"></asp:TextBox>
            <asp:Label ID="lblPage2" runat="server" Text="頁" 
                SkinID="Page"></asp:Label>
            <asp:Button ID="btnGoto" runat="server" Text="GO" CausesValidation="False" 
                OnClick="btnPageJumper_Click" SkinID="PageButton" />
                
        </td>
    </tr>
</table>
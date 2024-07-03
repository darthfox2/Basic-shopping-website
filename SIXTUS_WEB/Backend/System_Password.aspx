<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/MasterPage.master" AutoEventWireup="true" CodeFile="System_Password.aspx.cs" Inherits="Backend_System_Password" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="title" class="ContentTitle">
個人密碼修改</div>

<div class="ContentBody">

 <table style="width:70%;" align="center" >
        <tr >
            <td align="center" colspan="2">
                <asp:Label ID="lblTitle" runat="server" SkinID="MainSubject" Text="修改帳號密碼"></asp:Label>
            </td>
        </tr>
        <tr >
            <td align="right" class="style3">
                &nbsp;</td>
            <td align="left" style="width: 60%">
                &nbsp;</td>
        </tr>
        <tr >
            <td align="right" class="style3">
                <asp:Label ID="lblUseID" runat="server" SkinID="MainText" Text="帳號："></asp:Label>
            </td>
            <td align="left" style="width: 60%">
                <asp:Label ID="lblUseID1" runat="server" SkinID="MainText"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" valign="top" class="style3">
                <asp:Label ID="lblOrgPW" runat="server" SkinID="MainText" Text="舊密碼："></asp:Label>
            </td>
            <td align="left" style="width: 60%" >
                <asp:TextBox ID="txbOrgPW" runat="server" 
                            Width="40%" MaxLength="20" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvOrgPW" runat="server" 
                            ControlToValidate="txbOrgPW" ErrorMessage="RequiredFieldValidator" 
                            SkinID="MainWarn" ValidationGroup="OK">舊密碼不可為空</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right" valign="top" class="style3" >
                <asp:Label ID="lblPW" runat="server" SkinID="MainText" Text="新密碼："></asp:Label>
            </td>
            <td align="left" style="width: 60%">
                <asp:TextBox ID="txbPW" runat="server" MaxLength="20" Width="40%" 
                                TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPW" runat="server" ControlToValidate="txbPW" 
                            ErrorMessage="RequiredFieldValidator" SkinID="MainWarn" 
                    ValidationGroup="OK">新密碼不可為空</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr >
            <td align="right" valign="top" class="style3" >
                <asp:Label ID="lblPW2" runat="server" SkinID="MainText" Text="再次輸入新密碼："></asp:Label>
            </td>
            <td align="left" style="width: 60%" >
                <asp:TextBox ID="txbPW2" runat="server" MaxLength="20" Width="40%" 
                                TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPW2" runat="server" ControlToValidate="txbPW2" 
                            ErrorMessage="RequiredFieldValidator" SkinID="MainWarn" 
                    ValidationGroup="OK">新密碼不可為空</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr >
            <td align="right" valign="top" class="style3" >
                            &nbsp;</td>
            <td align="left" style="width: 60%">
                            &nbsp;</td>
        </tr>
        <tr >
            <td align="center" colspan="2" >
                <asp:Button ID="btnEdit" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="BigButton" onclick="btnEdit_Click" Text="確定修改" 
                            ValidationGroup="OK" />
            </td>
        </tr>
    </table>

</div>

</asp:Content>


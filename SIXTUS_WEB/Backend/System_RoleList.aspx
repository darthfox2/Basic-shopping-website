<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/MasterPage.master" AutoEventWireup="true" CodeFile="System_RoleList.aspx.cs" Inherits="Backend_System_RoleList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="title" class="ContentTitle">
    群組權限管理</div>

<div class="ContentBody">

 <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <table style="width: 60%;" align="center">
                <tr>
                    <td align="left" style="width: 80%;">
                        &nbsp;</td>
                    <td align="right" style="width: 20%;">
                        <asp:Button ID="btnAdd" runat="server" BorderColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" onclick="btnAdd_Click" Text="新增" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grid" runat="server" 
                            AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" 
                            onrowcommand="grid_RowCommand" Width="100%" DataKeyNames="RolID" 
                            onrowdatabound="grid_RowDataBound">
                            <PagerSettings Visible="False" />
                            <Columns>
                                <asp:BoundField DataField="RolID" HeaderText="群組編號" ReadOnly="True"><HeaderStyle HorizontalAlign="Center" /><ItemStyle HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="RolName" HeaderText="名稱" ReadOnly="True"><HeaderStyle HorizontalAlign="Center" /><ItemStyle 
                                    HorizontalAlign="Center" /></asp:BoundField>
                                <asp:TemplateField HeaderText="是否啟用"><ItemTemplate><asp:Label ID="lblIsEnable" runat="server" Text='<%# Eval("RolEnable") %>'></asp:Label></ItemTemplate><HeaderStyle HorizontalAlign="Center" /><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>
                                <asp:TemplateField HeaderText="修改"><ItemTemplate><asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("RolID") %>' CommandName="Modify" 
                                            ImageUrl="Images/Edit.gif" ToolTip="修改" /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>
                                <asp:TemplateField HeaderText="刪除"><ItemTemplate><asp:ImageButton ID="imgbtnDel" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("RolID") %>' CommandName="Cancel" 
                                            ImageUrl="Images/Delete.gif" 
                                            onclientclick="return window.confirm('確定要刪除嗎？');" ToolTip="刪除" /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetRoles" 
                            TypeName="DAL.DALClass"></asp:ObjectDataSource>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <table style="width:100%;">
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="lblTitle" runat="server" SkinID="MainSubject" Text="新增/修改群組"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="lblRolID" runat="server" SkinID="MainText" Text="群組編號："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:TextBox ID="txbRolID" runat="server" MaxLength="15" Width="30%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvRolID" runat="server" 
                            ControlToValidate="txbRolID" ErrorMessage="RequiredFieldValidator" 
                            SkinID="MainWarn" ValidationGroup="OK">群組編號不可為空</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%" valign="top">
                        <asp:Label ID="lblRolName" runat="server" SkinID="MainText" Text="名稱："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:TextBox ID="txbRolName" runat="server" 
                            Width="40%" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvRolName" runat="server" 
                            ControlToValidate="txbRolName" ErrorMessage="RequiredFieldValidator" 
                            SkinID="MainWarn" ValidationGroup="OK">名稱不可為空</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%" valign="top">
                        <asp:Label ID="lblIsEnable" runat="server" SkinID="MainText" Text="是否啟用："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:CheckBox ID="ckbIsEnable" runat="server" Text="啟用請打勾" Checked="True" />
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%" valign="top">
                        <asp:Label ID="lblPermList" runat="server" SkinID="MainText" Text="權限設定："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">


                        <asp:CheckBoxList ID="ckblPermission" runat="server">
                        </asp:CheckBoxList>


                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%" valign="top">
                            &nbsp;</td>
                    <td align="left" style="width: 75%">
                            &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnEdit" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="BigButton" onclick="btnEdit_Click" Text="確定修改" 
                            ValidationGroup="OK" />
                        <asp:Button ID="btnOK" runat="server" BackColor="Transparent" BorderWidth="0px" 
                            CssClass="SmallButton" onclick="btnOK_Click" Text="確定" ValidationGroup="OK" />
                        <asp:Button ID="btnBack" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" onclick="btnBack_Click" Text="返回" />
                        <asp:HiddenField ID="hideRolID" runat="server" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>

</div>

</asp:Content>


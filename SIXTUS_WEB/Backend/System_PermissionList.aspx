<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/MasterPage.master" AutoEventWireup="true" CodeFile="System_PermissionList.aspx.cs" Inherits="Backend_System_PermissionList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="title" class="ContentTitle">
權限管理</div>

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
                            onrowcommand="grid_RowCommand" Width="100%" DataKeyNames="PerID">
                                <PagerSettings Visible="False" />
                                <Columns>
                                    <asp:BoundField DataField="PerID" HeaderText="權限編號" ReadOnly="True">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PerName" HeaderText="名稱" ReadOnly="True">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PerPage" HeaderText="權限頁面" ReadOnly="True">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="修改">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("PerId") %>' CommandName="Modify" 
                                            ImageUrl="Images/Edit.gif" ToolTip="修改" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="刪除">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDel" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("PerId") %>' CommandName="Cancel" 
                                            ImageUrl="Images/Delete.gif" 
                                            onclientclick="return window.confirm('確定要刪除嗎？');" ToolTip="刪除" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetPermission" 
                            TypeName="DAL.DALClass"></asp:ObjectDataSource>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="View2" runat="server">
                <table style="width:100%;">
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Label ID="lblTitle" runat="server" SkinID="MainSubject" Text="新增/修改權限"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 25%">
                            <asp:Label ID="lblPerID" runat="server" SkinID="MainText" Text="權限編號："></asp:Label>
                        </td>
                        <td align="left" style="width: 75%">
                            <asp:TextBox ID="txbPerID" runat="server" MaxLength="15" Width="30%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPerID" runat="server" 
                                ControlToValidate="txbPerID" ErrorMessage="RequiredFieldValidator" 
                                SkinID="MainAlert" ValidationGroup="OK">權限編號不可為空</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 25%" valign="top">
                            <asp:Label ID="lblPerName" runat="server" SkinID="MainText" Text="名稱："></asp:Label>
                        </td>
                        <td align="left" style="width: 75%">
                            <asp:TextBox ID="txbPerName" runat="server" 
                            Width="50%" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPerName" runat="server" 
                            ControlToValidate="txbPerName" ErrorMessage="RequiredFieldValidator" 
                            SkinID="MainAlert" ValidationGroup="OK">名稱不可為空</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 25%" valign="top">
                            <asp:Label ID="lblPerAddr" runat="server" SkinID="MainText" Text="權限頁面："></asp:Label>
                        </td>
                        <td align="left" style="width: 75%">
                            <asp:TextBox ID="txbPerAddr" runat="server" MaxLength="100" Width="50%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPerAddr" runat="server" ControlToValidate="txbPerAddr" 
                            ErrorMessage="RequiredFieldValidator" SkinID="MainAlert" ValidationGroup="OK">權限頁面不可為空</asp:RequiredFieldValidator>
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
                            <asp:HiddenField ID="hideID" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>

</div>

</asp:Content>


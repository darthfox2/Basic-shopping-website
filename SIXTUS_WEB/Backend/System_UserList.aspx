<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/MasterPage.master" AutoEventWireup="true" CodeFile="System_UserList.aspx.cs" Inherits="Backend_System_UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="title" class="ContentTitle">
帳號管理</div>

<div class="ContentBody">

  <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="View1" runat="server">
                <table style="width: 60%;" align="center">
                    <tr>
                        <td align="left" style="width: 80%;">
                            <asp:Label ID="Label34" runat="server" SkinID="MainText" Text="群組權限："></asp:Label>
                            <asp:DropDownList ID="ddlSelRole" runat="server" AutoPostBack="True" 
                                Width="25%">
                            </asp:DropDownList>
                        </td>
                        <td align="right" style="width: 20%;">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 80%;">
                            <asp:Label ID="Label35" runat="server" SkinID="MainText" Text="帳號名稱："></asp:Label>
                            <asp:TextBox ID="txbSelUseID" runat="server" MaxLength="15" Width="30%"></asp:TextBox>
                            &nbsp;<asp:Button ID="btnSearch" runat="server" BorderColor="Transparent" 
                                BorderWidth="0px" CssClass="SmallButton" onclick="btnSearch_Click" Text="查詢" />
                        </td>
                        <td align="right" style="width: 20%;">
                            <asp:Button ID="btnAdd" runat="server" BorderColor="Transparent" 
                                BorderWidth="0px" CssClass="SmallButton" onclick="btnAdd_Click" Text="新增" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="grid" runat="server" 
                            AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" 
                            onrowcommand="grid_RowCommand" Width="100%" DataKeyNames="UseID" 
                            onrowdatabound="grid_RowDataBound">
                                <PagerSettings Visible="False" />
                                <Columns>
                                    <asp:BoundField DataField="UseID" HeaderText="帳號" ReadOnly="True"><HeaderStyle HorizontalAlign="Center" /><ItemStyle HorizontalAlign="Left" /></asp:BoundField>
                                    <asp:BoundField DataField="UseName" HeaderText="中文名稱" ReadOnly="True"><HeaderStyle HorizontalAlign="Center" /><ItemStyle HorizontalAlign="Left" /></asp:BoundField>
                                    <asp:BoundField DataField="RolName" HeaderText="群組權限" ReadOnly="True"><HeaderStyle HorizontalAlign="Center" /><ItemStyle HorizontalAlign="Center" /></asp:BoundField>
                                    <asp:TemplateField HeaderText="是否啟用"><ItemTemplate><asp:Label ID="lblUseEnable" runat="server" Text='<%# Eval("UseEnable") %>'></asp:Label></ItemTemplate><HeaderStyle HorizontalAlign="Center" /><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>
                                    <asp:TemplateField HeaderText="修改"><ItemTemplate><asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("UseID") %>' CommandName="Modify" 
                                            ImageUrl="Images/Edit.gif" ToolTip="修改" /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>
                                    <asp:TemplateField HeaderText="刪除"><ItemTemplate><asp:ImageButton ID="imgbtnDel" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("UseID") %>' CommandName="Cancel" 
                                            ImageUrl="Images/Delete.gif" 
                                            onclientclick="return window.confirm('確定要刪除嗎？');" ToolTip="刪除" /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetUserByUseIDAndRolID" 
                            TypeName="DAL.DALClass">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="txbSelUseID" DefaultValue="" Name="UseID" 
                                        PropertyName="Text" Type="String" />
                                    <asp:ControlParameter ControlID="ddlSelRole" Name="RolID" 
                                        PropertyName="SelectedValue" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="View2" runat="server">
                <table style="width:100%;">
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Label ID="lblTitle" runat="server" SkinID="MainSubject" Text="新增/修改帳號"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 25%">
                            <asp:Label ID="lblUseID" runat="server" SkinID="MainText" Text="帳號："></asp:Label>
                        </td>
                        <td align="left" style="width: 75%">
                            <span>
                            <asp:Label ID="lblUseID2" runat="server" SkinID="MainAlert">(請輸入英文或數字的組合)</asp:Label>
                            </span>
                            <br />
                            <asp:TextBox ID="txbUseID" runat="server" MaxLength="15" Width="30%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvUseID" runat="server" 
                            ControlToValidate="txbUseID" ErrorMessage="RequiredFieldValidator" 
                            SkinID="MainWarn" ValidationGroup="OK">帳號不可為空</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revUseID" runat="server" 
                                ControlToValidate="txbUseID" ErrorMessage="帳號輸入錯誤" ForeColor="Red" 
                                ValidationExpression="^[A-Za-z0-9]+$" ValidationGroup="OK"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 25%" valign="top">
                            <asp:Label ID="lblName" runat="server" SkinID="MainText" Text="中文名稱："></asp:Label>
                        </td>
                        <td align="left" style="width: 75%">
                            <asp:TextBox ID="txbName" runat="server" 
                            Width="50%" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" 
                            ControlToValidate="txbName" ErrorMessage="RequiredFieldValidator" 
                            SkinID="MainWarn" ValidationGroup="OK">名稱不可為空</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 25%" valign="top">
                            <asp:Label ID="lblPW" runat="server" SkinID="MainText" Text="密碼："></asp:Label>
                        </td>
                        <td align="left" style="width: 75%">
                            <asp:TextBox ID="txbPW" runat="server" MaxLength="20" Width="30%" 
                                TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPW" runat="server" ControlToValidate="txbPW" 
                            ErrorMessage="RequiredFieldValidator" SkinID="MainWarn" ValidationGroup="OK">密碼不可為空</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 25%" valign="top">
                            <asp:Label ID="lblName0" runat="server" SkinID="MainText" Text="國家/地區："></asp:Label>
                        </td>
                        <td align="left" style="width: 75%">
                            <asp:DropDownList ID="ddlCountry" runat="server" 
                                Width="25%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 25%" valign="top">
                            <asp:Label ID="lblName4" runat="server" SkinID="MainText" Text="郵遞區號："></asp:Label>
                        </td>
                        <td align="left" style="width: 75%">
                            <asp:TextBox ID="txbZipCode" runat="server" MaxLength="30" Width="20%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 25%" valign="top">
                            <asp:Label ID="lblName1" runat="server" SkinID="MainText" Text="地址："></asp:Label>
                        </td>
                        <td align="left" style="width: 75%">
                            <asp:TextBox ID="txbAddr" runat="server" MaxLength="500" Width="50%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 25%" valign="top">
                            <asp:Label ID="lblName2" runat="server" SkinID="MainText" Text="手機號碼："></asp:Label>
                        </td>
                        <td align="left" style="width: 75%">
                            <asp:TextBox ID="txbCell" runat="server" MaxLength="30" Width="20%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 25%" valign="top">
                            <asp:Label ID="lblName3" runat="server" SkinID="MainText" Text="電子信箱："></asp:Label>
                        </td>
                        <td align="left" style="width: 75%">
                            <asp:TextBox ID="txbEmail" runat="server" MaxLength="250" Width="50%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 25%" valign="top">
                            <asp:Label ID="lblRole" runat="server" SkinID="MainText" Text="群組權限："></asp:Label>
                        </td>
                        <td align="left" style="width: 75%">
                            <asp:DropDownList ID="ddlRole" runat="server" Width="30%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 25%" valign="top">
                            <asp:Label ID="lblIsEnable" runat="server" SkinID="MainText" Text="是否啟用："></asp:Label>
                        </td>
                        <td align="left" style="width: 75%">
                            <asp:CheckBox ID="ckbIsEnable" runat="server" Checked="True" Text="啟用請打勾" />
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
                            <asp:HiddenField ID="hideUseID" runat="server" />
                            <asp:HiddenField ID="hideLoginUseID" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>

</div>

</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/MasterPage.master" AutoEventWireup="true" CodeFile="MANAGE_ArticleTypelist.aspx.cs" Inherits="Backend_MANAGE_ArticleTypelist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            height: 20px;
        }
        .auto-style3 {}
        .auto-style4 {
            height: 20px;
            width: 274px;
        }
        .auto-style5 {}
        .auto-style6 {
            height: 18px;
        }
        .auto-style7 {
            height: 29px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="title" class="ContentTitle">
        分類管理</div>
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <table style="width: 75%;" align="center">
                <tr>
                    <td class="auto-style6" colspan="2"></td>
                </tr>
                <tr>
                    <td class="auto-style7"></td>
                    <td align="right" class="auto-style7">
                        <asp:Button ID="btnAdd" runat="server" BorderColor="Transparent" BorderWidth="0px" CssClass="SmallButton" Text="新增" OnClick="btnAdd_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" colspan="2">
                        <asp:GridView ID="grid" runat="server" AutoGenerateColumns="False"  Width="100%" DataSourceID="ObjectDataSource1" OnRowCommand="grid_RowCommand" PageSize="50" >
                            <Columns>
                                <asp:BoundField DataField="AttSort" HeaderText="排序" />
                                <asp:BoundField DataField="AttName" HeaderText="類型名稱" />
                                <asp:BoundField DataField="AttDesc" HeaderText="類型描述" />
                                <asp:TemplateField HeaderText="往上">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnUp" runat="server" CausesValidation="False" CommandArgument='<%# Eval("AttSort") %>' CommandName="up" ImageUrl="Images/SortUp.gif" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="往下">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnDown" runat="server" CausesValidation="False" CommandArgument='<%# Eval("AttSort") %>' CommandName="dowm" ImageUrl="Images/SortDown.gif" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="修改">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("AttID") %>' CommandName="modify" ImageUrl="Images/Edit.gif" ToolTip="修改" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="刪除">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument='<%# Eval("AttID") %>' CommandName="cancel" ImageUrl="Images/Delete.gif" OnClientClick="return window.confirm('確定要刪除嗎？');" ToolTip="刪除" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="auto-style3" colspan="2">
                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetArticleType" TypeName="DAL.DALClass"></asp:ObjectDataSource>
                        <asp:HiddenField ID="hideUserID" runat="server" />
                        <asp:HiddenField ID="hideID" runat="server" />
                        <asp:HiddenField ID="hideSort" runat="server" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="center" class="auto-style3" colspan="2">&nbsp;</td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <table style="width: 100%;">
                <tr>
                    <td colspan="2" class="auto-style1"></td>
                </tr>
                <tr>
                    <td class="auto-style1" colspan="2">
                        <asp:Label ID="Label4" runat="server" Text="新增/修改類型"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td align="right" class="auto-style4" style="width: 30%" valign="top">
                        <asp:Label ID="Label2" runat="server" Text="類型名稱 : " style="text-align: center"></asp:Label>
                    </td>
                    <td align="left" class="auto-style1">
                        <asp:TextBox ID="txbAttName" runat="server" MaxLength="50" Width="50%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvAttName" runat="server" ControlToValidate="txbAttName" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="OK">名稱不可為空</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="auto-style5" style="width: 30%" valign="top">
                        <asp:Label ID="Label3" runat="server" Text="類型描述 : "></asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txbAttDesc" runat="server" Height="150px" MaxLength="500" TextMode="MultiLine" Width="80%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="auto-style5">&nbsp;</td>
                    <td align="left">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center" class="auto-style5" colspan="2">
                        <asp:Button ID="btnEdit" runat="server" BorderColor="Transparent" BorderWidth="0px" CssClass="BigButton" Text="確定修改" OnClick="btnEdit_Click" ValidationGroup="OK" />
                        <asp:Button ID="btnOK" runat="server" BorderColor="Transparent" BorderWidth="0px" CssClass="SmallButton" Text="確定" OnClick="btnOK_Click" ValidationGroup="OK" />
                        <asp:Button ID="btnBack" runat="server" BorderColor="Transparent" BorderWidth="0px" CssClass="SmallButton" Text="返回" OnClick="btnBack_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="center" class="auto-style5" colspan="2">&nbsp;</td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>


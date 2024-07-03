<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/MasterPage.master" AutoEventWireup="true" CodeFile="MANAGE_RateList.aspx.cs" Inherits="Backend_MANAGE_RateList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="title" class="ContentTitle">
貨幣匯差設定</div>

<div class="ContentBody">

 <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <table style="width: 60%;" align="center">
                <tr>
                    <td align="left" style="width: 80%;">
                        &nbsp;</td>
                    <td align="right" style="width: 20%;">
                        <asp:Button ID="btnAdd" runat="server" BorderColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" onclick="btnAdd_Click" Text="新增" 
                            Visible="False" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grid" runat="server" 
                            AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" 
                            onrowcommand="grid_RowCommand" Width="100%" DataKeyNames="OerID" 
                            onrowdatabound="grid_RowDataBound">
                            <PagerSettings Visible="False" />
                            <Columns>
                                <asp:BoundField DataField="OerName" HeaderText="名稱" ReadOnly="True"><HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" /></asp:BoundField>
                                <asp:BoundField DataField="OerCurrency" HeaderText="貨幣代號" ReadOnly="True"><HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" /></asp:BoundField>
                                <asp:BoundField DataField="OerRate" HeaderText="匯率" ReadOnly="True"><HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle 
                                    HorizontalAlign="Right" /></asp:BoundField>
                                <asp:BoundField DataField="LastUpdateDate" HeaderText="最後更新日期" ReadOnly="True" 
                                    DataFormatString="{0:yyyy/MM/dd HH:mm:ss}">
                                <HeaderStyle HorizontalAlign="Center" Width="150px" />
                                <ItemStyle 
                                    HorizontalAlign="Center" /></asp:BoundField>
                                <asp:TemplateField HeaderText="是否啟用" Visible="False"><ItemTemplate><asp:Label ID="lblIsEnable" runat="server" Text='<%# Eval("OerID") %>'></asp:Label></ItemTemplate><HeaderStyle HorizontalAlign="Center" /><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>
                                <asp:TemplateField HeaderText="修改"><ItemTemplate><asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("OerID") %>' CommandName="Modify" 
                                            ImageUrl="Images/Edit.gif" ToolTip="修改" /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>
                                <asp:TemplateField HeaderText="刪除" Visible="False"><ItemTemplate><asp:ImageButton ID="imgbtnDel" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("OerID") %>' CommandName="Cancel" 
                                            ImageUrl="Images/Delete.gif" 
                                            onclientclick="return window.confirm('確定要刪除嗎？');" ToolTip="刪除" /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetExchangeRate" 
                            TypeName="DAL.DALClass">
                        </asp:ObjectDataSource>
                        <asp:HiddenField ID="hideUserID" runat="server" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <table style="width:100%;">
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="lblTitle" runat="server" SkinID="MainSubject" Text="新增/修改運費"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 50%">
                        <asp:Label ID="lbl01" runat="server" SkinID="MainText" Text="名稱："></asp:Label>
                    </td>
                    <td align="left" style="width: 50%">
                        <asp:TextBox ID="txbOerName" runat="server" MaxLength="30" Width="15%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvOerName" runat="server" 
                            ControlToValidate="txbOerName" ErrorMessage="RequiredFieldValidator" 
                            SkinID="MainWarn" ValidationGroup="OK">名稱不可為空</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 50%" valign="top">
                        <asp:Label ID="lbl02" runat="server" SkinID="MainText" Text="貨幣代號："></asp:Label>
                    </td>
                    <td align="left" style="width: 50%">
                        <asp:TextBox ID="txbOerCurrency" runat="server" 
                            Width="15%" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvOerCurrency" runat="server" 
                            ControlToValidate="txbOerCurrency" ErrorMessage="RequiredFieldValidator" 
                            SkinID="MainWarn" ValidationGroup="OK">貨幣代號不可為空</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 50%" valign="top">
                        <asp:Label ID="lbl3" runat="server" SkinID="MainText" Text="匯率："></asp:Label>
                    </td>
                    <td align="left" style="width: 50%">
                        <asp:TextBox ID="txbOerRate" runat="server" MaxLength="10" Width="15%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvOerRate" runat="server" 
                            ControlToValidate="txbOerRate" ErrorMessage="RequiredFieldValidator" 
                            SkinID="MainWarn" ValidationGroup="OK">匯率不可為空</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 50%" valign="top">
                        &nbsp;</td>
                    <td align="left" style="width: 50%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 50%" valign="top">
                        <asp:Label ID="lbl4" runat="server" SkinID="MainText" Text="最後更新日期："></asp:Label>
                    </td>
                    <td align="left" style="width: 50%">
                        <asp:Label ID="lblLastUpdateDate" runat="server" SkinID="MainWarn"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 50%" valign="top">
                        <asp:Label ID="lblIsEnable" runat="server" SkinID="MainText" Text="是否啟用：" 
                            Visible="False"></asp:Label>
                    </td>
                    <td align="left" style="width: 50%">
                        <asp:CheckBox ID="ckbIsEnable" runat="server" Text="啟用請打勾" Checked="True" 
                            Visible="False" />
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 50%" valign="top">
                            &nbsp;</td>
                    <td align="left" style="width: 50%">
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


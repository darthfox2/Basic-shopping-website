<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/MasterPage.master" AutoEventWireup="true" CodeFile="ORDER_FareList.aspx.cs" Inherits="Backend_ORDER_FareList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="title" class="ContentTitle">
運費設定</div>

<div class="ContentBody">

 <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <table style="width: 70%;" align="center">
                <tr>
                    <td align="left" style="width: 80%;">
                            &nbsp;</td>
                    <td align="right" style="width: 20%;">
                        &nbsp;</td>
                </tr>
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
                            onrowcommand="grid_RowCommand" Width="100%" DataKeyNames="OdfID" 
                            onrowdatabound="grid_RowDataBound">
                            <PagerSettings Visible="False" />
                            <Columns>
                                <asp:BoundField DataField="OdfName" HeaderText="名稱" ReadOnly="True"><HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" /></asp:BoundField>
                                <asp:BoundField DataField="OdfDesc" HeaderText="說明" ReadOnly="True"><HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="OdfPrice" HeaderText="價格(NTD)" ReadOnly="True"><HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle 
                                    HorizontalAlign="Right" /></asp:BoundField>
                                <asp:TemplateField HeaderText="是否啟用"><ItemTemplate><asp:Label ID="lblIsEnable" runat="server" Text='<%# Eval("OdfEnable") %>'></asp:Label></ItemTemplate><HeaderStyle HorizontalAlign="Center" /><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>
                                <asp:TemplateField HeaderText="修改"><ItemTemplate><asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("OdfID") %>' CommandName="Modify" 
                                            ImageUrl="Images/Edit.gif" ToolTip="修改" /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>
                                <asp:TemplateField HeaderText="刪除" Visible="False"><ItemTemplate><asp:ImageButton ID="imgbtnDel" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("OdfID") %>' CommandName="Cancel" 
                                            ImageUrl="Images/Delete.gif" 
                                            onclientclick="return window.confirm('確定要刪除嗎？');" ToolTip="刪除" /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetOrderFareByEnable" 
                            TypeName="DAL.DALClass">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="-1" Name="OdfEnable" Type="String" />
                            </SelectParameters>
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
                    <td align="right" style="width: 25%">
                        <asp:Label ID="lbl01" runat="server" SkinID="MainText" Text="名稱："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:TextBox ID="txbOdfName" runat="server" MaxLength="30" Width="30%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvOdfName" runat="server" 
                            ControlToValidate="txbOdfName" ErrorMessage="RequiredFieldValidator" 
                            SkinID="MainWarn" ValidationGroup="OK">名稱不可為空</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%" valign="top">
                        <asp:Label ID="lbl02" runat="server" SkinID="MainText" Text="說明："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:TextBox ID="txbOdfDesc" runat="server" 
                            Width="50%" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%" valign="top">
                        <asp:Label ID="lbl3" runat="server" SkinID="MainText" Text="價格(NTD)："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:TextBox ID="txbOdfPrice" runat="server" MaxLength="10" Width="15%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvOdfPrice" runat="server" 
                            ControlToValidate="txbOdfPrice" ErrorMessage="RequiredFieldValidator" 
                            SkinID="MainWarn" ValidationGroup="OK">價格不可為空</asp:RequiredFieldValidator>
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


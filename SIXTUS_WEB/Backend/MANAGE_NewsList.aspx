<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/MasterPage.master" AutoEventWireup="true"
    CodeFile="MANAGE_NewsList.aspx.cs" Inherits="Backend_MANAGE_NewsList" %>

<%@ Register Src="~/WebControls/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>

<%@ Register assembly="CKEditor.NET" namespace="CKEditor.NET" tagprefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 30%;
            height: 23px;
        }
        .auto-style2 {
            width: 70%;
            height: 23px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        //開始日期
        $(function () {
            $("#ContentPlaceHolder1_txbSDate").datepicker({ dateFormat: "yy/mm/dd" }); ;
        });

        //結束日期
        $(function () {
            $("#ContentPlaceHolder1_txbEDate").datepicker({ dateFormat: "yy/mm/dd" }); ;
        });

        //公告期間
        $(function () {
            $("#ContentPlaceHolder1_txbNewsStartDate").datepicker({ dateFormat: "yy/mm/dd" }); ;
        });

        $(function () {
            $("#ContentPlaceHolder1_txbNewsEndDate").datepicker({ dateFormat: "yy/mm/dd" }); ;
        });

        //建立日期
        $(function () {
            $("#ContentPlaceHolder1_txbNewsCreaDate").datepicker({ dateFormat: "yy/mm/dd" });;
        });

    </script>
    <div id="title" class="ContentTitle">
        最新消息上架</div>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>--%>
    <div class="ContentBody">
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="View1" runat="server">
                <table style="width: 90%;" align="center">
                    <tr>
                        <td align="left" style="width: 80%;">
                            &nbsp;</td>
                        <td align="right" style="width: 20%;">
                            <asp:Button ID="Button1" runat="server" BackColor="Transparent" BorderWidth="0px" CssClass="BigButton" OnClick="btnExport_Click" Text="匯出Excel" Visible="False" />
                            <%--<input class="BigButton" onclick="print();" type="button" value="列印本頁" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 80%;">
                            <asp:Label ID="Label34" runat="server" SkinID="MainText" Text="類別：" Visible="False"></asp:Label>
                            <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" Width="20%" Visible="False">
                            </asp:DropDownList>
                        </td>
                        <td align="right" style="width: 20%;">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 80%;">
                            <asp:Label ID="Label16" runat="server" SkinID="MainText" Text="發佈日期："></asp:Label>
                            <asp:TextBox ID="txbSDate" runat="server" MaxLength="10"></asp:TextBox>
                            &nbsp;<asp:Label ID="Label33" runat="server" SkinID="MainText" Text="~"></asp:Label>
                            &nbsp;<asp:TextBox ID="txbEDate" runat="server" MaxLength="10"></asp:TextBox>
                            &nbsp;<asp:Button ID="btnSearch" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                CssClass="SmallButton" OnClick="btnSearch_Click" Text="查詢" />
                            <asp:Label ID="lblMeg" runat="server" ForeColor="Red" SkinID="MainWarn"></asp:Label>
                        </td>
                        <td align="right" style="width: 20%;">
                            <asp:Button ID="btnAdd" runat="server" BorderColor="Transparent" BorderWidth="0px" CssClass="SmallButton" OnClick="btnAdd_Click" Text="新增" />
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <uc1:PageControl ID="PageControl1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="grid" runat="server" AutoGenerateColumns="False" Width="100%" OnRowCommand="grid_RowCommand"
                                OnRowDataBound="grid_RowDataBound" DataSourceID="ObjectDataSource1" PageSize="20"
                                AllowPaging="True">
                                <PagerSettings Visible="False" />
                                <Columns>
                                    <asp:BoundField DataField="NwsCreaDate" HeaderText="發佈日期" DataFormatString="{0:yyyy/MM/dd}" >
                                    <ItemStyle Width="120px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NwtName" HeaderText="類別" Visible="False">
                                        <HeaderStyle Width="70px" />
                                        <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NwsTitle" HeaderText="公告標題" ReadOnly="True">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="公告人">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastUpdateUser" runat="server" Text='<%# Eval("LastUpdateUser") %>'></asp:Label></ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="是否過期">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNwsStartDate" runat="server" Text='<%# Eval("NwsStartDate") %>'
                                                Visible="false"></asp:Label><asp:Label ID="lblNwsEndDate" runat="server" Text='<%# Eval("NwsEndDate") %>'
                                                    Visible="false"></asp:Label><asp:Label ID="lblIsOverdate" runat="server" Text='沒有資料'></asp:Label></ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="是否啟用">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsEnable" runat="server" Text='<%# Eval("NwsEnable") %>'></asp:Label></ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="瀏覽明細" Visible="false">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnView" runat="server" CausesValidation="True" CommandArgument='<%# Eval("NwsIndex") %>'
                                                CommandName="View" ImageUrl="Images/Check.gif" ToolTip="瀏覽明細" /></ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="修改">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("NwsIndex") %>'
                                                CommandName="Modify" ImageUrl="Images/Edit.gif" ToolTip="修改" /></ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="刪除">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDel" runat="server" CausesValidation="False" CommandArgument='<%# Eval("NwsIndex") %>'
                                                CommandName="Cancel" ImageUrl="Images/Delete.gif" OnClientClick="return window.confirm('確定要刪除嗎？');"
                                                ToolTip="刪除" /></ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetNewsListDateRang"
                                TypeName="DAL.DALClass">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hideSDate" Name="dtStartSet" PropertyName="Value"
                                        Type="DateTime" />
                                    <asp:ControlParameter ControlID="hideEDate" Name="dtEndSet" PropertyName="Value"
                                        Type="DateTime" />
                                    <asp:ControlParameter ControlID="ddlType" Name="NewsType" PropertyName="SelectedValue"
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:HiddenField ID="hideSDate" runat="server" />
                            <asp:HiddenField ID="hideEDate" runat="server" />
                            <asp:HiddenField ID="hideUserID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="View2" runat="server">
                <table style="width: 100%;" align="center">
                    <tr>
                        <td align="right" colspan="2" style="text-align: center;" valign="top">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2" style="text-align: center;" valign="top">
                            <asp:Label ID="lblView2Title" runat="server" SkinID="MainSubject" Text="新增/修改公告"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2" style="text-align: center;" valign="top">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="trNewsID" runat="server">
                        <td align="right" style="width: 30%">
                            <asp:Label ID="Label1" runat="server" SkinID="MainText" Text="公告編號："></asp:Label>
                        </td>
                        <td align="left" style="width: 70%">
                            <asp:TextBox ID="txbNewsID" runat="server" SkinID="MainText" Text="沒有資料"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 30%">
                            <asp:Label ID="Label2" runat="server" SkinID="MainText" Text="公告類別：" Visible="False"></asp:Label>
                        </td>
                        <td align="left" style="width: 70%">
                            <asp:DropDownList ID="ddlNewsType" runat="server" AutoPostBack="False" Width="25%" Visible="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 30%" valign="top">
                            <asp:Label ID="Label4" runat="server" SkinID="MainText" Text="公告期間："></asp:Label>
                        </td>
                        <td align="left" style="width: 70%">
                            <asp:TextBox ID="txbNewsStartDate" runat="server" SkinID="MainText" Text="沒有資料" Width="120px"></asp:TextBox>
                            &nbsp;～&nbsp;<asp:TextBox ID="txbNewsEndDate" runat="server" SkinID="MainText" Text="沒有資料"
                                Width="120px"></asp:TextBox>
                            <asp:Label ID="lblMeg2" runat="server" ForeColor="Red" SkinID="MainWarn"></asp:Label><br />
                            <asp:CheckBox ID="ckbIsPermanent" runat="server" Text="永久請打勾" Checked="False" OnCheckedChanged="ckbIsPermanent_CheckedChanged"
                                AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr id="trNewsCreaDate" runat="server">
                        <td align="right" style="width: 30%">
                            <asp:Label ID="Label5" runat="server" SkinID="MainText" Text="發佈日："></asp:Label>
                        </td>
                        <td align="left" style="width: 70%">
                            <asp:TextBox ID="txbNewsCreaDate" runat="server" SkinID="MainText" Text="沒有資料"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trLastUpdateUser" runat="server">
                        <td align="right" style="width: 30%">
                            <asp:Label ID="Label3" runat="server" SkinID="MainText" Text="公告人："></asp:Label>
                        </td>
                        <td align="left" style="width: 70%">
                            <asp:TextBox ID="txbLastUpdateUser" runat="server" SkinID="MainText" Text="沒有資料"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2" style="text-align: center;" valign="top">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 30%">
                            <asp:Label ID="Label7" runat="server" SkinID="MainText" Text="標題："></asp:Label>
                        </td>
                        <td align="left" style="width: 70%">
                            <asp:TextBox ID="txbNewsTitle" runat="server" SkinID="MainText" Text="沒有資料" Width="587px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 30%;" valign="top">
                            <asp:Label ID="Label37" runat="server" SkinID="MainText" Text="圖片："></asp:Label>
                        </td>
                        <td align="left" style="width: 70%">
                            <asp:Image ID="imgFiePicPath2" runat="server" Height="250px" ImageUrl="~/Backend/Images/default.jpg" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="auto-style1" valign="top"></td>
                        <td align="left" class="auto-style2">
                            <asp:FileUpload ID="uploadFiePicPath2" runat="server" Width="40%" />
                            <asp:HiddenField ID="hideFiePicPath2" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 30%" valign="top">
                            <asp:Label ID="Label8" runat="server" SkinID="MainText" Text="內文："></asp:Label>
                        </td>
                        <td align="left" style="width: 70%">
                            <CKEditor:CKEditorControl ID="CKEditorControl2" runat="server" BasePath="js/ckeditor" Height="400px" Width=""></CKEditor:CKEditorControl>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 30%">
                            <asp:Label ID="Label6" runat="server" SkinID="MainText" Text="來源網址：" Visible="False"></asp:Label>
                        </td>
                        <td align="left" style="width: 70%">
                            <asp:TextBox ID="txbNewsUrl" runat="server" SkinID="MainText" Text="沒有資料" Width="587px" Visible="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2" style="text-align: center;" valign="top">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 30%">
                            <asp:Label ID="Label9" runat="server" SkinID="MainText" Text="是否啟用："></asp:Label>
                        </td>
                        <td align="left" style="width: 70%">
                            <asp:CheckBox ID="ckbIsEnable" runat="server" Text="啟用請打勾" Checked="True" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2" style="text-align: center;" valign="top">
                            <asp:Label ID="lblMeg3" runat="server" ForeColor="Red" SkinID="MainWarn"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnEdit" runat="server" BackColor="Transparent" BorderWidth="0px"
                                CssClass="BigButton" Text="確定修改" ValidationGroup="OK" OnClick="btnEdit_Click" />
                            <asp:Button ID="btnOK" runat="server" BackColor="Transparent" BorderWidth="0px" CssClass="SmallButton"
                                Text="確定" ValidationGroup="OK" OnClick="btnOK_Click" />
                            <asp:Button ID="btnBack" runat="server" BackColor="Transparent" BorderWidth="0px"
                                CssClass="SmallButton" Text="返回" OnClick="btnBack_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:HiddenField ID="hideNewsStartDate" runat="server" />
                            <asp:HiddenField ID="hideNewsEndDate" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </div>
    <%--</ContentTemplate></asp:UpdatePanel>--%>
</asp:Content>

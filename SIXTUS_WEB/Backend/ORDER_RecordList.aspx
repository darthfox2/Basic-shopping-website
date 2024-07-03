<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/MasterPage.master" AutoEventWireup="true" CodeFile="ORDER_RecordList.aspx.cs" Inherits="Backend_ORDER_RecordList" %>

<%@ Register src="~/WebControls/PageControl.ascx" tagname="PageControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 25%;
            height: 20px;
        }
        .style2
        {
            width: 75%;
            height: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

      <script type="text/javascript">
          //開始日期
          $(function () {
              $("#ContentPlaceHolder1_txbSDate").datepicker({ dateFormat: "yy/mm/dd" });;
          });

          //結束日期
          $(function () {
              $("#ContentPlaceHolder1_txbEDate").datepicker({ dateFormat: "yy/mm/dd" });;
          });

    </script>

    <div id="title" class="ContentTitle">訂單查詢</div>

<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>--%>

    <div class="ContentBody">

<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <table style="width: 90%;" align="center">
                <tr>
                    <td align="left" style="width: 80%;">
                            &nbsp;</td>
                    <td align="right" style="width: 20%;">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" colspan="2">
                        <asp:Button ID="btnExport" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="BigButton" onclick="btnExport_Click" 
                            Text="匯出Excel" Visible="False" />
                        <input class="BigButton" onclick="print();" type="button" 
                                value="列印本頁" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 80%;">
                          <asp:Label ID="Label46" runat="server" SkinID="MainText" Text="狀態："></asp:Label>
                          <asp:DropDownList ID="ddlStatus" runat="server" Width="20%">
                          </asp:DropDownList>
                       </td>
                    <td align="right" style="width: 20%;">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="left" style="width: 80%;">
                        <asp:Label ID="Label16" runat="server" SkinID="MainText" Text="訂購日期："></asp:Label>
                        <asp:TextBox ID="txbSDate" runat="server" MaxLength="10"></asp:TextBox>
                        &nbsp;<asp:Label ID="Label33" runat="server" SkinID="MainText" Text="~"></asp:Label>
                        &nbsp;<asp:TextBox ID="txbEDate" runat="server" MaxLength="10"></asp:TextBox>
                        <asp:Button ID="btnSearch" runat="server" BorderColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" onclick="btnSearch_Click" Text="查詢" />
                        <asp:Label ID="lblMeg" runat="server" ForeColor="Red" SkinID="MainWarn"></asp:Label>
                    </td>
                    <td align="right" style="width: 20%;">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                     
                    <uc1:PageControl ID="PageControl1" runat="server" />
                     
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grid" runat="server" 
                            AutoGenerateColumns="False" Width="100%" 
                            onrowcommand="grid_RowCommand" onrowdatabound="grid_RowDataBound" 
                            DataSourceID="ObjectDataSource1" PageSize="20" AllowPaging="True">
                            <PagerSettings Visible="False" />
                            <Columns>
                                <asp:BoundField DataField="OdrID" HeaderText="訂單編號" />
                                <asp:BoundField DataField="CreateDate" HeaderText="訂購日期" 
                                    DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" />
                                    <asp:BoundField DataField="UseID" HeaderText="帳號名稱" />
                                    <asp:TemplateField HeaderText="中文名稱"><ItemTemplate><asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UseID") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                <asp:BoundField DataField="OdrTotalPrice" HeaderText="總金額(NTD)" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="狀態"><ItemTemplate><asp:Label ID="lblStatus" 
                                        runat="server" Text='<%# Eval("OdrStatus") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                <asp:TemplateField HeaderText="瀏覽明細"><ItemTemplate><asp:ImageButton ID="imgbtnView" runat="server" CausesValidation="True" 
                                CommandArgument='<%# Eval("OdrID") %>' CommandName="View" 
                                ImageUrl="Images/Check.gif" 
                                ToolTip="瀏覽明細" /></ItemTemplate><ItemStyle HorizontalAlign="Center" /></asp:TemplateField>
                                 <asp:TemplateField HeaderText="修改">
                                       <ItemTemplate><asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("OdrID") %>' CommandName="Modify" 
                                            ImageUrl="Images/Edit.gif" ToolTip="修改" /></ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="70px" /></asp:TemplateField>

                              

                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                            SelectMethod="GetOrderRecordByDateRang" 
                            TypeName="DAL.DALClass">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hideSDate" Name="dtStartSet" 
                                    PropertyName="Value" Type="DateTime" />
                                <asp:ControlParameter ControlID="hideEDate" Name="dtEndSet" 
                                    PropertyName="Value" Type="DateTime" />
                                <asp:ControlParameter ControlID="ddlStatus" DefaultValue="-1" Name="OdrStatus" 
                                    PropertyName="SelectedValue" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:HiddenField ID="hideSDate" runat="server" />
                        <asp:HiddenField ID="hideEDate" runat="server" />
                        <asp:HiddenField ID="hideUserID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:View>

        <%--===============================================================================================================================================--%>

        <asp:View ID="View2" runat="server">
         <table style="width:90%;" align="center">
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="Label2" runat="server" SkinID="MainSubject" Text="瀏覽訂單明細"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label5" runat="server" SkinID="MainText" Text="編號："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblOdrID2" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label3" runat="server" SkinID="MainText" Text="訂購日期："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblCreateDate2" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label41" runat="server" SkinID="MainText" Text="備註："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblOdrDesc2" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label51" runat="server" SkinID="MainText" Text="訂單狀態："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblOdrStatus2" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        &nbsp;</td>
                    <td align="left" style="width: 75%">
                        &nbsp;</td>
                </tr>
                </table>

                <table style="width:90%;" align="center">
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="Label34" runat="server" SkinID="MainSubject" Text="商品資訊"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:GridView ID="gridPD" runat="server" AutoGenerateColumns="False" 
                            PageSize="20" Width="100%" BackColor="White" BorderColor="#DEDFDE" 
                            BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" 
                            GridLines="Vertical" SkinID="NOSET" onrowdatabound="gridPD_RowDataBound">
                            <FooterStyle BackColor="#CCCC99" />
                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                            <PagerSettings Visible="False" />
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="類別">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrtName" runat="server" Text='<%# Eval("PrtID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="PrdName" HeaderText="名稱" ReadOnly="True">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="說明">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrdDesc" runat="server" Text='<%# Eval("PrdDesc") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="PrdPrice" HeaderText="價格(NTD)" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Right" Width="100px" />
                                </asp:BoundField>
                                                 
                                <asp:BoundField DataField="PrdTotalCount" HeaderText="數量" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Right" Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PrdTotalPrice" HeaderText="小計(NTD)" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Right" Width="100px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="修改" Visible="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("PrdID") %>' CommandName="Modify" 
                                            ImageUrl="Images/Edit.gif" ToolTip="修改" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="刪除" Visible="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnDel" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("PrdID") %>' CommandName="Cancel" 
                                            ImageUrl="Images/Delete.gif" 
                                            onclientclick="return window.confirm('確定要刪除嗎？');" ToolTip="刪除" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <RowStyle BackColor="#F7F7DE" />
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#FBFBF2" />
                            <SortedAscendingHeaderStyle BackColor="#848384" />
                            <SortedDescendingCellStyle BackColor="#EAEAD3" />
                            <SortedDescendingHeaderStyle BackColor="#575357" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width:97%">
                        <asp:Label ID="Label47" runat="server" SkinID="MainWarn" Text="商品總數量："></asp:Label>
                    </td>
                    <td align="right" style="width: 3%">
                        <asp:Label ID="lblOdrTotalCount2" runat="server" SkinID="MainWarn"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width:97%">
                        <asp:Label ID="Label48" runat="server" SkinID="MainWarn" Text="商品總金額(NTD)："></asp:Label>
                    </td>
                    <td align="right" style="width: 3%">
                        <asp:Label ID="lblProductPrice2" runat="server" SkinID="MainWarn"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width:97%">
                        <asp:Label ID="Label49" runat="server" SkinID="MainWarn" Text="運費(NTD)："></asp:Label>
                    </td>
                    <td align="right" style="width: 3%">
                        <asp:Label ID="lblOdrFare2" runat="server" SkinID="MainWarn"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width:97%">
                        <asp:Label ID="Label40" runat="server" SkinID="MainWarn" Text="總金額(NTD)："></asp:Label>
                    </td>
                    <td align="right" style="width: 3%">
                        <asp:Label ID="lblOdrTotalPrice2" runat="server" SkinID="MainWarn"></asp:Label>
                    </td>
                </tr>
                    <tr>
                        <td align="right" style="width:97%">
                            &nbsp;</td>
                        <td align="right" style="width: 3%">
                            &nbsp;</td>
                    </tr>
                </table>

                <table style="width:90%;" align="center">
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="Label42" runat="server" SkinID="MainSubject" Text="訂購人資訊"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label22" runat="server" SkinID="MainText" Text="帳號：" 
                            Visible="False"></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblUseID2" runat="server" SkinID="MainText" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label24" runat="server" SkinID="MainText" Text="中文名稱："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblUseName2" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label17" runat="server" SkinID="MainText" Text="國家/地區："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblUseCountry2" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label43" runat="server" SkinID="MainText" Text="郵遞區號："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblUseZipCode2" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label26" runat="server" SkinID="MainText" Text="地址："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblUseAddr2" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label27" runat="server" SkinID="MainText" Text="手機號碼："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblUseCell2" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label44" runat="server" SkinID="MainText" Text="電子信箱："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblUseEmail2" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%" valign="top">
                            &nbsp;</td>
                    <td align="left" style="width: 75%">
                            &nbsp;</td>
                </tr>
            </table>

                            <table style="width:90%;" align="center">
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="Label1" runat="server" SkinID="MainSubject" Text="收件人資訊"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label7" runat="server" SkinID="MainText" Text="中文名稱："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblOUseName2" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label9" runat="server" SkinID="MainText" Text="國家/地區："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblOUseCountry2" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label11" runat="server" SkinID="MainText" Text="郵遞區號："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblOUseZipCode2" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label13" runat="server" SkinID="MainText" Text="地址："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblOUseAddr2" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label15" runat="server" SkinID="MainText" Text="手機號碼："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblOUseCell2" runat="server" SkinID="MainText"></asp:Label>
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
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnBack2" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" onclick="btnBack_Click" 
                            Text="返回" />
                    </td>
                </tr>
            </table>


        </asp:View>

        <%--===============================================================================================================================================--%>

         <asp:View ID="View3" runat="server">
         <table style="width:90%;" align="center">
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="Label4" runat="server" SkinID="MainSubject" Text="修改訂單明細" 
                            EnableViewState="False"></asp:Label>
                    </td>
                </tr>
                   <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label6" runat="server" SkinID="MainText" Text="編號："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblOdrID3" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label10" runat="server" SkinID="MainText" Text="訂購日期："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblCreateDate3" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label52" runat="server" SkinID="MainText" Text="備註："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lblOdrDesc3" runat="server" SkinID="MainText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%" valign="top">
                        <asp:Label ID="Label56" runat="server" SkinID="MainText" Text="客服回覆："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:TextBox ID="txbOdrFeedBack3" runat="server" Height="50px" MaxLength="200" 
                            TextMode="MultiLine" Width="100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        <asp:Label ID="Label54" runat="server" SkinID="MainText" Text="訂單狀態："></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:DropDownList ID="ddlStatus3" runat="server" Width="20%">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
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
                            CssClass="SmallButton" Text="確定" ValidationGroup="OK" Visible="False" />
                        <asp:Button ID="btnBack" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" onclick="btnBack_Click" Text="返回" />
                        <asp:HiddenField ID="hideID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 25%">
                        &nbsp;</td>
                    <td align="left" style="width: 75%">
                        &nbsp;</td>
                </tr>
         </table>
         </asp:View>


    </asp:MultiView>

</div>

<%--</ContentTemplate></asp:UpdatePanel>--%>

</asp:Content>


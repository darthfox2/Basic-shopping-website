<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/MasterPage.master" AutoEventWireup="true" CodeFile="REPORT_RecordMonth.aspx.cs" Inherits="Backend_REPORT_RecordMonth" %>

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
    <div id="title" class="ContentTitle">月報表查詢</div>

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
                            Text="匯出Excel" />
                        <input class="BigButton" onclick="print();" type="button" 
                                value="列印本頁" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 80%;">
                          <asp:Label ID="lblPW3" runat="server" SkinID="MainText" Text="年度："></asp:Label>
                          <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="True" Width="15%" 
                              onselectedindexchanged="ddlYear_SelectedIndexChanged">
                          </asp:DropDownList>
                          &nbsp;<asp:Label ID="lblPW4" runat="server" SkinID="MainText" Text="月份："></asp:Label>
                          <asp:DropDownList ID="ddlMonth" runat="server" 
                              onselectedindexchanged="ddlMonth_SelectedIndexChanged" Width="10%">
                              <asp:ListItem>1</asp:ListItem>
                              <asp:ListItem>2</asp:ListItem>
                              <asp:ListItem>3</asp:ListItem>
                              <asp:ListItem>4</asp:ListItem>
                              <asp:ListItem>5</asp:ListItem>
                              <asp:ListItem>6</asp:ListItem>
                              <asp:ListItem>7</asp:ListItem>
                              <asp:ListItem>8</asp:ListItem>
                              <asp:ListItem>9</asp:ListItem>
                              <asp:ListItem>10</asp:ListItem>
                              <asp:ListItem>11</asp:ListItem>
                              <asp:ListItem>12</asp:ListItem>
                          </asp:DropDownList>
                          &nbsp;<asp:Button ID="btnSearch" runat="server" BorderColor="Transparent" 
                              BorderWidth="0px" CssClass="SmallButton" onclick="btnSearch_Click" Text="查詢" />
                          <asp:Label ID="lblMeg" runat="server" ForeColor="Red" SkinID="MainWarn"></asp:Label>
                       </td>
                    <td align="right" style="width: 20%;">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grid" runat="server" 
                            AutoGenerateColumns="False" Width="100%" 
                            onrowdatabound="grid_RowDataBound" PageSize="20" AllowPaging="True" 
                            DataSourceID="ObjectDataSource1">
                            <PagerSettings Visible="False" />
                            <Columns>

                                <asp:TemplateField HeaderText="訂單已完成(單)"><ItemTemplate><asp:Label ID="lblRD" runat="server" ></asp:Label></ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="完成付款(單)"><ItemTemplate><asp:Label ID="lblCP" runat="server" ></asp:Label></ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="出貨中(單)"><ItemTemplate><asp:Label ID="lblSI" runat="server" ></asp:Label></ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="完成出貨(單)"><ItemTemplate><asp:Label ID="lblCS" runat="server" ></asp:Label></ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="訂單終結(單)"><ItemTemplate><asp:Label ID="lblOK" runat="server" ></asp:Label></ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="OdrTotalCount" HeaderText="商品總數量">
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OdrTotalPrice" HeaderText="商品總金額(NTD)">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>



                              

                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                            SelectMethod="GetRecordByDateRange" TypeName="DAL.DALClass">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hideSDate" Name="dtStartSet" 
                                    PropertyName="Value" Type="DateTime" />
                                <asp:ControlParameter ControlID="hideEDate" Name="dtEndSet" 
                                    PropertyName="Value" Type="DateTime" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
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

    </asp:MultiView>

</div>

<%--</ContentTemplate></asp:UpdatePanel>--%>

</asp:Content>


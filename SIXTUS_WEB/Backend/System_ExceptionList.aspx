<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/MasterPage.master" AutoEventWireup="true"
    CodeFile="System_ExceptionList.aspx.cs" Inherits="System_ExceptionList" %>

<%@ Register Src="~/WebControls/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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

    </script>
    <div id="title" class="ContentTitle">
        錯誤紀錄查詢</div>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>--%>
    <div class="ContentBody">
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="View1" runat="server">
                <table style="width: 90%;" align="center">
                    <tr>
                        <td align="right" colspan="2">
                            <asp:Button ID="btnExport" runat="server" BackColor="Transparent" BorderWidth="0px"
                                CssClass="BigButton" OnClick="btnExport_Click" Text="匯出Excel" Visible="False" />
                            <input class="BigButton" onclick="print();" type="button" value="列印本頁" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 80%;">
                            <asp:Label ID="Label34" runat="server" SkinID="MainText" Text="錯誤類別："></asp:Label>
                            <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" Width="15%">
                            </asp:DropDownList>
                        </td>
                        <td align="right" style="width: 20%;">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 80%;">
                            <asp:Label ID="Label16" runat="server" SkinID="MainText" Text="日期："></asp:Label>
                            <asp:TextBox ID="txbSDate" runat="server" MaxLength="10"></asp:TextBox>
                            &nbsp;<asp:Label ID="Label33" runat="server" SkinID="MainText" Text="~"></asp:Label>
                            &nbsp;<asp:TextBox ID="txbEDate" runat="server" MaxLength="10"></asp:TextBox>
                            &nbsp;<asp:Button ID="btnSearch" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                CssClass="SmallButton" OnClick="btnSearch_Click" Text="查詢" />
                            <asp:Label ID="lblMeg" runat="server" ForeColor="Red" SkinID="MainWarn"></asp:Label>
                        </td>
                        <td align="right" style="width: 20%;">
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
                                    <asp:BoundField DataField="CreateTime" HeaderText="日期" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" />
                                    <asp:BoundField DataField="ExtDesc" HeaderText="錯誤類別" ReadOnly="True" />
                                    <asp:BoundField DataField="ExrMsg" HeaderText="Exception內容" ReadOnly="True">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetExceptionByDateRang"
                                TypeName="DAL.DALClass">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hideSDate" Name="dtStartSet" PropertyName="Value"
                                        Type="DateTime" />
                                    <asp:ControlParameter ControlID="hideEDate" Name="dtEndSet" PropertyName="Value"
                                        Type="DateTime" />
                                    <asp:ControlParameter ControlID="ddlType" Name="ErrType" PropertyName="SelectedValue"
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
        </asp:MultiView>
    </div>
    <%--</ContentTemplate></asp:UpdatePanel>--%>
</asp:Content>

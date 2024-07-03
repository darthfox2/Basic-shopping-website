<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/MasterPage.master" AutoEventWireup="true" CodeFile="MANAGE_ProductList.aspx.cs" Inherits="Backend_MANAGE_ProductList" %>

<%@ Register src="~/WebControls/PageControl.ascx" tagname="PageControl" tagprefix="uc1" %>

<%@ Register assembly="CKEditor.NET" namespace="CKEditor.NET" tagprefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
    {
        }
    .style2
    {
        width: 30%;
        height: 23px;
    }
    .style3
    {
        width: 70%;
        height: 23px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="title" class="ContentTitle">商品上架</div>

<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>--%>

<div class="ContentBody">

<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <table style="width: 95%;" align="center">
                <tr>
                    <td align="left" class="style1">
                        <asp:Label ID="Label35" runat="server" SkinID="MainText" Text="狀態："></asp:Label>
                        <asp:DropDownList ID="ddlEnable" runat="server" Width="20%" AutoPostBack="True">
                            <asp:ListItem Value="1">啟用</asp:ListItem>
                            <asp:ListItem Value="0">停用</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="Label34" runat="server" SkinID="MainText" Text="語系：" 
                            Visible="False"></asp:Label>
                        <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" 
                            Visible="False" Width="15%">
                        </asp:DropDownList>
                    </td>
                    <td align="right" style="width: 20%;">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="left" class="style1">
                        <asp:Label ID="Label1" runat="server" SkinID="MainText" Text="類別："></asp:Label>
                        <asp:DropDownList ID="ddlProductType" runat="server" AutoPostBack="True" 
                            Width="25%">
                        </asp:DropDownList>
                    </td>
                    <td align="right" style="width: 20%;">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="left" class="style1">
                        &nbsp;</td>
                    <td align="right" style="width: 20%;">
                        <asp:Button ID="btnAdd" runat="server" BorderColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" Text="新增" 
                            onclick="btnAdd_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="style1" colspan="2">
                        <uc1:PageControl ID="PageControl1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grid" runat="server" 
                            AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" Width="100%" 
                            onrowcommand="grid_RowCommand" onrowdatabound="grid_RowDataBound" 
                            PageSize="100">
                            <PagerSettings Visible="False" />
                            <Columns>

                                <asp:TemplateField HeaderText="類別">
                                <ItemTemplate>
                                    <asp:Label ID="lblPrtName" runat="server" Text='<%# Eval("PrtID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="center" /></asp:TemplateField>

                                <asp:BoundField DataField="PrdName" HeaderText="名稱" ReadOnly="True" >
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="left" />
                                </asp:BoundField>

                               <asp:TemplateField HeaderText="簡介">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrdDesc" runat="server" Text='<%# Eval("PrdDesc") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" /></asp:TemplateField>

                                <asp:BoundField DataField="PrdDesc" HeaderText="說明" ReadOnly="True" 
                                    Visible="False">
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PrdColor" HeaderText="顏色" Visible="False">
                                 <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PrdPrice" HeaderText="原價格" ReadOnly="True" >
                                <ItemStyle HorizontalAlign="Right" Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PrdSalePrice" HeaderText="優惠價格" ReadOnly="True" >
                                <ItemStyle HorizontalAlign="Right" Width="70px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="圖片">
                                    <ItemTemplate>
                                        <asp:Image ID="imgPic" AlternateText = '<%# Eval("PrdID") %>'  runat="server" ImageUrl="
                                            /default.jpg" 
                                            Height="80px" Width="80px" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="80px" />
                                    <ItemStyle Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="是否啟用">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIsEnable" runat="server" Text='<%# Eval("PrdEnable") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="70px" /></asp:TemplateField>
                                   <asp:TemplateField HeaderText="修改">
                                       <ItemTemplate><asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("PrdID") %>' CommandName="Modify" 
                                            ImageUrl="Images/Edit.gif" ToolTip="修改" /></ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="70px" /></asp:TemplateField>
                                      <asp:TemplateField HeaderText="刪除">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnDel" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("PrdID") %>' CommandName="Cancel" 
                                            ImageUrl="Images/Delete.gif" 
                                            onclientclick="return window.confirm('確定要刪除嗎？');" ToolTip="刪除" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                            SelectMethod="GetProductByPrtID" TypeName="DAL.DALClass">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlProductType" DefaultValue="-1" Name="PrtID" 
                                    PropertyName="SelectedValue" Type="String" />
                                <asp:Parameter DefaultValue="-1" Name="RemovePrdID" Type="String" />
                                <asp:ControlParameter ControlID="ddlEnable" DefaultValue="" Name="PrdEnable" PropertyName="SelectedValue" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:HiddenField ID="hideUserID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <table style="width:100%;">
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="lblTitle" runat="server" SkinID="MainSubject" Text="新增/修改商品"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%">
                        <asp:Label ID="lable93" runat="server" SkinID="MainText" Text="商品類別："></asp:Label>
                    </td>
                    <td align="left" style="width: 70%">
                        <asp:DropDownList ID="ddlProductType2" runat="server" AutoPostBack="True" 
                            Width="25%">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%">
                        <asp:Label ID="lable88" runat="server" SkinID="MainText" Text="商品名稱："></asp:Label>
                    </td>
                    <td align="left" style="width: 70%">
                        <asp:TextBox ID="txbPrdName" runat="server" MaxLength="50" Width="35%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPrdName" runat="server" 
                            ControlToValidate="txbPrdName" ErrorMessage="RequiredFieldValidator" 
                            ForeColor="Red" SkinID="MainAlert" ValidationGroup="OK">名稱不可為空</asp:RequiredFieldValidator>
                    </td>
                </tr>
              
            
                <tr>
                    <td align="right" class="style2" valign="top">
                        <asp:Label ID="lable89" runat="server" SkinID="MainText" Text="商品簡介："></asp:Label>
                    </td>
                    <td align="left" class="style3">
                        <asp:TextBox ID="txbPrdDesc" runat="server" MaxLength="800" Width="60%" 
                            Height="180px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            
                <tr>
                    <td align="right" style="width: 30%" valign="top">
                        <asp:Label ID="lable103" runat="server" SkinID="MainText" Text="商品內容："></asp:Label>
                    </td>
                    <td align="left" style="width: 70%">
                        <CKEditor:CKEditorControl ID="CKEditorControl2" runat="server" BasePath="js/ckeditor" Height="300px" Width=""></CKEditor:CKEditorControl>
                    </td>
                </tr>
            
                <tr>
                    <td align="right" style="width: 30%" valign="top">
                        <asp:Label ID="lable90" runat="server" SkinID="MainText" Text="原價格(NTD)："></asp:Label>
                    </td>
                    <td align="left" style="width: 70%">
                        <asp:TextBox ID="txbPrdPrice" runat="server" MaxLength="30" Width="20%"></asp:TextBox>
                        &nbsp;<asp:Label ID="lblPicDesc3_3" runat="server" SkinID="MainAlert">單位：元</asp:Label>
                        <asp:RequiredFieldValidator ID="rfvPrdPrice" runat="server" ControlToValidate="txbPrdPrice" ErrorMessage="RequiredFieldValidator" ForeColor="Red" SkinID="MainAlert" ValidationGroup="OK">價格不可為空</asp:RequiredFieldValidator>
                    </td>
                </tr>
            
                <tr>
                    <td align="right" class="style2" valign="top">
                        <asp:Label ID="lable102" runat="server" SkinID="MainText" Text="優惠價格(NTD)："></asp:Label>
                    </td>
                    <td align="left" class="style3">
                        <asp:TextBox ID="txbPrdSalePrice" runat="server" MaxLength="30" Width="20%"></asp:TextBox>
                        &nbsp;<asp:Label ID="lblPicDesc3_8" runat="server" SkinID="MainAlert">單位：元</asp:Label>
                        <asp:RequiredFieldValidator ID="rfvPrdSalePrice" runat="server" ControlToValidate="txbPrdSalePrice" ErrorMessage="RequiredFieldValidator" ForeColor="Red" SkinID="MainAlert" ValidationGroup="OK">價格不可為空</asp:RequiredFieldValidator>
                    </td>
                </tr>
            
                <tr>
                    <td align="right" style="width: 30%" valign="top">
                        <asp:Label ID="lable101" runat="server" SkinID="MainText" Text="商品對外連結："></asp:Label>
                    </td>
                    <td align="left" style="width: 70%">
                        <asp:TextBox ID="txbPrdUrl" runat="server" MaxLength="500" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%" valign="top">
                        &nbsp;</td>
                    <td align="left" style="width: 70%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%" valign="top">
                        <asp:Label ID="lblIsEnable0" runat="server" SkinID="MainText" Text="是否啟用："></asp:Label>
                    </td>
                    <td align="left" style="width: 70%">
                        <asp:CheckBox ID="ckbIsEnable" runat="server" Checked="True" Text="啟用請打勾" />
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%" valign="top">&nbsp;</td>
                    <td align="left" style="width: 70%">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center" valign="top" colspan="2">
                        <asp:Label ID="lable100" runat="server" SkinID="MainSubject" Text="產品圖片"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%" valign="top">
                        <asp:Label ID="lable91" runat="server" SkinID="MainText" Text="圖片上傳："></asp:Label>
                    </td>
                    <td align="left" style="width: 70%">
                        <asp:FileUpload ID="fulSetPath" runat="server" Width="60%" />
                        <asp:Button ID="btnUpload" runat="server" BorderColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" onclick="btnUpload_Click" Text="上傳" />
                        <asp:HiddenField ID="hideFileName" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%" valign="top">
                        &nbsp;</td>
                    <td align="left" style="width: 70%">
                        <asp:Label ID="lblPicDesc3_2" runat="server" SkinID="MainAlert">（上傳最高限制為五張圖片，圖片支援（bmp,jpg,jpeg,png,gif）等格式。</asp:Label>
                        <br />
                        <asp:Label ID="lblPicDesc3_7" runat="server" SkinID="MainAlert">排序第一為商品頁大圖，其餘為小圖。）</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="top" colspan="2">

                    <asp:GridView ID="gridPic" runat="server" AutoGenerateColumns="False" 
                        DataKeyNames="ID" onrowcommand="gridPic_RowCommand" 
                        onrowdatabound="gridPic_RowDataBound" Width="80%" BackColor="White" 
                            BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                            ForeColor="Black" GridLines="Vertical" SkinID="NOSET">
                        <FooterStyle BackColor="#CCCC99" />
                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                        <PagerSettings Visible="False" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>

                            <asp:BoundField DataField="SORT" HeaderText="排序" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Center" Width="90px" />
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="NAME" HeaderText="名稱" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CREATE_DATE" DataFormatString="{0:yyyy/MM/dd}" 
                                HeaderText="建立日期" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="縮圖">
                                <ItemTemplate>
                                    <asp:Image ID="imgPic" runat="server" AlternateText='<%# Eval("ID") %>' 
                                        Height="70px" Width="70px" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="刪除">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnDel" runat="server" CausesValidation="False" 
                                        CommandArgument='<%# Eval("ID") %>' CommandName="PicCancel" 
                                        ImageUrl="Images/Delete.gif" 
                                        onclientclick="return window.confirm('確定要刪除嗎？');" ToolTip="刪除" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="60px" />
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
                    <td align="right" colspan="2" style="text-align: center;" valign="top">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" colspan="2" style="text-align: center;" valign="top">
                        <asp:Label ID="lblMeg" runat="server" ForeColor="Red" SkinID="MainWarn"></asp:Label>
                        <asp:Label ID="lblPrice_Check" runat="server" ForeColor="Red" 
                            SkinID="MainBigWarn"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnEdit" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="BigButton" Text="確定修改" 
                            ValidationGroup="OK" onclick="btnEdit_Click" />
                        <asp:Button ID="btnOK" runat="server" BackColor="Transparent" BorderWidth="0px" 
                            CssClass="SmallButton" Text="確定" ValidationGroup="OK" 
                            onclick="btnOK_Click" />
                        <asp:Button ID="btnBack" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" Text="返回" 
                            onclick="btnBack_Click" />
                        <asp:HiddenField ID="hideID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>

</div>

<%--</ContentTemplate></asp:UpdatePanel>--%>

</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/MasterPage.master" AutoEventWireup="true" CodeFile="MANAGE_ArticleList.aspx.cs" Inherits="Backend_MANAGE_ArticleList" %>

<%@ Register src="~/WebControls/PageControl.ascx" tagname="PageControl" tagprefix="uc1" %>

<%@ Register assembly="CKEditor.NET" namespace="CKEditor.NET" tagprefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script type="text/javascript">

    var jQuery132 = jQuery.noConflict(true);

    //設定日期
    jQuery132(function () {
        jQuery132("#ContentPlaceHolder1_txbAtcOpenDate2").datepicker({ dateFormat: "yy/mm/dd" });;
    });

</script>

    <style type="text/css">
        .style1
    {
        }
    .style2
    {
        height: 23px;
    }
    .style3
    {
        width: 70%;
        height: 23px;
    }
        .auto-style1 {
            width: 20%;
            height: 23px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="title" class="ContentTitle">網站內容管理</div>

<%--</ContentTemplate></asp:UpdatePanel>--%>

<div class="ContentBody">

<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <table style="width: 95%;" align="center">
                <tr>
                    <td align="left" class="style2">
                        </td>
                    <td align="right" class="auto-style1">
                        </td>
                </tr>
                <tr>
                    <td align="left" class="style1">
                        <asp:Label ID="Label16" runat="server" SkinID="MainText" Text="類別："></asp:Label>
                        <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" Width="25%">
                        </asp:DropDownList>
                        &nbsp;<asp:Button ID="btnSearch" runat="server" BorderColor="Transparent" BorderWidth="0px" CssClass="SmallButton" Text="查詢" Visible="False" />
                        <asp:Label ID="lblMeg" runat="server" ForeColor="Red" SkinID="MainWarn"></asp:Label>
                    </td>
                    <td align="right" style="width: 20%;">
                        <asp:Button ID="btnAdd" runat="server" BorderColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" Text="新增" OnClick="btnAdd_Click" />
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
                           
                               <asp:BoundField DataField="LastUpdateDate" 
                                    DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" HeaderText="最後修改時間">
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                                </asp:BoundField>
                               <asp:TemplateField HeaderText="類別" Visible="False">  <ItemTemplate>
                                    <asp:Label ID="lblAttName" runat="server" Text='<%# Eval("AttID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="80px" /></asp:TemplateField>

                                <asp:BoundField DataField="AtcName" HeaderText="名稱" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:TemplateField HeaderText="網址" Visible="False">
                                <ItemTemplate>
                                <asp:Label ID="lblLink" runat="server" Text='<%# Eval("AttID") + "," + Eval("AtcID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="left" Width="200px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="預計上架日期" Visible="False">
                                <ItemTemplate>
                                <asp:Label ID="lblOpenDate" runat="server" Text='<%# Eval("AtcID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="是否啟用">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIsEnable" runat="server" Text='<%# Eval("AtcEnable") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="圖片" >
                                    <ItemTemplate>
                                        <asp:Image ID="imgPic" AlternateText = '<%# Eval("AtcID") + "," + Eval("AtcPicPath") %>' runat="server" ImageUrl="Images/default.jpg" 
                                            Height="80px" Width="80px" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="80px" />
                                </asp:TemplateField>
                                   <asp:TemplateField HeaderText="預覽" Visible="False">
                                       <ItemTemplate>
                                           <asp:ImageButton ID="imgbtnView" runat="server" CausesValidation="False" CommandArgument='<%# Eval("AtcID") %>' CommandName="View" ImageUrl="Images/View.gif" ToolTip="瀏覽" />
                                       </ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:TemplateField>
                                   <asp:TemplateField HeaderText="修改">
                                       <ItemTemplate><asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("AtcID") %>' CommandName="Modify" 
                                            ImageUrl="Images/Edit.gif" ToolTip="修改" /></ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="70px" /></asp:TemplateField>

                                      <asp:TemplateField HeaderText="刪除">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnDel" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("AtcID") %>' CommandName="Cancel" 
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
                            SelectMethod="GetArticleByAttID" TypeName="DAL.DALClass">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlType" Name="AttID" 
                                    PropertyName="SelectedValue" Type="String" />
                                <asp:Parameter DefaultValue="-1" Name="AtcEnable" Type="String" />
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
                        <asp:Label ID="lblTitle0" runat="server" SkinID="MainSubject" Text="新增/修改文稿"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%">
                        <asp:Label ID="lable88" runat="server" SkinID="MainText" Text="名稱："></asp:Label>
                    </td>
                    <td align="left" style="width: 70%">
                        <asp:TextBox ID="txbAtcName2" runat="server" MaxLength="50" Width="60%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvAtcName2" runat="server" ControlToValidate="txbAtcName2" ErrorMessage="RequiredFieldValidator" ForeColor="Red" SkinID="MainAlert" ValidationGroup="OK">名稱不可為空</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%" valign="top">
                        <asp:Label ID="lable89" runat="server" SkinID="MainText" Text="說明："></asp:Label>
                    </td>
                    <td align="left" style="width: 70%">
                        <asp:TextBox ID="txbAtcDesc2" runat="server" Height="50px" MaxLength="800" TextMode="MultiLine" Width="60%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%" valign="top">
                        <asp:Label ID="lable107" runat="server" SkinID="MainText" Text="預計上架日期："></asp:Label>
                    </td>
                    <td align="left" style="width: 70%">
                        <asp:CheckBox ID="ckbAtcSetOD2" runat="server" Text="啟用請打勾" />
                        <asp:TextBox ID="txbAtcOpenDate2" runat="server" MaxLength="10"></asp:TextBox>
                        <asp:DropDownList ID="ddlDateH2" runat="server" Width="50px">
                            <asp:ListItem>00</asp:ListItem>
                            <asp:ListItem>01</asp:ListItem>
                            <asp:ListItem>02</asp:ListItem>
                            <asp:ListItem>03</asp:ListItem>
                            <asp:ListItem>04</asp:ListItem>
                            <asp:ListItem>05</asp:ListItem>
                            <asp:ListItem>06</asp:ListItem>
                            <asp:ListItem>07</asp:ListItem>
                            <asp:ListItem>08</asp:ListItem>
                            <asp:ListItem>09</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                            <asp:ListItem>13</asp:ListItem>
                            <asp:ListItem>14</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>16</asp:ListItem>
                            <asp:ListItem>17</asp:ListItem>
                            <asp:ListItem>18</asp:ListItem>
                            <asp:ListItem>19</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>21</asp:ListItem>
                            <asp:ListItem>22</asp:ListItem>
                            <asp:ListItem>23</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lable149" runat="server" SkinID="MainText" Text="時"></asp:Label>
                        <asp:DropDownList ID="ddlDateM2" runat="server" Width="50px">
                            <asp:ListItem>00</asp:ListItem>
                            <asp:ListItem>01</asp:ListItem>
                            <asp:ListItem>02</asp:ListItem>
                            <asp:ListItem>03</asp:ListItem>
                            <asp:ListItem>04</asp:ListItem>
                            <asp:ListItem>05</asp:ListItem>
                            <asp:ListItem>06</asp:ListItem>
                            <asp:ListItem>07</asp:ListItem>
                            <asp:ListItem>08</asp:ListItem>
                            <asp:ListItem>09</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                            <asp:ListItem>13</asp:ListItem>
                            <asp:ListItem>14</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>16</asp:ListItem>
                            <asp:ListItem>17</asp:ListItem>
                            <asp:ListItem>18</asp:ListItem>
                            <asp:ListItem>19</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>21</asp:ListItem>
                            <asp:ListItem>22</asp:ListItem>
                            <asp:ListItem>23</asp:ListItem>
                            <asp:ListItem>24</asp:ListItem>
                            <asp:ListItem>25</asp:ListItem>
                            <asp:ListItem>26</asp:ListItem>
                            <asp:ListItem>27</asp:ListItem>
                            <asp:ListItem>28</asp:ListItem>
                            <asp:ListItem>29</asp:ListItem>
                            <asp:ListItem>30</asp:ListItem>
                            <asp:ListItem>31</asp:ListItem>
                            <asp:ListItem>32</asp:ListItem>
                            <asp:ListItem>33</asp:ListItem>
                            <asp:ListItem>34</asp:ListItem>
                            <asp:ListItem>35</asp:ListItem>
                            <asp:ListItem>36</asp:ListItem>
                            <asp:ListItem>37</asp:ListItem>
                            <asp:ListItem>38</asp:ListItem>
                            <asp:ListItem>39</asp:ListItem>
                            <asp:ListItem>40</asp:ListItem>
                            <asp:ListItem>41</asp:ListItem>
                            <asp:ListItem>42</asp:ListItem>
                            <asp:ListItem>43</asp:ListItem>
                            <asp:ListItem>44</asp:ListItem>
                            <asp:ListItem>45</asp:ListItem>
                            <asp:ListItem>46</asp:ListItem>
                            <asp:ListItem>47</asp:ListItem>
                            <asp:ListItem>48</asp:ListItem>
                            <asp:ListItem>49</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                            <asp:ListItem>51</asp:ListItem>
                            <asp:ListItem>52</asp:ListItem>
                            <asp:ListItem>53</asp:ListItem>
                            <asp:ListItem>54</asp:ListItem>
                            <asp:ListItem>55</asp:ListItem>
                            <asp:ListItem>56</asp:ListItem>
                            <asp:ListItem>57</asp:ListItem>
                            <asp:ListItem>58</asp:ListItem>
                            <asp:ListItem>59</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lable150" runat="server" SkinID="MainText" Text="分"></asp:Label>
                        <br />
                        <asp:Label ID="lblPositionAlert10" runat="server" SkinID="MainAlert" Text="(設定上架日期當天則顯示)"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%">
                        <asp:Label ID="lblIsEnable3" runat="server" SkinID="MainText" Text="是否啟用："></asp:Label>
                    </td>
                    <td align="left" style="width: 70%">
                        <asp:CheckBox ID="ckbAtcEnable2" runat="server" Checked="True" Text="啟用請打勾" />
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%">&nbsp;</td>
                    <td align="left" style="width: 70%">&nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%" valign="top">
                        <asp:Label ID="Label36" runat="server" SkinID="MainText" Text="圖片："></asp:Label>
                    </td>
                    <td align="left" style="width: 70%">
                        <asp:Image ID="imgAtcPicPath2" runat="server" Height="150px" ImageUrl="Images/default.jpg" />
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%">&nbsp;</td>
                    <td align="left" style="width: 70%">
                        <asp:FileUpload ID="uploadAtcPicPath2" runat="server" />
                        <asp:Button ID="btnUploadPic2" runat="server" OnClick="btnUploadPic2_Click" Text="圖片上傳" />
                        <asp:HiddenField ID="hideAtcPicPath2" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%">&nbsp;</td>
                    <td align="left" style="width: 70%">&nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%" valign="top">
                        <asp:Label ID="Label38" runat="server" SkinID="MainText" Text="FaceBook分享圖片："></asp:Label>
                    </td>
                    <td align="left" style="width: 70%">
                        <asp:Image ID="imgAtcFBPicPath2" runat="server" Height="100px" ImageUrl="Images/default.jpg" />
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%" valign="top">&nbsp;</td>
                    <td align="left" style="width: 70%">
                        <asp:FileUpload ID="uploadAtcFBPicPath2" runat="server" />
                        <asp:Button ID="btnUploadFBPic2" runat="server" Text="圖片上傳" OnClick="btnUploadFBPic2_Click" />
                        <asp:HiddenField ID="hideAtcFBPicPath2" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%">&nbsp;</td>
                    <td align="left" style="width: 70%">&nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%">
                        <asp:Label ID="lable93" runat="server" SkinID="MainText" Text="內容類型："></asp:Label>
                    </td>
                    <td align="left" style="width: 70%">
                        <asp:DropDownList ID="ddlArticleClass2" runat="server" Width="30%">
                        </asp:DropDownList>
                        &nbsp;<asp:Button ID="btnAdd2" runat="server" BorderColor="Transparent" BorderWidth="0px" CssClass="SmallButton" OnClick="btnAdd2_Click" Text="新增" />

                    </td>
                </tr>
                <tr>
                    <td align="center" valign="top" colspan="2">

                    <asp:GridView ID="gridContent2" runat="server" AutoGenerateColumns="False" 
                        DataKeyNames="AtnID" Width="70%" BackColor="White" 
                            BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                            ForeColor="Black" GridLines="Vertical" SkinID="NOSET" DataSourceID="ObjectDataSource2" OnRowCommand="gridContent2_RowCommand">
                        <FooterStyle BackColor="#CCCC99" />
                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                        <PagerSettings Visible="False" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>

                            <asp:BoundField DataField="AtnSort" HeaderText="排序">
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:BoundField>

                            <asp:BoundField DataField="AtsName" HeaderText="內容類型">
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="AtnSubject" HeaderText="標題" ReadOnly="True">
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LastUpdateDate" DataFormatString="{0:yyyy/MM/dd}" 
                                HeaderText="最後修改日期" ReadOnly="True" Visible="False">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="向上" Visible="True">
                                       <ItemTemplate><asp:ImageButton ID="imgbtnSortUp" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("AtcID") + "," + Eval("AtnID") + "," + Eval("AtnSort") %>' CommandName="SortUp" 
                                            ImageUrl="Images/SortUp.gif" ToolTip="向上" /></ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="50px" /></asp:TemplateField>

                            <asp:TemplateField HeaderText="向下" Visible="True">
                                       <ItemTemplate><asp:ImageButton ID="imgbtnSortDown" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("AtcID") + "," + Eval("AtnID") + "," + Eval("AtnSort") %>' CommandName="SortDown" 
                                            ImageUrl="Images/SortDown.gif" ToolTip="向下" /></ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="50px" /></asp:TemplateField>

                            <asp:TemplateField HeaderText="修改" Visible="True">
                                       <ItemTemplate><asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="False" 
                                            CommandArgument='<%# Eval("AtsID") + "," + Eval("AtnID") %>' CommandName="Modify" 
                                            ImageUrl="Images/Edit.gif" ToolTip="修改" /></ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="50px" /></asp:TemplateField>

                            <asp:TemplateField HeaderText="刪除">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnDel" runat="server" CausesValidation="False" CommandArgument='<%# Eval("AtcID") + "," + Eval("AtnID") %>' CommandName="Cancel" ImageUrl="Images/Delete.gif" onclientclick="return window.confirm('確定要刪除嗎？');" ToolTip="刪除" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
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

                        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetArticleContentByAtcID" TypeName="DAL.DALClass">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hideID" Name="AtcID" PropertyName="Value" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>

                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2" style="text-align: center;" valign="top">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" colspan="2" style="text-align: center;" valign="top">
                        <asp:Label ID="lblMeg2" runat="server" ForeColor="Red" SkinID="MainWarn"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnEdit2" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="BigButton" Text="確定修改" 
                            ValidationGroup="OK" onclick="btnEdit2_Click" />
                        <asp:Button ID="btnOK2" runat="server" BackColor="Transparent" BorderWidth="0px" 
                            CssClass="SmallButton" Text="確定" ValidationGroup="OK" OnClick="btnOK2_Click" />
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

             <asp:View ID="View3" runat="server">
            <table style="width:100%;">
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="Label1" runat="server" SkinID="MainSubject" Text="新增/修改副標區塊"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 10%" valign="top">
                        <asp:Label ID="Label39" runat="server" SkinID="MainText" Text="標題："></asp:Label>
                    </td>
                    <td align="left" style="width: 90%">
                        <asp:TextBox ID="txbAtnSubject3" runat="server" MaxLength="100" Width="80%"></asp:TextBox>
                    </td>
                </tr>
          
                <tr>
                    <td align="right" style="width: 10%" valign="top">
                        <asp:Label ID="Label2" runat="server" SkinID="MainText" Text="內容："></asp:Label>
                    </td>
                    <td align="left" style="width: 90%">
                        <CKEditor:CKEditorControl ID="CKEditorControl3" runat="server" BasePath="js/ckeditor" Height="100px"></CKEditor:CKEditorControl>
                    </td>
                </tr>
          
                <tr>
                    <td align="right" colspan="2" style="text-align: center;" valign="top">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnEdit3" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="BigButton" Text="確定修改" 
                            ValidationGroup="OK" OnClick="btnEdit3_Click" />
                        <asp:Button ID="btnOK3" runat="server" BackColor="Transparent" BorderWidth="0px" CssClass="SmallButton" OnClick="btnOK3_Click" Text="確定" ValidationGroup="OK" />
                        <asp:Button ID="btnBack3" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" Text="返回" OnClick="btnBack3_Click" />
                        <asp:HiddenField ID="hideAtnID3" runat="server" />
                        <asp:HiddenField ID="hideAtsID3" runat="server" />
                        <asp:HiddenField ID="hideAtnSort3" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:View>

       <asp:View ID="View4" runat="server">
            <table style="width:100%;">
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="Label3" runat="server" SkinID="MainSubject" Text="新增/修改文字區塊"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 10%" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="MainText" Text="標題："></asp:Label>
                    </td>
                    <td align="left" style="width: 90%">
                        <asp:TextBox ID="txbAtnSubject4" runat="server" MaxLength="100" Width="80%"></asp:TextBox>
                    </td>
                </tr>
          
                <tr>
                    <td align="right" style="width: 10%" valign="top">
                        <asp:Label ID="Label5" runat="server" SkinID="MainText" Text="內容："></asp:Label>
                    </td>
                    <td align="left" style="width: 90%">
                        <CKEditor:CKEditorControl ID="CKEditorControl4" runat="server" BasePath="js/ckeditor" Height="500px"></CKEditor:CKEditorControl>
                    </td>
                </tr>
          
                <tr>
                    <td align="right" colspan="2" style="text-align: center;" valign="top">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnEdit4" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="BigButton" Text="確定修改" 
                            ValidationGroup="OK" OnClick="btnEdit4_Click" />
                        <asp:Button ID="btnOK4" runat="server" BackColor="Transparent" BorderWidth="0px" CssClass="SmallButton" OnClick="btnOK4_Click" Text="確定" ValidationGroup="OK" />
                        <asp:Button ID="btnBack4" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" Text="返回" OnClick="btnBack3_Click" />
                        <asp:HiddenField ID="hideAtnID4" runat="server" />
                        <asp:HiddenField ID="hideAtsID4" runat="server" />
                        <asp:HiddenField ID="hideAtnSort4" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:View>

    <asp:View ID="View5" runat="server">
            <table style="width:100%;">
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="Label6" runat="server" SkinID="MainSubject" Text="新增/修改圖片區塊"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 10%" valign="top">
                        <asp:Label ID="Label7" runat="server" SkinID="MainText" Text="標題："></asp:Label>
                    </td>
                    <td align="left" style="width: 90%">
                        <asp:TextBox ID="txbAtnSubject5" runat="server" MaxLength="100" Width="80%"></asp:TextBox>
                    </td>
                </tr>
          
                <tr>
                    <td align="right" style="width: 10%" valign="top">
                        <asp:Label ID="Label40" runat="server" SkinID="MainText" Text="圖片："></asp:Label>
                    </td>
                    <td align="left" style="width: 90%">
                        <asp:Image ID="imgAtnPicPath5" runat="server" Height="400px" ImageUrl="Images/default.jpg" />
                    </td>
                </tr>
          
                <tr>
                    <td align="right" style="width: 10%" valign="top">&nbsp;</td>
                    <td align="left" style="width: 90%">
                        <asp:FileUpload ID="uploadAtnPicPath5" runat="server" Width="40%" />
                        <asp:HiddenField ID="hideAtnPicPath5" runat="server" />
                    </td>
                </tr>
          
                <tr>
                    <td align="right" colspan="2" style="text-align: center;" valign="top">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnEdit5" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="BigButton" Text="確定修改" 
                            ValidationGroup="OK" OnClick="btnEdit5_Click" />
                        <asp:Button ID="btnOK5" runat="server" BackColor="Transparent" BorderWidth="0px" CssClass="SmallButton" OnClick="btnOK5_Click" Text="確定" ValidationGroup="OK" />
                        <asp:Button ID="btnBack5" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" Text="返回" OnClick="btnBack3_Click" />
                        <asp:HiddenField ID="hideAtnID5" runat="server" />
                        <asp:HiddenField ID="hideAtsID5" runat="server" />
                        <asp:HiddenField ID="hideAtnSort5" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:View>

    <asp:View ID="View6" runat="server">
            <table style="width:100%;">
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="Label8" runat="server" SkinID="MainSubject" Text="新增/修改串流影片區塊(YouTube)"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 10%" valign="top">
                        <asp:Label ID="Label9" runat="server" SkinID="MainText" Text="標題："></asp:Label>
                    </td>
                    <td align="left" style="width: 90%">
                        <asp:TextBox ID="txbAtnSubject6" runat="server" MaxLength="100" Width="80%"></asp:TextBox>
                    </td>
                </tr>
          
                <tr>
                    <td align="right" style="width: 10%" valign="top">
                        <asp:Label ID="Label41" runat="server" SkinID="MainText" Text="影片連結："></asp:Label>
                    </td>
                    <td align="left" style="width: 90%">
                        <asp:TextBox ID="txbAtnVideoPath6" runat="server" MaxLength="500" Width="80%"></asp:TextBox>
                        <br />
                        <asp:Label ID="Label42" runat="server" SkinID="MainAlert" Text="(網址請輸入：http://XXX.XXXXXX.XXX)"></asp:Label>
                    </td>
                </tr>
          
                <tr>
                    <td align="right" colspan="2" style="text-align: center;" valign="top">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnEdit6" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="BigButton" Text="確定修改" 
                            ValidationGroup="OK" OnClick="btnEdit6_Click" />
                        <asp:Button ID="btnOK6" runat="server" BackColor="Transparent" BorderWidth="0px" CssClass="SmallButton" OnClick="btnOK6_Click" Text="確定" ValidationGroup="OK" />
                        <asp:Button ID="btnBack6" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" Text="返回" OnClick="btnBack3_Click" />
                        <asp:HiddenField ID="hideAtnID6" runat="server" />
                        <asp:HiddenField ID="hideAtsID6" runat="server" />
                        <asp:HiddenField ID="hideAtnSort6" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:View>

      <asp:View ID="View7" runat="server">
            <table style="width:100%;">
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="Label10" runat="server" SkinID="MainSubject" Text="新增/修改串流影片區塊(Vimeo)"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 10%" valign="top">
                        <asp:Label ID="Label11" runat="server" SkinID="MainText" Text="標題："></asp:Label>
                    </td>
                    <td align="left" style="width: 90%">
                        <asp:TextBox ID="txbAtnSubject7" runat="server" MaxLength="100" Width="80%"></asp:TextBox>
                    </td>
                </tr>
          
                <tr>
                    <td align="right" style="width: 10%" valign="top">
                        <asp:Label ID="Label12" runat="server" SkinID="MainText" Text="影片連結："></asp:Label>
                    </td>
                    <td align="left" style="width: 90%">
                        <asp:TextBox ID="txbAtnVideoPath7" runat="server" MaxLength="500" Width="80%"></asp:TextBox>
                        <br />
                        <asp:Label ID="Label13" runat="server" SkinID="MainAlert" Text="(網址請輸入：http://XXX.XXXXXX.XXX)"></asp:Label>
                    </td>
                </tr>
          
                <tr>
                    <td align="right" colspan="2" style="text-align: center;" valign="top">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnEdit7" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="BigButton" Text="確定修改" 
                            ValidationGroup="OK" OnClick="btnEdit7_Click" />
                        <asp:Button ID="btnOK7" runat="server" BackColor="Transparent" BorderWidth="0px" CssClass="SmallButton" OnClick="btnOK7_Click" Text="確定" ValidationGroup="OK" />
                        <asp:Button ID="btnBack7" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" Text="返回" OnClick="btnBack3_Click" />
                        <asp:HiddenField ID="hideAtnID7" runat="server" />
                        <asp:HiddenField ID="hideAtsID7" runat="server" />
                        <asp:HiddenField ID="hideAtnSort7" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:View>
    <asp:View ID="View8" runat="server">
            <table style="width:100%;">
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="Label14" runat="server" SkinID="MainSubject" Text="新增/修改串流影片區塊(優酷)"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 10%" valign="top">
                        <asp:Label ID="Label15" runat="server" SkinID="MainText" Text="標題："></asp:Label>
                    </td>
                    <td align="left" style="width: 90%">
                        <asp:TextBox ID="txbAtnSubject8" runat="server" MaxLength="100" Width="80%"></asp:TextBox>
                    </td>
                </tr>
          
                <tr>
                    <td align="right" style="width: 10%" valign="top">
                        <asp:Label ID="Label17" runat="server" SkinID="MainText" Text="影片連結："></asp:Label>
                    </td>
                    <td align="left" style="width: 90%">
                        <asp:TextBox ID="txbAtnVideoPath8" runat="server" MaxLength="500" Width="80%"></asp:TextBox>
                        <br />
                        <asp:Label ID="Label18" runat="server" SkinID="MainAlert" Text="(網址請輸入：http://XXX.XXXXXX.XXX)"></asp:Label>
                    </td>
                </tr>
          
                <tr>
                    <td align="right" colspan="2" style="text-align: center;" valign="top">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnEdit8" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="BigButton" Text="確定修改" 
                            ValidationGroup="OK" OnClick="btnEdit8_Click" />
                        <asp:Button ID="btnOK8" runat="server" BackColor="Transparent" BorderWidth="0px" CssClass="SmallButton" OnClick="btnOK8_Click" Text="確定" ValidationGroup="OK" />
                        <asp:Button ID="btnBack8" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" Text="返回" OnClick="btnBack3_Click" />
                        <asp:HiddenField ID="hideAtnID8" runat="server" />
                        <asp:HiddenField ID="hideAtsID8" runat="server" />
                        <asp:HiddenField ID="hideAtnSort8" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:View>
    <asp:View ID="View9" runat="server">
            <table style="width:100%;">
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="Label19" runat="server" SkinID="MainSubject" Text="新增/修改串流影片區塊(土豆網)"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 10%" valign="top">
                        <asp:Label ID="Label20" runat="server" SkinID="MainText" Text="標題："></asp:Label>
                    </td>
                    <td align="left" style="width: 90%">
                        <asp:TextBox ID="txbAtnSubject9" runat="server" MaxLength="100" Width="80%"></asp:TextBox>
                    </td>
                </tr>
          
                <tr>
                    <td align="right" style="width: 10%" valign="top">
                        <asp:Label ID="Label21" runat="server" SkinID="MainText" Text="影片連結："></asp:Label>
                    </td>
                    <td align="left" style="width: 90%">
                        <asp:TextBox ID="txbAtnVideoPath9" runat="server" MaxLength="500" Width="80%"></asp:TextBox>
                        <br />
                        <asp:Label ID="Label43" runat="server" SkinID="MainAlert" Text="(請複製HTML代碼，只取SRC內網址即可＜embed src='http://XXX-XXXXX-XXX'＞＜/embed＞)"></asp:Label>
                    </td>
                </tr>
          
                <tr>
                    <td align="right" colspan="2" style="text-align: center;" valign="top">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnEdit9" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="BigButton" Text="確定修改" 
                            ValidationGroup="OK" OnClick="btnEdit9_Click" />
                        <asp:Button ID="btnOK9" runat="server" BackColor="Transparent" BorderWidth="0px" CssClass="SmallButton" OnClick="btnOK9_Click" Text="確定" ValidationGroup="OK" />
                        <asp:Button ID="btnBack9" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" Text="返回" OnClick="btnBack3_Click" />
                        <asp:HiddenField ID="hideAtnID9" runat="server" />
                        <asp:HiddenField ID="hideAtsID9" runat="server" />
                        <asp:HiddenField ID="hideAtnSort9" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:View>

    <asp:View ID="View10" runat="server">
            <table style="width:100%;">
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="Label22" runat="server" SkinID="MainSubject" Text="新增/修改MP4影片區塊"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 10%" valign="top">
                        <asp:Label ID="Label23" runat="server" SkinID="MainText" Text="標題："></asp:Label>
                    </td>
                    <td align="left" style="width: 90%">
                        <asp:TextBox ID="txbAtnSubject10" runat="server" MaxLength="100" Width="80%"></asp:TextBox>
                    </td>
                </tr>
          
                <tr>
                    <td align="right" style="width: 10%" valign="top">
                        <asp:Label ID="Label24" runat="server" SkinID="MainText" Text="影片連結："></asp:Label>
                    </td>
                    <td align="left" style="width: 90%">
                        <asp:TextBox ID="txbAtnVideoPath10" runat="server" MaxLength="500" Width="80%"></asp:TextBox>
                        <br />
                        <asp:Label ID="Label44" runat="server" SkinID="MainAlert" Text="(網址請輸入：http://XXX.XXXXXX.XXX)"></asp:Label>
                    </td>
                </tr>
          
                <tr>
                    <td align="right" colspan="2" style="text-align: center;" valign="top">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnEdit10" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="BigButton" Text="確定修改" 
                            ValidationGroup="OK" OnClick="btnEdit10_Click" />
                        <asp:Button ID="btnOK10" runat="server" BackColor="Transparent" BorderWidth="0px" CssClass="SmallButton" OnClick="btnOK10_Click" Text="確定" ValidationGroup="OK" />
                        <asp:Button ID="btnBack10" runat="server" BackColor="Transparent" 
                            BorderWidth="0px" CssClass="SmallButton" Text="返回" OnClick="btnBack3_Click" />
                        <asp:HiddenField ID="hideAtnID10" runat="server" />
                        <asp:HiddenField ID="hideAtsID10" runat="server" />
                        <asp:HiddenField ID="hideAtnSort10" runat="server" />
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


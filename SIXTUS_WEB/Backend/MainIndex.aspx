<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/MasterPage.master" AutoEventWireup="true" CodeFile="MainIndex.aspx.cs" Inherits="Backend_MainIndex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style3
        {
            height: 65px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div id="title" class="ContentTitle">首頁</div>

<asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>

<div class="ContentBody">

      <table style="width:100%;">
        <tr>
            <td align="center" class="style3">
            </td>
        </tr>
        <tr>
            <td align="center">

                <a href="MANAGE_NewsList.aspx"><img alt="" src="Images/IndexButton05.jpg" border='0px'/></a><P>

                <a href="MANAGE_ProductList.aspx"><img alt="" src="Images/IndexButton01.jpg" border='0px' /></a><P>

                <a href="ORDER_RecordList.aspx"><img alt="" src="Images/IndexButton04.jpg" border='0px'/></a><P>

               

             
                </td>
        </tr>
        <tr>
            <td align="right" class="style3">
                </td>
        </tr>
    </table>

</div>
</ContentTemplate></asp:UpdatePanel>
</asp:Content>


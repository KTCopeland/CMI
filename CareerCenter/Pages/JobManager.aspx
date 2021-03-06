﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JobManager.aspx.cs" Inherits="CareerCenter.Pages.JobManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script src ="/js/ga.js"></script>

    <style>
        .jobManagerList{
            border: 5px groove #000000;
            font-family:Arial, Helvetica, sans-serif;
                    }
        .jobManagerList .topTableRow{
            font-weight:bold;
        }
        .jobManagerList td {
            border-bottom: 3px solid #ffffff;
            border-right: 3px solid #ffffff;
            background-color:#f3f3f3;
            padding: 5px;
        }
        .cmdStyle{
            margin-left: 15px;
        }
        .appName {
            font-size: 28px;
            font-weight: 700;
            display: table-cell;
            vertical-align: middle;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table>
            <tr>
                <td>
                    <img src="../Images/ExperisLogo.png" /></td>
                <td class="appName">Career Center Manager</td>
            </tr>
        </table>
        <hr />
    <div>
    <table>
        <tr>
            <td><asp:Button ID="cmdBack" class="cmdStyle" runat="server" Text="Go Back" OnClick="cmdBack_Click" /></td>
            <td><asp:Button ID="cmdNew" class="cmdStyle" runat="server" Text="New Job" OnClick="cmdNew_Click" /></td>
            <td><asp:Button ID="cmdView" class="cmdStyle" runat="server" Text ="Show All Jobs" OnClick="cmdView_Click" /></td>

        </tr>
    </table>
        
        <hr />
        <asp:PlaceHolder ID="ph_List" runat="server"></asp:PlaceHolder>

    </div>
    </form>
</body>
</html>

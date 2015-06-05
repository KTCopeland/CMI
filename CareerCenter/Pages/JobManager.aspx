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
        #cmdBack{
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
    
        <asp:Button ID="cmdNew" runat="server" Text="New Job" OnClick="cmdNew_Click" />   <asp:Button ID="cmdBack" runat="server" Text="Go Back" OnClick="cmdBack_Click" />
        <hr />
        <asp:PlaceHolder ID="ph_List" runat="server"></asp:PlaceHolder>

    </div>
    </form>
</body>
</html>

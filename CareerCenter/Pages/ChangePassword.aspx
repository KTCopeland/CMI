<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="CareerCenter.Pages.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>

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
            <td>User Name</td>
            <td>
                <asp:TextBox ID="txtUser" runat="server" Width="200px" Enabled="False"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Current Password</td>
            <td>
                <asp:TextBox ID="txtCurrent" runat="server" TextMode="Password" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>New Password</td>
            <td>
                <asp:TextBox ID="txtNew" runat="server" TextMode="Password" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Verify Password</td>
            <td>
                <asp:TextBox ID="txtRepeat" runat="server" TextMode="Password" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <div align="right">
                <asp:Button ID="cmdCancel" runat="server" Text="Cancel" OnClick="cmdCancel_Click" />
                <asp:Button ID="cmdApply" class="cmdStyle" runat="server" Text="Apply" OnClick="cmdApply_Click" />
                    </div>
            </td>
        </tr>
    </table>
    </div>
        <br />
    <span id="spanMessage" runat="server" style="color: #800000">
        Messages go here
    </span>
    </form>
</body>
</html>

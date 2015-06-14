<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="CareerCenter.Pages.EditUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!-- jQuery 2.0.2 -->
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.0.2/jquery.min.js" type="text/javascript"></script>
    <!-- jQuery UI 1.10.3 -->
    <script src="js/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <!-- ckeditor -->
    <script src="/js/ckeditor/ckeditor.js" type="text/javascript"></script>
    <script src ="/js/ga.js"></script>
    <style>
        table {
            padding: 6px;
            font-family: Arial, Helvetica, sans-serif;
        }

        td {
            padding-right: 10px;
        }

        .divHeader {
            background-color: #eeeeee;
        }
        #divDescription{
            font-family: Arial, Helvetica, sans-serif;
        }
        .appName {
            font-size: 28px;
            font-weight: 700;
            display: table-cell;
            vertical-align: middle;
        }
        .command{
            margin-left: 15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hfX" runat="server" />
        <table>
            <tr>
                <td>
                    <img src="../Images/ExperisLogo.png" /></td>
                <td class="appName">Career Center Manager</td>
            </tr>
        </table>
        <hr />
    <div>
        <asp:Button ID="cmdBack" CssClass="command" runat="server" Text="Go Back" OnClick="cmdBack_Click" Width="80px" />
        <asp:Button ID="cmdSave" CssClass="command" runat="server" Text="Save" Width="80px" OnClick="cmdSave_Click"  />
        <hr />
    <table>
        <tr>
            <td>User Name</td>
            <td>
                <asp:TextBox ID="txtName" runat="server" Width="200px"></asp:TextBox>
            </td>
            <td>Password</td>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" Width="200px" TextMode="Password"></asp:TextBox>
            </td>
            <td>
                <asp:CheckBox ID="chkAvailable" runat="server" Text="Available" TextAlign="Left"  /> 
            </td>
        </tr>
        <tr>
            <td>Email</td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server" Width="200px"></asp:TextBox>
            </td>

            <td></td>
                <td>
                    <asp:Button ID="cmdReset" runat="server" Text="Reset Password" OnClick="cmdReset_Click" />
                </td>
        </tr>
    </table>
<br />
        <table>
            <tr>
                <td>Last Login:</td>
                <td>
                    <asp:Label ID="lblLastLogin" runat="server" Text="N/A"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Failed Attempts:</td>
                <td>
                    <asp:Label ID="lblFails" runat="server" Text="N/A"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

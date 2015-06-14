<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Manager.aspx.cs" Inherits="CareerCenter.Pages.Manager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .appName {
            font-size: 28px;
            font-weight: 700;
            display: table-cell;
            vertical-align: middle;
        }

        #cmdLogin {
            margin-left: 230px;
        }

        #divLogin {
            margin-left: 62px;
        }

        #divMain {
            margin-left: 62px;
            width: 250px;
        }

        a {
            font-size: 24px;
            color: #0094ff;
            text-decoration: none;
        }

            a:hover {
                color: #ff6a00;
            }

        .menuImage {
            height: 48px;
            width: 48px;
        }

        hr {
            border: 0;
            height: 1px;
            margin: 5px 0px;
            background-image: -webkit-linear-gradient(left, rgba(0,0,0,0.30), rgba(0,0,0,0.70), rgba(0,0,0,0.30));
            background-image: -moz-linear-gradient(left, rgba(0,0,0,0.30), rgba(0,0,0,0.70), rgba(0,0,0,0.30));
            background-image: -ms-linear-gradient(left, rgba(0,0,0,0.30), rgba(0,0,0,0.70), rgba(0,0,0,0.30));
            background-image: -o-linear-gradient(left, rgba(0,0,0,0.30), rgba(0,0,0,0.70), rgba(0,0,0,0.30));
        }

        .greyLine {
            display: block;
            height: 1px;
            border: 0;
            border-top: 1px solid #ccc;
            margin: 1px 0;
            padding: 0;
        }
        .instructions{
            font-size: 24px;
            color: #0094ff;
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

        <div id="divMain" runat="server">
            <table>
                <tr>
                    <td>
                        <img class="menuImage" src="../Images/JobManager.png" />
                    </td>
                    <td>
                        <a href="JobManager.aspx">Manage Jobs</a>
                    </td>
                    </tr>
                    <tr>
                    <td>
                        <img class="menuImage" src="../Images/Export.png" />
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkExportJobs" runat="server" OnClick="lnkExportJobs_Click">Export Jobs</asp:LinkButton>
                    </td>

                </tr>
                <tr>
                    <td>
                        <img class="menuImage" src="../Images/NewJob.png" />
                    </td>
                    <td>
                        <a href="EditJob.aspx">Create New Job</a>
                    </td>
                </tr>
            </table>
            <hr class="greyLine" />
            <table>
                <tr>
                    <td>
                        <img class="menuImage" src="../Images/Candidates.png" />
                    </td>
                    <td>
                        <a href="CandidateManager.aspx">Candidates</a>
                    </td>
                </tr>
                <tr>
                    <td>
                        <img class="menuImage" src="../Images/Employers.png" />
                    </td>
                    <td>
                        <a href="EmployerManager.aspx">Employers</a>
                    </td>
                </tr>
                <tr>
                    <td>
                        <img class="menuImage" src="../Images/Users.png" />
                    </td>
                    <td>
                        <a href="UserManager.aspx">Users</a>
                    </td>
                </tr>
            </table>
            <hr class="greyLine" />
            <table>
                <tr>
                    <td>
                        <img class="menuImage" src="../Images/Home.png" />
                    </td>
                    <td>
                        <a href="http://contentmarketinginstitute.careers/">Career Center Site</a>
                    </td>
                </tr>
                <tr>
                    <td>
                        <img class="menuImage" src="../Images/Analytics.png" />
                    </td>
                    <td>
                        <a href="https://www.google.com/analytics">Analytics</a><br />
                    </td>
                </tr>
            </table>
            <hr class="greyLine" />
            <table>
                <tr>
                    <td>
                        <img class="menuImage" src="../Images/Password.png" />
                    </td>
                    <td>
                        <a href="ChangePassword.aspx">Change Password</a><br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <img class="menuImage" src="../Images/Keys.png" />
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkLogOut" runat="server" OnClick="lnkLogOut_Click">Log Out</asp:LinkButton>
                    </td>
                </tr>
            </table>


        </div>

        <div id="divLogin" runat="server">
            <br />
            <span class="instructions">Please log in to access this site</span><br />
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Width="80px">User Name</asp:Label>
                        <asp:TextBox ID="txtUser" runat="server" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Width="80px">Password</asp:Label>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="cmdLogin" runat="server" Text="Log In" OnClick="cmdLogin_Click" /></td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>

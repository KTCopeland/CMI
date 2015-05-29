<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditJob.aspx.cs" Inherits="CareerCenter.Pages.EditJob" ValidateRequest="false" %>

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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hfDescription" runat="server" />

        <div id="divEditMenu"></div>

        <div class="divHeader">

            <table>
                <tr>
                    <td>Job ID</td>
                    <td>
                        <asp:TextBox ID="txtID" runat="server" Width="80px" Enabled="False"></asp:TextBox></td>
                    <td>Job Title</td>
                    <td>
                        <asp:TextBox ID="txtTitle" runat="server" Width="600px"></asp:TextBox></td>
                </tr>
            </table>

            <table>
                <tr>
                    <td>Employer</td>
                    <td>
                        <asp:DropDownList ID="ddlEmployer" runat="server" Width="250px"></asp:DropDownList></td>
                    <td>City</td>
                    <td>
                        <asp:TextBox ID="txtCity" runat="server" Width="150px"></asp:TextBox></td>
                    <td>State</td>
                    <td>
                        <asp:TextBox ID="txtTerritory" runat="server"></asp:TextBox></td>
                    <td>Zip</td>
                    <td>
                        <asp:TextBox ID="txtPostalCode" runat="server" Width="100px"></asp:TextBox></td>
                </tr>
            </table>

            <table>
                <tr>
                    <td>Active Date</td>
                    <td>
                        <asp:TextBox ID="txtActiveDate" runat="server"></asp:TextBox></td>
                    <td>Inactive Date</td>
                    <td>
                        <asp:TextBox ID="txtInactiveDate" runat="server"></asp:TextBox></td>
                    <td>
                        <asp:CheckBox ID="chkAvailable" runat="server" Text="Job Available" TextAlign="Left" /></td>
                </tr>
            </table>
        </div>

        <hr />

        <div id="divDescription" contenteditable ="true" runat="server"></div>

    </form>

    <script type="text/javascript">
        $(function () {
            CKEDITOR.config.sharedSpaces = { top: 'divEditMenu' };
            CKEDITOR.inline('divDescription');
        });

        function ckContentSave() {
            //alert('content will be saved');
            $("#hfDescription").val(CKEDITOR.instances.divDescription.getData());
            document.forms[0].submit();
        }

        function ckContentCancel() {
            //alert('edit will be cancelled');
            window.location = "\JobManager.aspx";
        }

        //$("#divDescription").focus(function () {
        //    document.getElementById('divEditMenu').style.display = "block";
        //});
    </script>
</body>
</html>

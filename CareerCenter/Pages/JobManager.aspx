<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JobManager.aspx.cs" Inherits="CareerCenter.Pages.JobManager" %>

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
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="cmdNew" runat="server" Text="New Job" OnClick="cmdNew_Click" />
        <hr />
        <asp:PlaceHolder ID="ph_List" runat="server"></asp:PlaceHolder>

    </div>
    </form>
</body>
</html>

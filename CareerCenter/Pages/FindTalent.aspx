<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FindTalent.aspx.cs" Inherits="CareerCenter.Pages.FindTalent" %>

<%@ Register Src="../Controls/EmployerControl.ascx" TagName="EmployerControl" TagPrefix="uc1" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <title>Find Talent</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    <link href="../CareerCenter.css" rel='stylesheet' type='text/css' />

</head>
<body>

    <form id="form1" runat="server">
        <div style="align-items: center;" align="center">
            <h1>Find Talent</h1>
                <uc1:EmployerControl ID="EmployerControl1" runat="server" />
        </div>
    </form>



</body>
</html>

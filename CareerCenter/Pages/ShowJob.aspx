<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowJob.aspx.cs" Inherits="CareerCenter.Pages.ShowJob" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    <link href="../CareerCenter.css" rel='stylesheet' type='text/css' />

</head>
<body>
    <form id="form1" runat="server">
        <div>
        <div class="leftColumn">
            <asp:PlaceHolder ID="ph_Left" runat="server"></asp:PlaceHolder>
        <br />
        </div>
        <div class="rightColumn">
            <span class='applyHeader'>APPLY NOW</span><br />
            <p class='applyText'>It's free for job seekers and 100% confidential.  We will contact you before submitting you for a job.</p>
            <asp:PlaceHolder ID="ph_Right" runat="server"></asp:PlaceHolder>
            <br />
        </div>

        </div>


        <div class="navSection">
            <hr />
            <a class="goBack" href='/pages/JobSearch.aspx'>  Return to Search</a>
        </div>


    </form>
</body>
</html>

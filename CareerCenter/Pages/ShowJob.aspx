<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowJob.aspx.cs" Inherits="CareerCenter.Pages.ShowJob" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    <style>
        .leftColumn {
            /*float: left;
            width: 61.8%;*/
        }

        .rightColumn {
            /*float: left;
            width: 38.2%;
            min-width: 300px;
            text-align: center;*/
        }

        .jobHeader {
            font-size: 24px;
            color: #F7931E;
        }

        .jobLocation {
            font-size: 20px;
            font-weight: bold;
        }

        .jobDetail {
            margin-top: 10px;
        }

        .navSection {
            font-size: 20px;
            color: #F7931E;
            clear: both;
        }

        .goBack {
            color: #F7931E; 
            text-decoration: none;            
        }

        hr {
            border: 0;
            height: 1px;
            background-image: -webkit-linear-gradient(left, rgba(0,0,0,0.30), rgba(0,0,0,0.70), rgba(0,0,0,0.30));
            background-image: -moz-linear-gradient(left, rgba(0,0,0,0.30), rgba(0,0,0,0.70), rgba(0,0,0,0.30));
            background-image: -ms-linear-gradient(left, rgba(0,0,0,0.30), rgba(0,0,0,0.70), rgba(0,0,0,0.30));
            background-image: -o-linear-gradient(left, rgba(0,0,0,0.30), rgba(0,0,0,0.70), rgba(0,0,0,0.30));
        }

        .applyHeader{
            font-size: 20px;
            font-weight:bold;
            color: #F7931E;
        }

        .applyText{

        }

        .controlTable {
            width: 100%;
        }

        .controlCell {
            padding: 10px 0px;
        }

        .controlItem {
            width: 95%;
        }

        .fileName {
            width: 80%;
        }

        .fileReset {
            width: 14.5%;
        }

        .submitButton {
            width: 200px;
            height: 50px;
            color: #ffffff;
            background-color: #428BCA;
            border-style: none;
            font-weight: bold;
        }

        .requiredEntry {
            color: red;
        }

        .redIndicator {
            color: red;
        }
    </style>

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

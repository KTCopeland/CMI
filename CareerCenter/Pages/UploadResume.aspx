<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadResume.aspx.cs" Inherits="CareerCenter.Pages.UploadResume" %>

<%@ Register Src="~/Controls/JobSeeker.ascx" TagPrefix="uc1" TagName="JobSeeker" %>

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
        <div class="callout">
            <h1>Upload Your Resume</h1>
            <p>It's free for job seekers and 100% confidential.  We will contact you before submitting you for a job.</p>
            <uc1:JobSeeker runat="server" ID="JobSeeker" />
        </div>
    </form>





</body>
</html>
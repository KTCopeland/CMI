<%@ Page Title="" Language="C#" MasterPageFile="~/Land/Land.Master" AutoEventWireup="true" CodeBehind="Job.aspx.cs" Inherits="CareerCenter.Land.Job" %>
<asp:Content ID="Content_Header" ContentPlaceHolderID="HeaderInformation" runat="server">
    <asp:PlaceHolder ID="ph_Header" runat="server"></asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content_Landing" ContentPlaceHolderID="LandingContent" runat="server">
        <script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-55920cfd00b77975" async="async"></script>

    <script type="text/javascript">
        var addthis_share = {
            url: "<%=Get_URL()%>",
            title: "<%=Get_Title()%>",
            description: "<%=Get_Description()%>"
        }

</script>

        <div class="leftColumn">
            <asp:PlaceHolder ID="ph_Left" runat="server"></asp:PlaceHolder>
        <br />
        </div>
    <hr />
    <br />
        <div class="rightColumn">
            <span class='applyHeader'>APPLY NOW</span><br />
            <p class='applyText'>It's free for job seekers and 100% confidential.  We will contact you before submitting you for a job.</p>
            <asp:PlaceHolder ID="ph_Right" runat="server"></asp:PlaceHolder>
            <br />
        </div>
</asp:Content>

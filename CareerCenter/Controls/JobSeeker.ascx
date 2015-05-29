<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JobSeeker.ascx.cs" Inherits="CareerCenter.Controls.JobSeeker" %>
<div id ='divEntry' runat ="server">
<table class='controlTable' >
    <thead>
        <tr>
            <td class='controlCell'>
                <span class="redIndicator">*</span>
                <asp:TextBox runat="server" ID="txtName" class='controlItem' placeholder="Your Name" ></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="rfvtxtName" runat="server" ControlToValidate="txtName" CssClass="requiredEntry" Display="Dynamic" ErrorMessage="Please enter Your Name."></asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td class='controlCell'>
                <span class="redIndicator">*</span>
                <asp:TextBox runat="server" ID="txtEmail" placeholder="E-mail" class='controlItem'></asp:TextBox><br />
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtEmail" ControlToValidate="txtEmail" CssClass="requiredEntry" Display="Dynamic" ErrorMessage="Please enter Email."></asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td class='controlCell'>
                <span class="redIndicator">*</span>
                <asp:TextBox runat="server" ID="txtPhone" placeholder="Phone number" class='controlItem'></asp:TextBox><br />
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtPhone" ControlToValidate="txtPhone" CssClass="requiredEntry" Display="Dynamic" ErrorMessage="Please enter Phone."></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revtxtPhone"
                    runat="server" ErrorMessage="Please enter phone in a standard format like (000) 000-0000." Display="Dynamic"
                    ControlToValidate="txtPhone" CssClass="required"
                    ValidationExpression="^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$" />

            </td>
        </tr>
        <tr>
            <td class='controlCell'>
                <span class="redIndicator">*</span>
                <asp:FileUpload ID="fluResume" runat="server" Style="display: none;" onchange="fileChanged(this)" />
                <input id="txtFileName" type="text" class='fileName' readonly placeholder="Add resume (.doc, .docx, .pdf, .txt)" onclick="document.getElementById('<%= fluResume.ClientID %>').click(); return false;" />
                <input id="FileUploadReset" type="button" class='fileReset' value="Clear" onclick="resetFileUpload()" /><br />
                <asp:RegularExpressionValidator ID="revFileUpload"
                    runat="server" ErrorMessage="Use an approved file type (.doc, .docx, .pdf, .txt)" Display="Dynamic"
                    ControlToValidate="fluResume" CssClass="requiredEntry"
                    ValidationExpression="(.*?)\.(txt|TXT|doc|DOC|docx|DOCX|pdf|PDF)$" />
            </td>
        </tr>

        <tr>
            <td style="text-align: center">
                <asp:Button runat="server" ID="btnUpload" class='submitButton' UseSubmitBehavior="True" Text="Upload" OnClick="btnUpload_Click"  />
            </td>
        </tr>
    </thead>
</table>
    </div>
<div id="divSubmitted" runat="server">

    Thanks for submitting your resume! 
    <br />
    When a job opens up that’s right for you, we’ll contact you right away with details. 

</div>

<script type="text/javascript">

    function resetFileUpload(){
        document.getElementById("<%= fluResume.ClientID %>").value = null;
        Page_ClientValidate()
        fileChanged();
    }

    function fileChanged() {
        var fileName = document.getElementById("<%= fluResume.ClientID %>").value;
        document.getElementById('txtFileName').value = fileName;
    }


    $(document).ready(function () {
        $(':input').inputWatermark();
<%--        //Make my watermarks look nice.  Remove all data
        document.getElementById("<%= txtName.ClientID %>").value = "";
        document.getElementById("<%= txtEmail.ClientID %>").value = "";
        document.getElementById("<%= txtPhone.ClientID %>").value = "";
        document.getElementById('txtFileName').value = "";--%>

    });

    (function ($) {
        $.fn.extend({
            inputWatermark: function () {
                return this.each(function () {
                    // retrieve the value of the ‘placeholder’ attribute
                    var watermarkText = $(this).attr('placeholder');
                    var $this = $(this);
                    if ($this.val() === '') {
                        $this.val(watermarkText);
                        // give the watermark a translucent look
                        $this.css({ 'opacity': '0.65' });
                    }

                    $this.blur(function () {
                        if ($this.val() === '') {
                            // If the text is empty put the watermark
                            // back

                            $this.val(watermarkText);
                            // give the watermark a translucent look
                            $this.css({ 'opacity': '0.65' });
                        }
                    });

                    $this.focus(function () {
                        if ($this.val() === watermarkText) {
                            $this.val('');
                            $this.css({ 'opacity': '1.0' });
                        }
                    });
                });
            }
        });

    })(jQuery);

</script>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployerControl.ascx.cs" Inherits="CareerCenter.Controls.EmployerControl" %>

<div id="divEntry" style="width:100%;" runat="server">
<table class='controlTable'>
    <thead>
        <tr>
            <td class='controlCell'>
                <span class="redIndicator">*</span>
                <asp:TextBox runat="server" ID="txtName" class='controlItem' placeholder="Your Name"></asp:TextBox>

                <asp:RequiredFieldValidator ID="rfvtxtName" runat="server" ControlToValidate="txtName" CssClass="required" Display="Dynamic" ErrorMessage="Please enter Your Name."></asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td class='controlCell'>
                <span class="redIndicator">*</span>
                <asp:TextBox runat="server" ID="txtCompany" class='controlItem' placeholder="Company"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvtxtCompany" runat="server" ControlToValidate="txtCompany" CssClass="required" Display="Dynamic" ErrorMessage="Please enter Company."></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class='controlCell'>
                <span class="redIndicator">*</span>
                <asp:TextBox runat="server" ID="txtEmail" class='controlItem' placeholder="E-mail"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtEmail" ControlToValidate="txtEmail" CssClass="required" Display="Dynamic" ErrorMessage="Please enter Email."></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator
                    ID="regtxtEmail"
                    ControlToValidate="txtEmail"
                    Text="Please enter email in correct format."
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    runat="server" Display="Dynamic" CssClass="required"/>
            </td>
        </tr>
        <tr>
            <td class='controlCell'>
                <span class="redIndicator">*</span>
                <asp:TextBox runat="server" ID="txtPhone" class='controlItem' placeholder="Phone Number"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtPhone" ControlToValidate="txtPhone" CssClass="required" Display="Dynamic" ErrorMessage="Please enter Phone."></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revtxtPhone"
                                                runat="server" ErrorMessage="Please enter phone in a standard format." Display="Dynamic"
                                                 ControlToValidate="txtPhone" CssClass="required"
                                                ValidationExpression="^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$" />
                    
            </td>
        </tr>
        <tr>
            <td class='controlCell'>
                <span class="redIndicator">*</span>
                <asp:TextBox runat="server" ID="txtJobTitle" class='controlItem' placeholder="Job To Fill"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtJobtitle" Display="Dynamic" CssClass="required" ControlToValidate="txtJobTitle" ErrorMessage="Please enter Job Title."></asp:RequiredFieldValidator>
            </td>
        </tr>

        <tr>
            <td style="text-align: center"><br />
                <asp:Button runat="server" ID="btnContactMe" class='submitButton' UseSubmitBehavior="True" Text="CONTACT ME" OnClick="btnContactMe_Click" />
            </td>
        </tr>
    </thead>
</table>
</div>

<div id="divSubmitted" runat="server">

    Thanks for your interest.  We look forward to working with you for your content needs!

</div>

    <script type="text/javascript">

        $(document).ready(function () {
            $(':input').inputWatermark();
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
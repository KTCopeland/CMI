using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace CareerCenter
{
    public class MailHandler
    {

        //Get the configuration so we will know how to send email
        string is_Mode = ConfigurationManager.AppSettings["SMTPMode"].ToLower();

        public bool SendEmail(string as_EMailRecipient, string as_Subject, string as_Body)
        {
            bool lb_Return = false;
            string ls_Error = "";

            if( SendEmail(as_EMailRecipient, as_Subject, as_Body, ref ls_Error))
            {
                lb_Return = true;
            }
            else
            {
                DataHandler.HandleError("MailHandler.SendEmail()", ls_Error, "Recipient = " + as_EMailRecipient);
            }

            return lb_Return;

        }

        public bool SendEmail(string as_EMailRecipient, string as_Subject, string as_Body, ref string as_Error)
        {
            bool lb_Return = false;
            MailMessage lo_Message = new MailMessage();

            try
            {
                //Gather SMTP Information
                string ls_Server = DataHandler.GetSetting("smtp_server_" + is_Mode);
                string ls_Pseudonym = DataHandler.GetSetting("email_pseudonym_" + is_Mode);
                string ls_FromAddress = DataHandler.GetSetting("smtp_account_" + is_Mode); 
                string ls_Password = DataHandler.GetSetting("smtp_credentials_" + is_Mode); //To Do: Need to encrpyt
                int li_Port = int.Parse(DataHandler.GetSetting("smtp_port_" + is_Mode));

                //Prepare MailMessage Instance for Submission
                lo_Message.Subject = as_Subject;
                lo_Message.Body = as_Body;
                lo_Message.From = new MailAddress(ls_FromAddress, ls_Pseudonym);
                string[] ls_Recipients = as_EMailRecipient.Replace(";",",").Replace(" ","").Split(',');
                for (int li_Loop = 0; li_Loop< ls_Recipients.Length; li_Loop++)
                {
                lo_Message.To.Add(new MailAddress(ls_Recipients[li_Loop].Trim())); 
                }
                lo_Message.IsBodyHtml = true;

                //SMTP Server Stuff
                //lo_Message.BodyEncoding = Encoding.UTF8;
                SmtpClient smtp = new SmtpClient(ls_Server); 
                smtp.Credentials = new System.Net.NetworkCredential(ls_FromAddress, ls_Password);
                smtp.Port = li_Port;
                smtp.EnableSsl = false;

                //Fire it off!
                smtp.Send(lo_Message);

                lb_Return = true;
            }
            catch (SmtpException ex)
            {
                lb_Return = false;
                as_Error = ex.Message;
            }
            finally
            {
                // Remove the pointer to the message object so the GC can close the thread.
                lo_Message.Dispose();
                lo_Message = null;
            }

            return lb_Return;
        }
    }
}
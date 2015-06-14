using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CareerCenter.Pages
{
    public partial class Manager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Timeout = 180; //Sets timeout for session to 180 minutes to keep authentication token alive for up to 3 hours...  We will handle actual timeout in the Encrpytion class.
            string ls_Session="";
            try
            {
                ls_Session = Session["Authentication"].ToString();
            }
            catch
            {
                Session.Add("Authentication","");
            }

            if (Encryption.ValidateToken(ls_Session) == "" || ls_Session =="")
            {
                //Token was no good...  Make user log in.
                divLogin.Visible = true;
                divMain.Visible = false;
            }
            else
            {
                //Token validated.  Allow user to continue.
                divLogin.Visible = false;
                divMain.Visible = true;
            }
        }

        protected void cmdLogin_Click(object sender, EventArgs e)
        {
            //KTC: Do this once to add users when there are none.
            //Encryption.CreateUser(txtUser.Text, txtPassword.Text, "changethisemail@change.me", true);

            string ls_Token = Encryption.ValidateUser(txtUser.Text,txtPassword.Text);
            if(ls_Token !="")
            {
                Session["Authentication"] = ls_Token;
                Response.Redirect("/pages/manager.aspx");
            }
            else
            {
                //Bad Login...  
            }
        }

        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            Session["Authentication"] = "";
            Response.Redirect("/pages/manager.aspx");
        }

        protected void lnkExportJobs_Click(object sender, EventArgs e)
        {
            //Time to export some data!
            string ls_SQL = "Select * from job";
            DataSet lo_Data = new DataSet();

            if(DataHandler.GetDatasetFromQuery(ref lo_Data, ls_SQL))
            {
                string ls_File = "";

                //We have a dataset that actually has some data in it.  Good first step!
                DataTable lo_Table = lo_Data.Tables[0]; //Peel the one table we need out of the dataset and give it a fancy name
                lo_Table.TableName = "Jobs";
                ls_File = DataHandler.Export(ref lo_Table); //Send the table on its journey into the unknown: "Life, not death, is the great adventure" - Sherwood Anderson

                if(ls_File!="")
                {
                    //Cool. We have a path that can be used to retrieve the file.  Excellent sign that all has gone well.
                    MailHandler lo_Mail = new MailHandler();
                    //Dang... Need to make a roundtrip to the database just to get the user's email address....  annoying :/
                    lo_Mail.SendEmail("todd.copeland@experis.com", "CMI Extract: Jobs", "<html>Hi Todd,<br/>The data you requested is available here: <a href = '" + ls_File + "'>" + ls_File + "</a><br/> Thanks!</html>");
                }


            }

        }
    }
}
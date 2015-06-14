using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CareerCenter.Pages
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ls_Session = "";
            try
            {
                ls_Session = Session["Authentication"].ToString();
            }
            catch
            {
                Session.Add("Authentication", "");
            }

            if (Encryption.ValidateToken(ls_Session) == "" || ls_Session == "")
            {
                Response.Redirect("Manager.aspx");
            }

            if(!IsPostBack)
            {
                txtUser.Text = Encryption.ValidateToken(ls_Session);
                spanMessage.InnerHtml = "";
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Manager.aspx");
        }

        protected void cmdApply_Click(object sender, EventArgs e)
        {
            //Make sure items are filled in
            if (txtUser.Text=="" || txtCurrent.Text=="" || txtNew.Text=="" || txtRepeat.Text=="")
            {
                spanMessage.InnerHtml = "All fields must be completed before you can proceed.";
                return;
            }
            //Make sure new passwords match
            if(txtNew.Text != txtRepeat.Text)
            {
                spanMessage.InnerHtml="New Password and Verification do not match.<br/>Passwords are case sensitive!";
                return;
            }

            //Validate the current password
            if(Encryption.ValidateUser(txtUser.Text, txtCurrent.Text)=="")
            {
                spanMessage.InnerHtml = "Unable to authenticate current password.<br/>Please modify and try again";
                return;
            }

            //Now generate hash and update record
            string ls_SQL = "Select * from appuser where appuser_name='" + txtUser.Text + "'";
            DataSet lo_Data = new DataSet();

            try
            {
                if (DataHandler.GetDatasetFromQuery(ref lo_Data, ls_SQL))
                {
                    string ls_Salt = lo_Data.Tables[0].Rows[0]["appuser_salt"].ToString();
                    string ls_Hash = Encryption.EncodePassword(txtNew.Text, ls_Salt);

                    string ls_Update = "update appuser set appuser_hash ='" + ls_Hash + "' where appuser_name='" + txtUser.Text + "'";

                    SqlCommand lo_Command = new SqlCommand();

                    DataHandler.InitializeCommandFromQuery(ref lo_Command, ls_Update);
                    lo_Command.ExecuteNonQuery();
                }
                else
                {
                    //Not able to retrieve data for this record.
                    spanMessage.InnerHtml = "Unable to retrieve user information.  Please contact your administrator if this problem persists.";
                    return;
                }
            }
            catch(Exception ex)
            {
                DataHandler.HandleError(ex);
                spanMessage.InnerHtml = "An error occurred:<br/>" + ex.ToString();
                return;
            }

            //If we made it this far, everything went as desired.
            spanMessage.InnerHtml = "Password updated successfully.";
            txtCurrent.Text = "";
            txtNew.Text = "";
            txtRepeat.Text = "";
        }
    }
}
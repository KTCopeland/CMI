using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CareerCenter.Pages
{
    public partial class EditUser : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                try
                {
                    string ls_User = Request.QueryString["u"];
                    string ls_SQL = "Select * from appuser where appuser_name ='" + ls_User + "'";
                    DataSet lo_data = new DataSet();
                    if (DataHandler.GetDatasetFromQuery(ref lo_data, ls_SQL))
                    {
                        txtName.Text = lo_data.Tables[0].Rows[0]["appuser_name"].ToString();
                        txtName.Enabled = false;
                        txtPassword.Text = "******";
                        txtPassword.Enabled = false;
                        txtEmail.Text = lo_data.Tables[0].Rows[0]["appuser_email"].ToString();
                        lblLastLogin.Text = lo_data.Tables[0].Rows[0]["appuser_lastlogin"].ToString();
                        lblFails.Text = lo_data.Tables[0].Rows[0]["appuser_fails"].ToString();
                        chkAvailable.Checked = (lo_data.Tables[0].Rows[0]["appuser_available"].ToString().ToUpper() == "TRUE");

                        cmdReset.Enabled = true;
                    }
                    else
                    {
                        //No record retrieved...
                        txtName.Text = "";
                        txtName.Enabled = true;
                        txtPassword.Text = "";
                        txtPassword.Enabled = true;
                        txtEmail.Text = "";
                        lblLastLogin.Text = "N/A";
                        lblFails.Text = "N/A";
                        cmdReset.Enabled = false;
                    }

                    lo_data.Dispose();
                }
                catch (Exception ex)
                {
                    DataHandler.HandleError(ex);
                }

            }
        }

        protected void cmdBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserManager.aspx");
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            string ls_SQL = "";
            //Decision to insert or update will be based on txtName.Enabled...
            //If true, then insert, else update
            if(!txtName.Enabled)
            {
                ls_SQL = "update appuser set appuser_email ='" + txtEmail.Text + "'";
                if(chkAvailable.Checked)
                {
                    //If the user is saved and available is checked, reset the failed attempts to 0.
                    //That way, if the user remembers the old password they can get in.  Otherwise, they will need to reset.
                    ls_SQL += ", appuser_available=1, appuser_fails=0 ";
                }
                else
                {
                    ls_SQL += ", appuser_available=0 ";
                }
                
                ls_SQL += " where appuser_name ='" + txtName.Text + "'";

                SqlCommand lo_Command = new SqlCommand();

                if (DataHandler.InitializeCommandFromQuery(ref lo_Command, ls_SQL))
                {
                    lo_Command.ExecuteNonQuery();
                }

                Response.Redirect("EditUser.aspx?u="+txtName.Text);

            }
            else
            {
                //This is the easier case since it will be handled on the back end.
                Encryption.CreateUser(txtName.Text, txtPassword.Text, txtEmail.Text, chkAvailable.Checked);
                Response.Redirect("EditUser.aspx?u=" + txtName.Text);
            }



        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            //1) Generate temporary password
            //2) Update user record
            //3) Send email to user to let them know


        }
    }
}
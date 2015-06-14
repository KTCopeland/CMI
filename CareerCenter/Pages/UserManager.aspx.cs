using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CareerCenter.Pages
{
    public partial class UserManager : System.Web.UI.Page
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

            Query lo_Query = new Query();
            ph_List.Controls.Add(new LiteralControl(lo_Query.GetUserList(false)));
        }

        protected void cmdBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Manager.aspx");
        }

        protected void cmdNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditUser.aspx");
        }
    }
}
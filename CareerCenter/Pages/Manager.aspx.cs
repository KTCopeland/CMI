using System;
using System.Collections.Generic;
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
            string ls_Session="";
            try
            {
                ls_Session = Session["Authentication"].ToString();
            }
            catch
            {
                Session.Add("Authentication","");
            }

            if (ls_Session == "")
            {
                divLogin.Visible = true;
                divMain.Visible = false;
            }
            else
            {
                divLogin.Visible = false;
                divMain.Visible = true;
            }
        }

        protected void cmdLogin_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Exper1$")
            {
                Session["Authentication"] = Guid.NewGuid().ToString();
                Response.Redirect("/pages/manager.aspx");
            }
        }

        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            Session["Authentication"] = "";
            Response.Redirect("/pages/manager.aspx");
        }
    }
}
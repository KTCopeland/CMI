using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CareerCenter.Pages
{
    public partial class JobManager : System.Web.UI.Page
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
                Query lo_Query = new Query();
                ph_List.Controls.Add(new LiteralControl(lo_Query.GetJobList()));
            }
        }

        protected void cmdNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditJob.aspx");
        }

        protected void cmdBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Manager.aspx");
        }

        protected void cmdView_Click(object sender, EventArgs e)
        {
            if (cmdView.Text.ToLower() == "show all jobs")
            {
                cmdView.Text = "Show Available Only";
                Query lo_Query = new Query();
                ph_List.Controls.Clear();
                ph_List.Controls.Add(new LiteralControl(lo_Query.GetJobList(false)));
            }
            else
            {
                cmdView.Text = "Show All Jobs";
                Query lo_Query = new Query();
                ph_List.Controls.Clear();
                ph_List.Controls.Add(new LiteralControl(lo_Query.GetJobList()));
            }
        }
    }
}
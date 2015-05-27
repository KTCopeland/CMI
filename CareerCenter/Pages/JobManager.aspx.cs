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
            Query lo_Query = new Query();
            ph_List.Controls.Add(new LiteralControl(lo_Query.GetJobList()));
        }

        protected void cmdNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditJob.aspx");
        }
    }
}
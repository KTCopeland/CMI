using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CareerCenter.Pages
{
    public partial class JobSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            phStyle.Controls.Add(new LiteralControl(DataHandler.GetSetting("style_job_search")));
        }
    }
}
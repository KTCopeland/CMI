using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CareerCenter.Controls;

namespace CareerCenter.Pages
{
    public partial class UploadResume : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            phStyle.Controls.Add(new LiteralControl(DataHandler.GetSetting("style_entry_form")));
        }
    }
}
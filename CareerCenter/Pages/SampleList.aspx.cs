using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CareerCenter.Pages
{
    public partial class SampleList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Query lo_Query = new Query();
            phSample.Controls.Add(new LiteralControl(lo_Query.GetSampleList()));
        }
    }
}
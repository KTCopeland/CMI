using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CareerCenter.Ajax
{
    public partial class JobQuery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ls_Keywords = "";
            string ls_PostalCode = "";
            string ls_Range = "";

            ls_Keywords = Request.Params["keywords"].ToString();
            ls_PostalCode = Request.Params["keywords"].ToString();
            ls_Range = Request.Params["keywords"].ToString();

            Query lo_Query = new Query();
            lo_Query.SearchKeywords(ls_Keywords);
            for (int li_Loop = 1; li_Loop<=lo_Query.Results.Count;li_Loop++)
            {
                Results.Controls.Add(new LiteralControl(lo_Query.Results[li_Loop]));
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CareerCenter.Controls;

namespace CareerCenter.Pages
{
    public partial class ShowJob : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int li_Job = 0;
            Job lo_job = new Job();
            try
            {
                li_Job = int.Parse(Request.Params["id"].ToString());
                lo_job.Retrieve(li_Job);
                ph_Left.Controls.Add(new LiteralControl(lo_job.GetJobDescription()));

                //Dynamically load an instance of JobSeeker.ascx and assign parameters
               JobSeeker lo_Control = (JobSeeker)Page.LoadControl("~/Controls/JobSeeker.ascx");
               lo_Control.Job_Id = li_Job;
               lo_Control.Source = "CMI";

               ph_Right.Controls.Add(lo_Control);
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
            }

        }
    }
}
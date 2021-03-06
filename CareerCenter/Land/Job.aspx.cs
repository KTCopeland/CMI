﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CareerCenter.Controls;

namespace CareerCenter.Land
{
    public partial class Job : System.Web.UI.Page
    {
        string is_URL = "";
        string is_Title = "";
        string is_Description = "Check out this great job on @CMIContentJobs and the Content Marketing Career Center!";
        StringBuilder lsb_Header = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            int li_Job = 0;
            CareerCenter.Job lo_job = new CareerCenter.Job();
            try
            {
                li_Job = int.Parse(Request.Params["id"].ToString());
                lo_job.Retrieve(li_Job);

                is_URL = @"http://app.contentmarketinginstitute.careers/job_" + li_Job.ToString();
                is_Title = lo_job.Job_Title;

                ph_Left.Controls.Add(new LiteralControl(lo_job.GetJobDescription()));

                //Dynamically load an instance of JobSeeker.ascx and assign parameters
                JobSeeker lo_Control = (JobSeeker)Page.LoadControl("~/Controls/JobSeeker.ascx");
                lo_Control.Job_Id = li_Job;
                lo_Control.Source = "CMI";
                ph_Right.Controls.Add(lo_Control);

                //So Far so Good.  Now add the header information
                lsb_Header.Append("<title>CareerCenter – Powered by Experis | " + lo_job.Job_Title + "</title>\r\n");
                //lsb_Header.Append("<meta name='description' content='Powered by Experis' />");
                //og:url, og:image, og:title, og:description
                lsb_Header.Append("<meta property='og:title' content='" + lo_job.Job_Title.Replace("<","").Replace(">","").Replace("'","`") + "' /> \r\n");
                lsb_Header.Append("<meta property='og:description' content='" + is_Description + "' />\r\n");
                lsb_Header.Append(@"<meta property='og:url' content='http://app.contentmarketinginstitute.careers/job_" + lo_job.Job_ID.ToString() + "' />\r\n");
                lsb_Header.Append("<meta property='og:image' content='http://app.contentmarketinginstitute.careers/Land/cmi-logo.png' />\r\n");

                ph_Header.Controls.Add(new LiteralControl(lsb_Header.ToString()));

            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
            }

        }

        public string Get_URL()
        {
            return is_URL;
        }

        public string Get_Title()
        {
            return is_Title;
        }

        public string Get_Description()
        {
            return is_Description;
        }
    }
}
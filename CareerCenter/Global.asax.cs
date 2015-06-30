using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace CareerCenter
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Capture and rewrite url as needed
            //string ls_Request = Request.
            string ls_Path;
            string ls_NewPath;
            int li_Start = 0;
            int li_Job = 0;

            ls_Path = Request.Url.ToString().ToLowerInvariant();

            //If there is a trailing slash, yank it out of there.
            ls_Path = ls_Path.TrimEnd('/');

            //Parse the incoming url and behave accordingly
            li_Start = ls_Path.IndexOf("/job_");
            if (li_Start >=0)
            {
                try
                {
                    li_Job = int.Parse(ls_Path.Substring(li_Start + 5));

                    ls_NewPath = "/Land/job.aspx?id=" + li_Job.ToString();
                    Context.RewritePath(ls_NewPath);
                }
                catch
                {
                    Response.Redirect("http://www.contentmarketinginstitute.careers");
                }


        }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
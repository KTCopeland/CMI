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
            //Testing only.  Remove this once finished
            //MailHandler lo_Mail = new MailHandler();
            //string ls_Error = "";
            //lo_Mail.SendEmail("ktcopeland@gmail.com", "Experis Career Center", "Time to move up!", "Hi Todd,\r\nThanks for stopping by!", ref ls_Error);

            //Candidate lo_Candidate = new Candidate();
            //lo_Candidate.Candidate_Name = "Herman Munster";
            //lo_Candidate.Candidate_Phone = "804.247.0952";
            //lo_Candidate.Candidate_Email = "todd.copeland@experis.com";
            //lo_Candidate.Candidate_Grade = "A";
            //lo_Candidate.Job_ID = 1;
            //lo_Candidate.Candidate_Source = "CMI";
            //lo_Candidate.Create();

            //lo_Candidate = new Candidate();
            //lo_Candidate.Candidate_Name = "Gomez Adams";
            //lo_Candidate.Candidate_Phone = "804.247.0952";
            //lo_Candidate.Candidate_Email = "todd.copeland@experis.com";
            //lo_Candidate.Candidate_Grade = "A";
            //lo_Candidate.Candidate_Source = "CMI";
            //lo_Candidate.Create();

            //Employer lo_Employer = new Employer();
            //lo_Employer.Employer_Company_Name = "Spacely Sprockets";
            //lo_Employer.Employer_Contact_Name = "Howard Spacely";
            //lo_Employer.Employer_Contact_Title = "Chief Executive Officer";
            //lo_Employer.Employer_Contact_Email = "todd.copeland@experis.com";
            //lo_Employer.Employer_Contact_Phone = "804.867-5309";
            //lo_Employer.Employer_Available = true;
            //lo_Employer.Create();

            //Job lo_Job = new Job();
            //lo_Job.Employer_ID = 8;
            //lo_Job.Job_Title = "Orangutan Trainer ";
            //lo_Job.Job_Description = "This role is key to our business.  We are leveraging orangutans to fill vacant seats in congress since they have proven to be more effective on multiple fronts.  The Orangutan Trainer will be responsible for instructing new orangutan recruits on how to push a button for voting purposes.  The Oragutan trainer will also be responsible for teaching oragutans to not accept bananas from special interest and lobbying groups.";
            //lo_Job.Job_City = "Chicago";
            //lo_Job.Job_Territory = "Illinois";
            //lo_Job.Job_Postal_Code = "60604";
            //lo_Job.Job_Country = "United States of America";
            //lo_Job.Job_Active_Date = DateTime.Parse("04/01/2015");
            //lo_Job.Job_Inactive_Date = DateTime.Parse("09/01/2015");
            //lo_Job.Job_Remote = false;
            //lo_Job.Job_Available = true;
            //lo_Job.Create();


        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;

namespace CareerCenter
{
    public class Query
    {

        #region Instance Variables
        DataSet io_Jobs;
        string[] is_Parameters;
        Dictionary<int, string> io_Results = new Dictionary<int, string>();
        
        #endregion

        public Query()
        {
            
        }

        bool LoadJobs()
        {
            io_Jobs = new DataSet();
            return DataHandler.GetDatasetFromQuery(ref io_Jobs,"Select *, -1 distance from vw_job_active order by job_id desc");
        }

        bool LoadJobs(string as_PostalCode)
        {
            io_Jobs = new DataSet();
            return DataHandler.GetDatasetFromQuery(ref io_Jobs, "Select * from vw_job_distance where postal_code='" + as_PostalCode + "' order by distance asc");
        }

        public int SearchKeywords(string as_Parameters)
        {
            return SearchKeywords(as_Parameters, "");
        }

        public int SearchKeywords(string as_Parameters, string as_PostalCode)
        {
            int li_Return = 0;

            try
            {
                if (as_PostalCode=="")
                {
                    LoadJobs();
                }
                else
                {
                    LoadJobs(as_PostalCode);
                }

                string ls_Compare = "";
                int li_Counter = 0;
                int li_MaxLength = int.Parse(DataHandler.GetSetting("job_summary_length"));
                StringBuilder lsb_JobSummary = new StringBuilder();
                is_Parameters = as_Parameters.Split(' ');

                for(int li_Loop=0;li_Loop<io_Jobs.Tables[0].Rows.Count;li_Loop++)
                {
                    for (int li_Loop2 = 0;li_Loop2<is_Parameters.Length;li_Loop2++)
                    {
                        //Get the data to compare against the keywords
                        try
                        {
                            ls_Compare = io_Jobs.Tables[0].Rows[li_Loop]["job_title"].ToString() + io_Jobs.Tables[0].Rows[li_Loop]["job_description"].ToString();
                        }
                        catch (Exception ex)
                        {
                            //Data could not be cast as string.  Set to EmptyString, report, and move on.
                            ls_Compare = "";
                            DataHandler.HandleError(ex);
                        }

                        if (ls_Compare.ToLower().Contains(is_Parameters[li_Loop2].ToLower()))
                        {
                            //We found a match.  Create a summary and throw it on the stack
                            li_Counter++;

                            lsb_JobSummary.Clear();
                            lsb_JobSummary.Append("<div id='SummaryWrapper_" + li_Counter.ToString() + "' class ='SummaryWrapper'>");
                            lsb_JobSummary.Append("<hr/>");
                            lsb_JobSummary.Append("<div class ='jobTitle' ><a href='" + @"/job_" + io_Jobs.Tables[0].Rows[li_Loop]["job_id"].ToString() +"' target ='_parent'>" + WebUtility.HtmlEncode(io_Jobs.Tables[0].Rows[li_Loop]["job_title"].ToString()) + "</a></div>");
                            //KTC: 06/02/2015: Add indicator to show job term.  Valid values are {C,T,H} for {Contract, Contract to Hire, and Direct Hire} respectively
                            switch (io_Jobs.Tables[0].Rows[li_Loop]["job_term"].ToString().ToUpper())
                            {
                                case "C":
                                    {
                                        lsb_JobSummary.Append("<div class='jobTerm termContract' title='Contract Position'>C</div>");
                                        break;
                                    }
                                case "T":
                                    {
                                        lsb_JobSummary.Append("<div class='jobTerm termCTH' title='Contract to Hire Position'>T</div>");
                                        break;
                                    }
                                case "H":
                                    {
                                        lsb_JobSummary.Append("<div class='jobTerm termHire' title='Direct Hire Position'>H</div>");
                                        break;
                                    }
                                default:
                                    {
                                        //We have a value that doesn't meet one of the criteria.  Leave blank for now.
                                        break;
                                    }
                            }
                            lsb_JobSummary.Append("<div class='dummy'></div>");
                            lsb_JobSummary.Append("<div class='jobLocation'><b>" + WebUtility.HtmlEncode(io_Jobs.Tables[0].Rows[li_Loop]["job_city"].ToString()) + ", " + WebUtility.HtmlEncode(io_Jobs.Tables[0].Rows[li_Loop]["job_territory"].ToString()) + "</b></div>");
                            
                            //KTC:06/02/2015: Add indicator if this position can be worked remotely
                            if (io_Jobs.Tables[0].Rows[li_Loop]["job_remote"].ToString() == "1" || io_Jobs.Tables[0].Rows[li_Loop]["job_remote"].ToString().ToUpper() == "TRUE")
                            {
                                lsb_JobSummary.Append("<div class='remotePosition' title='Remote work permitted'>Remote</div>");
                            }

                            //KTC:06/03/2015 - Show distance if a postal code was provided
                            int li_Distance = int.Parse(io_Jobs.Tables[0].Rows[li_Loop]["distance"].ToString());
                            if (li_Distance >= 0)
                            {
                                if (li_Distance != 1)
                                {
                                    lsb_JobSummary.Append("<div class='jobDistance' >" + string.Format("{0:n0}", li_Distance) + " miles</div>");
                                }
                                else
                                {
                                    lsb_JobSummary.Append("<div class='jobDistance' >1 mile</div>");
                                }
                            }
                            
                            lsb_JobSummary.Append("<div class='dummy'></div>");

                            if (io_Jobs.Tables[0].Rows[li_Loop]["job_description"].ToString().Length > li_MaxLength)
                            {
                                lsb_JobSummary.Append("<span class='jobDescription'>" + io_Jobs.Tables[0].Rows[li_Loop]["job_description"].ToString().Substring(0,li_MaxLength) + "...&nbsp;"); //Note: This should already be html safe
                            }
                            else
                            {
                                lsb_JobSummary.Append("<span class='jobDescription'>" + io_Jobs.Tables[0].Rows[li_Loop]["job_description"].ToString() + "&nbsp;");
                            }

                            lsb_JobSummary.Append("<a class='learnMore' href='" + @"/pages/ShowJob.aspx?id=" + io_Jobs.Tables[0].Rows[li_Loop]["job_id"].ToString() + "'> Learn More <img class='arrowRight' src='/images/arrow-right.png'/></a></span>");
                            lsb_JobSummary.Append("</div>");
                            io_Results.Add(li_Counter, lsb_JobSummary.ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
            }

            return li_Return;
        }

        public string GetSampleList()
        {
            StringBuilder lsb_Return = new StringBuilder();
            string ls_SQL = "select top 5 * from vw_job_active order by job_id desc";
            DataSet lo_Jobs = new DataSet();

            //lsb_Return.Append("Open Jobs<br/>");
            lsb_Return.Append("<ul>");
            try
            {
                if (DataHandler.GetDatasetFromQuery(ref lo_Jobs, ls_SQL))
                {
                    for (int li_Loop = 0; li_Loop < lo_Jobs.Tables[0].Rows.Count; li_Loop++)
                    {
                        //No longer merely sending users to the job search page.  We can send them to the landing page for a specific job now.
                        //lsb_Return.Append("<li><a class='learnMore' href='" + @"http://contentmarketinginstitute.careers/find-a-job/'>" + lo_Jobs.Tables[0].Rows[li_Loop]["job_title"].ToString() + "</a><br/>" + lo_Jobs.Tables[0].Rows[li_Loop]["job_city"].ToString() + ", " + lo_Jobs.Tables[0].Rows[li_Loop]["job_territory"].ToString() + "</li>");
                        lsb_Return.Append("<li><a class='learnMore' href='" + @"/job_" + lo_Jobs.Tables[0].Rows[li_Loop]["job_id"].ToString() + "' target='_parent'>" + lo_Jobs.Tables[0].Rows[li_Loop]["job_title"].ToString() + "</a><br/>" + lo_Jobs.Tables[0].Rows[li_Loop]["job_city"].ToString() + ", " + lo_Jobs.Tables[0].Rows[li_Loop]["job_territory"].ToString() + "</li>");
                    }

                }
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
            }

            lsb_Return.Append("</ul>");
            lo_Jobs.Dispose();

            return lsb_Return.ToString();
        }

        public string GetJobList()
        {
            return GetJobList(true, "");
        }

        public string GetJobList(bool ab_ActiveOnly)
        {
            return GetJobList(ab_ActiveOnly, "");
        }

        public string GetJobList(bool ab_ActiveOnly, string as_OrderBy)
        {
            StringBuilder lsb_Return = new StringBuilder();
            string ls_View = "vw_job_";

            if (ab_ActiveOnly)
            {
                ls_View += "active";
            }
            else
            {
                ls_View += "all";
            }

            string ls_SQL = "select * from " + ls_View;

            if (as_OrderBy.Trim()!="")
            {
                ls_SQL += " order by " + as_OrderBy;
            }

            DataSet lo_Jobs = new DataSet();

            lsb_Return.Append("<table class='jobManagerList'>");
            lsb_Return.Append("<tr class='topTableRow'>");
            lsb_Return.Append("<td>ID</td>");
            lsb_Return.Append("<td>Job Title</td>");
            lsb_Return.Append("<td>Location</td>");
            lsb_Return.Append("<td>Employer</td>");
            lsb_Return.Append("<td>Active Date</td>");
            lsb_Return.Append("<td>Inactive Date</td>");
            lsb_Return.Append("<td>Time Stamp</td>");
            lsb_Return.Append("<td>Fox Code</td>");
            lsb_Return.Append("<td>Available</td>");
            lsb_Return.Append("</tr>");
            try
            {
                if (DataHandler.GetDatasetFromQuery(ref lo_Jobs, ls_SQL))
                {
                    for (int li_Loop = 0; li_Loop < lo_Jobs.Tables[0].Rows.Count; li_Loop++)
                    {
                        lsb_Return.Append("<tr>");

                        lsb_Return.Append("<td>");
                        lsb_Return.Append("<a href='" + @"/pages/EditJob.aspx?id=" + lo_Jobs.Tables[0].Rows[li_Loop]["job_id"].ToString() + "'>" + lo_Jobs.Tables[0].Rows[li_Loop]["job_id"].ToString().PadLeft(5, '0') + "</a>");
                        lsb_Return.Append("</td>");

                        lsb_Return.Append("<td>");
                        lsb_Return.Append("<a href='" + @"/pages/EditJob.aspx?id=" + lo_Jobs.Tables[0].Rows[li_Loop]["job_id"].ToString() + "'>" + lo_Jobs.Tables[0].Rows[li_Loop]["job_title"].ToString()+"</a>");
                        lsb_Return.Append("</td>");

                        lsb_Return.Append("<td>");
                        lsb_Return.Append(lo_Jobs.Tables[0].Rows[li_Loop]["job_city"].ToString() + ", " + lo_Jobs.Tables[0].Rows[li_Loop]["job_territory"].ToString());
                        lsb_Return.Append("</td>");

                        lsb_Return.Append("<td>");
                        lsb_Return.Append(lo_Jobs.Tables[0].Rows[li_Loop]["employer_company_name"].ToString());
                        lsb_Return.Append("</td>");

                        lsb_Return.Append("<td>");
                        lsb_Return.Append(((DateTime)lo_Jobs.Tables[0].Rows[li_Loop]["job_active_date"]).ToString("MM/dd/yyyy"));
                        lsb_Return.Append("</td>");

                        lsb_Return.Append("<td>");
                        lsb_Return.Append(((DateTime)lo_Jobs.Tables[0].Rows[li_Loop]["job_inactive_date"]).ToString("MM/dd/yyyy"));
                        lsb_Return.Append("</td>");

                        lsb_Return.Append("<td>");
                        lsb_Return.Append(((DateTime)lo_Jobs.Tables[0].Rows[li_Loop]["job_timestamp"]).ToString("MM/dd/yyyy"));
                        lsb_Return.Append("</td>");

                        lsb_Return.Append("<td>");
                        string ls_FoxCode = lo_Jobs.Tables[0].Rows[li_Loop]["job_foxcode"].ToString();
                        if (ls_FoxCode != "")
                        {
                            lsb_Return.Append(@"<a target = '_blank' href ='http://fox.experis.us/xp/jobDetails.do?job_uk=" + ls_FoxCode.ToUpper().Replace("J","").Replace("-","").Replace(" ","") + "'>");
                            lsb_Return.Append(lo_Jobs.Tables[0].Rows[li_Loop]["job_foxcode"].ToString());
                            lsb_Return.Append("</a>");
                        }
                        lsb_Return.Append("</td>");

                        lsb_Return.Append("<td>");
                        lsb_Return.Append(lo_Jobs.Tables[0].Rows[li_Loop]["job_available"].ToString());
                        lsb_Return.Append("</td>");

                        lsb_Return.Append("</tr>");

                    }

                }
                lsb_Return.Append("</table>");
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
            }

            lsb_Return.Append("</ul>");
            lo_Jobs.Dispose();

            return lsb_Return.ToString();
        }


        public string GetCandidateList(bool ab_ActiveOnly)
        {
            return GetCandidateList(ab_ActiveOnly, "");
        }

        public string GetCandidateList(bool ab_ActiveOnly, string as_Where)
        {
            StringBuilder lsb_Return = new StringBuilder();

            string ls_SQL = "Select * from vw_candidate";

            if (ab_ActiveOnly)
            {
                ls_SQL += "_active";
            }
            else
            {
                ls_SQL += "_all";
            }

            if (as_Where!= "")
            {
                ls_SQL += " where " + as_Where;
            }

            DataSet lo_Candidates = new DataSet();

            lsb_Return.Append("<table class='candidateManagerList'>");
            lsb_Return.Append("<tr class='topTableRow'>");
            lsb_Return.Append("<td>ID</td>");
            lsb_Return.Append("<td>Candidate</td>");
            lsb_Return.Append("<td>Email</td>");
            lsb_Return.Append("<td>Phone</td>");
            lsb_Return.Append("<td>Resume</td>");
            lsb_Return.Append("<td>Job ID</td>");
            lsb_Return.Append("<td>Job Title</td>");
            lsb_Return.Append("</tr>");

            if (DataHandler.GetDatasetFromQuery(ref lo_Candidates, ls_SQL + " order by candidate_id desc"))
            {
                for (int li_Loop = 0; li_Loop < lo_Candidates.Tables[0].Rows.Count; li_Loop++)
                {
                    lsb_Return.Append("<tr>");

                    lsb_Return.Append("<td>");
                    lsb_Return.Append(lo_Candidates.Tables[0].Rows[li_Loop]["candidate_id"].ToString().PadLeft(5, '0'));
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("<td>");
                    lsb_Return.Append(lo_Candidates.Tables[0].Rows[li_Loop]["candidate_name"].ToString());
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("<td>");
                    lsb_Return.Append("<a href='mailto:" + lo_Candidates.Tables[0].Rows[li_Loop]["candidate_email"].ToString() + "'>" + lo_Candidates.Tables[0].Rows[li_Loop]["candidate_email"].ToString() +"</a>");
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("<td>");
                    if (lo_Candidates.Tables[0].Rows[li_Loop]["candidate_phone"] != DBNull.Value)
                    {
                    lsb_Return.Append(lo_Candidates.Tables[0].Rows[li_Loop]["candidate_phone"].ToString());
                    }
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("<td>");
                    if (lo_Candidates.Tables[0].Rows[li_Loop]["candidate_file"] != DBNull.Value)
                    {
                        lsb_Return.Append("<a href='" + @"/cv/resumes/" + lo_Candidates.Tables[0].Rows[li_Loop]["candidate_file"].ToString() + "'>Resume</a>");
                    }
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("<td>");
                    if (lo_Candidates.Tables[0].Rows[li_Loop]["job_id"] != DBNull.Value)
                    {
                        lsb_Return.Append("<a href='" + @"/pages/EditJob.aspx?id=" + lo_Candidates.Tables[0].Rows[li_Loop]["job_id"].ToString() + "'>" + lo_Candidates.Tables[0].Rows[li_Loop]["job_id"].ToString().PadLeft(5, '0') + "</a>");
                    }
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("<td>");
                    if (lo_Candidates.Tables[0].Rows[li_Loop]["job_title"] != DBNull.Value)
                    {
                        lsb_Return.Append("<a href='" + @"/pages/EditJob.aspx?id=" + lo_Candidates.Tables[0].Rows[li_Loop]["job_id"].ToString() + "'>" + lo_Candidates.Tables[0].Rows[li_Loop]["job_title"].ToString() + "</a>");
                    }
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("</tr>");

                }

            }

            return lsb_Return.ToString();
        }


        public string GetEmployerList(bool ab_ActiveOnly)
        {
            return GetEmployerList(ab_ActiveOnly, "");
        }

        public string GetEmployerList(bool ab_ActiveOnly, string as_Where)
        {
            StringBuilder lsb_Return = new StringBuilder();

            string ls_SQL = "Select * from vw_employer";

            if (ab_ActiveOnly)
            {
                ls_SQL += "_active";
            }
            else
            {
                ls_SQL += "_all";
            }

            if (as_Where != "")
            {
                ls_SQL += " where " + as_Where;
            }

            DataSet lo_Employers = new DataSet();
            //employer_id, employer_company_name, employer_contact_name, employer_contact_email, employer_contact_phone, employer_contact_title, employer_available
            lsb_Return.Append("<table class='employerManagerList'>");
            lsb_Return.Append("<tr class='topTableRow'>");
            lsb_Return.Append("<td>ID</td>");
            lsb_Return.Append("<td>Employer</td>");
            lsb_Return.Append("<td>Contact Name</td>");
            lsb_Return.Append("<td>Email</td>");
            lsb_Return.Append("<td>Phone</td>");
            lsb_Return.Append("<td>Job Title</td>");
            lsb_Return.Append("<td>Available</td>");
            lsb_Return.Append("</tr>");

            if (DataHandler.GetDatasetFromQuery(ref lo_Employers, ls_SQL + " order by employer_id desc"))
            {
                for (int li_Loop = 0; li_Loop < lo_Employers.Tables[0].Rows.Count; li_Loop++)
                {
                    lsb_Return.Append("<tr>");

                    lsb_Return.Append("<td>");
                    lsb_Return.Append(lo_Employers.Tables[0].Rows[li_Loop]["employer_id"].ToString().PadLeft(5, '0'));
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("<td>");
                    lsb_Return.Append(lo_Employers.Tables[0].Rows[li_Loop]["employer_company_name"].ToString());
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("<td>");
                    lsb_Return.Append(lo_Employers.Tables[0].Rows[li_Loop]["employer_contact_name"].ToString());
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("<td>");
                    lsb_Return.Append("<a href='mailto:" + lo_Employers.Tables[0].Rows[li_Loop]["employer_contact_email"].ToString() + "'>" + lo_Employers.Tables[0].Rows[li_Loop]["employer_contact_email"].ToString() + "</a>");
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("<td>");
                    if (lo_Employers.Tables[0].Rows[li_Loop]["employer_contact_phone"] != DBNull.Value)
                    {
                        lsb_Return.Append(lo_Employers.Tables[0].Rows[li_Loop]["employer_contact_phone"].ToString());
                    }
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("<td>");
                    if (lo_Employers.Tables[0].Rows[li_Loop]["employer_contact_title"] != DBNull.Value)
                    {
                        lsb_Return.Append(lo_Employers.Tables[0].Rows[li_Loop]["employer_contact_title"].ToString());
                    }
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("<td>");
                    lsb_Return.Append(lo_Employers.Tables[0].Rows[li_Loop]["employer_available"].ToString());
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("</tr>");

                }

            }

            return lsb_Return.ToString();
        }

        public string GetUserList(bool ab_ActiveOnly)
        {
            return GetUserList(ab_ActiveOnly, "");
        }

        public string GetUserList(bool ab_ActiveOnly, string as_Where)
        {
            StringBuilder lsb_Return = new StringBuilder();

            string ls_SQL = "Select * from vw_appuser";

            if (ab_ActiveOnly)
            {
                ls_SQL += "_active";
            }
            else
            {
                ls_SQL += "_all";
            }

            if (as_Where != "")
            {
                ls_SQL += " where " + as_Where;
            }

            DataSet lo_Users = new DataSet();
            //
            lsb_Return.Append("<table class='userManagerList'>");
            lsb_Return.Append("<tr class='topTableRow'>");
            lsb_Return.Append("<td>User Name</td>");
            lsb_Return.Append("<td>Email</td>");
            lsb_Return.Append("<td>Last Login</td>");
            lsb_Return.Append("<td>Failed Attempts</td>");
            lsb_Return.Append("<td>Available</td>");
            lsb_Return.Append("</tr>");

            if (DataHandler.GetDatasetFromQuery(ref lo_Users, ls_SQL + " order by appuser_name asc"))
            {
                for (int li_Loop = 0; li_Loop < lo_Users.Tables[0].Rows.Count; li_Loop++)
                {
                    lsb_Return.Append("<tr>");

                    lsb_Return.Append("<td>");
                    lsb_Return.Append("<a href='EditUser.aspx?u=" + lo_Users.Tables[0].Rows[li_Loop]["appuser_name"].ToString() + "'>" + lo_Users.Tables[0].Rows[li_Loop]["appuser_name"].ToString() + "</a>");
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("<td>");
                    lsb_Return.Append("<a href='mailto:" + lo_Users.Tables[0].Rows[li_Loop]["appuser_email"].ToString() + "'>" + lo_Users.Tables[0].Rows[li_Loop]["appuser_email"].ToString() + "</a>");
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("<td>");
                    lsb_Return.Append(lo_Users.Tables[0].Rows[li_Loop]["appuser_lastlogin"].ToString());
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("<td>");
                    lsb_Return.Append(lo_Users.Tables[0].Rows[li_Loop]["appuser_fails"].ToString());
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("<td>");
                    lsb_Return.Append(lo_Users.Tables[0].Rows[li_Loop]["appuser_available"].ToString());
                    lsb_Return.Append("</td>");

                    lsb_Return.Append("</tr>");
                }

            }

            return lsb_Return.ToString();
        }

        public Dictionary<int, string> Results
        {
            get
            {
                return io_Results;
            }
        }


    }

}
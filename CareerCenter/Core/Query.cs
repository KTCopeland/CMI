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
            LoadJobs();
        }

        public Query(string as_Parameters)
        {
            LoadJobs();
            SearchKeywords(as_Parameters);
        }


        bool LoadJobs()
        {
            io_Jobs = new DataSet();
            return DataHandler.GetDatasetFromQuery(ref io_Jobs,"Select * from vw_job_active");
        }

        public int SearchKeywords(string as_Parameters)
        {
            int li_Return = 0;

            try
            {
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
                            lsb_JobSummary.Append("<p><span class ='jobTitle' ><a href='" + @"/pages/ShowJob.aspx?id=" + io_Jobs.Tables[0].Rows[li_Loop]["job_id"].ToString() +"'>" + WebUtility.HtmlEncode(io_Jobs.Tables[0].Rows[li_Loop]["job_title"].ToString()) + "</a></span><br/>");
                            lsb_JobSummary.Append("<span class='jobLocation'><b>" + WebUtility.HtmlEncode(io_Jobs.Tables[0].Rows[li_Loop]["job_city"].ToString()) + ", " + WebUtility.HtmlEncode(io_Jobs.Tables[0].Rows[li_Loop]["job_territory"].ToString()) + "</b></span><br/>");
                            if (io_Jobs.Tables[0].Rows[li_Loop]["job_description"].ToString().Length > li_MaxLength)
                            {
                                lsb_JobSummary.Append("<span class='jobDescription'>" + io_Jobs.Tables[0].Rows[li_Loop]["job_description"].ToString().Substring(0,li_MaxLength) + "...&nbsp;"); //Note: This should already be html safe
                            }
                            else
                            {
                                lsb_JobSummary.Append("<span class='jobDescription'>" + io_Jobs.Tables[0].Rows[li_Loop]["job_description"].ToString() + "&nbsp;");
                            }

                            lsb_JobSummary.Append("<a class='learnMore' href='" + @"/pages/ShowJob.aspx?id=" + io_Jobs.Tables[0].Rows[li_Loop]["job_id"].ToString() + "'> Learn More <img class='arrowRight' src='/images/arrow-right.png'/></a></span></p>");
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
            string ls_SQL = "select top 5 * from vw_job_active";
            DataSet lo_Jobs = new DataSet();

            lsb_Return.Append("Open Jobs<br/>");
            lsb_Return.Append("<ul>");
            try
            {
                if (DataHandler.GetDatasetFromQuery(ref lo_Jobs, ls_SQL))
                {
                    for (int li_Loop = 0; li_Loop < lo_Jobs.Tables[0].Rows.Count; li_Loop++)
                    {
                        lsb_Return.Append("<li><a class='learnMore' href='" + @"/pages/ShowJob.aspx?id=" + lo_Jobs.Tables[0].Rows[li_Loop]["job_id"].ToString() + "'>" + lo_Jobs.Tables[0].Rows[li_Loop]["job_title"].ToString() + "</a><br/>" + lo_Jobs.Tables[0].Rows[li_Loop]["job_city"].ToString() + ", " + lo_Jobs.Tables[0].Rows[li_Loop]["job_territory"].ToString() + "</li>");
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

        public Dictionary<int, string> Results
        {
            get
            {
                return io_Results;
            }
        }

    }

}
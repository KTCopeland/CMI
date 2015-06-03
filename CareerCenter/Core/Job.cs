using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Web;

namespace CareerCenter
{
    public class Job
    {
        #region Instance Variables

        int ii_Job_ID = DataHandler.IntNull;
        int ii_Employer_ID = DataHandler.IntNull;
        string is_Job_Title = DataHandler.StringNull;
        string is_Job_Description = DataHandler.StringNull;
        string is_Job_City = DataHandler.StringNull;
        string is_Job_Territory = DataHandler.StringNull;
        string is_Job_Postal_Code = DataHandler.StringNull;
        string is_Job_Country = DataHandler.StringNull;
        string is_Job_Term = DataHandler.StringNull;
        DateTime idt_Job_Active_Date = DataHandler.DateTimeNull;
        DateTime idt_Job_Inactive_Date = DataHandler.DateTimeNull;
        bool ib_Job_Remote = false; //default to not remote
        bool ib_Job_Available = false; //default to job not available

        string is_DBProc = "usp_job";
        string is_DBTable = "job";

        #endregion

        #region Properties

        public int Job_ID
        {
            get
            {
                return ii_Job_ID;
            }
        }

        public int Employer_ID
        {
            get
            {
                return ii_Employer_ID;
            }
            set
            {
                ii_Employer_ID = value;
            }
        }

        public string Job_Title
        {
            get
            {
                return is_Job_Title;
            }
            set
            {
                if (value.Length > 100)
                {
                    is_Job_Title = value.Substring(0, 100);
                }
                else
                {
                    is_Job_Title = value;
                }
            }
        }

        public string Job_Description
        {
            get
            {
                return is_Job_Description;
            }
            set
            {
                //KTC: 06/02/2015 - Changed this to not restrict size.   Database can support text up to 1Gb now.
                //if (value.Length > 4000)
                //{
                //    is_Job_Description = value.Substring(0, 4000);
                //}
                //else
                //{
                    is_Job_Description = value;
                //}
            }
        }

        public string Job_City
        {
            get
            {
                return is_Job_City;
            }
            set
            {
                if (value.Length > 100)
                {
                    is_Job_City = value.Substring(0, 100);
                }
                else
                {
                    is_Job_City = value;
                }
            }
        }

        public string Job_Territory
        {
            get
            {
                return is_Job_Territory;
            }
            set
            {
                if (value.Length > 100)
                {
                    is_Job_Territory = value.Substring(0, 100);
                }
                else
                {
                    is_Job_Territory = value;
                }
            }
        }

        public string Job_Postal_Code
        {
            get
            {
                return is_Job_Postal_Code;
            }
            set
            {
                if (value.Length > 50)
                {
                    is_Job_Postal_Code = value.Substring(0, 50);
                }
                else
                {
                    is_Job_Postal_Code = value;
                }
            }
        }

        public string Job_Country
        {
            get
            {
                return is_Job_Country;
            }
            set
            {
                if (value.Length > 100)
                {
                    is_Job_Country = value.Substring(0, 100);
                }
                else
                {
                    is_Job_Country = value;
                }
            }
        }

        public string Job_Term
        {
            get
            {
                return is_Job_Term;
            }
            set
            {
                if (value.Length > 10)
                {
                    is_Job_Term = value.Substring(0, 10);
                }
                else
                {
                    is_Job_Term = value;
                }
            }
        }

        public DateTime Job_Active_Date
        {
            get
            {
                return idt_Job_Active_Date;
            }
            set
            {
                idt_Job_Active_Date = value;
            }
        }

        public DateTime Job_Inactive_Date
        {
            get
            {
                return idt_Job_Inactive_Date;
            }
            set
            {
                idt_Job_Inactive_Date = value;
            }
        }

        public bool Job_Remote
        {
            get
            {
                return ib_Job_Remote;
            }
            set
            {
                ib_Job_Remote = value;
            }
        }

        public bool Job_Available
        {
            get
            {
                return ib_Job_Available;
            }
            set
            {
                ib_Job_Available = value;
            }
        }

        #endregion

        #region Methods

        public bool Create()
        {
            bool lb_Return = false;
            try
            {
                SqlCommand lo_Command = new SqlCommand();
                if (DataHandler.InitializeCommandFromProcedure(ref lo_Command, is_DBProc))
                {
                    int li_ID;
                    int li_Result_ID;
                    string ls_Result_Msg;
                    
                    DataHandler.SetVal(ref lo_Command, "@Action", "INSERT");
                    DataHandler.SetVal(ref lo_Command, "@job_id", DataHandler.IntNull);
                    DataHandler.SetVal(ref lo_Command, "@employer_id", ii_Employer_ID);
                    DataHandler.SetVal(ref lo_Command, "@job_title", is_Job_Title);
                    DataHandler.SetVal(ref lo_Command, "@job_description", is_Job_Description);
                    DataHandler.SetVal(ref lo_Command, "@job_city", is_Job_City);
                    DataHandler.SetVal(ref lo_Command, "@job_territory", is_Job_Territory);
                    DataHandler.SetVal(ref lo_Command, "@job_postal_code", is_Job_Postal_Code);
                    DataHandler.SetVal(ref lo_Command, "@job_country", is_Job_Country);
                    DataHandler.SetVal(ref lo_Command, "@job_term", is_Job_Term);
                    DataHandler.SetVal(ref lo_Command, "@job_active_date", idt_Job_Active_Date);
                    DataHandler.SetVal(ref lo_Command, "@job_inactive_date", idt_Job_Inactive_Date);
                    DataHandler.SetVal(ref lo_Command, "@job_remote", ib_Job_Remote);
                    DataHandler.SetVal(ref lo_Command, "@job_available", ib_Job_Available);

                    lo_Command.ExecuteNonQuery();

                    li_ID = int.Parse(lo_Command.Parameters["@job_id"].Value.ToString());
                    li_Result_ID = int.Parse(lo_Command.Parameters["@Result_ID"].Value.ToString());
                    ls_Result_Msg = lo_Command.Parameters["@Result_Msg"].Value.ToString();

                    lo_Command.Dispose();

                    if (li_Result_ID < 0)
                    {
                        DataHandler.HandleError("Job.Create()", ls_Result_Msg, "Job = " + this.Job_Title);
                    }
                    else
                    {
                        ii_Job_ID = li_ID;
                        lb_Return = true;
                    }
                }
                else
                {
                    DataHandler.HandleError("Job.Create()", "Unable to create command object", "Job = " + this.Job_Title);
                }
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
            }

            return lb_Return;
        }

        public bool Retrieve(int ai_Job)
        {
            bool lb_Return = false;
            string ls_SQL = "Select * from " + is_DBTable + " where job_id = " + ai_Job.ToString();
            DataSet lo_Data = new DataSet();

            try
            {
                if (DataHandler.GetDatasetFromQuery(ref lo_Data, ls_SQL))
                {
                    this.ii_Job_ID = ai_Job;
                    DataHandler.GetVal(ref this.ii_Employer_ID, lo_Data.Tables[0].Rows[0]["employer_id"]);
                    DataHandler.GetVal(ref this.is_Job_Title, lo_Data.Tables[0].Rows[0]["job_title"]);
                    DataHandler.GetVal(ref this.is_Job_Description, lo_Data.Tables[0].Rows[0]["job_description"]);
                    DataHandler.GetVal(ref this.is_Job_City, lo_Data.Tables[0].Rows[0]["job_city"]);
                    DataHandler.GetVal(ref this.is_Job_Territory, lo_Data.Tables[0].Rows[0]["job_territory"]);
                    DataHandler.GetVal(ref this.is_Job_Postal_Code, lo_Data.Tables[0].Rows[0]["job_postal_code"]);
                    DataHandler.GetVal(ref this.is_Job_Country, lo_Data.Tables[0].Rows[0]["job_country"]);
                    DataHandler.GetVal(ref this.is_Job_Term, lo_Data.Tables[0].Rows[0]["job_term"]);
                    DataHandler.GetVal(ref this.idt_Job_Active_Date, lo_Data.Tables[0].Rows[0]["job_active_date"]);
                    DataHandler.GetVal(ref this.idt_Job_Inactive_Date, lo_Data.Tables[0].Rows[0]["job_inactive_date"]);
                    DataHandler.GetVal(ref this.ib_Job_Remote, lo_Data.Tables[0].Rows[0]["job_remote"]);
                    DataHandler.GetVal(ref this.ib_Job_Available, lo_Data.Tables[0].Rows[0]["job_available"]);

                    lo_Data.Dispose();
                    //Made it to the end without an error  Should be good to go
                    lb_Return = true;
                }
                else
                {
                    DataHandler.HandleError("Job.Retrieve()", "Retrieve failed", "GetDatasetFromQuery(" + ai_Job.ToString() + ")");
                }
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
            }

            return lb_Return;
        }

        public bool Update()
        {
            bool lb_Return = false;
            try
            {
                SqlCommand lo_Command = new SqlCommand();
                if (DataHandler.InitializeCommandFromProcedure(ref lo_Command, is_DBProc))
                {
                    int li_Result_ID;
                    string ls_Result_Msg;

                    DataHandler.SetVal(ref lo_Command, "@Action", "UPDATE");
                    DataHandler.SetVal(ref lo_Command, "@job_id",ii_Job_ID);
                    DataHandler.SetVal(ref lo_Command, "@employer_id", ii_Employer_ID);
                    DataHandler.SetVal(ref lo_Command, "@job_title", is_Job_Title);
                    DataHandler.SetVal(ref lo_Command, "@job_description", is_Job_Description);
                    DataHandler.SetVal(ref lo_Command, "@job_city", is_Job_City);
                    DataHandler.SetVal(ref lo_Command, "@job_territory", is_Job_Territory);
                    DataHandler.SetVal(ref lo_Command, "@job_postal_code", is_Job_Postal_Code);
                    DataHandler.SetVal(ref lo_Command, "@job_country", is_Job_Country);
                    DataHandler.SetVal(ref lo_Command, "@job_country", is_Job_Term);
                    DataHandler.SetVal(ref lo_Command, "@job_active_date", idt_Job_Active_Date);
                    DataHandler.SetVal(ref lo_Command, "@job_inactive_date", idt_Job_Inactive_Date);
                    DataHandler.SetVal(ref lo_Command, "@job_remote", ib_Job_Remote);
                    DataHandler.SetVal(ref lo_Command, "@job_available", ib_Job_Available);

                    lo_Command.ExecuteNonQuery();

                    li_Result_ID = int.Parse(lo_Command.Parameters["@Result_ID"].Value.ToString());
                    ls_Result_Msg = lo_Command.Parameters["@Result_Msg"].Value.ToString();

                    lo_Command.Dispose();

                    if (li_Result_ID < 0)
                    {
                        DataHandler.HandleError("Job.Update()", ls_Result_Msg, "Job_ID = " + this.ii_Job_ID.ToString());
                    }
                    else
                    {
                        lb_Return = true;
                    }
                }
                else
                {
                    DataHandler.HandleError("Job.Update()", "Unable to create command object", "Job_ID = " + this.ii_Job_ID.ToString());
                }
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
            }

            return lb_Return;
        }

        public bool Delete()
        {
            bool lb_Return = false;
            try
            {
                SqlCommand lo_Command = new SqlCommand();
                if (DataHandler.InitializeCommandFromProcedure(ref lo_Command, is_DBProc))
                {
                    int li_Result_ID;
                    string ls_Result_Msg;

                    lo_Command.Parameters["@Action"].Value = "DELETE";
                    lo_Command.Parameters["@job_id"].Value = this.ii_Job_ID;

                    lo_Command.ExecuteNonQuery();

                    li_Result_ID = int.Parse(lo_Command.Parameters["@Result_ID"].Value.ToString());
                    ls_Result_Msg = lo_Command.Parameters["@Result_Msg"].Value.ToString();

                    lo_Command.Dispose();

                    if (li_Result_ID < 0)
                    {
                        DataHandler.HandleError("Job.Delete()", ls_Result_Msg, "Job_ID = " + this.ii_Job_ID.ToString());
                    }
                    else
                    {
                        lb_Return = true;
                    }
                }
                else
                {
                    DataHandler.HandleError("Job.Delete()", "Unable to create command object", "Employer_ID = " + this.ii_Job_ID.ToString());
                    lb_Return = false;
                }
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
            }

            return lb_Return;
        }

        public string GetJobDescription()
        {
            StringBuilder lsb_Return = new StringBuilder();

            try
            {
                lsb_Return.Append("<div class='jobWrapper'>");
                lsb_Return.Append("<div class='jobHeader'>" + WebUtility.HtmlEncode(this.Job_Title) + "</div>");
                //KTC: 06/02/2015: Add indicator to show job term.  Valid values are {C,T,H} for {Contract, Contract to Hire, and Direct Hire} respectively
                switch(this.Job_Term.ToUpper())
                {
                    case "C":
                        {
                            lsb_Return.Append("<div class='jobTerm termContract' title='Contract Position'>C</div>");
                            break;
                        }
                    case "T":
                        {
                            lsb_Return.Append("<div class='jobTerm termCTH' title='Contract to Hire Position'>T</div>");
                            break;
                        }
                    case "H":
                        {
                            lsb_Return.Append("<div class='jobTerm termHire' title='Direct Hire Position'>H</div>");
                            break;
                        }
                    default:
                        {
                            //We have a value that doesn't meet one of the criteria.  Leave blank for now.
                            break;
                        }
                }

                lsb_Return.Append("<div class='dummy'></div>");

                lsb_Return.Append("<div class='jobLocation'>" + WebUtility.HtmlEncode(this.Job_City + ", " + this.Job_Territory) + "</div>");
                //KTC:06/02/2015: Add indicator if this position can be worked remotely
                if (this.Job_Remote)
                {
                    lsb_Return.Append("<div class='remotePosition' title='Remote work permitted'>Remote</div>");
                }

                lsb_Return.Append("<div class='dummy'></div>");

                lsb_Return.Append("<div class='jobDetail'>" + this.Job_Description + "</div>");
                lsb_Return.Append("</div>");


            }
            catch(Exception ex)
            {
                DataHandler.HandleError(ex);
                lsb_Return.Clear();
                lsb_Return.Append("Unable to retrieve information for this job.  ID = " + this.Job_ID.ToString());
            }

            return lsb_Return.ToString();
        }

        #endregion
    }
}
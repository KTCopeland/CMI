using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace CareerCenter
{
    public class Candidate
    {

        #region Instance Variables

        int ii_Candidate_ID = DataHandler.IntNull;
        int ii_Job_ID = DataHandler.IntNull;
        string is_Candidate_Name = DataHandler.StringNull;
        string is_Candidate_Email = DataHandler.StringNull;
        string is_Candidate_Phone = DataHandler.StringNull;
        string is_Candidate_Grade = DataHandler.StringNull;
        string is_Candidate_Source = DataHandler.StringNull;
        string is_Candidate_File = DataHandler.StringNull;

        string is_DBProc = "usp_candidate";
        string is_DBTable = "candidate";

        #endregion

        #region Properties

        public int Candidate_ID
        {
            get
            {
                return ii_Candidate_ID;
            }
        }

        public int Job_ID
        {
            get
            {
                return ii_Job_ID;
            }
            set
            {
                ii_Job_ID = value;
            }
        }

        public string Candidate_Name
        {
            get
            {
                return is_Candidate_Name;
            }
            set
            {
                if (value.Length > 100)
                {
                    is_Candidate_Name = value.Substring(0, 100);
                }
                else
                {
                    is_Candidate_Name = value;
                }
            }
        }

        public string Candidate_Email
        {
            get
            {
                return is_Candidate_Email;
            }
            set
            {
                if (value.Length > 150)
                {
                    is_Candidate_Email = value.Substring(0, 150);
                }
                else
                {
                    is_Candidate_Email = value;
                }
            }
        }

        public string Candidate_Phone
        {
            get
            {
                return is_Candidate_Phone;
            }
            set
            {
                if (value.Length > 50)
                {
                    is_Candidate_Phone = value.Substring(0, 50);
                }
                else
                {
                    is_Candidate_Phone = value;
                }
            }
        }

        public string Candidate_Grade
        {
            get
            {
                return is_Candidate_Grade;
            }
            set
            {
                if (value.Length > 50)
                {
                    is_Candidate_Grade = value.Substring(0, 50);
                }
                else
                {
                    is_Candidate_Grade = value;
                }
            }
        }

        public string Candidate_Source
        {
            get
            {
                return is_Candidate_Source;
            }
            set
            {
                if (value.Length > 50)
                {
                    is_Candidate_Source = value.Substring(0, 50);
                }
                else
                {
                    is_Candidate_Source = value;
                }
            }
        }

        public string Candidate_File
        {
            get
            {
                return is_Candidate_File;
            }
            set
            {
                if (value.Length > 50)
                {
                    is_Candidate_File = value.Substring(0, 50);
                }
                else
                {
                    is_Candidate_File = value;
                }
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
                    DataHandler.SetVal(ref lo_Command, "@candidate_id", DataHandler.IntNull);
                    DataHandler.SetVal(ref lo_Command, "@job_id", ii_Job_ID);
                    DataHandler.SetVal(ref lo_Command, "@candidate_name", is_Candidate_Name);
                    DataHandler.SetVal(ref lo_Command, "@candidate_email", is_Candidate_Email);
                    DataHandler.SetVal(ref lo_Command, "@candidate_phone", is_Candidate_Phone);
                    DataHandler.SetVal(ref lo_Command, "@candidate_grade", is_Candidate_Grade);
                    DataHandler.SetVal(ref lo_Command, "@candidate_source", is_Candidate_Source);
                    DataHandler.SetVal(ref lo_Command, "@candidate_file", is_Candidate_File);

                    lo_Command.ExecuteNonQuery();

                    li_ID = int.Parse(lo_Command.Parameters["@candidate_id"].Value.ToString());
                    li_Result_ID = int.Parse(lo_Command.Parameters["@Result_ID"].Value.ToString());
                    ls_Result_Msg = lo_Command.Parameters["@Result_Msg"].Value.ToString();

                    lo_Command.Dispose();

                    if (li_Result_ID < 0)
                    {
                        DataHandler.HandleError("Candidate.Create()", ls_Result_Msg, "Candidate = " + this.Candidate_Name);
                    }
                    else
                    {
                        ii_Candidate_ID = li_ID;

                        //Resume Submission Response
                        if (DataHandler.GetSetting("candidate_response_enabled").ToUpper() == "TRUE" && this.Job_ID <= 0)
                        {
                            string[] ls_Name = this.Candidate_Name.Split(' ');
                            string ls_Body =  DataHandler.GetSetting("candidate_response_text").Replace("{NAME}",ls_Name[0]).Replace("{EMAIL_HEADER}",DataHandler.GetSetting("email_header"));
                            DataHandler.SendEmail(this.Candidate_Email, "Thank you for your submission", ls_Body);
                        }

                        //Resume Submission Notification
                        if (DataHandler.GetSetting("candidate_notification_enabled").ToUpper() == "TRUE" && this.Job_ID <= 0)
                        {
                            StringBuilder lo_CandidateInfo = new StringBuilder();
                            lo_CandidateInfo.Append("Name: " + this.is_Candidate_Name + "<br/>");
                            lo_CandidateInfo.Append("Email: " + this.is_Candidate_Email + "<br/>");
                            lo_CandidateInfo.Append("Phone: " + this.is_Candidate_Phone + "<br/>");

                            string ls_Body = DataHandler.GetSetting("candidate_notification_text").Replace("{CANDIDATE_DETAIL}",lo_CandidateInfo.ToString()).Replace("{EMAIL_HEADER}",DataHandler.GetSetting("email_header"));
                            DataHandler.SendEmail(DataHandler.GetSetting("candidate_notification_recipients"), "New Resume Submission", ls_Body);
                        }

                        //Job Submission Response
                        if (DataHandler.GetSetting("job_response_enabled").ToUpper() == "TRUE" && this.Job_ID > 0)
                        {
                            string[] ls_Name = this.Candidate_Name.Split(' ');
                            Job lo_Job = new Job();
                            if (lo_Job.Retrieve(this.Job_ID))
                            {
                                string ls_Body = DataHandler.GetSetting("job_response_text").Replace("{NAME}", ls_Name[0]).Replace("{JOB_TITLE}",lo_Job.Job_Title).Replace("{EMAIL_HEADER}",DataHandler.GetSetting("email_header"));
                                DataHandler.SendEmail(this.Candidate_Email, "Thank you for applying", ls_Body);
                            }
                        }

                        //Job Submission Notification
                        if (DataHandler.GetSetting("job_notification_enabled").ToUpper() == "TRUE" && this.Job_ID > 0)
                        {
                            Job lo_Job = new Job();
                            if (lo_Job.Retrieve(this.Job_ID))
                            {
                                Employer lo_Employer = new Employer();

                                StringBuilder lo_CandidateInfo = new StringBuilder();
                                lo_CandidateInfo.Append("Name: " + this.is_Candidate_Name + "<br/>");
                                lo_CandidateInfo.Append("Email: " + this.is_Candidate_Email + "<br/>");
                                lo_CandidateInfo.Append("Phone: " + this.is_Candidate_Phone + "<br/>");

                                StringBuilder lo_JobInfo = new StringBuilder();
                                lo_JobInfo.Append("Job Title:" + lo_Job.Job_Title + "<br/>");
                                lo_JobInfo.Append("Job_Location:" + lo_Job.Job_City + "," + lo_Job.Job_Territory + " " + lo_Job.Job_Postal_Code + " | " + lo_Job.Job_Country + "Remote Allowed: " );
                                if(lo_Job.Job_Remote)
                                {
                                    lo_JobInfo.Append("Yes");
                                }
                                else
                                {
                                    lo_JobInfo.Append("No");
                                }
                                lo_JobInfo.Append("<br/>");
                                lo_Employer.Retrieve(lo_Job.Employer_ID);
                                lo_JobInfo.Append("Company: " + lo_Employer.Employer_Company_Name + "<br/>");
                                lo_JobInfo.Append("Contact: " + lo_Employer.Employer_Contact_Name + ", " + lo_Employer.Employer_Contact_Title + "<br/>");
                                lo_JobInfo.Append("Contact Info: " + lo_Employer.Employer_Contact_Email + " | " + lo_Employer.Employer_Contact_Phone + "<br/>");
                                string ls_Body = DataHandler.GetSetting("job_notification_text").Replace("{CANDIDATE_DETAIL}", lo_CandidateInfo.ToString()).Replace("{JOB_DETAIL}",lo_JobInfo.ToString()).Replace("{EMAIL_HEADER}",DataHandler.GetSetting("email_header"));
                                DataHandler.SendEmail(DataHandler.GetSetting("job_notification_recipients"), "New Job Submission", ls_Body);
                            }
                        }

                        lb_Return = true;
                    }
                }
                else
                {
                    DataHandler.HandleError("Candidate.Create()", "Unable to create command object", "Candidate = " + this.Candidate_Name);
                }
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
            }
            
            return lb_Return;
        }

        public bool Retrieve(int ai_Candidate)
        {
            bool lb_Return = false;
            string ls_SQL = "Select * from " + is_DBTable + " where candidate_id = " + ai_Candidate.ToString();
            DataSet lo_Data = new DataSet();

            try
            {
                if (DataHandler.GetDatasetFromQuery(ref lo_Data, ls_SQL))
                {
                    this.ii_Candidate_ID = ai_Candidate;
                    DataHandler.GetVal(ref this.ii_Job_ID, lo_Data.Tables[0].Rows[0]["job_id"]);
                    DataHandler.GetVal(ref this.is_Candidate_Name, lo_Data.Tables[0].Rows[0]["candidate_name"]);
                    DataHandler.GetVal(ref this.is_Candidate_Email, lo_Data.Tables[0].Rows[0]["candidate_email"]);
                    DataHandler.GetVal(ref this.is_Candidate_Phone, lo_Data.Tables[0].Rows[0]["candidate_phone"]);
                    DataHandler.GetVal(ref this.is_Candidate_Grade, lo_Data.Tables[0].Rows[0]["candidate_grade"]);
                    DataHandler.GetVal(ref this.is_Candidate_Source, lo_Data.Tables[0].Rows[0]["candidate_source"]);
                    DataHandler.GetVal(ref this.is_Candidate_File, lo_Data.Tables[0].Rows[0]["candidate_file"]);
                    lo_Data.Dispose();
                    //Made it to the end without an error  Should be good to go
                    lb_Return = true;
                }
                else
                {
                    DataHandler.HandleError("Candidate.Retrieve()", "Retrieve failed", "GetDatasetFromQuery(" + ai_Candidate.ToString() + ")");
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
                    DataHandler.SetVal(ref lo_Command, "@candidate_id", ii_Candidate_ID);
                    DataHandler.SetVal(ref lo_Command, "@job_id", ii_Job_ID);
                    DataHandler.SetVal(ref lo_Command, "@candidate_name", is_Candidate_Name);
                    DataHandler.SetVal(ref lo_Command, "@candidate_email", is_Candidate_Email);
                    DataHandler.SetVal(ref lo_Command, "@candidate_phone", is_Candidate_Phone);
                    DataHandler.SetVal(ref lo_Command, "@candidate_grade", is_Candidate_Grade);
                    DataHandler.SetVal(ref lo_Command, "@candidate_source", is_Candidate_Source);
                    DataHandler.SetVal(ref lo_Command, "@candidate_file", is_Candidate_File);

                    lo_Command.ExecuteNonQuery();

                    li_Result_ID = int.Parse(lo_Command.Parameters["@Result_ID"].Value.ToString());
                    ls_Result_Msg = lo_Command.Parameters["@Result_Msg"].Value.ToString();

                    lo_Command.Dispose();

                    if (li_Result_ID < 0)
                    {
                        DataHandler.HandleError("Candidate.Update()", ls_Result_Msg, "Candidate_ID = " + this.Candidate_ID.ToString());
                    }
                    else
                    {
                        lb_Return = true;
                    }
                }
                else
                {
                    DataHandler.HandleError("Candidate.Update()", "Unable to create command object", "Candidate_ID = " + this.Candidate_ID.ToString());
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
                    lo_Command.Parameters["@candidate_id"].Value = this.ii_Candidate_ID;

                    lo_Command.ExecuteNonQuery();

                    li_Result_ID = int.Parse(lo_Command.Parameters["@Result_ID"].Value.ToString());
                    ls_Result_Msg = lo_Command.Parameters["@Result_Msg"].Value.ToString();

                    lo_Command.Dispose();

                    if (li_Result_ID < 0)
                    {
                        DataHandler.HandleError("Candidate.Delete()", ls_Result_Msg, "Candidate_ID = " + this.ii_Candidate_ID.ToString());
                    }
                    else
                    {
                        lb_Return = true;
                    }
                }
                else
                {
                    DataHandler.HandleError("Candidate.Delete()", "Unable to create command object", "Candidate_ID = " + this.ii_Candidate_ID.ToString());
                    lb_Return = false;
                }
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
            }

            return lb_Return;
        }


        #endregion
    }
}
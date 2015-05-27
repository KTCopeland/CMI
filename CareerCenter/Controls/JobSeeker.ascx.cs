using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CareerCenter.Controls
{
    public partial class JobSeeker : System.Web.UI.UserControl
    {
        int ii_Job = DataHandler.IntNull;
        string is_Source = DataHandler.StringNull;


        #region Properties
        public int Job_Id
        {
            get
            {
                return ii_Job;
            }

            set
            {
                ii_Job = value;
            }
        }

        public string Source
        {
            get
            {
                return is_Source;
            }
            set
            {
                is_Source = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            divEntry.Visible = true;
            divSubmitted.Visible = false;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            var candidate = new Candidate();
            candidate.Candidate_Email = txtEmail.Text;
            candidate.Candidate_Name = txtName.Text;
            candidate.Candidate_Phone = txtPhone.Text;
            if (is_Source != DataHandler.StringNull)
            {
                candidate.Candidate_Source = is_Source;
            }
            else
            {
                candidate.Candidate_Source = "Unknown";
            }
            
            if (ii_Job != DataHandler.IntNull)
            {
                candidate.Job_ID = ii_Job;
            }

            var result = false;
            result = candidate.Create();
            
            //Ensure record saved in database and file is uploaded.
            if (result && fluResume.HasFile)
                
            {

                try
                {
                    // get file extension and append to candidate id to prepare file name
                    string filename = string.Format("{0}{1}", candidate.Candidate_ID.ToString(),
                        Path.GetExtension(fluResume.PostedFile.FileName));

                    // Get resume store location from configuration file.
                    string fileStoreFolder = ConfigurationManager.AppSettings["CandidateResumeFolderName"].ToString();
                    //Prepare resume store location.
                    string fileStorePath = string.Format(@"{0}\{1}\{2}", Server.MapPath(@"~\cv"), fileStoreFolder, filename);
                    

                    fluResume.SaveAs(fileStorePath);

                    divEntry.Visible = false;
                    divSubmitted.Visible = true;

                }
                catch (Exception fileException)
                {
                    DataHandler.HandleError(fileException); 
                }
                


            }
        }
    }
}
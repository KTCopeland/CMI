using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace CareerCenter
{
    public class Setting
    {
        Dictionary<string,string> io_Settings = new Dictionary<string,string>();

        public Setting()
        {
            Refresh();
        }

        public bool Refresh()
        {
            bool lb_Return = false;

            try
            {
                io_Settings.Clear();
                DataSet lo_Data = new DataSet();
                if (DataHandler.GetDatasetFromQuery(ref lo_Data, "Select * from setting"))
                {
                    for (int li_Loop = 0; li_Loop < lo_Data.Tables[0].Rows.Count; li_Loop++)
                    {
                        io_Settings.Add(lo_Data.Tables[0].Rows[li_Loop]["setting_id"].ToString(), lo_Data.Tables[0].Rows[li_Loop]["setting_value"].ToString());
                    }
                    lb_Return = true;
                }
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
            }

            return lb_Return;
        }

        public string GetSetting(string as_Key)
        {
            string ls_Return;

            try
            {
                ls_Return = io_Settings[as_Key];
            }
            catch (Exception ex)
            {
                ls_Return = "";
                DataHandler.HandleError(ex);
            }

            return ls_Return;

        }
    }
}
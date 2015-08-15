using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;
using ASH.Logging;
using System.IO;
using ASH.Business;
using ASH.DTO;

namespace WindowsServiceDemo
{
    public enum Status
    {
        created = 100,
        Moved = 150
    }
    public partial class Service1 : ServiceBase
    {
        #region Private Members
        private string interval;
        private string sourcePath = string.Empty;
        private string destPath = string.Empty;
        private Timer timer;
        BLOperations objBL = null;
        #endregion

        #region Constructor
        public Service1()
        {
            InitializeComponent();
            timer = new Timer();
            objBL = new BLOperations();
        }
        #endregion

        #region service
        protected override void OnStart(string[] args)
        {
            Log.LogMessage("Service Started.....");
            //loading Configurations..
            LoadConfigurations();
            timer.Interval = Convert.ToDouble(interval);
            timer.Enabled = true;
            Log.LogMessage("Calling timer_Elapsed");
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            Log.LogMessage("DoProcess started..");
            DoProcess();
            Log.LogMessage("DoProcess completed..");
            timer.Enabled = true;
        }

        public void DoProcess()
        {
            
            LoadConfigurations();
            string[] files = Directory.GetFiles(sourcePath);
            Log.LogMessage(string.Format("No of Files : {0}", files.Length.ToString()));
            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    //creating job with status 100
                    Log.LogMessage("creating the job..");
                    var jobnumber = CreateJob(file);
                    string filename = file.Substring(file.LastIndexOf("\\") + 1, file.Length - file.LastIndexOf("\\") - 1);
                    MoveFiles(filename);
                    Log.LogMessage("Updating the job..");
                    UpdateJob(jobnumber);
                }
            }
            else
            {
                Log.LogMessage("No files Found..");
            }

        }

        private void UpdateJob(long jobnumber)
        {
            DTOJob job = new DTOJob()
            {
               JobNumber = jobnumber,
               status = (int)Status.Moved,
               Path = destPath
            };
            try
            {
                objBL.UpdateJob(job);
                Log.LogMessage("job Updated.....");
                InsertIntoJobTracking(job);
            }
            catch (Exception ex)
            {
                Log.LogMessage("job updation failed");
                Log.LogMessage(ex.Message);
            }
        }

        private long CreateJob(string file)
        {
            DTOJob job = new DTOJob()
            {
                JobNumber = objBL.GetJobNumber(),
                status = (int)Status.created,
                Path = file
            };
            try
            {
                objBL.AddJob(job);
                Log.LogMessage("Job created....");
                InsertIntoJobTracking(job);
            }
            catch(Exception ex)
            {
                Log.LogMessage("job creation failed, Exception");
                Log.LogMessage(ex.Message);
            }
            return job.JobNumber;
        }

        private void InsertIntoJobTracking(DTOJob job)
        {
            DTOJobTracking jobTrack = new DTOJobTracking()
            {
                JobNumber = job.JobNumber,
                status = job.status,
                Path = job.Path
            };
            try
            {
                objBL.AddJobTracking(jobTrack);
            }
            catch (Exception ex)
            {
                Log.LogMessage("Add Job Tracking Failed");
                Log.LogMessage(ex.Message);
            }
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
            Log.LogMessage("Service Stopped.....");
        }

        private void LoadConfigurations()
        {
            Log.LogMessage("Loading the Configurations...");
            try
            {
                interval = string.IsNullOrEmpty(ConfigurationManager.AppSettings["Interval"]) ? "5000" : ConfigurationManager.AppSettings["Interval"];
                sourcePath = ConfigurationManager.AppSettings["SourcePath"] != null ? ConfigurationManager.AppSettings["SourcePath"].EndsWith("\\") ? ConfigurationManager.AppSettings["SourcePath"]
                    : ConfigurationManager.AppSettings["SourcePath"] + "\\" : "";
                destPath = ConfigurationManager.AppSettings["DestinationPath"] != null ? ConfigurationManager.AppSettings["DestinationPath"].EndsWith("\\") ? ConfigurationManager.AppSettings["DestinationPath"]
                    : ConfigurationManager.AppSettings["DestinationPath"] + "\\" : "";
            }
            catch (Exception ex)
            {
                Log.LogMessage("Loading Configurations failed..");
                Log.LogMessage(ex);
            }

        }
        private void MoveFiles(string file)
        {
            Log.LogMessage("File Movement Started..");
            try
            {
                string sourceFile = sourcePath + file;
                Log.LogMessage(string.Format("sourceFile: {0}", sourceFile));
                string destFile = destPath + file;
                Log.LogMessage(string.Format("destFile: {0}", destFile));
                File.Move(sourceFile, destFile);
                Log.LogMessage("File Movement Completed..");
            }
            catch (Exception ex)
            {
                Log.LogMessage("File Movement failed.....");
                Log.LogMessage(ex);
            }
        }
        #endregion
    }
}

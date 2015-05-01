using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace BaseLib
{
    public class AppFileHelper
    {
        #region Global declaration
        static string LoggerFileAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "InBoardPro\\Logger.txt");
        static string LoggerFileDesktop = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "InBoardPro\\Logger.txt");
        static string EmailFileDeskTop = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "InBoardPro\\ListOfEmails.txt"); 
        #endregion

        #region AddLoggerFile
        public static void AddLoggerFile(string Item)
        {
            try
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(Item, Globals.Path_LinkedinErrorLogs);
                GlobusFileHelper.AppendStringToTextfileNewLine(Item, Globals.DesktopFolder);
            }
            catch (Exception ex)
            {

            }
        } 
        #endregion

        #region AddingLinkedInDataToCSVFile
        public static void AddingLinkedInDataToCSVFile(string Data, string FileName)
        {
            try
            {
                string LinkedInAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "InBoardProGetData\\" + FileName + ".csv");
                string LinkedInDeskTop = Globals.DesktopFolder + "\\InBoardProGetData" + FileName + ".csv";

                #region LinkedIn Writer
                if (!File.Exists(LinkedInAppData))
                {
                    string Header = "ProfileType" + "," + "UserProfileLink" + "," + "ProfileID" + "," + "FirstName" + "," + "LastName" + "," + "HeadLineTitle" + "," + "CurrentTitle" + "," + "Current Company"  + "," + "Current Company Url" + "," + "Description of all Company" + "," + "Background - Summary" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedInLoginID" + "," + "AccountType" + ",";
                    GlobusFileHelper.AppendStringToTextfileNewLine(Header, LinkedInAppData);
                }

                //Checking File Exixtance
                if (!File.Exists(LinkedInDeskTop))
                {
                    string Header = "ProfileType" + "," + "UserProfileLink" + "," + "ProfileID" + "," + "FirstName" + "," + "LastName" + "," + "HeadLineTitle" + "," + "CurrentTitle" + "," + "Company" + "," + "Current Company Url" + "," + "Background - Summary" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedInLoginID" + "," + "AccountType" + ",";
                    GlobusFileHelper.AppendStringToTextfileNewLine(Header, LinkedInDeskTop);
                }

                if (!string.IsNullOrEmpty(Data))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Data, LinkedInDeskTop);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } 
        #endregion

        #region AddingLinkedInDataToCSVFile-CompanyEmployeeScraper
        public static void AddingLinkedInDataToCSVFileCompanyEmployeeScraper(string Data, string FileName)
        {
            try
            {
                string LinkedInAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "InBoardProGetData\\" + FileName + ".csv");
                string LinkedInDeskTop = Globals.DesktopFolder + "\\" + FileName + ".csv";

                #region LinkedIn Writer
                if (!File.Exists(LinkedInAppData))
                {
                    string Header = "ProfileType" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "HeadLineTitle" + "," + "CurrentTitle" + "," + "Company" + "," + "Current Company Url" + "," + "Description of all Company" + "," + "Background - Summary" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedInLoginID" + "," + "AccountType" + ",";
                    GlobusFileHelper.AppendStringToTextfileNewLine(Header, LinkedInAppData);
                }

                //Checking File Exixtance
                if (!File.Exists(LinkedInDeskTop))
                {
                    string Header = "ProfileType" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "HeadLineTitle" + "," + "CurrentTitle" + "," + "Company" + "," + "Current Company Url" + "," + "Background - Summary" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedInLoginID" + "," + "AccountType" + ",";
                    GlobusFileHelper.AppendStringToTextfileNewLine(Header, LinkedInDeskTop);
                }

                if (!string.IsNullOrEmpty(Data))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Data, LinkedInDeskTop);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region AddingLinkedInDataToCSVFileWithoutGoingToMainProfile
        public static void AddingLinkedInDataToCSVFileWithoutGoingToMainProfile(string Data, string FileName)
        {
            try
            {
                string LinkedInAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "InBoardProGetData\\" + FileName + ".csv");
                string LinkedInDeskTop = Globals.DesktopFolder  + "\\" + FileName + ".csv";

                #region LinkedIn Writer
                if (!File.Exists(LinkedInAppData))
                {
                    string Header = "FirstName" + "," + "LastName" + "," + "HeadlineTitle" + "," + "Location" + "," + "Industry" + "," + "CurrentTitle" + "," + "PastTitle" + "," + "DegreeOfConnection" + ",";
                    GlobusFileHelper.AppendStringToTextfileNewLine(Header, LinkedInAppData);
                }

                //Checking File Exixtance
                if (!File.Exists(LinkedInDeskTop))
                {
                    string Header = "FirstName" + "," + "LastName" + "," + "HeadlineTitle" + "," + "Location" + "," + "Industry" + "," + "CurrentTitle" + "," + "PastTitle" + "," + "DegreeOfConnection" + ",";
                    GlobusFileHelper.AppendStringToTextfileNewLine(Header, LinkedInDeskTop);
                }

                if (!string.IsNullOrEmpty(Data))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Data, LinkedInDeskTop);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region SalesNavigatorScraperWriteToCSV
        public static void SalesNavigatorScraperWriteToCSV(string Data, string Header, string FileName)
        {
            try
            {
                string LinkedInAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "InBoardProGetData\\" + FileName + ".csv");
                string LinkedInDeskTop = Globals.DesktopFolder + "\\" + FileName + ".csv";

                #region LinkedIn Writer
                if (!File.Exists(LinkedInAppData))
                {
                    
                    GlobusFileHelper.AppendStringToTextfileNewLine(Header, LinkedInAppData);
                }

                //Checking File Exixtance
                if (!File.Exists(LinkedInDeskTop))
                {
                    
                    GlobusFileHelper.AppendStringToTextfileNewLine(Header, LinkedInDeskTop);
                }

                if (!string.IsNullOrEmpty(Data))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Data, LinkedInDeskTop);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region AddingLinkedInGroupMemberDataToCSVFile
        public static void AddingLinkedInGroupMemberDataToCSVFile(string Data, string FileName)
        {
            try
            {
                string LinkedInAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "InBoardProGetDataGroupMember.csv");
                string LinkedInDeskTop = Globals.DesktopFolder + "\\InBoardProGetDataGroupMember.csv";

                #region LinkedIn Writer
                if (!File.Exists(LinkedInAppData))
                {
                    string Header = "ProfileType" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "HeadLineTitle" + "," + "CurrentTitle " + "," + "Company" + "," + "Description of all Company" + "," + "Background - Summary" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedInLoginID" + ",";
                    GlobusFileHelper.AppendStringToTextfileNewLine(Header, LinkedInAppData);
                }

                //Checking File Exixtance
                if (!File.Exists(LinkedInDeskTop))
                {
                    string Header = "ProfileType" + "," + "UserProfileLink" + "," + "FirstName" + "," + "LastName" + "," + "HeadLineTitle" + "," + "CurrentTitle " + "," + "Company" + "," + "Description of all Company" + "," + "Background - Summary" + "," + "Connection" + "," + "Recommendations " + "," + "SkillAndExpertise " + "," + "Experience " + "," + " Education" + "," + "Groups" + "," + "UserEmail" + "," + "UserContactNumber" + "," + "PastTitles" + "," + "PastCompany" + "," + "Location" + "," + "Country" + "," + "Industry" + "," + "WebSites" + "," + "LinkedInLoginID" + ",";
                    GlobusFileHelper.AppendStringToTextfileNewLine(Header, LinkedInDeskTop);
                }

                if (!string.IsNullOrEmpty(Data))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Data, LinkedInDeskTop);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } 
        #endregion

        #region AppendStringToTextFile
        public static void AppendStringToTextFile(string FilePath, string content)
        {
            using (StreamWriter sw = new StreamWriter(EmailFileDeskTop, true))
            {
                sw.WriteLine(content);
            }
        } 
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using InBoardPro;
using BaseLib.DB_Repository;

using System.Web;
using System.Net;
using Microsoft.Win32;

using Groups;
using InBoardProGetData;


using BaseLib;


namespace InBoardPro
{
    public class SalesNavigatorScraper
    {
        List<string> lstProfileUrls = new List<string>();
        public static Events loggerSalesNavigator = new Events();
        public void StartSalesavigatorScraper(ref GlobusHttpHelper objHttpHelper, string mainUrl)
        {
            try
            {
                //("[ " + DateTime.Now + " ] => [ Select The Email Id From The Dropdown and Fill All Mandatory (*) Fields ]");
                Pagination(ref objHttpHelper, mainUrl);
                ScrapeProfileDetails(ref objHttpHelper);
                try
                {
                    string[] profileUrls = lstProfileUrls.ToArray();
                    System.IO.File.WriteAllLines(Globals.profileUrlsSalesNavigatorScraper, profileUrls);
                }
                catch
                { }
            } 
            catch
            { }
        }

        public void Pagination(ref GlobusHttpHelper objHttpHelper,string mainUrl)
        {
            try
            {
                string totalResults = string.Empty;
                bool dispTotalResults = true;
                string mainPageResponse = string.Empty;
                int paginationCounter = 0;
                do
                {
                    //mainUrl = mainUrl.Replace("replaceVariableCounter", paginationCounter.ToString());
                    
                    if (SalesNavigatorGlobals.isStop)
                    {
                        return;
                    }
                    mainPageResponse = objHttpHelper.getHtmlfromUrl(new Uri(mainUrl.Replace("replaceVariableCounter", paginationCounter.ToString())));                  
                    
                    if (string.IsNullOrEmpty(mainPageResponse))
                    {
                        if (string.IsNullOrEmpty(mainPageResponse))
                        {
                            MessageBox.Show("Null response from internet. Please check your internet connection and restart the software.");
                            AddToLogger("[ " + DateTime.Now + " ] => [ No response from internet. Please check your internet connection. ] ");
                        }
                        Thread.Sleep(2000);
                        mainPageResponse = objHttpHelper.getHtmlfromUrl(new Uri(mainUrl.Replace("replaceVariableCounter", paginationCounter.ToString())));
                        
                    }
                    try
                    {
                        if (dispTotalResults)
                        {
                            totalResults = Utils.getBetween(mainPageResponse, "\"total\":", ",\"").Trim();
                            AddToLogger("[ " + DateTime.Now + " ] => [ Total results found : " + totalResults + " ]");
                            AddToLogger("[ " + DateTime.Now + " ] => [ Scraping profile Url ]");
                            dispTotalResults = false;
                        }
                    }
                    catch
                    { }
                    int checkCountUrls = 0;
                    string[] profileUrl_Split = Regex.Split(mainPageResponse, "\"profileUrl\"");
                    foreach (string profileUrlItem in profileUrl_Split)
                    {
                        if (!profileUrlItem.Contains("<!DOCTYPE"))
                        {
                            checkCountUrls++;
                            string profileUrl = Utils.getBetween(profileUrlItem, ":\"", "\",\"");
                            lstProfileUrls.Add(profileUrl);
                            AddToLogger("[ " + DateTime.Now + " ] => [ Scraped Url : " + profileUrl +" ] ");
                            
                        }                        
                    }
                    
                    paginationCounter = paginationCounter + 100; 
                } while (mainPageResponse.Contains("\"profileUrl\":\""));
            }
            catch (Exception ex)
            {
            }
        }

        public void ScrapeProfileDetails(ref GlobusHttpHelper objHttpHelper)
        {
            
            foreach (string profileURL in lstProfileUrls)
            {
                string name = string.Empty;
                string memberID = string.Empty;
                string imageUrl = string.Empty;
                string connection = string.Empty;
                string location = string.Empty;
                string industry = string.Empty;
                string headlineTitle = string.Empty;
                string currentTitle = string.Empty;
                string pastTitles = string.Empty;
                string currentCompany = string.Empty;
                string pastCompanies = string.Empty;
                string skills = string.Empty;
                string numberOfConnections = string.Empty;
                string education = string.Empty;
                string email = string.Empty;
                string phoneNumber = string.Empty;
                if (SalesNavigatorGlobals.isStop)
                {
                    return;
                }
                try
                {
                    AddToLogger("[ " + DateTime.Now + " ] => [ Scraping profile details of profile url : " + profileURL + " ]");

                    string pgSource = objHttpHelper.getHtmlfromUrl(new Uri(profileURL));
                    if (string.IsNullOrEmpty(pgSource))
                    {
                        pgSource = objHttpHelper.getHtmlfromUrl(new Uri(profileURL));
                    }
                    if (!pgSource.Contains("\"profile\":"))
                    {
                        Thread.Sleep(2000);
                        pgSource = objHttpHelper.getHtmlfromUrl(new Uri(profileURL));
                    }

                    name = GetName(pgSource);

                    memberID = Utils.getBetween(profileURL, "profile/", ",").Trim();

                    imageUrl = GetImageUrl(pgSource);

                    email = GetEmail(pgSource);

                    phoneNumber = GetPhoneNumber(pgSource);

                    connection = GetConnection(pgSource);

                    location = GetLocation(pgSource);

                    industry = GetIndustry(pgSource);

                    headlineTitle = GetHeadlineTitle(pgSource);

                    headlineTitle = headlineTitle.Replace("\\u002d", string.Empty);

                    string allTitles = GetAllTitle(pgSource).Replace("d/b/a", string.Empty).Replace("&amp;", string.Empty); //title at company : title at company : title at company

                    try
                    {
                        string[] titles = Regex.Split(allTitles, " : ");

                        currentTitle = Utils.getBetween(titles[0], "", " at ");

                        foreach (string item in titles)
                        {
                            if (string.IsNullOrEmpty(pastTitles))
                            {
                                pastTitles = Utils.getBetween(item, "", " at ");
                            }
                            else
                            {
                                pastTitles = pastTitles + ":" + Utils.getBetween(item, "", " at ");
                            }
                        }


                        currentCompany = Utils.getBetween(titles[0] + "@", " at ", "@").Replace("d/b/a", string.Empty);

                        foreach (string item in titles)
                        {
                            if (string.IsNullOrEmpty(pastCompanies))
                            {
                                pastCompanies = Utils.getBetween(item + "@", " at ", "@").Replace("d/b/a", string.Empty);
                            }
                            else
                            {
                                if (!pastCompanies.Contains(Utils.getBetween(item + "@", " at ", "@")))
                                {
                                    pastCompanies = pastCompanies + ":" + Utils.getBetween(item + "@", " at ", "@").Replace("d/b/a", string.Empty);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    skills = GetSkills(pgSource);

                    //string additionalInfo = GetAdditionalInfo(pgSource);

                    numberOfConnections = GetNumberOfConnections(pgSource);

                    education = GetEducation(pgSource);

                    //string check = profileURL;
                    //if (string.IsNullOrEmpty(headlineTitle))
                    //{
                    //}
                    //if (string.IsNullOrEmpty(location))
                    //{
                    //}
                    //if (string.IsNullOrEmpty(industry))
                    //{
                    //}
                    //if (string.IsNullOrEmpty(education))
                    //{

                    //}
                    //if (string.IsNullOrEmpty(skills))
                    //{
                    //}
                }
                catch (Exception ex)
                {
                }

                WriteDataToCSV(name, profileURL, memberID, connection, location, industry, headlineTitle, currentTitle, pastTitles, currentCompany, pastCompanies, skills, numberOfConnections, education, email, phoneNumber);
            }
        }

        public string GetName(string source)
        {
            string name = string.Empty;
            try
            {
                string rawName = Utils.getBetween(source, "\"profile\":", "\"numConnections\"");
                name = Utils.getBetween(rawName, "\"fullName\":\"", "\",");

                if (string.IsNullOrEmpty(name))
                {
                    string fName = Utils.getBetween(source, "\"firstName\":\"", "\",\"");
                    string lName = Utils.getBetween(source, "\"lastName\":\"", "\",\"");
                    name = fName + " " + lName;
                }
            }
            catch (Exception ex)
            {
            }
            return name;
        }

        public string GetImageUrl(string source)
        {
            string imageUrl = string.Empty;
            try
            {
                imageUrl = Utils.getBetween(source, "\"imageUri\":\"", "\",\"");
            }
            catch (Exception ex)
            {
            }
            return imageUrl;
        }

        public string GetLocation(string source)
        {
            string location = string.Empty;
            try
            {
                string rawLocation = Utils.getBetween(source, "\"profile\":", "authToken\":\"");
                location = Utils.getBetween(rawLocation, "\"location\":\"", "\",\"");
            }
            catch (Exception ex)
            {
            }
            return location;
        }

        public string GetConnection(string source)
        {
            string connection = string.Empty;
            try
            {
                string rawConnection = Utils.getBetween(source, "\"profile\":", "\"numConnections\"");
                connection = Utils.getBetween(rawConnection, "\",\"degree\":", ",\"");
            }
            catch (Exception ex)
            {
            }
            return connection;
        }

        public string GetEmail(string source)
        {
            string email = string.Empty;
            try
            {
                string rawEmail = Utils.getBetween(source, "\"profile\":", "\"industry\":\"");
                email = Utils.getBetween(rawEmail, "emails\":[\"", "\"],");
            }
            catch (Exception ex)
            {
            }
            return email;
        }

        public string GetPhoneNumber(string source)
        {
            string phoneNumber = string.Empty;
            try
            {
                string rawPhoneNumber = Utils.getBetween(source, "\"profile\":", "\"industry\":\"");
                phoneNumber = Utils.getBetween(rawPhoneNumber, "\"phones\":[\"", "\"],");
            }
            catch (Exception ex)
            {
            }
            return phoneNumber;
        }

        public string GetIndustry(string source)
        {
            string industry = string.Empty;
            try
            {
                industry = Utils.getBetween(source, "\"industry\":\"", "\",\"");
            }
            catch (Exception ex)
            {
            }
            return industry;
        }

        public string GetHeadlineTitle(string source)
        {
            string headlineTitle = string.Empty;
            try
            {
                string rawHeadLineTitle = Utils.getBetween(source, "\"profile\":", "\"numConnections\"");
                headlineTitle = Utils.getBetween(rawHeadLineTitle, "\"headline\":\"", "\",\"");
            }
            catch (Exception ex)
            {
            }
            return headlineTitle;
        }

        public string GetAllTitle(string source)
        {
            string allTitles = string.Empty;
            try
            {
                List<string> lstAllTitles = new List<string>();
                string[] title_split = Regex.Split(source, "position\":");
                foreach (string item in title_split)
                {
                    if (!item.Contains("<!DOCTYPE"))
                    {
                        string companies = Utils.getBetween(item, "\"companyName\":\"", "\",\"").Replace("&amp;", "&");
                        string titles = Utils.getBetween(item, "\"title\":\"", "\",\"").Replace("&amp;", "&");
                        if (!string.IsNullOrEmpty(companies))
                        {
                            lstAllTitles.Add(titles + " at " + companies);
                        }
                    }
                }

                lstAllTitles = lstAllTitles.Distinct().ToList();
                foreach (string item in lstAllTitles)
                {
                    if (string.IsNullOrEmpty(allTitles))
                    {
                        allTitles = item;
                    }
                    else
                    {
                        allTitles = allTitles + " : " + item;
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return allTitles;
        }

        public string GetPastTitles(string source)
        {
            string pastTitles = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
            }
            return pastTitles;
        }

        public string GetCurrentCompany(string source)
        {
            string currentCompany = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
            }
            return currentCompany;
        }

        public string GetPastCompanies(string source)
        {
            string pastCompanies = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
            }
            return pastCompanies;
        }

        public string GetSkills(string source)
        {
            string skills = string.Empty;
            try
            {
                 skills = Utils.getBetween(source, "skills\":[\"", "\"],").Replace("\",\"", ":");
            }
            catch (Exception ex)
            {
            }
            return skills;
        }

        public string GetAdditionalInfo(string source)
        {
            string additionalInfo = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
            }
            return additionalInfo;
        }

        public string GetNumberOfConnections(string source)
        {
            string numberOfConnection = string.Empty;
            try
            {
                numberOfConnection = Utils.getBetween(source, "\"numConnections\":", ",\"");
            }
            catch (Exception ex)
            {
            }
            return numberOfConnection;
        }

        public string GetEducation(string source)
        {
            string education = string.Empty;
            try
            {
                List<string> lstEducation = new List<string>();
                string rawEducation = Utils.getBetween(source, "[{\"educationId\"", "]},\"");
                string[] educationSplit = Regex.Split(rawEducation, "\"endDateMonth\"");
                foreach (string item in educationSplit)
                {
                    if (item.Contains("degree"))
                    {
                        string degree = Utils.getBetween(item, "\"degree\":\"", "\",\"");
                        string school = Utils.getBetween(item, "\"schoolName\":\"", "\",\"");
                        string field = Utils.getBetween(item, "\"fieldOfStudy\":\"", "\",\"");
                        string startYear = Utils.getBetween(item, "startDateYear\":", ",\"");
                        string endYear = Utils.getBetween(item, "endDateYear\":", ",\"");
                        string edu = "[Degree:" + degree + "," + field + "  School:" + school + " {" + startYear + "-" + endYear + "}]";
                        lstEducation.Add(edu);
                    }
                }
                foreach (string item in lstEducation)
                {
                    if (string.IsNullOrEmpty(education))
                    {
                        education = item;
                    }
                    else
                    {
                        education = education + "::" + item;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return education;
        }

        public void WriteDataToCSV(string name, string profileUrl, string memberID, string connection, string location, string industry, string headlineTitle, string currentTitle, string pastTitles, string currentCompany, string pastCompany, string skills, string numberOfConnections, string education, string email, string phoneNumber)
        {
            try
            {
                AddToLogger("[ " + DateTime.Now + " ] => [ Profile details saved in CSV file of profile URL : " + profileUrl + " ]");

                if (name.Trim() == string.Empty) name = "LinkedIn member";
                if (profileUrl.Trim() == string.Empty) profileUrl = "--";
                if (connection.Trim() == string.Empty) connection = "--";
                if (location.Trim() == string.Empty) location = "--";
                if (industry.Trim() == string.Empty) industry = "--";
                if (headlineTitle.Trim() == string.Empty) headlineTitle = "--";
                if (currentTitle.Trim() == string.Empty) currentTitle = "--";
                if (pastTitles.Trim() == string.Empty) pastTitles = "--";
                if (currentCompany.Trim() == string.Empty) currentCompany = "--";
                if (pastCompany.Trim() == string.Empty) pastCompany = "--";
                if (skills.Trim() == string.Empty) skills = "--";
                if (numberOfConnections.Trim() == string.Empty) numberOfConnections = "--";
                if (education.Trim() == string.Empty) education = "--";
                if (email.Trim() == string.Empty) email = "--";
                if (phoneNumber.Trim() == string.Empty) phoneNumber = "--";

                
                string Header = "Profile name" + "," + "Profile URL" + "," + "Member ID" + "," + "Degree of connection" + "," + "Location" + "," + "Industry" + "," + "Headline title" + "," + "Current title" + "," + "Past titles" + "," + "Current company" + "," + "Past company" + "," + "Skills" + "," + "Number of connections" + "," + "Education" + "," + "Email" + "," + "Phone number" + "," +  "Account Used" + ",";
                string LDS_FinalData = name.Replace(",", ";") + "," + profileUrl.Replace(",", ";") + "," + memberID.Replace(",", ";") + "," + connection.Replace(",", ";") + "," + location.Replace(",", ";") + "," + industry.Replace(",", ";") + "," + headlineTitle.Replace(",", ";") + "," + currentTitle.Replace(",", ";") + "," + pastTitles.Replace(",", ";") + "," + currentCompany.Replace(",", ";") + "," + pastCompany.Replace(",", ";") + "," + skills.Replace(",", ";") + "," + numberOfConnections.Replace(",", ";") + "," + education.Replace(",", ";") + "," + email.Replace(",", ";") + "," + phoneNumber.Replace(",", ";") + "," + SalesNavigatorGlobals.loginId.Replace(",", ";");
                string FileName = "SalesNavigatorScraper";
                AppFileHelper.SalesNavigatorScraperWriteToCSV(LDS_FinalData, Header, FileName);
            }
            catch (Exception ex)
            {
            }
        }

        #region AddToLogger
        private void AddToLogger(string log)
        {
            try
            {
                EventsArgs loggerEventsArgs = new EventsArgs(log);
                loggerSalesNavigator.LogText(loggerEventsArgs);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}

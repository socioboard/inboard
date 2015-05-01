using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace BaseLib
{
    public class Globals
    {
        public static bool IsFreeVersion = false;

        public static List<string> listAccounts = new List<string>();
        public static AccountMode accountMode;
        public static Dictionary<string, string> tempDict = new Dictionary<string, string>();
        public static string DeCaptcherHost = string.Empty;
        public static string DeCaptcherPort = string.Empty;
        public static string DeCaptcherUsername = string.Empty;
        public static string DeCaptcherPassword = string.Empty;
        public static bool IsStop_CompanyEmployeeScraperThread = false;
        public static string DBCUsername = string.Empty;
        public static string DBCPassword = string.Empty;
        public static List<Thread> lstCompanyEmployeeScraperThread = new List<Thread>();
        public static string _ChangeDefaultFolderPath = string.Empty;
        public static bool IsStop = false;
        public static List<Thread> lstComposeMessageThread = new List<Thread>();
        public static bool scrapeWithoutGoingToMainProfile = false;
        public static bool scrapeWithoutLoggingIn = false;
        public static bool btnSaveClickedCampaignSearchWithInvite = false;

       

        public static string DesktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InBoardPro";

        public static string path_AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\InBoardPro";

        public static string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\InBoardPro";

        public static string pathCapcthaLogin = DesktopFolder + "\\CapcthaLoginError.txt";

        public static string profileRankFolder = DesktopFolder + "\\ProfileRank";

        public static string memberRanktxt = profileRankFolder + "\\MemberRankDetails.txt";

        public static string selfRanktxt = profileRankFolder + "\\SelfRankDetails.txt";

        public static string profileUrlsSalesNavigatorScraper = DesktopFolder + "\\Sales Navigator Scraper - Profile URLs.txt";

        //to be deleted
        public static string stateNames = profileRankFolder + "\\StateName.txt";

        public static string regionNames = profileRankFolder + "\\RegionName.txt";

        public static string cityNames = profileRankFolder + "\\CityName.txt";

        //above line to be deleted

        public static string selfRankCsv = profileRankFolder + "\\SelfRankDetails.csv";

        public static string memberRankCsv = profileRankFolder + "\\MemberRankDetails.csv";

        public static string profilestatsCsv = profileRankFolder + "\\ProfileViewStats.csv";
        
        //****************************************update by san***********************************************
        public static string path_AddConnectionSuccess1 = DesktopFolder + "\\AddConnectionSuccess.csv";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InBoardPro\\AddConnectionSuccess.csv";

        public static string path_ResentVerfication = DesktopFolder + "\\ResentVerfication.txt";

        public static string path_NonResentVerfication = DesktopFolder + "\\NonResentVerfication.txt";

        public static string path_VerifiedAccounts = DesktopFolder + "\\VerifiedAccounts.txt";

        public static string path_NonVerifiedAccounts = DesktopFolder + "\\NonVerifiedAccounts.txt";

        public static string path_AddConnectionEmail = DesktopFolder + "\\AddConnectionEmail.txt";

        public static string path_NonAddConnectionEmail = DesktopFolder + "\\NotAddConnectionEmail.txt";

        public static string path_CapcthaNotSolvedAccounts = DesktopFolder + "\\CaptchaNotSolvedAccounts.txt";

        public static string path_ConnectionthroughKeywordSearchnotadded = DesktopFolder + "\\ConnnectionNotAddedbyKeyword.csv";

        public static string path_OnlyVisitProfileUsingKeyword = DesktopFolder + "\\OnlyVisitProfileUsingKeyword.csv";

        public static string path_ConnectionThroughUnkonwPeopleLink = DesktopFolder + "\\ConnectionToPeopleNoTknown.csv";

        public static string path_SentInvitationGroup = DesktopFolder + "\\SentInvitationGroup.csv";

        public static string path_NotSentInvitationGroup = DesktopFolder + "\\NotSentInvitationGroup.csv";

        public static string path_InviationUrlIncorrect = DesktopFolder + "\\IncorrectUrlForInviattaion.csv";

        public static string Path_LinkedinFollowCompanyErrorLogs = path_AppDataFolder + "\\AddSearchGroupErrorLog.txt";

        public static string path_AcceptInvitationEmail = DesktopFolder + "\\AddAcceptInvitationEmail.txt";

        public static string path_AddConnectionSuccess
        {
            get
            {
                return DesktopFolder + "\\AddConnectionSuccess.csv";
            }
        }

        public static string path_AddConnectionSuccessWith2ndDegree
        {
            get
            {
                return DesktopFolder + "\\AddConnectionSuccessWith2ndDegree.csv";
            }
        }

        public static string path_ComposeMessageSent1 = DesktopFolder + "\\ComposeMessageSent.csv";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InBoardPro\\ComposeMessageSent.csv";

        public static string path_ComposeMessageSent
        {
            get
            {
                return DesktopFolder + "\\ComposeMessageSent.csv";
            }

        }

        public static string path_AddConnectionFail1 = DesktopFolder + "\\AddConnectionFail.csv";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InBoardPro\\AddConnectionFail.csv";

        public static string path_AddConnectionFail
        {
            get
            {
                return DesktopFolder + "\\AddConnectionFail.csv";
            }

        }

        public static string path_PostMessageToGroup1 = DesktopFolder + "\\PostMessageToGroup .csv";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InBoardPro\\PostMessageToGroup .csv";

        public static string path_PostMessageToGroup
        {
            get
            {
                return DesktopFolder + "\\PostMessageToGroup .csv";
            }

        }

        public static string path_MessageSentGroupMember1 = DesktopFolder+ "\\MessageSentGroupMember.csv";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InBoardPro\\MessageSentGroupMember.csv";

        public static string path_MessageSentGroupMember
        {
            get
            {
                return DesktopFolder + "\\MessageSentGroupMember.csv";
            }

        }

        public static string path_MessageAlreadySentComposeMgs
        {
            get
            {
                return DesktopFolder + "\\MessageAlreadySentComposeMgs.csv";
            }

        }

        public static string path_MessageAlreadySentGroupMember
        {
            get
            {
                return DesktopFolder + "\\MessageAlreadySentGroupMember.csv";
            }

        }

        public static string path_PostInvitation1 = DesktopFolder+ "\\PostInvitation .csv";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InBoardPro\\PostInvitation .csv";

        public static string path_PostInvitation
        {
            get
            {
                return DesktopFolder + "\\PostInvitation.csv";
            }

        }

        public static string path_LinkedinSearchByPeople
        {
            get
            {
                return DesktopFolder + "\\LinkedinSearchByPeople.csv";
            }

        }

        public static string path_LinkedinSearchByCompany
        {
            get
            {
                return DesktopFolder + "\\LinkedinSearchByCompany.csv";
            }

        }


        public static string path_LinkedinSearchByCompanyWithFilter
        {
            get
            {
                return DesktopFolder + "\\LinkedinSearchByCompanyWithFilter.csv";
            }

        }

        public static string path_LinkedinSearchByProfileURL
        {
            get
            {
                return DesktopFolder + "\\LinkedinSearchByProfileURL.csv";
            }

        }

        public static string scrappedMembersFromGroup = string.Empty;

        public static string path_ScrappedMembersFromGroup
        {
            
            get
            {
                return DesktopFolder + "\\ScrappedMembersFromGroup_" + scrappedMembersFromGroup + ".csv";
            }
            set
            {
                scrappedMembersFromGroup = value;
            }

        }

        public static string path_InBoardProGetDataResultCount
        {
            get
            {
                return DesktopFolder + "\\InBoardProGetDataResultCount.csv";
            }

        }

        public static string path_InBoardProGetDataResultCountZipDistanceIndustrySirnameWise
        {
            get
            {
                return DesktopFolder + "\\InBoardProGetDataZipDistanceIndustrySurnameWiseResultCount.csv";
            }

        }

        public static string path_InBoardProGetDataResultCountLastNameWise
        {
            get
            {
                return DesktopFolder + "\\InBoardProGetDataResultCountLastNameWise.csv";
            }

        }

        public static string path_InBoardProGetDataResultCountIndustryZoneWise
        {
            get
            {
                return DesktopFolder + "\\InBoardProGetDataResultCountIndustryZoneWise.csv";
            }

        }

        public static string path_InBoardProGetDataResultCountZipCodeWise
        {
            get
            {
                return DesktopFolder + "\\InBoardProGetDataResultCountZipCodeWise.csv";
            }

        }

        public static string path_InBoardProGetDataResultLastNameWiseData
        {
            get
            {
                return DesktopFolder + "\\InBoardProGetDataResultLastNameWiseData.csv";
            }

        }

        public static string path_InBoardProGetDataResultIndustryZoneWiseData
        {
            get
            {
                return DesktopFolder + "\\InBoardProGetDataResultIndustryZoneWiseData.csv";
            }

        }

        public static string path_InBoardProGetDataResultZipCodeWiseData
        {
            get
            {
                return DesktopFolder + "\\InBoardProGetDataResultZipCodeWiseData.csv";
            }

        }


        public static string path_InBoardProGetDataResultUrlData
        {
            get
            {
                return DesktopFolder + "\\InBoardProGetDataResultUrlData.csv";
            }
            
        }

        public static string path_InBoardProGetDatagrpUrlData
        {
            get
            {
                return DesktopFolder + "\\InBoardProGetDatagrpUrlData.csv";
            }

        }


        public static string path_LinkedinJoinSearchGroup
        {
            get
            {
                return DesktopFolder + "\\LinkedinJoinSearchGroup.csv";
            }

        }

        public static string path_LinkedinJoinGroupUsingUrl
        {
            get
            {
                return DesktopFolder + "\\LinkedinJoinGroupUsingUrl.csv";
            }

        }

        
        public static string path_Not_FollowCompany
        {
            get
            {
                return DesktopFolder + "\\LinkedInNonFollowCompany.txt";
            }

        }


        public static string path_LinkedininvitememberResultUrlData
        {
            get
            {
                return DesktopFolder + "\\LinkedininvitememberResultUrlData.csv";
            }

        }
       

        public static string path_GroupUpdates1 = DesktopFolder + "\\GroupUpdate.csv";

        public static string path_GroupUpdates
        {
            get
            {
                //return DesktopFolder + "\\GroupUpdate.csv";
                return DesktopFolder + "\\GroupStatusUpdate.csv";
            }

        }

      
        public static string path_CreateGroups1 = DesktopFolder + "\\CreateGroup.csv";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InBoardPro\\CreateGroup .csv";

        public static string path_CreateGroups
        {
            get
            {
                return DesktopFolder + "\\CreateGroup.csv";
            }

        }

        public static string path_ComentLiker
        {
            get
            {
                //return DesktopFolder + "\\ComentLiker.csv";
                return DesktopFolder + "\\StatusUpdateLiker.csv";
            }

        }

        public static string path_JoinGroups1 = DesktopFolder + "\\JoinGroups.csv";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InBoardPro\\JoinGroups .csv";

        public static string path_JoinGroups
        {
            get
            {
                return DesktopFolder + "\\JoinGroups.csv";
            }

        }

        public static string path_AlreadyJoinGroups1 = DesktopFolder + "\\AlreadyJoinGroups.csv";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InBoardPro\\AlreadyJoinGroups .csv";

        public static string path_AlreadyJoinGroups
        {
            get
            {
                return DesktopFolder + "\\AlreadyJoinGroups.csv";
            }

        }

        public static string path_ErrorJoinGroups1 = DesktopFolder + "\\ErrorJoinGroups.csv";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InBoardPro\\ErrorJoinGroups .csv";

        public static string path_ErrorJoinGroups
        {
            get
            {
                return DesktopFolder + "\\ErrorJoinGroups .csv";
            }

        }

        public static string path_LinkedinAddExperience
        {
            get
            {
                return DesktopFolder + "\\LinkedinAddExperience.csv";
            }

        }

        public static void ExportDataCSVFile(string CSV_Heder, string CSV_Content, string CSV_FilePath)
        {
            try
            {
                if (!File.Exists(CSV_FilePath))
                {
                    if (!string.IsNullOrEmpty(CSV_Heder))
                    {
                        CSVUtilities.WriteCSVLineByLine(CSV_FilePath, CSV_Heder);
                    }
                    CSVUtilities.WriteCSVLineByLine(CSV_FilePath, CSV_Content);
                }
                else
                {
                    CSVUtilities.WriteCSVLineByLine(CSV_FilePath, CSV_Content);
                }
            }
            catch (Exception)
            {

            }
        }

      //  public static string path_PostMessageToGroupFail = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InBoardPro\\PostMessageToGroup .csv";
        //*******************************************************************************************************

        public static string path_CreatedAccounts1 = DesktopFolder + "\\LinkedInCreatedAccounts.txt";

        public static string path_NonCreatedAccount1s = DesktopFolder + "\\LinkedInNonCreatedAccounts.txt";

        public static string path_PostStatus1 = DesktopFolder + "\\LinkedInStatusUpdate.txt";

        public static string path_CreateGroup1 = DesktopFolder + "\\LinkedInCreateGroup.txt";

        public static string path_AddConnection1 = DesktopFolder + "\\LinkedInAddConnectionRequestSent.txt";

        public static string path_JoinFriendsGroup1 = DesktopFolder + "\\LinkedInJoinFriendsGroup.txt";

        public static string path_JoinSearchGroup1 = DesktopFolder + "\\LinkedInJoinSearchGroup.txt";

        public static string path_GroupUpdate1 = DesktopFolder + "\\LinkedInGroupUpdate.txt";

        public static string path_ComposeMessage1 = DesktopFolder + "\\LinkedInComposeMessage.txt";

        public static string path_MessageGroupMember1 = DesktopFolder + "\\LinkedInMessageGroupMember.txt";

        public static string path_AlreadyCreatedEmails1 = DesktopFolder + "\\LinkedInAlreadyCreatedEmails.txt";

        public static string path_CreateGroupNotConfirmedAccounts = DesktopFolder + "\\LinkedInCreateGroup-NotConfirmedAccounts";

        public static string path_CreateGroupCreatedGroups = DesktopFolder + "\\LinkedInCreateGroup-CreatedGroups";

        public static string path_EndorsePeople = DesktopFolder + "\\ErrorLogs_EndorsePeople.txt";

        public static string path_EndorsedPeopleText = DesktopFolder + "\\EndorsePeople.csv";

        public static string path_NotEndorsedPeopleText = DesktopFolder + "\\NotEndorsePeople.csv";

        public static string path_NotCreatedGroups = DesktopFolder + "\\LinkedInCreateGroup-NotCreatedGroups.csv";
        
        public static string path_AlreadyCreatedEmails
        {
            get
            {
                return DesktopFolder + "\\LinkedInAlreadyCreatedEmails.txt";
            }
           
        }

        public static string path_MessageGroupMember
        {
            get
            {
                return DesktopFolder + "\\LinkedInMessageGroupMember.txt";
            }

        }

        public static string path_ComposeMessage
        {
            get
            {
                return DesktopFolder + "\\LinkedInComposeMessage.txt";
            }

        }

        public static string path_GroupUpdate
        {
            get
            {
                return DesktopFolder + "\\LinkedInGroupUpdate.txt";
            }

        }

        public static string path_JoinSearchGroupSuccess
        {
            get
            {
                return DesktopFolder + "\\LinkedInJoinSearchGroupSuccess.txt";
            }

        }

        public static string path_PendingGroupWithdrawn
        {
            get
            {
                return DesktopFolder + "\\LinkedInPendingGroupWithdrawn.txt";
            }

        }

        public static string path_JoinSearchGroupFail
        {
            get
            {
                return DesktopFolder + "\\LinkedInJoinSearchGroupFail.txt";
            }

        }

        public static string path_JoinFriendsGroup
        {
            get
            {
                return DesktopFolder + "\\LinkedInJoinFriendsGroup.txt";
            }

        }

        public static string path_AddConnection
        {
            get
            {
                return DesktopFolder + "\\LinkedInAddConnectionRequestSent.txt";
            }

        }

        public static string path_CreateGroup
        {
            get
            {
                return DesktopFolder + "\\LinkedInCreateGroup.txt";
            }

        }

        public static string path_NotCreateGroup
        {
            get
            {
                return DesktopFolder + "\\LinkedInNotCreateGroup.txt";
            }

        }

        public static string path_PostStatus
        {
            get
            {
                return DesktopFolder + "\\LinkedInStatusUpdate.txt";
            }

        }


        public static string path_NonPostStatus = Globals.DesktopFolder + "\\LinkedinNoStatusUpdate.txt";

        public static string path_NonCreatedAccounts
        {
            get
            {
                return DesktopFolder + "\\LinkedInNonCreatedAccounts.txt";
            }

        }

        public static string path_CreatedAccounts
        {
            get
            {
                return DesktopFolder + "\\LinkedInCreatedAccounts.txt";
            }

        }

        public static string path_LinkedinFollowCompanyUsingUrl
        {
            get
            {
                return DesktopFolder + "\\LinkedinFollowCompanyUsingUrl.csv";
            }

        }

        public static string path_LinkedinFriendsGroupScraper
        {
            get
            {
                return DesktopFolder + "\\LinkedinFriendsGroupScraper.csv";
            }

        }

        public static string path_SuccesfullyCreatedAccounts1 = DesktopFolder + "\\SuccesfullyCreatedAccounts.txt";

        public static string path_SuccesfullyCreatedAccounts
        {
            get
            {
                return DesktopFolder + "\\SuccesfullyCreatedAccounts.txt";
            }

        }

        public static string Path_ExsistingProxies1 = DesktopFolder + "\\ExsistingProxies.txt";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InBoardPro\\ExsistingProxies.txt";

        public static string Path_ExsistingProxies
        {
            get
            {
                return DesktopFolder + "\\ExsistingProxies.txt";
            }

        }

        public static string Path_WorkingPvtProxies = DesktopFolder + "\\ExsistingPrivateProxies.txt";

        public static string Path_Non_ExsistingProxies1 = DesktopFolder + "\\NonExsistingProxies.txt";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InBoardPro\\NonExsistingProxies.txt";

        public static string Path_Non_ExsistingProxies
        {
            get
            {
                return DesktopFolder + "\\NonExsistingProxies.txt";
            }

        }

        public static string Path_WorkingAccount_AccountChecker
        {
            get
            {
                return DesktopFolder + "\\WorkingAccount_AccountChecker.txt";
            }

        }

        public static string Path_NonWorkingAccount_AccountChecker
        {
            get
            {
                return DesktopFolder + "\\NonWorkingAccount_AccountChecker.txt";
            }

        }

        public static string Path_ChangeYourPassword_AccountChecker
        {
            get
            {
                return DesktopFolder + "\\ChangeYourPassword_AccountChecker.txt";
            }

        }

        public static string Path_TemporarilyRestrictedAccount_AccountChecker
        {
            get
            {
                return DesktopFolder + "\\TemporarilyRestrictedAccount_AccountChecker.txt";
            }

        }

        public static string Path_ConfirmYourEmailAddress_AccountChecker
        {
            get
            {
                return DesktopFolder + "\\ConfirmYourEmailAddress_AccountChecker.txt";
            }

        }

        public static string Path_EmailAddressOrPasswordDoesNotMatch_AccountChecker
        {
            get
            {
                return DesktopFolder + "\\EmailAddressOrPasswordDoesNotMatch_AccountChecker.txt";
            }

        }

        public static string Path_SecurityVerification_AccountChecker
        {
            get
            {
                return DesktopFolder + "\\SecurityVerification_AccountChecker.txt";
            }

        }

        

        public static string Path_ErrorFile_AccountChecker
        {
            get
            {
                return DesktopFolder + "\\ErrorFile_AccountChecker.txt";
            }

        }

        public static string Path_ExistingEmail_EmailChecker
        {
            get
            {
                return CreatePath_EmailChecker() + "\\ExistingEmail_EmailChecker.txt";
            }

        }

        public static string Path_NonExistingEmail_EmailChecker
        {
            get
            {
                return CreatePath_EmailChecker() + "\\NonExistingEmail_EmailChecker.txt";
            }

        }

        public static string Path_SecurityVerification_EmailChecker
        {
            get
            {
                return CreatePath_EmailChecker() + "\\SecurityVerification_EmailChecker.txt";
            }

        }

        public static string Path_ErrorFile_EmailChecker
        {
            get
            {
                return CreatePath_EmailChecker() + "\\ErrorFile_EmailChecker.txt";
            }

        }

        public static string Path_OtherProblem_EmailChecker
        {
            get
            {
                return CreatePath_EmailChecker() + "\\OtherProblem_EmailChecker.txt";
            }

        }

        private static string CreatePath_EmailChecker()
        {
            string path = string.Empty;
            try
            {
                if (!Directory.Exists(Globals.DesktopFolder + "\\EmailChecker"))
                {
                    DirectoryInfo di = Directory.CreateDirectory(Globals.DesktopFolder + "\\EmailChecker");
                    di.Create();
                }
                path = Globals.DesktopFolder + "\\EmailChecker";
            }
            catch
            {
            }
            return path;

        }

        public static string path_ProfPicSuccess
        {
            get
            {
                return DesktopFolder + "\\ProfilePicSuccess.txt";
            }

        }

        public static string path_ProfPicFail
        {
            get
            {
                return DesktopFolder + "\\ProfilePicFail.txt";
            }

        }

        public static string path_FailLogin
        {
            get
            {
                return DesktopFolder + "\\LoginFail.txt";
            }

        }
        public static string path_TempRestrictedLogin
        {
            get
            {
                return DesktopFolder + "\\LoginTempRestricted.txt";
            }

        }
        public static string path_SuccessfulLogin
        {
            get
            {
                return DesktopFolder + "\\LoginSuccessful.txt";
            }

        }

        public static string path_ComposeMessage_FriendList
        {
            get
            {
                return DesktopFolder + "\\FriendList.txt";
            }

        }

        public static string Path_ProxySettingErroLog = path_AppDataFolder + "\\ErrorProxySetting.txt";

        public static bool CapchaTag = false;
        public static string CapchaLoginID = string.Empty;
        public static string CapchaLoginPassword = string.Empty;

        public static Dictionary<string, string> Industry = new Dictionary<string, string>();
        public static Dictionary<string, string> SeniorLevel = new Dictionary<string, string>();
        public static Dictionary<string, string> Groups = new Dictionary<string, string>();
        public static Dictionary<string, string> CompanySize = new Dictionary<string, string>();
        public static Dictionary<string, string> InterestedIN = new Dictionary<string, string>();
        public static Dictionary<string, string> Language = new Dictionary<string, string>();
        public static Dictionary<string, string> Function = new Dictionary<string, string>();
        public static Dictionary<string, string> RelationShip = new Dictionary<string, string>();
        public static Dictionary<string, string> YearOfExperience = new Dictionary<string, string>();
        public static Dictionary<string, string> Fortune1000 = new Dictionary<string, string>();
        public static Dictionary<string, string> RecentlyJoined = new Dictionary<string, string>();

        public static string groupStatusString = string.Empty;

        #region ErrorLogPaths
        public static string Path_LinkedinErrorLogs1 = DesktopFolder+"\\ErrorLog.txt";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InBoardPro\\ErrorLog.txt";

        public static string Path_LinkedinErrorLogs
        {
            get
            {
                return DesktopFolder + "\\ErrorLog.txt";
            }

        }

        //TEMP FOR CHECK PAGESOURCE
        public static string Path_InBoardProGetDataPagesource = path_AppDataFolder + "\\InBoardProGetDataPagesource.txt";
        public static string Path_LinkedinCompanyScrapperPagesource = path_AppDataFolder + "\\LinkedinCompanyScrapperPagesource.txt";

        public static string Path_LinkedinAccountCraetionErrorLogs = path_AppDataFolder + "\\AccountCreatorErrorLog.txt";
        public static string Path_LinkedinGroupstatusErrorLogs = path_AppDataFolder + "\\GroupstatusErrorLog.txt";
        public static string Path_LinkedinAddConnectionErrorLogs = path_AppDataFolder + "\\AddConnectionErrorLog.txt";
        public static string Path_LinkedinPreScrapperErrorLogs = path_AppDataFolder + "\\PreScraperErrorLog.txt";
        public static string Path_LinkedinStatusUpdateErrorLogs = path_AppDataFolder + "\\StatusUpdateErrorLog.txt";
        public static string Path_LinkedinStatusImageUpdateErrorLogs = path_AppDataFolder + "\\StatusImageUpdateErrorLog.txt";
        public static string Path_LinkedinCreateGroupErrorLogs = path_AppDataFolder + "\\CreateGroupErrorLog.txt";
        public static string Path_LinkedinAddFriendsGroupErrorLogs = path_AppDataFolder + "\\AddFriendsGroupErrorLog.txt";
        public static string Path_LinkedinPendingGroupErrorLogs = path_AppDataFolder + "\\AddPendingGroupErrorLog.txt";
        public static string Path_LinkedinAddSearchGroupErrorLogs = path_AppDataFolder + "\\AddSearchGroupErrorLog.txt";
        public static string Path_LinkedinGroupUpdateErrorLogs = path_AppDataFolder + "\\GroupUpdateErrorLog.txt";
        public static string Path_LinkedinGetGroupMemberErrorLogs = path_AppDataFolder + "\\GetGroupMemberErrorLog.txt";
        public static string Path_LinkedinComposeMessageErrorLogs = path_AppDataFolder + "\\ComposeMessageErrorLog.txt";
        public static string Path_LinkedinProxySettingErrorLogs = path_AppDataFolder + "\\ProxySettingErrorLog.txt";
        public static string Path_LinkedinCommentLikerErrorLogs = path_AppDataFolder + "\\ComentLikerErrorLog.txt";
        public static string Path_LinkedinDefaultSave = path_AppDataFolder + "\\LDDefaultFolderPath.txt";
        public static string Path_LinkedinAddFriendsGroupScraperErrorLogs = path_AppDataFolder + "\\AddFriendsGroupScraperErrorLog.txt";
        public static string Path_LinkedInProfileRankErrorLogs = path_AppDataFolder + "\\ProfileRankErrorLog.txt";
        #endregion


        //public static string DisableFbAccountPath = string.Empty;
        //public static string DisableFbAccountPath = string.Empty;
        //public static string DisableFbAccountPath = string.Empty;
        //public static string DisableFbAccountPath = string.Empty;
        //public static string DisableFbAccountPath = string.Empty;

        public static void CreateFolder(string FolderPath)
        {
            try
            {
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
            }
            catch
            {
            }
        }
    }

    public enum Module
    {
        SearchkeywordInvites
    }

    public enum AccountMode
    {
        NoProxy, PublicProxy, PrivateProxy
    }

    
}

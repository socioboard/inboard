using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campaign
{
    public class clsAccountManager
    {
        public CampaignAccountContainer AccountManager(List<string> templist)
        {
            CampaignAccountContainer objCampaignAccountContainer = new CampaignAccountContainer();
            //List<CampaignTweetAccountContainer> lst_CampaignTweetAccount = new List<CampaignTweetAccountContainer>();
            foreach (string item in templist)
            {

                string account = item;
                string[] AccArr = account.Split(':');
                if (AccArr.Count() > 1)
                {
                    string accountUser = account.Split(':')[0];
                    string accountPass = account.Split(':')[1];
                    string screanName = string.Empty;
                    string proxyAddress = string.Empty;
                    string proxyPort = string.Empty;
                    string proxyUserName = string.Empty;
                    string proxyPassword = string.Empty;
                    string status = string.Empty;

                    int DataCount = account.Split(':').Length;
                    if (DataCount == 2)
                    {
                        //Globals.accountMode = AccountMode.NoProxy;
                        accountUser = account.Split(':')[0];
                        accountPass = account.Split(':')[1];

                    }
                    else if (DataCount == 4)
                    {
                        //Globals.accountMode = AccountMode.PublicProxy;
                        accountUser = account.Split(':')[0];
                        accountPass = account.Split(':')[1];
                        proxyAddress = account.Split(':')[2];
                        proxyPort = account.Split(':')[3];
                    }
                    else if (DataCount > 5 && DataCount < 7)
                    {
                        //Globals.accountMode = AccountMode.PrivateProxy;
                        accountUser = account.Split(':')[0];
                        accountPass = account.Split(':')[1];
                        proxyAddress = account.Split(':')[2];
                        proxyPort = account.Split(':')[3];
                        proxyUserName = account.Split(':')[4];
                        proxyPassword = account.Split(':')[5];
                        //dt.Rows.Add(accountUser, accountPass, string.Empty , string.Empty, proxyAddress, proxyPort, proxyUserName, proxyPassword, "", "0");
                    }
                    else if (DataCount == 7)
                    {
                        //Globals.accountMode = AccountMode.PrivateProxy;
                        accountUser = account.Split(':')[0];
                        accountPass = account.Split(':')[1];
                        proxyAddress = account.Split(':')[3];
                        proxyPort = account.Split(':')[4];
                        proxyUserName = account.Split(':')[5];
                        proxyPassword = account.Split(':')[6];
                        //dt.Rows.Add(accountUser, accountPass, string.Empty , string.Empty, proxyAddress, proxyPort, proxyUserName, proxyPassword, "", "0");
                    }
                    else if (DataCount == 9)
                    {
                        //Globals.accountMode = AccountMode.PrivateProxy;
                        accountUser = account.Split(':')[0];
                        accountPass = account.Split(':')[1];
                        proxyAddress = account.Split(':')[2];
                        proxyPort = account.Split(':')[3];
                        proxyUserName = account.Split(':')[4];
                        proxyPassword = account.Split(':')[5];
                        //dt.Rows.Add(accountUser, accountPass, string.Empty , string.Empty, proxyAddress, proxyPort, proxyUserName, proxyPassword, "", "0");
                    }
                   

                    //dt.Rows.Add(accountUser, accountPass, screanName, string.Empty, proxyAddress, proxyPort, proxyUserName, proxyPassword, "", "0");

                    try
                    {
                        CampaignAccountManager twitter = new CampaignAccountManager();
                        twitter.Username = accountUser;
                        twitter.Password = accountPass;
                        twitter.proxyAddress = proxyAddress;
                        twitter.proxyPort = proxyPort;
                        twitter.proxyUsername = proxyUserName;
                        twitter.proxyPassword = proxyPassword;
                        // twitter.profileStatus = 0;
                        twitter.AccountStatus = "";
                        objCampaignAccountContainer.dictionary_CampaignAccounts.Add(twitter.Username, twitter);

                        //lst_CampaignTweetAccount.Add(objCampaignTweetAccountContainer);
                    }
                    catch { }

                    //string profileStatus = "0";
                }
                else
                {
                    // AddToListBox("Account has some problem : " + item);
                }
            }
            return objCampaignAccountContainer;
        }
    }
}

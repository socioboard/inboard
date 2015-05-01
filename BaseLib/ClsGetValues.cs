using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BaseLib
{
   public class ClsSelect
    {
        public string country = "us:United States,af:Afghanistan,ax:Aland Islands,al:Albania,dz:Algeria,as:American Samoa,ad:Andorra,ao:Angola,ai:Anguilla,aq:Antarctica,ag:Antigua and Barbuda,ar:Argentina,am:Armenia,aw:Aruba,au:Australia,at:Austria,az:Azerbaijan,bs:Bahamas,bh:Bahrain,bd:Bangladesh,bb:Barbados,by:Belarus,be:Belgium,bz:Belize,bj:Benin,bm:Bermuda,bt:Bhutan,bo:Bolivia,ba:Bosnia and Herzegovina,bw:Botswana,br:Brazil,io:British Indian Ocean Territory,bn:Brunei Darussalam,bg:Bulgaria,bf:Burkina Faso,bi:Burundi,kh:Cambodia,cm:Cameroon,ca:Canada,cv:Cape Verde,cb:Caribbean Nations,ky:Cayman Islands,cf:Central African Republic,td:Chad,cl:Chile,cn:China,cx:Christmas Island,cc:Cocos (Keeling) Islands,co:Colombia,km:Comoros,cg:Congo,ck:Cook Islands,cr:Costa Rica,ci:Cote D'Ivoire (Ivory Coast),hr:Croatia,cu:Cuba,cy:Cyprus,cz:Czech Republic,cd:Democratic Republic of the Congo,dk:Denmark,dj:Djibouti,dm:Dominica,do:Dominican Republic,tp:East Timor,ec:Ecuador,eg:Egypt,sv:El Salvador,gq:Equatorial Guinea,er:Eritrea,ee:Estonia,et:Ethiopia,fk:Falkland Islands (Malvinas),fo:Faroe Islands,fm:Federated States of Micronesia,fj:Fiji,fi:Finland,fr:France,gf:French Guiana,pf:French Polynesia,tf:French Southern Territories,ga:Gabon,gm:Gambia,ge:Georgia,de:Germany,gh:Ghana,gi:Gibraltar,gr:Greece,gl:Greenland,gd:Grenada,gp:Guadeloupe,gu:Guam,gt:Guatemala,gg:Guernsey,gn:Guinea,gw:Guinea-Bissau,gy:Guyana,ht:Haiti,hn:Honduras,hk:Hong Kong,hu:Hungary,is:Iceland,in:India ,id:Indonesia,ir:Iran,iq:Iraq,ie:Ireland,im:Isle of Man,il:Israel,it:Italy,jm:Jamaica,jp:Japan,je:Jersey,jo:Jordan,kz:Kazakhstan,ke:Kenya,ki:Kiribati,kr:Korea,kp:Korea (North),kw:Kuwait,kg:Kyrgyzstan,la:Laos,lv:Latvia,lb:Lebanon,ls:Lesotho,lr:Liberia,ly:Libya,li:Liechtenstein,lt:Lithuania,lu:Luxembourg,mo:Macao,mk:Macedonia,mg:Madagascar,mw:Malawi,my:Malaysia,mv:Maldives,ml:Mali,mt:Malta,mh:Marshall Islands,mq:Martinique,mr:Mauritania,mu:Mauritius,yt:Mayotte,mx:Mexico,md:Moldova,mc:Monaco,mn:Mongolia,me:Montenegro,ms:Montserrat,ma:Morocco,mz:Mozambique,mm:Myanmar,na:Namibia,nr:Nauru,np:Nepal,nl:Netherlands,an:Netherlands Antilles,nc:New Caledonia,nz:New Zealand,ni:Nicaragua,ne:Niger,ng:Nigeria,nu:Niue,nf:Norfolk Island,mp:Northern Mariana Islands,no:Norway,om:Oman,pk:Pakistan,pw:Palau,ps:Palestinian Territory,pa:Panama,pg:Papua New Guinea,py:Paraguay,pe:Peru,ph:Philippines,pn:Pitcairn,pl:Poland,pt:Portugal,pr:Puerto Rico,qa:Qatar,re:Reunion,ro:Romania,ru:Russian Federation,rw:Rwanda,sh:Saint Helena,kn:Saint Kitts and Nevis,lc:Saint Lucia,pm:Saint Pierre and Miquelon,vc:Saint Vincent and the Grenadines,ws:Samoa,sm:San Marino,st:Sao Tome and Principe,sa:Saudi Arabia,sn:Senegal,rs:Serbia,sc:Seychelles,sl:Sierra Leone,sg:Singapore,sk:Slovak Republic,si:Slovenia,sb:Solomon Islands,so:Somalia,za:South Africa,es:Spain,lk:Sri Lanka,sd:Sudan,sr:Suriname,sj:Svalbard and Jan Mayen,sz:Swaziland,se:Sweden,ch:Switzerland,,sy:Syria,tw:Taiwan,tj:Tajikistan,tz:Tanzania,th:Thailand,tl:Timor-Leste,tg:Togo,tk:Tokelau,to:Tonga,tt:Trinidad and Tobago,tn:Tunisia,tr:Turkey,tm:Turkmenistan,tc:Turks and Caicos Islands,tv:Tuvalu,ug:Uganda,ua:Ukraine,ae:United Arab Emirates,gb:United Kingdom,uy:Uruguay,uz:Uzbekistan,vu:Vanuatu,va:Vatican City State (Holy See),ve:Venezuela,vn:Vietnam,vg:Virgin Islands (British),vi:Virgin Islands (U.S.),wf:Wallis and Futuna,eh:Western Sahara,ye:Yemen,zm:Zambia,zw:Zimbabwe,oo:Other";
        public string Industry = "47:Accounting,94:Airlines/Aviation,120:Alternative Dispute Resolution,125:Alternative Medicine,127:Animation,19:Apparel & Fashion,50:Architecture & Planning,111:Arts and Crafts,53:Automotive,52:gov Aviation & Aerospace,41:Banking,12:gov hlth Biotechnology,36:Broadcast Media,49:Building Materials,138:corp Business Supplies and Equipment,129 :Capital Markets,54:Chemicals,90:Civic & Social Organization,51:Civil Engineering,128:Commercial Real Estate,118:Computer & Network Security,109:Computer Games,3:Computer Hardware,5:Computer Networking,4:Computer Software,48:Construction,24:good Consumer Electronics,25:good Consumer Goods,91:Consumer Services,18:Cosmetics,65:Dairy,1:gov Defense & Space,99:Design,69:Education Management,132:E-Learning,112:good Electrical/Electronic Manufacturing,28:Entertainment,86:Environmental Services,110:Events Services,76:Executive Office,122:Facilities Services,63:Farming,43:Financial Services,38:Fine Art,66:Fishery,34:Food & Beverages,23:Food Production,101:Fund-Raising,26:good Furniture,29:Gambling & Casinos,145:cons Glass, Ceramics & Concrete,75:Government Administration,148:Government Relations,140:Graphic Design,124:hlth Health, Wellness and Fitness,68:Higher Education,14:Hospital & Health Care,31:Hospitality,137:Human Resources,134:corp good Import and Export,88:Individual & Family Services,147:cons Industrial Automation,84:Information Services,96:Information Technology and Services,42:Insurance,74:International Affairs,141:International Trade and Development,6:Internet,45:Investment Banking,46:Investment Management,73:Judiciary,77:Law Enforcement,9:Law Practice,10:Legal Services,72:Legislative Office,30:Leisure, Travel & Tourism,85:med Libraries,116:corp Logistics and Supply Chain,143:Luxury Goods & Jewelry,55:Machinery,11:Management Consulting,95:Maritime,80:Marketing and Advertising,97:Market Research,135:cons gov Mechanical or Industrial Engineering,126:Media Production,17:Medical Devices,13:Medical Practice,139:Mental Health Care,71:Military,56:Mining & Metals,35:Motion Pictures and Film,37:Museums and Institutions,115:art Music,114:gov man Nanotechnology,81:Newspapers,100:Nonprofit Organization Management,57:Oil & Energy,113:Online Media,123:Outsourcing/Offshoring,87:Package/Freight Delivery,146:good Packaging and Containers,61:Paper & Forest Products,39:Performing Arts,15:hlth Pharmaceuticals,131:Philanthropy,136:Photography,117:Plastics,107:gov Political Organization,67:Primary/Secondary Education,83:Printing,105:Professional Training & Coaching,102:corp Program Development,79:Public Policy,98:Public Relations and Communications,78:Public Safety,82:Publishing,62:Railroad Manufacture,64:Ranching,44:cons fin Real Estate,40:Recreational Facilities and Services,89:Religious Institutions,144:gov man Renewables & Environment,70:edu Research,32:Restaurants,27:good Retail,121:corp Security and Investigations,7:Semiconductors,58:Shipbuilding,20:good Sporting Goods,33:Sports,104:Staffing and Recruiting,22:Supermarkets,8:gov Telecommunications,60:Textiles,130:gov Think Tanks,21:Tobacco,108:Translation and Localization,92:Transportation/Trucking/Railroad,59:Utilities,106:fin Venture Capital & Private Equity,16:Veterinary,93:Warehousing,133:Wholesale,142:good man Wine and Spirits,119:Wireless,103:Writing and Editing";
        public  string Language = "Bahasa Indonesia:in_ID,Czech:cs_CZ,Dutch:nl_NL,English:en_US,French:fr_FR,German:de_DE,Italian:it_IT,Japanese:ja_JP,Korean:ko_KR,Malay:ms_MY,Polish:pl_PL,Portuguese:pt_BR,Romanian:ro_RO,Russian:ru_RU,Spanish:es_ES,Swedish:sv_SE,Turkish:tr_TR,Albanian:sq_AL,Armenian:hy_AM,Bosnian:bs_BA,Burmese (Myanmar):my_MM,Chinese (Simplified):zh_CN,Chinese (Traditional):zh_TW,Croatian:hr_HR,Danish:da_DK,Finnish:fi_FI,Greek:el_GR,Hindi:hi_IN,Hungarian:hu_HU,Icelandic:is_IS,Javanese:jv_JV,Kannada:kn_IN,Latvian:lv_LV,Lithuanian:lt_LT,Malayalam:ml_IN,Norwegian:no_NO,Serbian:sr_BA,Slovak:sk_SK,Tagalog:tl_PH,Tamil:ta_IN,Telugu:te_IN,Thai:th_TH,Ukrainian:uk_UA,Vietnamese:vi_VN,Other:xx_XX";

        public static string getVed = string.Empty;

        #region getCountry()
        public Dictionary<string , string> getCountry()
        {
            Dictionary<string, string> CountryCode = new Dictionary<string, string>();
            List<string> lststate = new List<string>();
            lststate = spliter(country);

            foreach (string Country in lststate)
            {
                try
                {
                    string[] array = Regex.Split(Country, ":");
                    CountryCode.Add(array[0], array[1]);
                }
                catch (Exception ex)
                {
 
                }
            }

            return CountryCode;
        }
        #endregion

        #region Dictionary<string , string> getIndustry()
        public Dictionary<string , string> getIndustry()
        {
            Dictionary<string, string> IndustryCode = new Dictionary<string, string>();
            List<string> lststate = new List<string>();
            lststate = spliter(Industry);

            foreach (string Country in lststate)
            {
                try
                {
                    string[] array = Regex.Split(Country, ":");
                    IndustryCode.Add(array[0], array[1]);
                }
                catch (Exception ex)
                {
 
                }
            }

            return IndustryCode;
        }
        #endregion

        #region Dictionary<string, string> getLangauge()
        public Dictionary<string, string> getLangauge()
        {
            Dictionary<string, string> LanguageCode = new Dictionary<string, string>();
            List<string> lstLang = new List<string>();
            lstLang = spliter(Language);

            foreach (string Lang in lstLang)
            {
                try
                {
                    string[] array = Regex.Split(Lang, ":");
                    LanguageCode.Add(array[0], array[1]);
                }
                catch { }
                
            }
            return LanguageCode;
        }
        #endregion

        #region List<string> spliter(string data)
        public List<string> spliter(string data)
        {
            List<string> list = new List<string>();
            string[] statedata = data.Split(',');
            foreach (string item in statedata)
            {
                list.Add(item);
            }
            return list;
        }
        #endregion
    }
}

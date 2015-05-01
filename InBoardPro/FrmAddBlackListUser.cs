using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;
using InBoardPro;
using BaseLib;

namespace InBoardPro
{
    public partial class FrmAddBlackListUser : Form
    {
        public System.Drawing.Image image;
        List<string> listProfileIDWithName = new List<string>();

        public FrmAddBlackListUser()
        {
            InitializeComponent();
        }

        private void FrmAddBlackListUser_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.background;

            View();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            foreach (var item in listProfileIDWithName)
            {
                InsertComposeMsgData(item);
            }
        }


        #region InsertComposeMsgData
        public void InsertComposeMsgData(string ProfileIDWithName)
        {
            string ProfileID = ProfileIDWithName;
            string ProfileName = "";
            
            try
            {
                string strQuery = "INSERT INTO tb_BlackListAccount (ProfileID,ProfileName,DateTime) VALUES('" + ProfileID + "','" + ProfileName + "','" + DateTime.Now + "')";
                DataBaseHandler.InsertQuery(strQuery, "tb_BlackListAccount");
            }
            catch (Exception)
            { }
        }
        #endregion

        public void View()
        {
            try
            {
                string select_query = "select * from tb_BlackListAccount";
                DataSet select_ds = DataBaseHandler.SelectQuery(select_query, "tb_BlackListAccount");
                DGridViewBlackListUserDetails.DataSource = select_ds.Tables[0];
            }
            catch { }
        }

        private void btnLoadAccounts_Click(object sender, EventArgs e)
        {
            View();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FrmAddBlackListUser_Paint(object sender, PaintEventArgs e)
        {

            Graphics g;
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

        

        private void btnBrowse_AddProfileID_Click(object sender, EventArgs e)
        {
            try
            {
                txtBlackListIDPath.Text = string.Empty;
                Thread thread_FirstName = new Thread(ProfileIDWithName);
                thread_FirstName.SetApartmentState(ApartmentState.STA);
                thread_FirstName.Start();
            }
            catch { }
        }

        public void ProfileIDWithName()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        txtBlackListIDPath.Text = ofd.FileName;
                    }));


                    listProfileIDWithName.Clear();

                    List<string> templist = GlobusFileHelper.ReadFile(ofd.FileName);
                    foreach (string item in templist)
                    {
                        string newItem = item.Replace(" ", "").Replace("\t", "");
                        if (!listProfileIDWithName.Contains(newItem) && !string.IsNullOrEmpty(newItem))
                        {
                            listProfileIDWithName.Add(newItem);
                            
                        }
                    }

                    listProfileIDWithName = listProfileIDWithName.Distinct().ToList();
                }
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {


                bool value = false;

                foreach (DataGridViewRow row in DGridViewBlackListUserDetails.Rows)
                {
                    try
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value) == true)
                        {
                            value = true;

                            break;
                        }
                    }
                    catch { };
                }

                if (!value)
                {
                    MessageBox.Show("Please Select Account");
                    return;
                }


                bool Flage = false;
                List<int> delete = new List<int>();
                if (DGridViewBlackListUserDetails.Rows.Count != 0)
                {
                    try
                    {
                        try
                        {
                            for (int i = 0; i <= DGridViewBlackListUserDetails.Rows.Count - 1; i++)
                            {
                                if (Convert.ToBoolean(DGridViewBlackListUserDetails.Rows[i].Cells[0].Value) == true)
                                {

                                    delete.Add(i);

                                }


                            }

                            DialogResult result = MessageBox.Show("Do you want to delete selected Accounts permanently", "Delete Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                foreach (var delitm in delete)
                                {
                                    string email = DGridViewBlackListUserDetails.Rows[delitm].Cells[1].Value.ToString();
                                    DeleteBlackListUser(email);
                                    Flage = true;
                                }

                            }

                            else
                            {
                                Flage = false;
                            }


                        }
                        catch { }

                        if (Flage)
                        {

                            chkSelectAll.Checked = false;
                            View();
                        }
                    }
                    catch
                    {
                    }
                }
                else
                {
                    MessageBox.Show("Please Selected Account");

                }

            }
            catch { };
        }

        private void DeleteBlackListUser(string ID)
        {
            try
            {
                string delete_query = "delete from tb_BlackListAccount where ProfileID = '" + ID.ToString() + "'";
                DataSet select_ds = DataBaseHandler.SelectQuery(delete_query, "tb_BlackListAccount");
                DGridViewBlackListUserDetails.DataSource = select_ds.Tables[0];
            }
            catch { }
        }

        private void chkSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i <= DGridViewBlackListUserDetails.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(DGridViewBlackListUserDetails.Rows[i].Cells[0].Value) == false)
                    {
                        if (chkSelectAll.Checked == true)
                        {
                            DGridViewBlackListUserDetails.Rows[i].Cells[0].Value = true;

                        }
                        else
                        {
                            DGridViewBlackListUserDetails.Rows[i].Cells[0].Value = false;
                        }
                    }
                    else
                    {
                        if (chkSelectAll.Checked == true)
                        {
                            DGridViewBlackListUserDetails.Rows[i].Cells[0].Value = true;
                        }
                        else
                        {
                            DGridViewBlackListUserDetails.Rows[i].Cells[0].Value = false;
                        }
                    }

                }
            }
            catch { }
        }
    }
}

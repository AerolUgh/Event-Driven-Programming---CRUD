using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spire.Xls;
using System.IO;
using System.Threading;

namespace EVDRV
{
    public partial class Form4: Form
    {
        Workbook book = new Workbook();
        int basketball = 0;
        int volleyball = 0;
        int onlinegames = 0;
        int others = 0;
        string name;
        FileSystemWatcher fileWatcher;
        
        
        public Form4(string Name)
        {
            InitializeComponent();
            name = Name;
            InitializeFileWatcher();
            LoadData();
            DataSorting.datasorting();
        }

        private int ShowCounts(int c, string value)
        {
            book.LoadFromFile(path.pathfile); //Change the path to where is the excel locate.
            Worksheet sheet = book.Worksheets[0];
            int count = 0;
            int rows = sheet.Rows.Length;

            for (int i = 2; i <= rows; i++)
            {
                if (sheet.Range[i, c].Value == value)
                {
                    count++;
                }
            }
            return count;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            
            DisplayLogs();
        }
        private void InitializeFileWatcher()
        {
            fileWatcher = new FileSystemWatcher();
            fileWatcher.Path = Path.GetDirectoryName(path.pathfile);
            fileWatcher.Changed += fileSystemWatcher1_Changed;
            fileWatcher.EnableRaisingEvents = true; // Start watching
        }

        public void DisplayLogs()
        {
            book.LoadFromFile(path.pathfile); //Change the path to where is the excel locate.
            Worksheet sheet = book.Worksheets[1];
            DataTable dt = new DataTable();
            dt = sheet.ExportDataTable();

            dataGridView1.DataSource = dt;
        }

        public void LoadData()
        {
            book.LoadFromFile(path.pathfile); //Change the path to where is the excel locate.
            Worksheet sheet = book.Worksheets[0];

            int Rows = sheet.Rows.Length;

            for (int i = 2; i < Rows; i++)
            {
                string values = sheet.Range[i, 3].Value;
                string[] data = values.Split(' ');
                foreach (var hobbies in data)
                {
                    if (hobbies.Contains("Basketball"))
                    {
                        basketball++;
                    }
                    if (hobbies.Contains("Volleyball"))
                    {
                        volleyball++;
                    }
                    if (hobbies.Contains("Online-Games"))
                    {
                        onlinegames++;
                    }
                    if (hobbies.Contains("Others."))
                    {
                        others++;
                    }
                }
            }

            timer1.Start();
            lblActive.Text = ShowCounts(11, "1").ToString();
            lblInactive.Text = ShowCounts(11, "0").ToString();
            lblMale.Text = ShowCounts(2, "Male").ToString();
            lblFemale.Text = ShowCounts(2, "Female").ToString();
            lblBlue.Text = ShowCounts(4, "Blue").ToString();
            lblYellow.Text = ShowCounts(4, "Yellow").ToString();
            lblBlack.Text = ShowCounts(4, "Black").ToString();
            lblWhite.Text = ShowCounts(4, "White").ToString();
            lblPink.Text = ShowCounts(4, "Pink").ToString();
            lblRed.Text = ShowCounts(4, "Red").ToString();
            lblOrange.Text = ShowCounts(4, "Orange").ToString();
            lblGreen.Text = ShowCounts(4, "Green").ToString();
            lblBSIT.Text = ShowCounts(6, "BSIT").ToString();
            lblBSComEng.Text = ShowCounts(6, "BSComEng").ToString();
            lblBSCS.Text = ShowCounts(6, "BSCS").ToString();
            lblBSNursing.Text = ShowCounts(6, "BSNursing").ToString();
            lblBasketball.Text = basketball.ToString();
            lblVolleyball.Text = volleyball.ToString();
            lblOL.Text = onlinegames.ToString();
            lblOthers.Text = others.ToString();
            lblName.Text = name;

            basketball = volleyball = onlinegames = others = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime currentDateTime = DateTime.Now;
            dateTimePicker1.Value = currentDateTime;
            lblDate.Text = currentDateTime.ToString("MM/dd/yyyy hh:mm:ss tt");
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            panel10.Visible = false;
        }

        private void btnInactive_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
            panel10.Visible = false;
        }

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(500);

            if (this.IsHandleCreated)
            {
                this.Invoke(new Action(() =>
                {
                    LoadData();
                }));
            }
        }

        private void btnLogs_Click(object sender, EventArgs e)
        {
            panel10.Visible = true;
            panel2.Visible = false;
            DisplayLogs();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure you want to logout? ", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                Logs.Log(name, "Has been log out");
                Form3 form3 = new Form3();
                this.Hide();
                form3.Show();
            }
        }

        private void btnAddStud_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            Form1 form1 = new Form1(form);
            form1.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();

            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = null;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    string firstName = row.Cells[0].Value?.ToString().ToLower();

                    row.Visible = string.IsNullOrEmpty(searchText) || (firstName != null && firstName.Contains(searchText));
                }
            }
        }
    }
}

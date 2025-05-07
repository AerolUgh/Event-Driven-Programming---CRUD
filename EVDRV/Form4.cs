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
using System.Windows.Forms.DataVisualization.Charting;

namespace EVDRV
{
    public partial class Form4: Form
    {
        Workbook book = new Workbook();
        string name;
        FileSystemWatcher fileWatcher;
        
        public Form4(string Name)
        {
            InitializeComponent();
            name = Name;
            InitializeFileWatcher();
            DataSorting.datasorting();
            LoadPieChartForInActiveAndActive();
            LoadPieChartForMaleAndFemale();
            LoadBarChartForColors();
            LoadBarChartForHobbies();
            LoadBarChartForCourses();
            chart1.Titles.Add("Students");
            chart2.Titles.Add("Gender");
            chart3.Titles.Add("Colors");
            chart4.Titles.Add("Hobbies");
            chart5.Titles.Add("Colors");
            lblName.Text = name;
        }

        private void LoadPieChartForInActiveAndActive()
        {
            chart1.Series.Clear();

            Series series = new Series
            {
                Name = "Students",
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Pie
            };

            chart1.Series.Add(series);

            series.Points.AddXY($"Active \n{ShowCounts(11, "1")}", ShowCounts(11, "1"));
            series.Points.AddXY($"Inactive \n{ShowCounts(11, "0")}", ShowCounts(11, "0"));
        }

        private void LoadPieChartForMaleAndFemale()
        {
            chart2.Series.Clear();

            Series series = new Series
            {
                Name = "Gender",
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Pie
            };

            chart2.Series.Add(series);
            
            series.Points.AddXY($"Male \n{ShowCounts(2, "Male")}", ShowCounts(2, "Male"));
            series.Points.AddXY($"Female \n{ShowCounts(2, "Female")}", ShowCounts(2, "Female"));
        }

        private void LoadBarChartForColors()
        {
            chart3.Series.Clear();
            

            Series series = new Series
            {
                Name = "Colors",
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Bar
            };

            chart3.Series.Add(series);

            series.Points.AddXY($"Blue", ShowCounts(4, "Blue"));
            series.Points.AddXY($"Yellow", ShowCounts(4, "Yellow"));
            series.Points.AddXY($"Black", ShowCounts(4, "Black"));
            series.Points.AddXY($"White", ShowCounts(4, "White"));
            series.Points.AddXY($"Pink", ShowCounts(4, "Pink"));
            series.Points.AddXY($"Red", ShowCounts(4, "Red"));
            series.Points.AddXY($"Orange", ShowCounts(4, "Orange"));
            series.Points.AddXY($"Green", ShowCounts(4, "Green"));
        }

        private void LoadBarChartForCourses()
        {
            chart5.Series.Clear();
            

            Series series = new Series
            {
                Name = "Colors",
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Bar
            };

            chart5.Series.Add(series);
            
            series.Points.AddXY($"BSIT", ShowCounts(6, "BSIT"));
            series.Points.AddXY($"BSComEng", ShowCounts(6, "BSComEng"));
            series.Points.AddXY($"BSCS", ShowCounts(6, "BSCS"));
            series.Points.AddXY($"BSNursing", ShowCounts(6, "BSNursing"));
        }

        private void LoadBarChartForHobbies()
        {
            int basketball = 0;
            int volleyball = 0;
            int onlinegames = 0;
            int others = 0;

            book.LoadFromFile(path.pathfile);
            Worksheet sheet = book.Worksheets[0];

            int Rows = sheet.Rows.Length;

            for (int i = 2; i < Rows; i++)
            {
                string values = sheet.Range[i, 3].Value;
                string[] data = values.Split(' ');
                foreach (var hobbies in data)
                {
                    if (hobbies.Contains("Basketball")) basketball++;
                    if (hobbies.Contains("Volleyball")) volleyball++;
                    if (hobbies.Contains("Online-Games")) onlinegames++;
                    if (hobbies.Contains("Others.")) others++;
                }
            }

            chart4.Series.Clear();
            
            Series series = new Series
            {
                Name = "Hobbies",
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Bar
            };

            chart4.Series.Add(series);

            series.Points.AddXY($"Basketball", basketball);
            series.Points.AddXY($"Volleyball", volleyball);
            series.Points.AddXY($"Online-Games", onlinegames);
            series.Points.AddXY($"Others", others);
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
            try
            {
                string fullPath = path.pathfile;

                // Validate the path first
                if (string.IsNullOrWhiteSpace(fullPath) || !File.Exists(fullPath))
                {
                    MessageBox.Show("The file path is not valid or the file does not exist: " + fullPath);
                    return;
                }

                fileWatcher = new FileSystemWatcher
                {
                    Path = Path.GetDirectoryName(fullPath),
                    Filter = Path.GetFileName(fullPath), // Watch only the specific file
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName
                };

                fileWatcher.Changed += FileSystemWatcher1_Changed;
                fileWatcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing file watcher: " + ex.Message);
            }
        }

        private void FileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(500); // Wait for the file write to complete

            if (this.IsHandleCreated)
            {
                this.Invoke(new Action(() =>
                {
                    try
                    {
                        fileWatcher.EnableRaisingEvents = false; // 🛑 Stop watching

                        LoadPieChartForInActiveAndActive();
                        LoadPieChartForMaleAndFemale();
                        LoadBarChartForColors();
                        LoadBarChartForHobbies();
                        LoadBarChartForCourses();
                    }
                    finally
                    {
                        fileWatcher.EnableRaisingEvents = true; // ✅ Resume watching
                    }
                }));
            }

        }


        public void DisplayLogs()
        {
            book.LoadFromFile(path.pathfile); //Change the path to where is the excel locate.
            Worksheet sheet = book.Worksheets[1];
            DataTable dt = new DataTable();
            dt = sheet.ExportDataTable();

            dataGridView1.DataSource = dt;
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
            panel2.Visible = true;
            panel10.Visible = false;
        }

        private void btnInactive_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
            panel2.Visible = true;
            panel10.Visible = false;
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

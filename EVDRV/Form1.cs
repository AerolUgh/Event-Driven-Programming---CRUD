﻿using Spire.Xls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace EVDRV
{
    public partial class Form1 : Form
    {
        private Form2 form2;
        Form4 form4;
        Workbook book = new Workbook();
        string message = "";

        public Form1(Form2 form)
        {
            InitializeComponent();
            form2 = form;
            form4 = new Form4(Admin.Name);
            lblName.Text = Admin.Name;
            pictureBox6.ImageLocation = path.picpath;
        }

        private bool ValidateMyForm()
        {
            bool isValid = true;
            message = string.Empty;
            bool hasRadioSelected = false;
            bool hasCheckboxSelected = false;

            foreach (Control ctrl in GetAllControls(this))
            {
                if (ctrl is System.Windows.Forms.TextBox textBox)
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        message += (textBox.Tag?.ToString() ?? textBox.Name) + " is required.\n";
                        textBox.BackColor = Color.LightPink;
                        isValid = false;
                    }
                    else
                    {
                        textBox.BackColor = Color.White;
                    }
                }
                else if (ctrl is System.Windows.Forms.ComboBox comboBox)
                {
                    if (string.IsNullOrWhiteSpace(comboBox.Text))
                    {
                        message += (comboBox.Tag?.ToString() ?? "Favorite Color") + " is required.\n";
                        comboBox.BackColor = Color.LightPink;
                        isValid = false;
                    }
                    else
                    {
                        comboBox.BackColor = Color.White;
                    }
                }
                else if (ctrl is RadioButton radioButton)
                {
                    if (radioButton.Checked)
                    {
                        hasRadioSelected = true;
                    }
                }
                else if (ctrl is CheckBox checkBox)
                {
                    if (checkBox.Checked)
                    {
                        hasCheckboxSelected = true;
                    }
                }
            }

            if (!hasRadioSelected)
            {
                message += "Please select a radio button option.\n";
                isValid = false;
            }

            if (!hasCheckboxSelected)
            {
                message += "Please check at least one checkbox.\n";
                isValid = false;
            }

            if (!isValid)
            {
                MessageBox.Show(message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return isValid;
        }

        private IEnumerable<Control> GetAllControls(Control container)
        {
            foreach (Control ctrl in container.Controls)
            {
                foreach (Control child in GetAllControls(ctrl))
                {
                    yield return child;
                }
                yield return ctrl;
            }
        }

        public void InsertUpdatedData(int ID, string name, string rad, string chk, string favcolor, string saying, string course, string username, string password, string status, string email, string profilepath)
        {
            try
            {
                
                if (radFemale.Checked)
                {
                    rad = radFemale.Text;
                }
                else if (radMale.Checked)
                {
                    rad = radMale.Text;
                }

                if (chkBasketball.Checked)
                {
                    chk += $"{chkBasketball.Text} ";
                }
                if (chkOG.Checked)
                {
                    chk += $"{chkOG.Text} ";
                }
                if (chkVolleyball.Checked)
                {
                    chk += $"{chkVolleyball.Text} ";
                }
                if (chkOthers.Checked)
                {
                    chk += $"{chkOthers.Text} ";
                }

                ID = Convert.ToInt32(lblID.Text);

                form2.GetUpdatedDataFromFr1(ID, name, rad, chk, favcolor, saying, course, username, password, status, email, pathpic, CalculateAge(dateTimePicker1.Value).ToString());
                form2.UpdateDataToExcel(ID, name, rad, chk, favcolor, saying, course, username, password, status, email, pathpic, CalculateAge(dateTimePicker1.Value).ToString());
                form2.LoadActiveData();
                form2.Show();
                this.Hide();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void InsertData(string name, string gender, string hobbies, string favcolor, string saying, string course, string username, string password, string status, string email, string profilepath, string age)
        {
            try
            {
                book.LoadFromFile(path.pathfile); //Change the path to where is the excel locate.
                Worksheet sheet = book.Worksheets[0];
                int i = sheet.Rows.Length + 1;

                string rad = "";
                string chk = "";
                if (radFemale.Checked)
                {
                    rad = radFemale.Text;
                }
                else if (radMale.Checked)
                {
                    rad = radMale.Text;
                }

                if (chkBasketball.Checked)
                {
                    chk += $"{chkBasketball.Text} ";
                }
                if (chkOG.Checked)
                {
                    chk += $"{chkOG.Text} ";
                }
                if (chkVolleyball.Checked)
                {
                    chk += $"{chkVolleyball.Text} ";
                }
                if (chkOthers.Checked)
                {
                    chk += $"{chkOthers.Text} ";
                }

                //string data = "";

                //data += $"{txtName.Text}, ";
                //data += $"{rad}, ";
                //data += $"{chk}, ";
                //data += $"{cmbFavcolor.SelectedItem}, ";
                //data += $"{txtSaying.Text}";

                //people[i] = data;


                //form2.GetDataFromFr1(name, rad, chk, favcolor, saying);

                sheet.Range[i, 1].Value = name;
                sheet.Range[i, 2].Value = rad;
                sheet.Range[i, 3].Value = chk;
                sheet.Range[i, 4].Value = favcolor;
                sheet.Range[i, 5].Value = saying;
                sheet.Range[i, 6].Value = course;
                sheet.Range[i, 7].Value = email;
                sheet.Range[i, 8].Value = age;
                sheet.Range[i, 9].Value = username;
                sheet.Range[i, 10].Value = password;
                sheet.Range[i, 11].Value = status;
                sheet.Range[i, 12].Value = profilepath;

                book.SaveToFile(path.pathfile);
                Logs.Log(Admin.Name, $"Inserting a data");
                Form4 form4 = new Form4(Admin.Name);

                SendBirthdateToExcel(txtUserName.Text, dateTimePicker1.Value);

                DataSorting.datasorting();

                txtName.Text = string.Empty;
                txtSaying.Text = string.Empty;
                txtUserName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                radFemale.Checked = false;
                radMale.Checked = false;
                chkBasketball.Checked = false;
                chkOG.Checked = false;
                chkOthers.Checked = false;
                chkVolleyball.Checked = false;
                cmbFavcolor.Text = string.Empty;
                cmbStatus.Text = string.Empty;
                cmbCourses.Text = string.Empty;
                txtEmail.Text = string.Empty;
                pictureBox1.Image = null;
                dateTimePicker1.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SendBirthdateToExcel(string name, DateTime date)
        {
            book.LoadFromFile(path.pathfile); //Change the path to where is the excel locate.
            Worksheet sheet = book.Worksheets[2];

            int i = sheet.Rows.Length + 1;

            sheet.Range[i, 1].Value = name;
            sheet.Range[i, 2].Value = date.ToString("MM/dd/yyyy");

            book.SaveToFile(path.pathfile);
        }

        public int CalculateAge(DateTime date)
        {
            int age = 0;
            if (date < DateTime.Now)
            {
                int days = (DateTime.Now - dateTimePicker1.Value).Days;
                age = days / 365;
            }

            return age;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string rad = "";
            string chk = "";
            bool isRepeated = false;

            book.LoadFromFile(path.pathfile); //Change the path to where is the excel locate.
            Worksheet sheet = book.Worksheets[0];
            if (ValidateMyForm())
            {
                
                string email = txtEmail.Text;
                string gmailPattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
                if (!Regex.IsMatch(email, gmailPattern))
                {
                    MessageBox.Show("Please enter a valid Gmail address (e.g., example@gmail.com).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }
                else
                {
                    for (int i = 2; i <= sheet.Rows.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(txtUserName.Text) && sheet.Range[i, 9].Value == txtUserName.Text)
                        {
                            isRepeated = true;
                            break;
                        }
                        else
                        {
                            isRepeated = false;
                        }
                    }
                }

                if (isRepeated == true)
                {
                    MessageBox.Show("Username already exists. Please choose a different username.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUserName.Focus();
                }
                else if (isRepeated == false)
                {
                    if(pictureBox1.ImageLocation != null)
                    {
                        if (dateTimePicker1.Value >= DateTime.Now)
                        {
                            MessageBox.Show("Please enter Correct Date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            ProfileSaveToFolder();

                            InsertData(txtName.Text, rad, chk, cmbFavcolor.Text, txtSaying.Text, cmbCourses.Text, txtUserName.Text, txtPassword.Text, cmbStatus.Text, txtEmail.Text, pathpic, CalculateAge(dateTimePicker1.Value).ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("No image in PictureBox to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            this.Hide();
            form2.Show();
            form2.LoadActiveData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string rad = "";
            string chk = "";

            book.LoadFromFile(path.pathfile); //Change the path to where is the excel locate.
            Worksheet sheet = book.Worksheets[0];

            if (ValidateMyForm())
            {
                string email = txtEmail.Text;
                string gmailPattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
                if (!Regex.IsMatch(email, gmailPattern))
                {
                    MessageBox.Show("Please enter a valid Gmail address (e.g., example@gmail.com).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }
                else
                {
                    if (pictureBox1.ImageLocation != null)
                    {
                        if (dateTimePicker1.Value >= DateTime.Now)
                        {
                            MessageBox.Show("Please enter Correct Date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            int ID = Convert.ToInt32(lblID.Text);

                            MessageBox.Show("Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            ProfileSaveToFolder();

                            if (lblName.Text == Admin.Name)
                            {
                                form4.pictureBox1.ImageLocation = path.picpath;
                            }

                            InsertUpdatedData(ID, txtName.Text, rad, chk, cmbFavcolor.Text, txtSaying.Text, cmbCourses.Text, txtUserName.Text, txtPassword.Text, cmbStatus.Text, txtEmail.Text, pathpic);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No image in PictureBox to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnChoosePic_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            pictureBox1.ImageLocation = openFileDialog1.FileName;
        }

        private string pathpic;


        private void ProfileSaveToFolder()
        {
            if (pictureBox1.Image != null)
            {
                //Get the project directory(two levels up from bin\Debug or bin\Release)
                string projectDir = Directory.GetParent(Application.StartupPath).Parent.Parent.FullName;
                string savedPhotoFolder = Path.Combine(projectDir, "EVDRV", "Profiles");

                // Ensure the folder exists
                if (!Directory.Exists(savedPhotoFolder))
                {
                    Directory.CreateDirectory(savedPhotoFolder);
                }

                // Save the image using the username as the file name
                string fileName = txtUserName.Text + ".png";
                string savePath = Path.Combine(savedPhotoFolder, fileName);

                pathpic = savePath;

                pictureBox1.Image.Save(savePath, ImageFormat.Png);
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime currentDateTime = DateTime.Now;
            dateTimePicker2.Value = currentDateTime;
            lblDate.Text = currentDateTime.ToString("MM/dd/yyyy hh:mm:ss tt");
        }
    }
}

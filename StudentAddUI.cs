using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DojoStudentManagement
{
    public partial class StudentAddUI : Form
    {
        IDataAccess dataAccess;
        public event EventHandler StudentAdded;

        public StudentAddUI(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
            InitializeComponent();
        }

        private bool ValidateStudentData()
        {
            bool isDataValid = false;
            StudentMaintenanceFunctions s = new StudentMaintenanceFunctions();
            s.IsValidEmail(txtEmailAddress.Text);

            return isDataValid;
        }

        private void CreateNewStudent()
        {
            Student newStudent = new Student();
            newStudent.StudentID = 0000;    //Default for a new student; will autogenerate actual student ID when it gets added to the database
            newStudent.FirstName = txtFirstName.Text.Trim();
            newStudent.LastName = txtLastName.Text.Trim();
            newStudent.ActiveMember = cbActiveStudent.Checked;
            newStudent.Address1 = txtAddress1.Text.Trim();
            newStudent.Address2 = txtAddress2.Text.Trim();
            newStudent.AddressCity = txtCity.Text.Trim();
            newStudent.AddressState = txtState.Text.Trim();
            newStudent.AddressZip = txtZipCode.Text.Trim();
            newStudent.EmailAddress = txtEmailAddress.Text.Trim();
            newStudent.HomeDojo = txtHomeDojo.Text.Trim();
            newStudent.PrimaryPhoneNumber = txtPrimaryPhone.Text.Trim();
            newStudent.SecondaryPhoneNumber = txtSecondaryPhone.Text.Trim();
            newStudent.DateOfBirth = dtBirthdate.Value;
            newStudent.StartMonth = DateTime.Today.Month;
            newStudent.StudentGender = SetGender();

            if (dataAccess.AddNewStudent(newStudent))
                MessageBox.Show($"Student {newStudent.FullName} successfully added.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private Gender SetGender()
        {
            if (rbMale.Checked)
                return Gender.MALE;
            else if (rbFemale.Checked)
                return Gender.FEMALE;
            else
                return Gender.UNKNOWN;
        }

        protected virtual void OnStudentAdded()
        {
            StudentAdded?.Invoke(this, EventArgs.Empty);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ValidateStudentData();
            CreateNewStudent();
            OnStudentAdded();
        }

        private void StudentAddUI_Load(object sender, EventArgs e)
        {

        }
    }
}

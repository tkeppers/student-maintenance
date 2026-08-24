using System;

namespace DojoStudentManagement
{
    public class Dojo
    {
        public string ClubID { get; set; }
        public string Name { get; set; }
        public string Instructor { get; set; }
        public string InstructorEmail { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public bool Active { get; set; }
        public string Notes { get; set; }
        public decimal AnnualDues { get; set; }
    }
}

namespace DojoStudentManagement
{
    /// <summary>
    /// Backing item for a dojo selector, so a combo can show a friendly dojo name while carrying
    /// the club id that Students.stud_club actually has to hold.
    /// </summary>
    internal class DojoOption
    {
        public string ClubID { get; set; }
        public string Display { get; set; }
    }
}

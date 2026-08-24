using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace DojoStudentManagement
{
    public class KubkManagementFunctions
    {
        private const decimal DefaultAnnualDuesFallback = 70m;

        private readonly IDataRepository dataRepository;

        public KubkManagementFunctions(IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        public List<Dojo> GetDojos()
        {
            return dataRepository.GetDojos();
        }

        public bool SaveDojo(Dojo dojo, bool isNew, out string validationError)
        {
            if (!ValidateDojo(dojo, out validationError))
                return false;

            bool success = isNew ? dataRepository.AddDojo(dojo) : dataRepository.UpdateDojo(dojo);

            if (!success)
                Log.Error($"Failed to save dojo {dojo.ClubID}");

            return success;
        }

        public bool ValidateDojo(Dojo dojo, out string validationError)
        {
            validationError = string.Empty;

            if (dojo == null)
            {
                validationError = "Dojo information is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(dojo.ClubID))
            {
                validationError = "Club ID is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(dojo.Name))
            {
                validationError = "Dojo name is required.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(dojo.InstructorEmail) && !IsValidEmail(dojo.InstructorEmail))
            {
                validationError = "Instructor email address is not valid.";
                return false;
            }

            return true;
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch (Exception e)
            {
                Log.Error($"Error validating email address {email}\n{e.Message}\n{e.Source}\n{e.StackTrace}");
                return false;
            }
        }

        public DataTable GetKubkRoster(string clubId, int duesYear)
        {
            return dataRepository.GetKubkRoster(clubId, duesYear);
        }

        /// <summary>
        /// Default annual dues amount from App.config's KubkAnnualDuesAmount setting, falling
        /// back to $70 if the key is missing or not a parseable decimal. The caller can always
        /// override this with a different amount when recording a payment.
        /// </summary>
        public decimal GetDefaultAnnualDuesAmount()
        {
            return ParseAnnualDuesAmount(ConfigurationManager.AppSettings["KubkAnnualDuesAmount"]);
        }

        /// <summary>
        /// Pure parsing logic behind GetDefaultAnnualDuesAmount, split out so the missing/unparseable
        /// fallback path is directly testable without needing to control the process's App.config.
        /// </summary>
        public static decimal ParseAnnualDuesAmount(string configValue)
        {
            if (decimal.TryParse(configValue, out decimal amount))
                return amount;

            return DefaultAnnualDuesFallback;
        }

        public bool RecordDuesPayment(int studentID, int year, DateTime paidDate, decimal? amount = null)
        {
            var dues = new StudentDuesRecord
            {
                StudentID = studentID,
                Year = year,
                PaidDate = paidDate,
                Amount = amount ?? GetDefaultAnnualDuesAmount()
            };

            return dataRepository.RecordDuesPayment(dues);
        }

        public bool RemoveDuesPayment(int studentID, int year)
        {
            return dataRepository.RemoveDuesPayment(studentID, year);
        }

        public List<StudentDuesRecord> GetDuesHistory(int studentID)
        {
            return dataRepository.GetDuesHistory(studentID);
        }

        public bool IsDuesPaidForYear(int studentID, int year)
        {
            return dataRepository.GetDuesHistory(studentID).Any(d => d.Year == year && d.IsPaid);
        }

        public bool VerifyStudentRank(int studentID, string artName, DateTime verifiedDate)
        {
            return dataRepository.VerifyStudentRank(studentID, artName, verifiedDate);
        }
    }
}

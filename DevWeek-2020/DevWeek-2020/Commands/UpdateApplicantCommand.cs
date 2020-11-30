using System;
using DevWeek.Models;
using System.ComponentModel.DataAnnotations;

namespace DevWeek.Commands
{
    public class UpdateApplicantCommand
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string Country { get; set; }
        [Required, Range(1, Int32.MaxValue)]
        public int CompanyId { get; set; }

        public Applicant ToModel(string username)
        {
            return new Applicant()
            {
                FirstName = FirstName,
                LastName = LastName,
                Gender = Gender,
                Age = Age.GetValueOrDefault(-1),
                Country = Country,
                CompanyId = CompanyId,
                Username = username
            };
        }
    }

    public class TransferApplicantCommand
    {
        public TransferApplicantCommand(string username, in int company)
        {
            Username = username;
            Company = company;
        }

        public string Username { get; set; }
        public int Company { get; set; }
    }

}
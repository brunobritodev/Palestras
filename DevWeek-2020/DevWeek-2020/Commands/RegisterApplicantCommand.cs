using DevWeek.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevWeek.Commands
{
    public class RegisterApplicantCommand
    {
        private static Random rnd = new Random();
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }

        public string Country { get; set; }

        public Applicant ToEntity()
        {
            var username = Username;
            if (string.IsNullOrEmpty(username))
                username = $"{FirstName}_{LastName}{rnd.Next(1000)}";
            return new Applicant()
            {
                Age = Age,
                CompanyId = CompanyId,
                FirstName = FirstName,
                LastName = LastName,
                Gender = Gender,
                Username = username.Trim(),
                Country = Country,
                Active = true,
            };
        }
    }
}

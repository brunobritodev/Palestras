using DevWeek.Commands;
using DevWeek.Contexts;
using DevWeek.Models;
using DevWeek.Notification;
using DevWeek.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevWeek.Service
{
    public class DummyUserService : IDummyUserService
    {
        private readonly IDomainNotificationMediatorService _domainNotification;
        private readonly RestfulContext _context;

        public DummyUserService(IDomainNotificationMediatorService domainNotification,
            RestfulContext context)
        {
            _domainNotification = domainNotification;
            _context = context;
        }
        private async Task CheckApplicants()
        {
            if (_context.Applicants.Any())
                return;

            var companies = Company.Get(250).Generate(2);

            await _context.Companies.AddRangeAsync(companies);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Applicant> Query()
        {
            CheckApplicants().Wait();
            return _context.Applicants.Where(w => w.Active).AsQueryable();
        }

        public async Task<IEnumerable<Applicant>> All()
        {
            await CheckApplicants();
            return await _context.Applicants.Where(w => w.Active).ToListAsync();
        }

        public async Task<string> Save(RegisterApplicantCommand command)
        {
            await CheckApplicants();

            var user = command.ToEntity();
            if (!(await CheckIfUserIsValid(user)))
                return null;

            var usernameExists = await Find(command.Username);
            if (usernameExists != null)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Username already exists"));
                return null;
            }

            await _context.Applicants.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Username;
        }

        public async Task Update(Applicant applicant)
        {
            await CheckApplicants();

            if (!(await CheckIfUserIsValid(applicant)))
                return;

            var actua = await _context.Applicants.FirstOrDefaultAsync(f => f.Active && f.Username.ToUpper().Equals(applicant.Username.Trim().ToUpper()));
            if (actua == null)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Not found"));
                return;
            }
            actua.Update(applicant);
            _context.Applicants.Update(actua);

            await _context.SaveChangesAsync();
        }

        public async Task<Applicant> Remove(string username)
        {
            await CheckApplicants();

            var actual = await _context.Applicants.FirstOrDefaultAsync(f => f.Username.ToUpper().Equals(username.Trim().ToUpper()));
            if (actual != null)
            {
                actual.Delete();
                _context.Applicants.Update(actual);
            }
            else
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Not found"));
                return null;
            }
            await _context.SaveChangesAsync();
            return actual;
        }

        public async Task Approve(string username)
        {
            await CheckApplicants();

            var applicant = await Find(username);
            if (applicant == null)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Not found"));
                return;
            }

            applicant.Approve();
            await _context.SaveChangesAsync();
        }

        public async Task Decline(string username)
        {
            await CheckApplicants();

            var applicant = await Find(username);
            if (applicant == null)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Not found"));
                return;
            }

            applicant.Decline();
            await _context.SaveChangesAsync();
        }

        public async Task<Applicant> Transfer(TransferApplicantCommand command)
        {
            await CheckApplicants();

            var applicant = await Find(command.Username);
            if (applicant == null)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Not found"));
                return null;
            }
            if (command.Company == applicant.CompanyId)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Can't transfer for same company"));
                return null;
            }
            if (command.Company <= 0)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Invalid company"));
                return null;
            }

            applicant.Delete();
            var newApplicant = new Applicant(applicant, command.Company);
            _context.Update(applicant);
            await _context.AddAsync(newApplicant);
            await _context.SaveChangesAsync();
            return newApplicant;
        }



        private async Task<bool> CheckIfUserIsValid(Applicant command)
        {
            var valid = true;
            if (string.IsNullOrEmpty(command.FirstName))
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Invalid firstname"));
                valid = false;
            }

            if (string.IsNullOrEmpty(command.LastName))
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Invalid firstname"));
                valid = false;
            }

            if (command.CompanyId <= 0)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Invalid company"));
                valid = false;
            }

            var companyDb = await FindCompany(command.CompanyId);
            if (companyDb == null)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Company not found"));
                valid = false;
            }


            return valid;
        }

        private Task<Company> FindCompany(int companyId)
        {
            return _context.Companies.FirstOrDefaultAsync(f => f.Id == companyId);
        }

        public Task<Applicant> Find(string username)
        {
            if (string.IsNullOrEmpty(username))
                return Task.FromResult<Applicant>(null);

            return _context.Applicants.AsNoTrackingWithIdentityResolution()
                .Include(i => i.Company)
                .FirstOrDefaultAsync(f => f.Active && f.Username.ToUpper().Equals(username.Trim().ToUpper()));

        }
    }
}

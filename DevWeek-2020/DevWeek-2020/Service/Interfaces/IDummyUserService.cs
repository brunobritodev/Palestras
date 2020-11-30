using DevWeek.Commands;
using DevWeek.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevWeek.Service.Interfaces
{
    public interface IDummyUserService
    {
        IQueryable<Applicant> Query();
        Task<IEnumerable<Applicant>> All();
        Task<Applicant> Find(string id);
        Task<string> Save(RegisterApplicantCommand command);
        Task Update(Applicant actualApplicant);
        Task<Applicant> Remove(string username);
        Task Approve(string username);
        Task Decline(string username);
        Task<Applicant> Transfer(TransferApplicantCommand command);
    }
}
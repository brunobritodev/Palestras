using AspNetCore.IQueryable.Extensions;
using AspNetCore.IQueryable.Extensions.Filter;
using AutoMapper;
using DevWeek.Commands;
using DevWeek.Models;
using DevWeek.Notification;
using DevWeek.Service.Interfaces;
using DevWeek.ViewModels;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DevWeek.Controllers
{
    [Route("applicants")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ApplicantsController : ApiBaseController
    {
        private readonly INotificationHandler<DomainNotification> _notifications;
        private readonly IDummyUserService _dummyUserService;
        private readonly IMapper _mapper;

        public ApplicantsController(
            INotificationHandler<DomainNotification> notifications,
            IDomainNotificationMediatorService mediator,
            IDummyUserService dummyUserService,
            IMapper mapper) : base(notifications, mediator)
        {
            _notifications = notifications;
            _dummyUserService = dummyUserService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all applicants, optionally filter them.
        /// </summary>
        /// <returns>List of <see cref="Applicant"/></returns>
        [HttpGet("")]
        public async Task<ActionResult<List<ApplicantViewModel>>> Get([FromQuery] ApplicantSearch search)
        {
            var result = _dummyUserService.Query().Apply(search);

            return ResponseGet(await _mapper.ProjectTo<ApplicantViewModel>(result).ToListAsync());
        }


        /// <summary>
        /// Get all olders applicants, which is actually declined.
        /// </summary> 
        /// <returns>List of <see cref="Applicant"/></returns>
        [HttpGet("youngers-from-brazil")]
        public async Task<ActionResult<List<ApplicantViewModel>>> GetYoungersFromBrazil()
        {
            var search = new ApplicantSearch() { OlderThan = 25, Country = "Brazil" };
            var expression = _dummyUserService.Query().FilterExpression(search);
            var result = _dummyUserService.Query().Apply(search);

            return ResponseGet(await _mapper.ProjectTo<ApplicantViewModel>(result).ToListAsync());
        }

        /// <summary>
        /// Get the specified Applicant
        /// </summary>
        /// <param name="username">username of Applicant</param>
        /// <returns><see cref="Applicant"/></returns>
        [HttpGet("{username}")]
        public async Task<ActionResult<Applicant>> GetByUsername(string username)
        {
            return ResponseGet(await _dummyUserService.Find(username));
        }

        /// <summary>
        /// Create new Applicant
        /// </summary>
        /// <param name="command"><see cref="RegisterApplicantCommand"/></param>
        /// <returns><see cref="Applicant"/></returns>
        [HttpPost("")]
        public async Task<ActionResult<Applicant>> Post([FromBody] RegisterApplicantCommand command)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            var username = await _dummyUserService.Save(command);
            var newUser = await _dummyUserService.Find(username);
            return ResponsePost(nameof(GetByUsername), new { username }, newUser);
        }

        /// <summary>
        /// Partially update an Applicant
        /// </summary>
        [HttpPatch("{username}")]
        public async Task<ActionResult> Patch(string username, [FromBody] JsonPatchDocument<Applicant> model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            var actualUser = await _dummyUserService.Find(username);
            if (actualUser == null)
            {
                await _notifications.Handle(new DomainNotification("Applicant", "Not found"), CancellationToken.None);
            }
            else
            {
                model.ApplyTo(actualUser);
                await _dummyUserService.Update(actualUser);
            }

            return ResponsePutPatch();
        }

        /// <summary>
        /// Update an Applicant
        /// </summary>
        [HttpPut("{username}")]
        public async Task<ActionResult> Put(string username, [FromBody] UpdateApplicantCommand command)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            var applicant = command.ToModel(username);
            await _dummyUserService.Update(applicant);
            return ResponsePutPatch();
        }

        /// <summary>
        /// Remove an Applicant
        /// </summary>
        [HttpDelete("{username}")]
        public async Task<ActionResult<Applicant>> Delete(string username)
        {
            return ResponseDelete(await _dummyUserService.Remove(username));
        }

        [HttpPut("{username}/approve")]
        public async Task<ActionResult<Applicant>> Approve(string username)
        {
            await _dummyUserService.Approve(username);

            return ResponsePutPatch();
        }

        [HttpPut("{username}/decline")]
        public async Task<ActionResult<Applicant>> Decline(string username)
        {
            await _dummyUserService.Decline(username);

            return ResponsePutPatch();
        }

        [HttpPost("{username}/transfer/{company}")]
        public async Task<ActionResult<Applicant>> Transfer(string username, int company)
        {

            await _dummyUserService.Transfer(new TransferApplicantCommand(username, company));

            var newUser = await _dummyUserService.Find(username);
            return ResponsePost(nameof(GetByUsername), new { username }, newUser);
        }

    }


}

using Refactoring.LegacyService.Candidates.Services;

namespace Refactoring.JuniorApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Refactoring.LegacyService;
    using System;

    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {
        private readonly ILogger<CandidatesController> _logger;
        private readonly ICandidateService _candidateService;

        public CandidatesController(ILogger<CandidatesController> logger, ICandidateService candidateService)
        {
            _logger = logger;

            _candidateService = candidateService;
        }

        [HttpPost]
        public IActionResult Add(CandidateRequest request)
        {
            try
            {
                if (ValidateCandidateRequest(request) is BadRequestObjectResult badResult)
                {
                    return badResult;
                }

                var result = _candidateService.AddCandidate(request.Firstname, request.Surname, request.EmailAddress, request.DateOfBirth, request.PositionId);
                return Accepted();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to add Candidate. PositionId: {request.PositionId}, Candidate Name: {request.Firstname} {request.Surname}");
                throw e;
            }
        }

        private BadRequestObjectResult ValidateCandidateRequest(CandidateRequest request)
        {
            if (string.IsNullOrEmpty(request.Firstname))
            {
                return BadRequest("Firstname cannot be null or empty");
            }

            if (string.IsNullOrEmpty(request.Surname))
            {
                return BadRequest("Surname cannot be null or empty");
            }

            if (string.IsNullOrEmpty(request.EmailAddress))
            {
                return BadRequest("EmailAddress cannot be null or empty");
            }

            if (request.PositionId < 0)
            {
                return BadRequest($"PositionId: {request.PositionId} is invalid");
            }

            if (request.DateOfBirth > DateTime.Now)
            {
                return BadRequest($"DateOfBirth: {request.DateOfBirth} is invalid");
            }

            return default;
        }
    }

    public class CandidateRequest
    {
        public DateTime DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public int PositionId { get; set; }
    }
}

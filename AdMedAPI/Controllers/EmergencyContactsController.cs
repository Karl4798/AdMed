using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdMedAPI.Models;
using AdMedAPI.Models.Dtos;
using AdMedAPI.Repository.IRepository;

namespace AdMedAPI.Controllers
{
    [Route("api/v{version:apiVersion}/EmergencyContacts")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecEmergencyContacts")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class EmergencyContactsController : ControllerBase
    {

        private readonly IEmergencyContactRepository _EmergencyContactRepo;
        private readonly IMapper _mapper;

        public EmergencyContactsController(IEmergencyContactRepository EmergencyContactRepo, IMapper mapper)
        {

            _EmergencyContactRepo = EmergencyContactRepo;
            _mapper = mapper;

        }

        /// <summary>
        /// Get list of EmergencyContacts.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<EmergencyContactCreateDto>))]
        public IActionResult GetEmergencyContacts()
        {

            var objList = _EmergencyContactRepo.GetEmergencyContacts();

            var objDto = new List<EmergencyContactCreateDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<EmergencyContactCreateDto>(obj));
            }

            return Ok(objDto);

        }

        /// <summary>
        /// Get individual EmergencyContact
        /// </summary>
        /// <param name="EmergencyContactId">The Id of the EmergencyContact</param>
        /// <returns></returns>
        [HttpGet("{EmergencyContactId:int}", Name="GetEmergencyContact")]
        [ProducesResponseType(200, Type = typeof(EmergencyContactCreateDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "Admin")]
        public IActionResult GetEmergencyContact(int EmergencyContactId)
        {

            var obj = _EmergencyContactRepo.GetEmergencyContact(EmergencyContactId);

            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<EmergencyContactCreateDto>(obj);

            return Ok(objDto);

        }

        [HttpGet("[action]/{applicationId:int}")]
        [ProducesResponseType(200, Type = typeof(EmergencyContactCreateDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetEmergencyContactInApplication(int applicationId)
        {

            var objList = _EmergencyContactRepo.GetEmergencyContactsInApplication(applicationId);

            if (objList == null)
            {
                return NotFound();
            }

            var objData = new List<EmergencyContactCreateDto>();

            foreach (var obj in objList)
            {
                objData.Add(_mapper.Map<EmergencyContactCreateDto>(obj));
            }

            return Ok(objData);

        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(EmergencyContactCreateDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateEmergencyContact([FromBody] EmergencyContactCreateDto EmergencyContactCreateDto)
        {

            if (EmergencyContactCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_EmergencyContactRepo.EmergencyContactExists(EmergencyContactCreateDto.FirstName))
            {
                ModelState.AddModelError("", "EmergencyContact Exists!");
                return StatusCode(404, ModelState);
            }

            var EmergencyContactObj = _mapper.Map<EmergencyContact>(EmergencyContactCreateDto);

            if (!_EmergencyContactRepo.CreateEmergencyContact(EmergencyContactObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {EmergencyContactObj}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetEmergencyContact", new { version = HttpContext.GetRequestedApiVersion().ToString(),
                                                                                 EmergencyContactId = EmergencyContactObj.Id },
                                                                            EmergencyContactObj);

        }

        [HttpPatch("{EmergencyContactId:int}", Name = "UpdateEmergencyContact")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateEmergencyContact(int EmergencyContactId, [FromBody] EmergencyContactUpdateDto EmergencyContactUpdateDto)
        {


            if (EmergencyContactUpdateDto == null || EmergencyContactId != EmergencyContactUpdateDto.Id)
            {
                return BadRequest(ModelState);
            }

            var EmergencyContactObj = _mapper.Map<EmergencyContact>(EmergencyContactUpdateDto);

            if (!_EmergencyContactRepo.UpdateEmergencyContact(EmergencyContactObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {EmergencyContactObj.IdentityNumber}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{EmergencyContactId:int}", Name = "DeleteEmergencyContact")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteEmergencyContact(int EmergencyContactId)
        {

            if (!_EmergencyContactRepo.EmergencyContactExists(EmergencyContactId))
            {
                return NotFound();
            }

            var EmergencyContactObj = _EmergencyContactRepo.GetEmergencyContact(EmergencyContactId);

            if (!_EmergencyContactRepo.DeleteEmergencyContact(EmergencyContactObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {EmergencyContactObj.IdentityNumber}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}

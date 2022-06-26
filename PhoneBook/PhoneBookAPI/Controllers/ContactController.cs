using BL.AppServices;
using BL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        ContactAppService _contactAppService;
        public ContactController(ContactAppService contactAppService)
        {
            this._contactAppService = contactAppService;
        }

        [HttpGet]
        public IActionResult GetAllContacts()
        {
            return Ok(_contactAppService.GetAllContact());
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User , Admin")]
        public IActionResult PutContact(int id, ContactViewModel contactViewModel)
        {
            try
            {
                _contactAppService.UpdateContact(contactViewModel);
                return Ok(contactViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public ActionResult<ContactViewModel> GetContactById(int id)
        {

            var res = _contactAppService.GetContact(id);
            return Ok(_contactAppService.GetContact(id));
        }

        [Authorize(Roles = "User , Admin")]
        [HttpPost("CreateContact")]
        public IActionResult Create(ContactViewModel contactViewModel)
        {

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _contactAppService.AddNewContact(contactViewModel);

                return Created("CreateContact", contactViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [Authorize(Roles = "User , Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _contactAppService.DeleteContact(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}

using ContactApi.Core.Interface;
using ContactApi.Data.Repository.Interface;
using ContactApi.Model;
using ContactApi.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactRepository _contactRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IConfiguration _configuration;

        public ContactController(IContactRepository contactRepository, ICloudinaryService cloudinaryService, IConfiguration configuration)
        {
            _contactRepository = contactRepository;
            _cloudinaryService = cloudinaryService;
            _configuration = configuration;
        }


        [HttpGet]
        //[Authorize]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ContactResponseDTO>>> GetContacts()
        {
            try
            {
                var contacts = await _contactRepository.GetAllContactsAsync(1, 10);
                var contactResponseDTOs = new List<ContactResponseDTO>();
                foreach (var contact in contacts)
                {
                    var dto = new ContactResponseDTO
                    {
                        Id = contact.Id,
                        FullName = contact.FullName,
                        Email = contact.Email,
                        Phone = contact.Phone,
                        Address = contact.Address
                    };
                    contactResponseDTOs.Add(dto);
                }

                return Ok(contactResponseDTOs);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("{id:guid}")]
        //[Authorize]
        public async Task<ActionResult<ContactResponseDTO>> GetContact([FromRoute] Guid id)
        {
            try
            {
                var contact = await _contactRepository.GetContactByIdAsync(id);
                if (contact == null)
                {
                    return NotFound();
                }
                var contactResponseDTO = new ContactResponseDTO
                {
                    Id = contact.Id,
                    FullName = contact.FullName,
                    Email = contact.Email,
                    Phone = contact.Phone,
                    Address = contact.Address
                };

                return Ok(contactResponseDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<ContactResponseDTO>> AddContact([FromBody]
        AddContactRequestDTO addContactRequest)
        {
            try
            {
                var contactupdate = new AddContactRequest()
                {
                    FullName = addContactRequest.FullName,
                    Address = addContactRequest.Address,
                    Phone = addContactRequest.Phone,
                    Email = addContactRequest.Email,
                };
                var addedContact = await _contactRepository.AddContactAsync(contactupdate);
                var contactResponseDTO = new ContactResponseDTO
                {
                    Id = addedContact.Id,
                    FullName = addedContact.FullName,
                    Email = addedContact.Email,
                    Phone = addedContact.Phone,
                    Address = addedContact.Address
                };

                return Ok(contactResponseDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        //[Authorize]
        public async Task<ActionResult<ContactResponseDTO>> UpdateContact([FromRoute] Guid id,
            UpdateContactRequestDTO updateContactRequest)
        {
            var contact = await _contactRepository.GetContactByIdAsync(id);
            if (contact != null)
            {
                var contactupdate = new UpdateContactRequest()
                {
                    FullName = updateContactRequest.FullName,
                    Address = updateContactRequest.Address,
                    Phone = updateContactRequest.Phone,
                    Email = updateContactRequest.Email
                };


                var updatedContact = await _contactRepository.UpdateContactAsync(id, contactupdate);

                var contactResponseDTO = new ContactResponseDTO
                {
                    Id = updatedContact.Id,
                    FullName = updatedContact.FullName,
                    Email = updatedContact.Email,
                    Phone = updatedContact.Phone,
                    Address = updatedContact.Address
                };

                return Ok(contactResponseDTO);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        //[Authorize]
        public async Task<ActionResult<ContactResponseDTO>> DeleteContact([FromRoute] Guid id)
        {
            var contact = await _contactRepository.GetContactByIdAsync(id);
            if (contact != null)
            {
                var deleted = await _contactRepository.DeleteContactAsync(id);
                if (deleted)
                {
                    var contactResponseDTO = new ContactResponseDTO
                    {
                        Id = contact.Id,
                        FullName = contact.FullName,
                        Email = contact.Email,
                        Phone = contact.Phone,
                        Address = contact.Address
                    };

                    return Ok(contactResponseDTO);
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NotFound();
        }

        [HttpPatch("photo/{id}")]
        //[Authorize] 
        public async Task<IActionResult> UpdateImage(Guid id, [FromForm] PhotoToAddDto model)
        {
            try
            {
                var contact = await _contactRepository.GetContactByIdAsync(id);

                if (contact == null)

                    return NotFound("Contact not found");


                var file = model.PhotoFile;
                if (file == null || file.Length <= 0)

                    return BadRequest("Invalid file size");


                var imageUrl = await _cloudinaryService.UploadImageAsync(file);

                await _contactRepository.UpdatePhotoAsync(id, imageUrl);

                return Ok(new { Url = imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ContactResponseDTO>>> SearchContacts([FromQuery]
        string searchTerm, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var contacts = await _contactRepository.SearchContactsAsync(searchTerm, page, pageSize);
                var contactResponseDTOs = new List<ContactResponseDTO>();
                foreach (var contact in contacts)
                {
                    var dto = new ContactResponseDTO
                    {
                        Id = contact.Id,
                        FullName = contact.FullName,
                        Email = contact.Email,
                        Phone = contact.Phone,
                        Address = contact.Address
                    };
                    contactResponseDTOs.Add(dto);
                }

                return Ok(contactResponseDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}


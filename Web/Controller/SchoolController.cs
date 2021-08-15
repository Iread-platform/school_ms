using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iread_interaction_ms.Web.Service;
using iread_school_ms.Web.Service;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.Web.Dto.School;

namespace iread_school_ms.Web.Controller
{

    [ApiController]
    [Route("api/[controller]/")]
    public class SchoolController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly SchoolService _schoolService;
        private readonly IConsulHttpClientService _consulHttpClient;

        public SchoolController(SchoolService schoolService, IMapper mapper,
          IConsulHttpClientService consulHttpClient)
        {
            _schoolService = schoolService;
            _mapper = mapper;
            _consulHttpClient = consulHttpClient;
        }

        // GET: api/School/get/1
        [HttpGet("get/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            School school = await _schoolService.GetById(id);

            if (school == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<SchoolDto>(school));
        }

        //POST: api/School/add
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] SchoolCreateDto schoolCreateDto)
        {
            if (schoolCreateDto == null)
            {
                return BadRequest();
            }

            School schoolEntity = _mapper.Map<School>(schoolCreateDto);

            _schoolService.Insert(schoolEntity);
            return CreatedAtAction("GetById", new { id = schoolEntity.SchoolId }, _mapper.Map<SchoolDto>(schoolEntity));
        }


        [HttpPut("{id}/update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Update([FromBody] SchoolUpdateDto school, [FromRoute] int id)
        {
            if (school == null)
            {
                return BadRequest();
            }

            School oldSchool = _schoolService.GetById(id).Result;
            if (oldSchool == null)
            {
                return NotFound();
            }

            School schoolEntity = _mapper.Map<School>(school);

            schoolEntity.SchoolId = id;
            _schoolService.Update(schoolEntity, oldSchool);

            return NoContent();
        }


        // //DELETE: api/interaction/audio/5/delete
        // [HttpDelete("{id}/delete")]
        // public IActionResult Delete([FromRoute] int id)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ErrorMessage.ModelStateParser(ModelState));
        //     }
        //     var audio = _audioService.GetById(id).Result;
        //     if (audio == null)
        //     {
        //         return NotFound();
        //     }

        //     _audioService.Delete(audio);
        //     return NoContent();
        // }



        // private void ValidationLogicForUpdating(Audio audio)
        // {
        //     AttachmentDTO attachmentDto = _consulHttpClient.GetAsync<AttachmentDTO>("attachment_ms", $"/api/Attachment/get/{audio.AttachmentId}").Result;

        //     if (attachmentDto == null || attachmentDto.Id < 1)
        //     {
        //         ModelState.AddModelError("AudioId", "Attachment not found");
        //     }
        //     else
        //     {
        //         if (!AudioExtensions.All.Contains(attachmentDto.Extension.ToLower()))
        //         {
        //             ModelState.AddModelError("Audio", "Audio not have valid extension, should be one of [" + string.Join(",", AudioExtensions.All) + "]");
        //         }
        //     }
        // }
    }
}
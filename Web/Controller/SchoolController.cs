using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iread_school_ms.Web.Util;
using iread_interaction_ms.Web.Service;
using iread_school_ms.Web.Service;
using System.Threading.Tasks;
using iread_school_ms.DataAccess.Data.Entity;
using iread_school_ms.Web.Dto.AudioDto;

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



        // // GET: api/interaction/audio/get-by-page/1
        // [HttpGet("get-by-page/{pageId}")]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // public async Task<IActionResult> GetByPageId([FromRoute] int pageId)
        // {
        //     List<Audio> audios = await _audioService.GetByPageId(pageId);

        //     if (audios == null || !audios.Any())
        //     {
        //         return NotFound();
        //     }

        //     return Ok(_mapper.Map<List<AudioDto>>(audios));
        // }

        // //POST: api/audio/add
        // [HttpPost("add")]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // public async Task<IActionResult> Post([FromBody] AudioCreateDto audioCreateDto)
        // {
        //     if (audioCreateDto == null)
        //     {
        //         return BadRequest();
        //     }

        //     Audio audioEntity = _mapper.Map<Audio>(audioCreateDto);
        //     ValidationLogicForAdding(audioEntity);
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ErrorMessage.ModelStateParser(ModelState));
        //     }

        //     if (!_audioService.Insert(audioEntity))
        //     {
        //         return BadRequest();
        //     }

        //     return CreatedAtAction("GetById", new { id = audioEntity.AudioId }, _mapper.Map<AudioDto>(audioEntity));
        // }


        // [HttpPut("{id}/update")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // public IActionResult Update([FromBody] AudioUpdateDto audio, [FromRoute] int id)
        // {
        //     if (audio == null)
        //     {
        //         return BadRequest();
        //     }

        //     Audio oldAudio = _audioService.GetById(id).Result;
        //     if (oldAudio == null)
        //     {
        //         return NotFound();
        //     }

        //     Audio audioEntity = _mapper.Map<Audio>(audio);
        //     ValidationLogicForUpdating(audioEntity);
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ErrorMessage.ModelStateParser(ModelState));
        //     }

        //     audioEntity.AudioId = id;
        //     _audioService.Update(audioEntity, oldAudio);
        //     return NoContent();
        // }


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

        // private void ValidationLogicForAdding(Audio audio)
        // {

        //     ViewStoryDto storyDto = _consulHttpClient.GetAsync<ViewStoryDto>("story_ms", $"/api/story/get/{audio.Interaction.StoryId}").Result;

        //     if (storyDto == null || storyDto.StoryId < 1)
        //     {
        //         ModelState.AddModelError("StoryId", "Story not found");
        //     }

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

        //     PageDto pageDto = _consulHttpClient.GetAsync<PageDto>("story_ms", $"/api/story/Page/get/{audio.Interaction.PageId}").Result;

        //     if (pageDto == null || pageDto.PageId < 1)
        //     {
        //         ModelState.AddModelError("PageId", "Page not found");
        //     }

        //     UserDto userDto = _consulHttpClient.GetAsync<UserDto>("identity_ms", $"/api/identity_ms/SysUsers/{audio.Interaction.StudentId}/get").Result;

        //     if (userDto == null || string.IsNullOrEmpty(userDto.Id))
        //     {
        //         ModelState.AddModelError("StudentId", "Student not found");
        //     }
        //     else
        //     {
        //         if (!userDto.Role.Equals(RoleTypes.Student.ToString()))
        //         {
        //             ModelState.AddModelError("StudentId", "User not a student");
        //         }
        //     }
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
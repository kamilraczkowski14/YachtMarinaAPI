using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Exceptions;
using YachtMarinaAPI.Services;

namespace YachtMarinaAPI.Controllers
{
    [Route("document")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _service;
        private readonly FileService _fileService;

        public DocumentController(IDocumentService service, FileService fileService)
        {
            _service = service;
            _fileService = fileService;
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateDocument([FromForm] CreateDocumentDto dto)
        {
            var documentId = await _service.Create(dto);

            return Created($"{documentId}", null);
        }

        [HttpGet("getFiles")]
        public async Task<IActionResult> ListAllFiles()
        {
            var files = await _service.GetAllFiles();

            return Ok(files);
        }

        //[HttpGet("getAll")]
        //[Authorize(Roles = "Właściciel, Bosman")]
        //public async Task<ActionResult<List<Document>>> getAll()
        //{
        //var documents = _service.GetAll();

        //return Ok(documents);
        //}

        [HttpDelete("{id}/deny")]
        [Authorize(Roles = "Właściciel, Bosman")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            await _service.DenyDocument(id);
            return Ok();
        }

        [HttpDelete("{id}/accept")]
        [Authorize(Roles = "Właściciel, Bosman")]
        public async Task<IActionResult> AcceptDocument(int id)
        {
            await _service.AcceptDocument(id);
            return Ok();
        }

        [HttpGet("getDocuments")]
        [Authorize(Roles = "Właściciel, Bosman")]
        public async Task<ActionResult<List<DocumentDto>>> GetAllDocuments()
        {
            var documents = await _service.GetAllDocuments();
            return Ok(documents);
        }

        [HttpGet("download")]
        [Authorize(Roles = "Właściciel, Bosman")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            var result = await _fileService.Download(filename);

            if (result == null)
            {
                throw new NotFoundException("Nie znaleziono pliku");
            }

            return File(result.Content, result.ContentType, result.Name);
        }

    }
}

using AutoMapper;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Exceptions;
using YachtMarinaAPI.Models;

namespace YachtMarinaAPI.Services
{
    public interface IDocumentService
    {
        Task<int> Create(CreateDocumentDto dto);
        Task<List<BlobDto>> GetAllFiles();
        Task DenyDocument(int documentId);
        Task AcceptDocument(int documentId);
        Task<List<DocumentDto>> GetAllDocuments();
        Task<BlobDto> Download(string filename);

    }
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly FileService _fileService;

        public DocumentService(ApplicationDbContext context, IMapper mapper, IUserContextService userContextService,
            FileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
            _fileService = fileService;
        }
        public async Task<int> Create(CreateDocumentDto dto)
        {
            var newDocument = _mapper.Map<Document>(dto);

            var user = _context.Users
                .FirstOrDefault(u => u.Id == _userContextService.LoggedUserId);

            if (user == null)
            {
                throw new NotFoundException("Coś poszło nie tak");
            }

            newDocument.UserId = user.Id;
            newDocument.Username = user.Username;
            newDocument.Filename = dto.File.FileName;


            var role = _context.Roles.FirstOrDefault(r => r.RoleId == dto.RoleId);

            if (role == null)
            {
                throw new BadRequestException("Coś poszło nie tak");
            }

            newDocument.RoleName = role.Rolename;

            await _fileService.Upload(dto.File);

            _context.Documents.Add(newDocument);
            await _context.SaveChangesAsync();

            return newDocument.Id;
        }

        //public List<DocumentDto> GetAll()
        //{
        //var documents = _context.Documents.ToList();

        //var documentsDto = _mapper.Map<List<DocumentDto>>(documents);

        //return documentsDto;
        //}

        public async Task DenyDocument(int documentId)
        {
            var document = GetDocument(documentId);

            if (document == null)
            {
                throw new NotFoundException("Nie znaleziono dokumentu");
            }

            await _fileService.Delete(document.Filename);

            _context.Remove(document);
            await _context.SaveChangesAsync();
        }

        public async Task AcceptDocument(int documentId)
        {
            var document = GetDocument(documentId);

            if (document == null)
            {
                throw new NotFoundException("Nie znaleziono dokumentu");
            }

            var userId = document.UserId;
            var roleId = document.RoleId;

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("Nie znaleziono takiego użytkownika");
            }

            user.RoleId = roleId;

            await _fileService.Delete(document.Filename);

            _context.Remove(document);
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BlobDto>> GetAllFiles()
        {
            var files = await _fileService.List();

            return files;
        }

        public async Task<List<DocumentDto>> GetAllDocuments()
        {
            var documents = _context.Documents.ToList();

            var documentsDto = _mapper.Map<List<DocumentDto>>(documents);

            return documentsDto;
        }

        public async Task<BlobDto?> Download(string filename)
        {
            var result = await _fileService.Download(filename);

            if (result == null)
            {
                throw new BadRequestException("Coś poszło nie tak");
            }

            return result;

        }

        private Document GetDocument(int documentId)
        {
            var document = _context.Documents.FirstOrDefault(u => u.Id == documentId);

            return document;
        }

    }
}

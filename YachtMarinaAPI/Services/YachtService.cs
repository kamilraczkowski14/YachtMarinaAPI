using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Exceptions;
using YachtMarinaAPI.Models;

namespace YachtMarinaAPI.Services
{
    public interface IYachtService
    {
        Task<int> Create(CreateYachtDto dto);
        Task<List<YachtDto>> GetAll();
        Task Delete(int id);
        Task Edit(UpdateYachtDto dto);
    }
    public class YachtService : IYachtService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;

        public YachtService(ApplicationDbContext context, IUserContextService userContextService, IMapper mapper,
            ImageService imageService)
        {
            _context = context;
            _userContextService = userContextService;
            _mapper = mapper;
            _imageService = imageService;
        }
        public async Task<int> Create(CreateYachtDto dto)
        {
            var newYacht = _mapper.Map<Yacht>(dto);

            newYacht.userId = (int)_userContextService.LoggedUserId;

            if (dto.File != null)
            {
                var imageResult = _imageService.AddImage(dto.File);

                if (imageResult.Result.Error != null)
                {
                    throw new BadRequestException("Coś poszlo nie tak z plikiem!");
                }

                newYacht.PictureUrl = imageResult.Result.SecureUrl.ToString();
            }

            _context.Yachts.Add(newYacht);
            await _context.SaveChangesAsync();

            return newYacht.Id;
        }

        public async Task Delete(int id)
        {
            var yacht = GetYacht(id);

            if (yacht == null)
            {
                throw new NotFoundException("Nie znaleziono takiego jachtu");
            }

            if (yacht.User.Id != _userContextService.LoggedUserId)
            {
                throw new ForbidException("Nie masz uprawnień dla tej operacji");
            }

            _context.Remove(yacht);
            await _context.SaveChangesAsync();
        }

        public async Task Edit(UpdateYachtDto dto)
        {

            var yacht = GetYacht(dto.Id);

            if (yacht == null)
            {
                throw new NotFoundException("Nie znaleziono jachtu");
            }

           
            if (yacht.User.Id != _userContextService.LoggedUserId)
            {
                throw new ForbidException("Nie masz uprawnień dla tej operacji");
            }
            

            yacht.Name = dto.Name;
            yacht.Type = dto.Type;
            yacht.Brand = dto.Brand;
            yacht.Length = dto.Length;
            yacht.YearOfProduction = dto.YearOfProduction;
            yacht.PictureUrl = yacht.PictureUrl;

            if (dto.File != null)
            {
                var imageResult = _imageService.AddImage(dto.File);

                yacht.PictureUrl = imageResult.Result.SecureUrl.ToString();
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<YachtDto>> GetAll()
        {
            var yachts = _context.Yachts
                    .Where(y => y.userId == _userContextService.LoggedUserId)
                    .ToList();

            var yachtsDto = _mapper.Map<List<YachtDto>>(yachts);

            return yachtsDto;
        }


        private Yacht GetYacht(int id)
        {
            var yacht = _context.Yachts
                .Include(y => y.User)
                .FirstOrDefault(y => y.Id  == id);

            return yacht;
        }
    }
}

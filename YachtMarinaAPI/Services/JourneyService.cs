using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Entities;
using YachtMarinaAPI.Exceptions;
using YachtMarinaAPI.Models;

namespace YachtMarinaAPI.Services
{
    public interface IJourneyService
    {
        Task<int> Start(CreateJourneyDto dto);
        Task<List<JourneyDto>> GetAll();
        Task Delete(int id);
        Task End(int id);
        Task<JourneyDto> GetById(int id);
        Task AddNote(int id, CreateNoteDto dto);
        Task AddPhoto(int id, AddPhotoDto dto);
        Task<Yacht> GetYacht(int journeyId);
        Task<List<UserDto>> GetUsers(int journeyId);
    }
    public class JourneyService : IJourneyService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;

        public JourneyService(ApplicationDbContext context, IUserContextService userContextService, IMapper mapper,
            ImageService imageService)
        {
            _context = context;
            _userContextService = userContextService;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<int> Start(CreateJourneyDto dto)
        {

            int userId = (int)_userContextService.LoggedUserId;

            var user = await _context.Users
                .Include(u => u.Journeys)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new BadRequestException("Błąd przy tworzeniu podróży. Nie znaleziono użytkownika");
            }

            var lineCoordinates = dto.LineCoordinates.Select(coord => new LineCoordinate
            {
                Lat = coord.Lat,
                Lng = coord.Lng
            }).ToList();

            var markers = dto.Markers.Select(marker => new Marker
            {
                Lat = marker.Lat, 
                Lng = marker.Lng
            }).ToList();

            var newJourney = new Journey()
            {
                Name = dto.Name,
                UserId = userId,
                YachtId = dto.YachtId,
                Distance = dto.Distance,
                StartDate = dto.StartDate,
                FriendsIds = dto.FriendsIds,
                Status = "W trakcie",
                LineCoordinates = lineCoordinates,
                Markers = markers
            };

            _context.Journeys.Add(newJourney);
            user.Journeys.Add(newJourney);
            //await _context.SaveChangesAsync();


            var yacht = await _context.Yachts
               .Include(y => y.User)
               .FirstOrDefaultAsync(y => y.Id == dto.YachtId);

            if (yacht == null)
            {
                throw new NotFoundException("Nie wybrano jachtu ");
            }

            if(yacht.userId != user.Id)
            {
                throw new ForbidException("Nie masz uprawnień aby skorzystać z tego jachtu");
            }

            
            if(yacht.Type == "Żaglowo-motorowy")
            {
                if(user.RoleId == 5 && user.RoleId == 6 && user.RoleId == 1)
                {
                    throw new BadRequestException("Nie możesz zacząć podróży tym jachtem.");
                }

                if(yacht.Length > 12 && user.RoleId < 3)
                {
                    throw new BadRequestException("Nie możesz zacząć podrózy tym jachtem");
                }

                if(yacht.Length > 18 && user.RoleId < 4)
                {
                    throw new BadRequestException("Nie możesz zacząć podrózy tym jachtem");
                }
            }

            if(yacht.Type == "Motorowy")
            {
                if (user.RoleId < 5)
                {
                    throw new BadRequestException("Nie możesz zacząć podróży tym jachtem.");
                }
                 
                if (user.RoleId < 6 && yacht.Length < 12)
                {
                    throw new BadRequestException("Nie możesz zacząć podróży tym jachtem.");
                }
            }
            

            if(newJourney.Name.IsNullOrEmpty())
            {
                throw new BadRequestException("Nie podano nazwy podróży");
            }

            if(newJourney.LineCoordinates.Count < 2) 
            {
                throw new BadRequestException("Podano zbyt małą liczbę tras - wymagane: 1");
            }

            if(newJourney.Markers.Count < 2)
            {
                throw new BadRequestException("Podano zbyt małą liczbę punktów - wymagane: 2");
            }

            
            if(dto.FriendsIds != null)
            {
                foreach (var item in dto.FriendsIds)
                {
                    if (item.friendId == userId)
                    {
                        continue;
                    }

                    var guest = _context.Users
                        .Include(u => u.Journeys)
                        .FirstOrDefault(u => u.Id == item.friendId);


                    if (guest == null)
                    {
                        throw new NotFoundException("Nie znaleziono uzytkownika");
                    }


                    guest.Journeys.Add(newJourney);

                    await _context.SaveChangesAsync();
                }
            }
            
            
            await _context.SaveChangesAsync();

            return newJourney.Id;
        }

        public async Task Delete(int id)
        {
            var journey = GetJourney(id);

            Authorize(id);

            _context.Remove(journey);
            await _context.SaveChangesAsync();
        }

        public async Task<List<JourneyDto>> GetAll()
        {

            var journeys = _context.Journeys.Where(j => j.UserId == _userContextService.LoggedUserId ||
            j.FriendsIds.Any(id => id.friendId == _userContextService.LoggedUserId))
            .ToList();

            var journeysDto = _mapper.Map<List<JourneyDto>>(journeys);

            return journeysDto;
        }

        public async Task End(int id)
        {
            var journey = GetJourney(id);

            Authorize(id);

            if (journey.Status == "Zakończona")
            {
                throw new BadRequestException("Podróż jest już zakończona");
            }

            var user = _context.Users.FirstOrDefault(x => x.Id == journey.UserId);

            if (user == null)
            {
                throw new NotFoundException("Nie znaleziono uzytkownika");
            }

            DateTime currentDate = DateTime.Now;

            journey.Status = "Zakończona";
            journey.EndDate = currentDate;

            
            _context.Users.Update(user);
            _context.Journeys.Update(journey);
            await _context.SaveChangesAsync();

        }

        public async Task<JourneyDto> GetById(int id)
        {
            var journey = GetJourney(id);

            var journeyDto = _mapper.Map<JourneyDto>(journey);  

            return journeyDto;
        }

        public async Task AddNote(int journeyId, CreateNoteDto dto)
        {
            var journey = GetJourney(journeyId);

            Authorize(journeyId);

            journey.Note = dto.Note;

            _context.Journeys.Update(journey);

            await _context.SaveChangesAsync();

        }

        public async Task AddPhoto(int journeyId, AddPhotoDto dto)
        {
            var journey = GetJourney(journeyId);

            Authorize(journeyId);

            var imageResult = _imageService.AddImage(dto.File);

            journey.PhotosUrls ??= new List<Photo>();

            journey.PhotosUrls.Add(new Photo { Url = imageResult.Result.SecureUrl.ToString() });

            _context.Journeys.Update(journey);

            await _context.SaveChangesAsync();
        }

        public async Task<Yacht> GetYacht(int journeyId)
        {
            var journey = GetJourney(journeyId);

            var yacht = await _context.Yachts.FirstOrDefaultAsync(y => y.Id == journey.YachtId);

            return yacht;
        }

        public async Task<List<UserDto>> GetUsers(int journeyId)
        {
            var journey = GetJourney(journeyId);

            var users = new List<User>();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == journey.UserId);

            users.Add(user);

            foreach (var item in journey.FriendsIds)
            {
                var friend = await _context.Users.FirstOrDefaultAsync(u => u.Id == item.friendId);

                if (friend != null)
                {
                    users.Add(friend);
                }
            }

            var usersDto = _mapper.Map<List<UserDto>>(users);

            return usersDto;
        }

        private Journey GetJourney(int id)
        {
            var journey = _context.Journeys
                .Include(j => j.FriendsIds)
                .FirstOrDefault(j => j.Id == id);

            if (journey == null)
            {
                throw new NotFoundException("Nie znaleziono podróży");
            }

            return journey;
        }

        private void Authorize(int id)
        {
            var journey = GetJourney(id);

            if (journey.UserId != _userContextService.LoggedUserId)
            {
                throw new ForbidException("Nie masz uprawnień dla tej operacji!");
            }
        }
        
    }
}

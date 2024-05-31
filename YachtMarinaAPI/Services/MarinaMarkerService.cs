using Microsoft.EntityFrameworkCore;
using YachtMarinaAPI.DAL;
using YachtMarinaAPI.Dtos;
using YachtMarinaAPI.Entities;
using YachtMarinaAPI.Exceptions;

namespace YachtMarinaAPI.Services
{
    public interface IMarinaMarkerService
    {
        Task<List<MarinaMarker>> GetAll();
        Task Add(CreateMarkersDto dto);
    }

    public class MarinaMarkerService : IMarinaMarkerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserContextService _userContextService;

        public MarinaMarkerService(ApplicationDbContext context, IUserContextService userContextService)
        {
            _context = context;
            _userContextService = userContextService;
        }

        public async Task Add(CreateMarkersDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == _userContextService.LoggedUserId);

            if (user == null)
            {
                throw new BadRequestException("Nie znaleziono użytkownika");
            }

            foreach (var marker in dto.newMarkers)
            {
                var newMarker = new MarinaMarker()
                {
                    Name = marker.Name,
                    Lat = marker.Lat,
                    Lng = marker.Lng,
                };

                _context.MarinaMarkers.Add(newMarker);
            }

            await _context.SaveChangesAsync();

        }

        public async Task<List<MarinaMarker>> GetAll()
        {
            var markers = await _context.MarinaMarkers.ToListAsync();
            return markers;
        }


    }
}

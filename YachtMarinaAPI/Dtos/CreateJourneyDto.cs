using YachtMarinaAPI.Entities;

namespace YachtMarinaAPI.Dtos
{
    public class CreateJourneyDto
    {
        public string Name { get; set; }
        public int YachtId { get; set; }
        public int UserId { get; set; }
        public decimal Distance { get; set; }
        public List<FriendJourney>? FriendsIds { get; set; }
        public List<LineCoordinate> LineCoordinates { get; set; }
        public List<Marker> Markers { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
    }
}

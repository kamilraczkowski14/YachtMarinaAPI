﻿using YachtMarinaAPI.Entities;

namespace YachtMarinaAPI.Dtos
{
    public class JourneyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string? Note { get; set; }
        public int YachtId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public decimal Distance { get; set; }  
        public List<FriendJourney> FriendsIds { get; set; }
        public List<LineCoordinate> LineCoordinates { get; set; }
        public List<Marker> Markers { get; set; }
        public List<Photo> PhotosUrls {  get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

}

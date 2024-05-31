using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace YachtMarinaAPI.Dtos
{
    public class MarinaMarkerDto
    {
        public string Name { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }
}

using Core.Entities;

namespace Entities.Dtos
{
    public class RentalDetailDto : IDto
    {
        public int RentalId { get; set; }
        public  int CarId { get; set; }
        public string CompanyName { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}

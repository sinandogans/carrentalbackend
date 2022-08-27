using Core.Entities;

namespace Entities.Dtos
{
    public class CarDetailDto:IDto
    {
        public int CarId { get; set; }
        public string BrandName { get; set; }
        public string ColorName { get; set; }
        public double DailyPrice { get; set; }

    }
}

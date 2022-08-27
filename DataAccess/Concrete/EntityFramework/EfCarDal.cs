using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCarDal : EfEntityRepositoryBase<Car, MSSQLContext>, ICarDal
    {
        public List<CarDetailDto> GetDetails()
        {
            using (MSSQLContext context = new MSSQLContext())
            {
                var result = from c in context.Cars
                             join b in context.Brands
                             on c.BrandId equals b.Id
                             join clr in context.Colors
                             on c.ColorId equals clr.Id
                             select new CarDetailDto
                             {
                                 CarId = c.Id,
                                 ColorName = clr.Name,
                                 BrandName = b.Name,
                                 DailyPrice = c.DailyPrice
                             };
                return result.ToList();
            }
        }
    }
}

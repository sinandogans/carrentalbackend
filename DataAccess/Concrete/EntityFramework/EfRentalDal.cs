using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfRentalDal : EfEntityRepositoryBase<Rental, MSSQLContext>, IRentalDal
    {
        public List<RentalDetailDto> GetDetails()
        {
            using (MSSQLContext context = new MSSQLContext())
            {
                var result =
                    from r in context.Rentals
                    join c in context.Customers
                        on r.CustomerId equals c.Id
                    select new RentalDetailDto
                    {
                        RentalId = r.Id,
                        CompanyName = c.CompanyName,
                        RentDate = r.RentDate,
                        ReturnDate = r.ReturnDate
                    };
                return result.ToList();
            }
        }
    }
}

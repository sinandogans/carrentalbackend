using Business.Abstract;
using Business.Constants;
using Core.Aspects.PostSharp;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    [ProviderAspect]
    public class RentalManager : IRentalService
    {
        readonly IRentalDal _rentalDal;

        public RentalManager(IRentalDal rentalDal)
        {
            _rentalDal = rentalDal;
        }

        public IResult Add(Rental rental)
        {
            var result = BusinessRules.Run(CheckIfCarReturned(rental));
            if (result != null)
                return new ErrorResult(result.Message);
            _rentalDal.Add(rental);
            return new SuccessResult(Messages.CarAdded);
        }

        public IResult Delete(Rental rental)
        {
            _rentalDal.Delete(rental);
            return new SuccessResult(Messages.RentalDeleted);
        }

        public IDataResult<List<Rental>> GetAll()
        {
            var data = _rentalDal.GetList();
            return new SuccessDataResult<List<Rental>>(Messages.AllRentalsGet, data);
        }

        public IDataResult<Rental> GetById(int rentalId)
        {
            var data = _rentalDal.Get(r => r.Id == rentalId);
            return new SuccessDataResult<Rental>(Messages.RentalGetById, data);
        }

        public IResult Update(Rental rental)
        {
            _rentalDal.Update(rental);
            return new SuccessResult(Messages.RentalUpdated);
        }

        private IResult CheckIfCarReturned(Rental rental)
        {
            var rentals = _rentalDal.GetList(r => r.CarId == rental.CarId);
            var lastRental = rentals.Last();
            if (lastRental.ReturnDate >= rental.RentDate || lastRental.ReturnDate == null)
                return new ErrorResult(Messages.CarHasNotReturned);
            return new SuccessResult();

        }
    }
}

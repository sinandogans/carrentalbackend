using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.PostSharp;
using Core.Aspects.PostSharp.Caching;
using Core.Aspects.PostSharp.Performance;
using Core.Aspects.PostSharp.Transaction;
using Core.Aspects.PostSharp.Validation;
using Core.Utilities.EmailService;
using Core.Utilities.Results;
using Core.Utilities.SmsService;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Concrete
{
    [ProviderAspect]
    public class CarManager : ICarService
    {
        readonly ICarDal _carDal;
        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }

        [PostSharpTransactionAspect]
        [PostSharpValidationAspect(typeof(CarValidator))]
        [PostSharpCacheRemoveAspect("CarManager.Get")]
        public IResult Add(Car car)
        {
            _carDal.Add(car);
            return new SuccessResult(Messages.CarAdded);
        }

        [PostSharpTransactionAspect]
        [PostSharpCacheRemoveAspect("CarManager.Get")]
        public IResult Delete(Car car)
        {
            _carDal.Delete(car);
            return new SuccessResult(Messages.CarDeleted);
        }

        [PostSharpCacheAspect]
        [PostSharpPerformanceAspect(5)]
        public IDataResult<List<Car>> GetAll()
        {
            var data = _carDal.GetList();
            return new SuccessDataResult<List<Car>>(Messages.AllCarsGet, data);
        }

        [PostSharpCacheAspect]
        [PostSharpPerformanceAspect(5)]
        public IDataResult<Car> GetById(int carId)
        {
            var data = _carDal.Get(c => c.Id == carId);
            return new SuccessDataResult<Car>(Messages.CarGetById, data);
        }

        [PostSharpCacheAspect]
        [PostSharpPerformanceAspect(5)]
        public IDataResult<List<CarDetailDto>> GetDetails()
        {
            var data = _carDal.GetDetails();
            return new SuccessDataResult<List<CarDetailDto>>(Messages.CarDetailGet, data);
        }

        [PostSharpTransactionAspect]
        [PostSharpCacheRemoveAspect("CarManager.Get")]
        public IResult Update(Car car)
        {
            _carDal.Update(car);
            return new SuccessResult(Messages.CarUpdated);
        }
    }
}

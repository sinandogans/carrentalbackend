using Business.Abstract;
using Business.Constants;
using Core.Aspects.PostSharp;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    [ProviderAspect]
    public class BrandManager : IBrandService
    {
        readonly IBrandDal _brandDal;

        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }

        public IResult Add(Brand brand)
        {
            _brandDal.Add(brand);
            return new SuccessResult(Messages.BrandAdded);
        }

        public IResult Delete(Brand brand)
        {
            _brandDal.Delete(brand);
            return new SuccessResult(Messages.BrandDeleted);
        }

        public IDataResult<List<Brand>> GetAll()
        {
            var data = _brandDal.GetList();
            return new SuccessDataResult<List<Brand>>(Messages.AllBrandsGet, data);
        }

        public IDataResult<Brand> GetById(int brandId)
        {
            var data = _brandDal.Get(b => b.Id == brandId);
            return new SuccessDataResult<Brand>(Messages.BrandGetById, data);
        }

        public IResult Update(Brand brand)
        {
            _brandDal.Update(brand);
            return new SuccessResult(Messages.BrandUpdated);
        }
    }
}

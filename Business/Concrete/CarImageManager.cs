using Business.Abstract;
using Business.Constants;
using Core.Aspects.PostSharp;
using Core.Utilities.Business;
using Core.Utilities.FileOperations;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using IResult = Core.Utilities.Results.IResult;

namespace Business.Concrete
{
    [ProviderAspect]
    public class CarImageManager : ICarImageService
    {
        readonly ICarImageDal _carImageDal;
        readonly FileHelper _fileHelper;

        public CarImageManager(ICarImageDal carImageDal, FileHelper fileHelper)
        {
            _carImageDal = carImageDal;
            _fileHelper = fileHelper;
        }

        public IResult Add(IFormFile image, CarImage carImage)
        {
            var result = BusinessRules.Run(this.CheckIfImageCountExceed(carImage));
            if (result != null)
                return new ErrorResult(result.Message);

            var path = _fileHelper.AddImageToProject(image);
            carImage.ImagePath = path;
            carImage.Date = DateTime.Now;
            _carImageDal.Add(carImage);

            return new SuccessResult(Messages.CarImageAdded);
        }

        public IResult Delete(CarImage carImage)
        {
            _carImageDal.Delete(carImage);
            return new SuccessResult(Messages.CarImageDeleted);
        }

        public IDataResult<List<CarImage>> GetAll()
        {
            var data = _carImageDal.GetList();
            return new SuccessDataResult<List<CarImage>>(Messages.AllCarImagesGet, data);
        }

        public IDataResult<CarImage> GetById(int carImageId)
        {
            var data = _carImageDal.Get(b => b.Id == carImageId);
            return new SuccessDataResult<CarImage>(Messages.CarImageGetById, data);
        }

        public IResult Update(CarImage carImage)
        {
            _carImageDal.Update(carImage);
            return new SuccessResult(Messages.CarImageUpdated);
        }

        private IResult CheckIfImageCountExceed(CarImage carImage)
        {
            int numberOfImages = _carImageDal.GetList(c => c.CarId == carImage.CarId).Count;
            if (numberOfImages < 5)
                return new SuccessResult();
            return new ErrorResult(Messages.CarCanHasMaxFiveImages);
        }
    }
}

using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using IResult = Core.Utilities.Results.IResult;

namespace Business.Abstract
{
    public interface ICarImageService
    {
        IResult Add(IFormFile image, CarImage carImage);
        IResult Update(CarImage carImage);
        IResult Delete(CarImage carImage);
        IDataResult<List<CarImage>> GetAll();
        IDataResult<CarImage> GetById(int id);
    }
}

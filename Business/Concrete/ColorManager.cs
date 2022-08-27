using Business.Abstract;
using Business.Constants;
using Core.Aspects.PostSharp;
using Core.Aspects.PostSharp.Caching;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    [ProviderAspect]
    public class ColorManager : IColorService
    {
        readonly IColorDal _colorDal;

        public ColorManager(IColorDal colorDal)
        {
            _colorDal = colorDal;
        }

        public IResult Add(Color color)
        {
            _colorDal.Add(color);
            return new SuccessResult(Messages.ColorAdded);
        }

        public IResult Delete(Color color)
        {
            _colorDal.Delete(color);
            return new SuccessResult(Messages.ColorDeleted);
        }
        [PostSharpCacheAspect]
        public IDataResult<List<Color>> GetAll()
        {
            var data = _colorDal.GetList();
            return new SuccessDataResult<List<Color>>(Messages.AllColorsGet, data);
        }

        public IDataResult<Color> GetById(int colorId)
        {
            var data = _colorDal.Get(c => c.Id == colorId);
            return new SuccessDataResult<Color>(Messages.ColorGetById, data);
        }

        public IResult Update(Color color)
        {
            _colorDal.Update(color);
            return new SuccessResult(Messages.ColorUpdated);
        }
    }
}

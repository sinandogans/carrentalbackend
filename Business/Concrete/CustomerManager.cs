using Business.Abstract;
using Business.Constants;
using Core.Aspects.PostSharp;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    [ProviderAspect]
    public class CustomerManager : ICustomerService
    {
        readonly ICustomerDal _customerDal;

        public CustomerManager(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        public IResult Add(Customer customer)
        {
            _customerDal.Add(customer);
            return new SuccessResult(Messages.CustomerAdded);
        }

        public IResult Delete(Customer customer)
        {
            _customerDal.Delete(customer);
            return new SuccessResult(Messages.CustomerDeleted);
        }

        public IDataResult<List<Customer>> GetAll()
        {
            var data = _customerDal.GetList();
            return new SuccessDataResult<List<Customer>>(Messages.AllCustomersGet, data);
        }

        public IDataResult<Customer> GetById(int customerId)
        {
            var data = _customerDal.Get(c => c.UserId == customerId);
            return new SuccessDataResult<Customer>(Messages.CustomerGetById, data);
        }

        public IResult Update(Customer customer)
        {
            _customerDal.Update(customer);
            return new SuccessResult(Messages.CustomerUpdated);
        }
    }
}

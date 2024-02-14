using API3.Models;
using API3.Repository.Interfaces;
using API3.Services.Interfaces;

namespace API3.Services
{
    public class CustomerServiceImplem : ICustomerService
    {
        private readonly ICustomerRepo _customerRepo;

        public CustomerServiceImplem(ICustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public Customer GetCustomerById(string id)
        {
            return _customerRepo.GetCustomerById(id);
        }

     
        public Customer CreateCustomer(Customer customer)
        {
          
            return _customerRepo.CreateCustomer(customer); 
        }

       
        public Customer UpdateCustomer(Customer customer,string id)
        {
           
            return _customerRepo.UpdateCustomer(customer,id); 
        }

 
        public void DeleteCustomer(string id)
        {
           
            _customerRepo.DeleteCustomer(id); 
        }

        public List<string> GetCustomers()
        {
            
            throw new NotImplementedException() ;
        }
    }
}

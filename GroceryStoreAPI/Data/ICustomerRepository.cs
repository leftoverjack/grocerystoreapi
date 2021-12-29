using GroceryStoreAPI.Models;
using System.Collections.Generic;

namespace GroceryStoreAPI.Data
{
    public interface ICustomerRepository
    {
        bool SaveChanges();

        public IEnumerable<Customer> GetAllCustomers();

        public Customer GetCustomerById(int id);

        public void AddCustomer(Customer customer);

        public void UpdateCustomer(Customer customer);

        public void DeleteCustomer(Customer customer);
    }
}

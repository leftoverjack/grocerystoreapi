using GroceryStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GroceryStoreAPI.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IJsonDataAdapter _context;

        public CustomerRepository(IJsonDataAdapter context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _context = context;
        }

        public void AddCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            // using json document as db, so we are managing id's ourselves rather than letting the built in dbcontext handle it
            customer.Id = NextId();

            _context.Add(customer);
        }

        public void DeleteCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            _context.Delete(customer);
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _context.Customers;
        }

        public Customer GetCustomerById(int id)
        {
            var customers = _context.Customers;

            return customers.FirstOrDefault(c => c.Id == id);
        }

        public bool SaveChanges()
        {
            _context.SaveJsonChanges();
            
            return true;
        }

        public void UpdateCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            _context.Update(customer);
        }

        private int NextId()
        {
            return _context.Customers.Max(c => c.Id) + 1;
        }
    }
}

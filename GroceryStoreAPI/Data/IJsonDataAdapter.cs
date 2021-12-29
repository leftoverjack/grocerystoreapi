using GroceryStoreAPI.Models;
using System;
using System.Collections.Generic;

namespace GroceryStoreAPI.Data
{
    public interface IJsonDataAdapter : IDisposable
    {
        public IEnumerable<Customer> Customers { get; set; }

        public void Add(Customer customer);

        public void Update(Customer customer);

        public void Delete(Customer customer);

        public void SaveJsonChanges();
    }
}

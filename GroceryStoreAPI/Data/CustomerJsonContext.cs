using GroceryStoreAPI.Exceptions;
using GroceryStoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace GroceryStoreAPI.Data
{
    public class CustomerJsonContext : DbContext, IJsonDataAdapter
    {
        private readonly string filename;

        public IEnumerable<Customer> Customers { get; set; }

        public CustomerJsonContext(IConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            filename = config.GetValue<string>("JsonDatabaseFilename");

            Customers = InitializeFromFile(filename);
        }

        public void Add(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            if (Customers.FirstOrDefault(c => c.Id == customer.Id) != null)
            {
                throw new InvalidOperationException("A customer with the specified key already exists");
            }

            Customers = Customers.Append(customer);
        }

        public void Update(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            if (Customers.FirstOrDefault(c => c.Id == customer.Id) == null)
            {
                throw new InvalidOperationException("Could not find the specified customer");
            }

            var customerList = Customers.ToList();

            Customers = Customers.Select(c => c.Id == customer.Id ? customer : c);
        }

        public void Delete(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            if (Customers.FirstOrDefault(c => c.Id == customer.Id) == null)
            {
                throw new InvalidOperationException("Could not find the specified customer");
            }

            Customers = Customers.Where(c => c.Id != customer.Id);
        }

        public void SaveJsonChanges()
        {
            var jsonString = JsonConvert.SerializeObject(new
            {
                customers = Customers
            });

            File.WriteAllText(filename, jsonString);
        }

        private IEnumerable<Customer> InitializeFromFile(string filename)
        {
            try
            {
                using (var reader = new StreamReader(filename))
                {
                    var json = reader.ReadToEnd();
                    var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;
                    var customersJson = root.GetProperty("customers");
                    var customers = JsonConvert.DeserializeObject<List<Customer>>(customersJson.ToString());

                    return customers;
                }
            }
            catch (Exception)
            {
                throw new JsonDatabaseInitializationException();
            }
        }
    }
}

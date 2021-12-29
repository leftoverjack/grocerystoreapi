using FluentAssertions;
using GroceryStoreAPI.Data;
using GroceryStoreAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace GroceryStoreAPI.Tests
{
    [TestClass]
    public class CustomerJsonContextUnitTests
    {
        private const string testDataFilename = "testdb.json";
        private IConfiguration configuration;
        private IEnumerable<Customer> testData;

        [TestInitialize]
        public void Init()
        {
            var inMemoryTestSettings = new Dictionary<string, string>
            {
                {"JsonDatabaseFilename", testDataFilename }
            };

            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryTestSettings)
                .Build();

            testData = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    Name = "Bob"
                },
                new Customer
                {
                    Id = 2,
                    Name = "Mary"
                },
                new Customer
                {
                    Id = 3,
                    Name = "Tim"
                },
                new Customer
                {
                    Id = 4,
                    Name = "Sally"
                }
            };

            SetupTestData();
        }

        #region Constructor

        [TestMethod]
        public void Constructor_WhenConfigurationIsNull_ShouldThrow()
        {
            this.configuration = null;

            Action act = () => GetSubjectUnderTest();

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("config");
        }

        [TestMethod]
        public void Constructor_WhenConfigurationIsValid_ShouldReturnInstance()
        {
            var result = GetSubjectUnderTest();

            result.Should().BeOfType<CustomerJsonContext>();
        }

        [TestMethod]
        public void Constructor_WhenConfigurationIsValid_ShouldHaveCustomerData()
        {
            var result = GetSubjectUnderTest();

            result.Customers.Should().NotBeNullOrEmpty();
            result.Customers.Should().BeEquivalentTo(testData);
        }

        #endregion

        #region Add

        [TestMethod]
        public void Add_WhenCustomerIsNull_ShouldThrow()
        {
            var context = GetSubjectUnderTest();

            Action act = () => context.Add(null);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("customer");
        }

        [TestMethod]
        public void Add_WhenCustomerIdIsNotUnique_ShouldThrow()
        {
            var context = GetSubjectUnderTest();

            Action act = () => context.Add(new Customer
            {
                Id = 2,
                Name = "Todd"
            });

            act.Should().Throw<InvalidOperationException>().And.Message.Should().Be("A customer with the specified key already exists");
        }

        [TestMethod]
        public void Add_WhenCustomerIsValid_ShouldAddCustomer()
        {
            var newCustomer = new Customer
            {
                Id = 5,
                Name = "Todd"
            };

            var context = GetSubjectUnderTest();

            Action act = () => context.Add(new Customer
            {
                Id = 5,
                Name = "Todd"
            });

            act.Should().NotThrow();
            context.Customers.Should().ContainEquivalentOf(newCustomer);
        }

        #endregion

        #region Update

        [TestMethod]
        public void Update_WhenCustomerIsNull_ShouldThrow()
        {
            var context = GetSubjectUnderTest();

            Action act = () => context.Update(null);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("customer");
        }

        [TestMethod]
        public void Update_WhenCustomerDoesNotExist_ShouldThrow()
        {
            var context = GetSubjectUnderTest();

            Action act = () => context.Update(new Customer
            {
                Id = 99,
                Name = "Todd"
            });

            act.Should().Throw<InvalidOperationException>().And.Message.Should().Be("Could not find the specified customer");
        }

        [TestMethod]
        public void Update_WhenCustomerExists_ShouldUpdateCustomer()
        {
            var originalCustomer = new Customer
            {
                Id = 2,
                Name = "Mary"
            };
            var updatedCustomer = new Customer
            {
                Id = 2,
                Name = "Todd"
            };

            var context = GetSubjectUnderTest();

            Action act = () => context.Update(updatedCustomer);

            act.Should().NotThrow();
            context.Customers.Should().NotContainEquivalentOf(originalCustomer);
            context.Customers.Should().ContainEquivalentOf(updatedCustomer);
        }

        #endregion

        #region Delete

        [TestMethod]
        public void DeleteCustomer_WhenCustomerIsNull_ShouldThrow()
        {
            var context = GetSubjectUnderTest();

            Action act = () => context.Delete(null);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("customer");
        }

        [TestMethod]
        public void DeleteCustomer_WhenCustomerDoesNotExist_ShouldThrow()
        {
            var context = GetSubjectUnderTest();

            Action act = () => context.Delete(new Customer
            {
                Id = 99,
                Name = "Todd"
            });

            act.Should().Throw<InvalidOperationException>().And.Message.Should().Be("Could not find the specified customer");
        }

        [TestMethod]
        public void DeleteCustomer_WhenCustomerExists_ShouldDeleteCustomer()
        {
            var customer = new Customer
            {
                Id = 2,
                Name = "Mary"
            };

            var context = GetSubjectUnderTest();

            Action act = () => context.Delete(customer);

            act.Should().NotThrow();
            context.Customers.Should().NotContainEquivalentOf(customer);
        }

        #endregion

        #region SaveJsonChanges

        [TestMethod]
        public void SaveJsonChanges_ShouldSaveChangesToFile()
        {
            var newCustomer = new Customer
            {
                Id = 5,
                Name = "Todd"
            };

            var context = GetSubjectUnderTest();
            context.Add(newCustomer);

            context.SaveJsonChanges();

            // initializing context results in loading customers from file specified
            var reloadedContext = GetSubjectUnderTest();
            reloadedContext.Customers.Should().ContainEquivalentOf(newCustomer);
        }

        #endregion

        private CustomerJsonContext GetSubjectUnderTest()
        {
            return new CustomerJsonContext(configuration);
        }

        private void SetupTestData()
        {
            var jsonString = JsonConvert.SerializeObject(new
            {
                customers = testData
            });

            File.WriteAllText(testDataFilename, jsonString);
        }
    }
}

using FluentAssertions;
using GroceryStoreAPI.Data;
using GroceryStoreAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace GroceryStoreAPI.Tests
{
    [TestClass]
    public class CustomerRepositoryUnitTests
    {
        private IJsonDataAdapter context;

        private IEnumerable<Customer> testData;

        [TestInitialize]
        public void Init()
        {
            this.testData = new List<Customer>
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
                    Name = "Joe"
                }
            };

            this.context = Substitute.For<IJsonDataAdapter>();
            context.Customers.Returns(testData);
        }

        #region Constructor

        [TestMethod]
        public void Constructor_WhenDataAdapterIsNull_ShouldThrow()
        {
            this.context = null;

            Action act = () => GetSubjectUnderTest();

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("context");
        }

        [TestMethod]
        public void Constructor_WhenDataAdapterValid_ShouldReturnInstance()
        {
            var result = GetSubjectUnderTest();

            result.Should().BeOfType<CustomerRepository>();
        }

        #endregion

        #region AddCustomer

        [TestMethod]
        public void AddCustomer_WhenCustomerIsNull_ShouldThrow()
        {
            var repo = GetSubjectUnderTest();

            Action act = () => repo.AddCustomer(null);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("customer");
        }

        #endregion

        #region DeleteCustomer

        [TestMethod]
        public void DeleteCustomer_WhenCustomerIsNull_ShouldThrow()
        {
            var repo = GetSubjectUnderTest();

            Action act = () => repo.DeleteCustomer(null);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("customer");
        }

        #endregion

        #region UpdateCustomer

        [TestMethod]
        public void UpdateCustomer_WhenCustomerIsNull_ShouldThrow()
        {
            var repo = GetSubjectUnderTest();

            Action act = () => repo.UpdateCustomer(null);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("customer");
        }

        #endregion

        #region GetAllCustomers

        [TestMethod]
        public void GetAllCustomers_ShouldReturnCustomers()
        {
            var repo = GetSubjectUnderTest();

            var result = repo.GetAllCustomers();

            result.Should().BeEquivalentTo(testData);
        }

        #endregion

        #region GetCustomerById

        [TestMethod]
        public void GetCustomerById_WhenCustomerExists_ShouldReturnCustomer()
        {
            var expectedResult = new Customer
            {
                Id = 2,
                Name = "Mary"
            };

            var repo = GetSubjectUnderTest();

            var result = repo.GetCustomerById(2);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void GetCustomerById_WhenCustomerDoesNotExist_ShouldReturnNull()
        {
            var repo = GetSubjectUnderTest();

            var result = repo.GetCustomerById(99);

            result.Should().BeNull();
        }

        #endregion

        private CustomerRepository GetSubjectUnderTest()
        {
            return new CustomerRepository(context);
        }
    }
}

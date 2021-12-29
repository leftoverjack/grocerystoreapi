using AutoMapper;
using FluentAssertions;
using GroceryStoreAPI.Controllers;
using GroceryStoreAPI.Data;
using GroceryStoreAPI.Dtos;
using GroceryStoreAPI.Models;
using GroceryStoreAPI.Profiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace GroceryStoreAPI.Tests
{
    [TestClass]
    public class CustomersControllerUnitTests
    {
        private ICustomerRepository customerRepository;
        private IMapper mapper;
        private IEnumerable<CustomerReadDto> testData;

        [TestInitialize]
        public void Init()
        {
            this.customerRepository = Substitute.For<ICustomerRepository>();
            this.mapper = new Mapper(
                new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new CustomersProfile());
                }));

            this.testData = new List<CustomerReadDto>
            {
                new CustomerReadDto
                {
                    Id = 1,
                    Name = "Bob"
                },
                new CustomerReadDto
                {
                    Id = 2,
                    Name = "Todd"
                }
            };
        }
        #region Constructor

        [TestMethod]
        public void Constructor_WhenRepositoryIsNull_ShouldThrow()
        {
            this.customerRepository = null;

            Action act = () => GetSubjectUnderTest();

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("customerRepository");
        }

        [TestMethod]
        public void Constructor_WhenMapperIsNull_ShouldThrow()
        {
            this.mapper = null;

            Action act = () => this.GetSubjectUnderTest();

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("mapper");
        }

        [TestMethod]
        public void Constructor_WhenParametersAreValid_ShouldReturnInstance()
        {
            var result = GetSubjectUnderTest();

            result.Should().BeOfType<CustomersController>();
        }

        #endregion

        #region GetAllCustomers

        [TestMethod]
        public void GetAllCustomers_WhenInvoked_ShouldReturnCustomerList()
        {
            this.customerRepository.GetAllCustomers().Returns(new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    Name = "Bob"
                },
                new Customer
                {
                    Id = 2,
                    Name = "Todd"
                }
            });

            var controllerUnderTest = GetSubjectUnderTest();

            var result = controllerUnderTest.GetAllCustomers();

            result.Result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult.Value.Should().BeEquivalentTo(testData);
        }

        #endregion

        #region GetCustomerById

        [TestMethod]
        public void GetCustomerById_WhenCustomerDoesNotExist_ReturnsNotFoundResult()
        {
            customerRepository.GetCustomerById(Arg.Any<int>()).Returns(l => null);

            var controllerUnderTest = GetSubjectUnderTest();

            var result = controllerUnderTest.GetCustomerById(99);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void GetCustomerById_WhenCustomerExists_ReturnsCustomerReadDto()
        {
            var expectedResult = new Customer
            {
                Id = 2,
                Name = "Todd"
            };

            customerRepository.GetCustomerById(Arg.Any<int>()).Returns(expectedResult);

            var controllerUnderTest = GetSubjectUnderTest();

            var result = controllerUnderTest.GetCustomerById(99);

            result.Result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult.Value.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region CreateCustomer

        [TestMethod]
        public void CreateCustomer_WhenParameterIsNull_ReturnsBadRequestResult()
        {
            var controllerUnderTest = GetSubjectUnderTest();

            var result = controllerUnderTest.CreateCustomer(null);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Value.Should().BeEquivalentTo("Null Customer");
        }

        [TestMethod]
        public void CreateCustomer_WhenCustomerNameIsNull_ReturnsBadRequestResult()
        {
            var controllerUnderTest = GetSubjectUnderTest();

            var result = controllerUnderTest.CreateCustomer(new CustomerEditDto
            {
                Name = null
            });

            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Value.Should().BeEquivalentTo("Invalid Customer");
        }

        [TestMethod]
        public void CreateCustomer_WhenCustomerNameIsEmptyString_ReturnsBadRequestResult()
        {
            var controllerUnderTest = GetSubjectUnderTest();

            var result = controllerUnderTest.CreateCustomer(new CustomerEditDto
            {
                Name = string.Empty
            });

            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Value.Should().BeEquivalentTo("Invalid Customer");
        }

        [TestMethod]
        public void CreateCustomer_WhenCustomerIsValid_ReturnsOkResultWithCustomer()
        {
            var expectedResult = new CustomerReadDto
            {
                Id = 0,
                Name = "Fred"
            };

            var controllerUnderTest = GetSubjectUnderTest();

            var result = controllerUnderTest.CreateCustomer(new CustomerEditDto
            {
                Name = "Fred"
            });

            result.Result.Should().BeOfType<CreatedAtRouteResult>();
            var createdAtRouteResult = result.Result as CreatedAtRouteResult;
            createdAtRouteResult.RouteName.Should().Be("GetCustomerById");
            createdAtRouteResult.Value.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region UpdateCustomer
        [TestMethod]
        public void UpdateCustomer_WhenCustomerIsNull_ReturnsBadRequestResult()
        {
            var controllerUnderTest = GetSubjectUnderTest();

            var result = controllerUnderTest.UpdateCustomer(1, null);

            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Value.Should().BeEquivalentTo("Null Customer");
        }

        [TestMethod]
        public void UpdateCustomer_WhenCustomerNameIsNull_ReturnsBadRequestResult()
        {
            var controllerUnderTest = GetSubjectUnderTest();

            var result = controllerUnderTest.UpdateCustomer(1, new CustomerEditDto
            {
                Name = null
            });

            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Value.Should().BeEquivalentTo("Invalid Customer");
        }

        [TestMethod]
        public void UpdateCustomer_WhenCustomerNameIsEmptyString_ReturnsBadRequestResult()
        {
            var controllerUnderTest = GetSubjectUnderTest();

            var result = controllerUnderTest.UpdateCustomer(1, new CustomerEditDto
            {
                Name = string.Empty
            });

            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Value.Should().BeEquivalentTo("Invalid Customer");
        }

        [TestMethod]
        public void UpdateCustomer_WhenCustomerIsNotPresent_ReturnsNotFoundResult()
        {
            customerRepository.GetCustomerById(Arg.Any<int>()).Returns(l => null);

            var controllerUnderTest = GetSubjectUnderTest();

            var result = controllerUnderTest.UpdateCustomer(1, new CustomerEditDto
            {
                Name = "Sue"
            });

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void UpdateCustomer_WhenCustomerIsPresentAndValid_ReturnsNoContentResult()
        {
            var expectedRepoResult = new Customer
            {
                Id = 1,
                Name = "Bob"
            };

            customerRepository.GetCustomerById(Arg.Any<int>()).Returns(expectedRepoResult);

            var controllerUnderTest = GetSubjectUnderTest();

            var result = controllerUnderTest.UpdateCustomer(1, new CustomerEditDto
            {
                Name = "Sue"
            });

            result.Should().BeOfType<NoContentResult>();
        }

        #endregion

        #region DeleteCustomer

        [TestMethod]
        public void DeleteCustomer_WhenCustomerDoesNotExist_ReturnsNotFoundResult()
        {
            customerRepository.GetCustomerById(Arg.Any<int>()).Returns(l => null);

            var controllerUnderTest = GetSubjectUnderTest();

            var result = controllerUnderTest.DeleteCustomer(99);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void DeleteCustomer_WhenCustomerExists_ReturnsNoContentResult()
        {
            var expectedRepoResult = new Customer
            {
                Id = 1,
                Name = "Bob"
            };

            customerRepository.GetCustomerById(Arg.Any<int>()).Returns(expectedRepoResult);

            var controllerUnderTest = GetSubjectUnderTest();

            var result = controllerUnderTest.DeleteCustomer(1);

            result.Should().BeOfType<NoContentResult>();
        }

        #endregion

        private CustomersController GetSubjectUnderTest()
        {
            return new CustomersController(this.customerRepository, this.mapper);
        }
    }
}

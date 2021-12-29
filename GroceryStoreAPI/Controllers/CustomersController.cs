using System;
using System.Collections.Generic;
using AutoMapper;
using GroceryStoreAPI.Data;
using GroceryStoreAPI.Dtos;
using GroceryStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStoreAPI.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        private readonly IMapper _mapper;

        public CustomersController(
            ICustomerRepository customerRepository,
            IMapper mapper)
        {
            if (customerRepository == null)
            {
                throw new ArgumentNullException(nameof(customerRepository));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this._customerRepository = customerRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <returns>Collection of all existing customers</returns>
        [HttpGet]
        public ActionResult<IEnumerable<CustomerReadDto>> GetAllCustomers()
        {
            var customers = _customerRepository.GetAllCustomers();

            return Ok(_mapper.Map<IEnumerable<CustomerReadDto>>(customers));
        }

        /// <summary>
        /// Gets a specific customer by the customer id
        /// GET api/customers/5
        /// </summary>
        /// <param name="id">The unique id of the customer</param>
        /// <returns>The customer with the requested id</returns>
        [HttpGet("{id}", Name = "GetCustomerById")]
        public ActionResult<CustomerReadDto> GetCustomerById(int id)
        {
            var customer = _customerRepository.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CustomerReadDto>(customer));
        }
        
        /// <summary>
        /// Adds a Customer
        /// </summary>
        /// <param name="customer">The new customer to be added</param>
        /// <returns>The newly added customer</returns>
        [HttpPost]
        public ActionResult<CustomerReadDto> CreateCustomer(CustomerEditDto customer)
        {
            if (customer == null)
            {
                return BadRequest("Null Customer");
            }

            if (!customer.Validate())
            {
                return BadRequest("Invalid Customer");
            }

            var customerModel = _mapper.Map<Customer>(customer);

            _customerRepository.AddCustomer(customerModel);
            _customerRepository.SaveChanges();

            // get read dto to return
            var customerReadDto = _mapper.Map<CustomerReadDto>(customerModel);

            return CreatedAtRoute(nameof(GetCustomerById), new { Id = customerReadDto.Id }, customerReadDto);
        }

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="id">the id of the customer to be updated</param>
        /// <param name="customer">the updated customer</param>
        [HttpPut("{id}")]
        public ActionResult UpdateCustomer(int id, CustomerEditDto customer)
        {
            if (customer == null)
            {
                return BadRequest("Null Customer");
            }

            if (!customer.Validate())
            {
                return BadRequest("Invalid Customer");
            }

            var customerFromRepo = _customerRepository.GetCustomerById(id);

            if (customerFromRepo == null)
            {
                return NotFound();
            }

            var updatedCustomerModel = _mapper.Map(customer, customerFromRepo);

            _customerRepository.UpdateCustomer(updatedCustomerModel);
            _customerRepository.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Deletes an existing customer
        /// </summary>
        /// <param name="id">the id of the customer to be deleted</param>
        [HttpDelete("{id}")]
        public ActionResult DeleteCustomer(int id)
        {
            var customerFromRepo = _customerRepository.GetCustomerById(id);

            if (customerFromRepo == null)
            {
                return NotFound();
            }

            _customerRepository.DeleteCustomer(customerFromRepo);
            _customerRepository.SaveChanges();

            return NoContent();
        }
    }
}
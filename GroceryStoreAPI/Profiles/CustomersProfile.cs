using AutoMapper;
using GroceryStoreAPI.Dtos;
using GroceryStoreAPI.Models;

namespace GroceryStoreAPI.Profiles
{
    public class CustomersProfile : Profile
    {
        public CustomersProfile()
        {
            CreateMap<Customer, CustomerReadDto>();
            CreateMap<CustomerEditDto, Customer>();
        }
    }
}

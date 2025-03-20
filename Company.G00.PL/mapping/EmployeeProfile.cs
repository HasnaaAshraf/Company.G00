using System.Dynamic;
using AutoMapper;
using Company.G00.DAL.Models;
using Company.G00.PL.Dtos;

namespace Company.G00.PL.mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDto, Employee>();  // .ForMember(D => D.Name,o=>o.MapFrom(S.Name)) => For if The Name Difference 
            //CreateMap<CreateEmployeeDto, Employee>().reverseMap();
            CreateMap<Employee, CreateEmployeeDto>();
        }
    }
}

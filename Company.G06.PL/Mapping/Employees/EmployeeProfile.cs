using AutoMapper;
using Company.G06.DAL.Models;
using Company.G06.PL.ViewModels.Employees;

namespace Company.G06.PL.Mapping.Employees
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee,EmployeeViewModel>().ReverseMap();
        }
    }
}

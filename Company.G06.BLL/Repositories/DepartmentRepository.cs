using Company.G06.BLL.Interfaces;
using Company.G06.DAL.Data.Contexts;
using Company.G06.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G06.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department> , IDepartmentRepository
    {
        

        public DepartmentRepository(AppDbContext context) : base(context) // ASK CLR To Create Object From AppDbContext
        {
            
            
        }
      

      

        

      
    }
}

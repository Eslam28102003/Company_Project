﻿using Company.G06.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G06.BLL.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        //IEnumerable<Department> GetAll();

        //Department Get(int? id);

        //int Add(Department entity);
        //int Update(Department entity);
        //int Delete(Department entity);
    }
}
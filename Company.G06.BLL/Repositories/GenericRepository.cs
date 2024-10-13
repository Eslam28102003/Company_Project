using Company.G06.BLL.Interfaces;
using Company.G06.DAL.Data.Contexts;
using Company.G06.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G06.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private protected readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T)==typeof(Employee))
            {
                return (IEnumerable<T>) await _context.Employees.Include(E=>E.WorkFor).AsNoTracking().ToListAsync();
            }
            else
            {
                return  await _context.Set<T>().ToListAsync();

            }
        }

        public async Task<T> GetAsync(int? id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async void AddAsync(T entity)
        {
          await _context.AddAsync(entity);
        }
        public  async void UpdateAsync(T entity)
        {
           _context.Update(entity);
        }

        public async void DeleteAsync(T entity)
        {
            _context.Remove(entity);
        }

     

       

     
    }
}

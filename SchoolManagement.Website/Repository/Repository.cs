using SchoolManagement.Website.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EmployeeManagement.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ApplicationDbContext db;
        private DbSet<T> dbSet;

        public Repository()
        {
            db = new ApplicationDbContext();
            dbSet = db.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }
       
        public T GetById(object Id)
        {
            return dbSet.Find(Id);
        }

        public T GetByName(string name)
        {
            return dbSet.Find(name);
        }


        public void Insert(T obj)
        {
            dbSet.Add(obj);
        }
        public void Update(T obj)
        {
            db.Entry(obj).State = EntityState.Modified;
        }
        public void Delete(object Id)
        {
            T getObjById = dbSet.Find(Id);
            dbSet.Remove(getObjById);
        }
        public void Save()
        {
            db.SaveChanges();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.db != null)
                {
                    this.db.Dispose();
                    this.db = null;
                }
            }
        }


    }
}
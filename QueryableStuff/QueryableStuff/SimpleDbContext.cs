﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryableStuff
{
    public class SimpleDbContext : IDisposable
    {
        List<Product> _products;
        List<User> _users;

        public SimpleDbContext()
        {
            _products = new List<Product>();
            _users = new List<User>();

            // Initialization.
            _products.Add( new Product() { Name = "Aspirateur", Price = 150 } );
            _products.Add( new Product() { Name = "Vibro", Price = 15 } );
            _products.Add( new Product() { Name = "Sucette", Price = 5 } );
        }

        public IEnumerable<Product> Products { get { return _products; } }
        
        public IQueryable<Product> TProducts { get { return null; } }

        public IList<User> Users { get { return _users; } }

        public void SaveChanges()
        {
        }

        void IDisposable.Dispose()
        {
        }
    }
}

namespace gabquery
{
    public class SimpleDbContext : IDisposable
    {
        List<Product> _products;
        List<User> _users;

        public SimpleDbContext()
        {
            _products = new List<Product>();
            _users = new List<User>();

            // Initialization.
            _products.Add(new Product() { Name = "Aspirateur", Price = 150 });
            _products.Add(new Product() { Name = "Vibro", Price = 15 });
            _products.Add(new Product() { Name = "Sucette", Price = 5 });
        }

        public IList<Product> Products { get { return _products; } }

        public IList<User> Users { get { return _users; } }


        public void Dispose()
        {
        }
    }
}

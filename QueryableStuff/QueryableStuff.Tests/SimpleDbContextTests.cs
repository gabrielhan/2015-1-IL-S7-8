﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace QueryableStuff.Tests
{
    [TestFixture]
    public class SimpleDbContextTests
    {
        [Test]
        public void request_to_the_DAL()
        {
            SimpleDbContext ctx = new SimpleDbContext();
            foreach( var p in ctx.Products.Where( product => product.Price > 10 ) )
            {
                Console.WriteLine( p.Name );
            }
        }

    }
}


namespace gabquery.Tests
{
    using gabquery;

    [TestFixture]
    public class SimpleDbContextTests
    {
        [Test]
        public void request_to_the_DAL()
        {
            using (SimpleDbContext ctx = new SimpleDbContext())
            {
                foreach (var p in ctx.Products.Where(product => product.Price > 10))
                {
                    Console.WriteLine(p.Name);
                }
            }
        }

        [Test]
        public void request_to_the_DAL2()
        {
            using (SimpleDbContext ctx = new SimpleDbContext())
            {
                foreach (var p in Enumerable.Where(ctx.Products, product => product.Price > 10))
                {
                    Console.WriteLine(p.Name);
                }
            }
        }
    }
}

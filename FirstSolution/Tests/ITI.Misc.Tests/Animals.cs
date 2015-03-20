using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Misc.Tests
{
    [TestFixture]
    public class Animals
    {
        class Animal
        {
            public string Name;
            public int Position;

            public void Forward() { Position++; }
            
            public virtual void VForward() { Position++; }

            public virtual void MakeNoise() { }
        }

        class Dog : Animal 
        {
            public bool IsWild;

            public new void Forward() { Position += 5; }

            public override void MakeNoise() { Console.WriteLine( "oua!" ); }

            public override void VForward() { Position += 5; }
        }

        [Test]
        public void inheritance()
        {
            Dog d = new Dog();
            Animal a = new Dog();
            // Downcasting:
            ((Dog)a).Forward();
            (a as Dog).Forward();
            ((IDisposable)a).Dispose();

            d.Forward(); //<==> Animals_Dog_Forward( d )
            a.Forward(); //<==> Animals_Animal_Forward( a )

            d.VForward(); //<==> Animals_Dog_VForward( d )

            Animal realAnimal = new Animal();
            DoSomethingWithAnAnimal( a );
            DoSomethingWithAnAnimal( realAnimal );
        }

        static void DoSomethingWithAnAnimal( Animal a )
        {
            a.VForward(); //<==> Animals_Dog_VForward( a )
        }

    }
}

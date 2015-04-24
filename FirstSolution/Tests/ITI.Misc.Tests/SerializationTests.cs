using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Misc.Tests
{

    [Serializable]
    class Friend
    {
        string _firstName;
        string _lastName;

        public string FirstName { get { return _firstName; } set { _firstName = value; } }

        public string LastName { get { return _lastName; } set { _lastName = value; }}

    }

    [Serializable]
    class User
    {
        readonly string _name;
        int _age;
        readonly List<Friend> _friends;

        public User( string name )
        {
            _name = name;
            _friends = new List<Friend>();
        }

        public List<Friend> Friends { get { return _friends; } }

        public int Age
        {
            get { return _age; }
            set 
            {
                if( value < 0 || value > 500 ) throw new ArgumentException( "age" );
                _age = value; 
            }
        }

        public string Name { get { return _name; } }

    }


    [TestFixture]
    public class SerializationTests
    {
        [Test]
        public void how_standard_serialization_works()
        {
            User u = new User( "John" ) { Age = 58 };
            u.Friends.Add( new Friend(){ FirstName = "F1", LastName = "Alesi" } );
            u.Friends.Add( new Friend() { FirstName = "F2", LastName = "Alesi2" } );

            using( var s = new MemoryStream() )
            {
                Assert.That( s.Position == 0 );
                
                BinaryFormatter f = new BinaryFormatter();
                f.Serialize( s, u );
                
                Assert.That( s.Position > 0 );
                s.Position = 0;
                object o = f.Deserialize( s );
                Assert.IsInstanceOf<User>( o );
                User u2 = (User)o;

                Assert.That( u != u2 );
                Assert.That( u.Name == u2.Name );
                Assert.That( u.Age == u2.Age );
                Assert.That( u.Friends != u2.Friends );
                Assert.That( u.Friends[0] != u2.Friends[0] );
                Assert.That( u.Friends.Count == u2.Friends.Count );
                Assert.That( u.Friends.Count == u2.Friends.Count );
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Misc.Tests
{
    [TestFixture]
    public class BinarySerializationSample
    {

        class Friend
        {
            string _firstName;
            string _lastName;
            readonly User _user;

            public Friend( User u )
            {
                _user = u;
            }

            public User User { get { return _user; } }

            public string FirstName { get { return _firstName; } set { _firstName = value; } }

            public string LastName { get { return _lastName; } set { _lastName = value; } }

        }

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


        public void binary_writer_and_reader_are_the_keys()
        {
            User u = new User( "John" ) { Age = 58 };
            var f1 = new Friend( u ) { FirstName = "F1", LastName = "Alesi" };
            u.Friends.Add( f1 );
            u.Friends.Add( new Friend( u ) { FirstName = "F2", LastName = "Alesi2" } );

            using( var s = new MemoryStream() )
            {
                BinaryWriter w = new BinaryWriter( s, Encoding.UTF8 );
                u.Write( w );
                s.Position = 0;
                BinaryReader r = new BinaryReader( s, Encoding.UTF8 );
                User u2 = new User( r );
           }
        }

    }
}

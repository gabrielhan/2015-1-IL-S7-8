using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
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
        readonly User _user;

        public Friend( User u )
        {
            _user = u;
        }

        public User User { get { return _user; } }

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
        const string fileToWrite = @"E:\Intech\2015-1\S7-8\Dev\2015-1-IL-S7-8\FirstSolution\Tests\ITI.Misc.Tests\Serialization.Result.bin";

        [Test]
        public void how_standard_serialization_works()
        {
            User u = new User( "John" ) { Age = 58 };
            u.Friends.Add( new Friend( u ) { FirstName = "F1", LastName = "Alesi" } );
            u.Friends.Add( new Friend( u ) { FirstName = "F2", LastName = "Alesi2" } );

            using( var s = new MemoryStream() )
            {
                Assert.That( s.Position == 0 );

                BinaryFormatter f = new BinaryFormatter();
                f.Serialize( s, u );

                File.WriteAllBytes( fileToWrite, s.ToArray() );

                Assert.That( s.Position > 0 );
                s.Position = 0;
                object o = f.Deserialize( s );
                Assert.IsInstanceOf<User>( o );
                User u2 = (User)o;

                Assert.That( u != u2 );
                Assert.That( u.Name == u2.Name );
                Assert.That( u.Age == u2.Age );
                Assert.That( u.Friends != u2.Friends );
                Assert.That( u.Friends.Count == u2.Friends.Count );
                Assert.That( u.Friends[0] != u2.Friends[0] );
                Assert.That( u.Friends[0].FirstName == u2.Friends[0].FirstName );
                Assert.That( u.Friends[0].LastName == u2.Friends[0].LastName );
                Assert.That( u.Friends[1].FirstName == u2.Friends[1].FirstName );
                Assert.That( u.Friends[1].LastName == u2.Friends[1].LastName );

                Assert.That( u2.Friends[0].User == u2 );
                Assert.That( u2.Friends[1].User == u2 );
            }
        }

        [Test]
        public void soap_formatter_does_not_support_generics()
        {
            User u = new User( "John" ) { Age = 58 };
            u.Friends.Add( new Friend( u ) { FirstName = "F1", LastName = "Alesi" } );
            u.Friends.Add( new Friend( u ) { FirstName = "F2", LastName = "Alesi2" } );

            Assert.Throws<SerializationException>( () => CloneSerializableObject( u, new SoapFormatter() ) ); 
        }

        [Test]
        public void a_graph_has_no_head()
        {
            User u = new User( "John" ) { Age = 58 };
            var f1 = new Friend( u ) { FirstName = "F1", LastName = "Alesi" };
            u.Friends.Add( f1 );
            u.Friends.Add( new Friend( u ) { FirstName = "F2", LastName = "Alesi2" } );
            using( var s = new MemoryStream() )
            {
                BinaryFormatter f = new BinaryFormatter();
                f.Serialize( s, f1 );
                s.Position = 0;
                Friend f1Bis = (Friend)f.Deserialize( s );

                User u2 = f1Bis.User;

                Assert.That( u != u2 );
                Assert.That( u.Name == u2.Name );
                Assert.That( u.Age == u2.Age );
                Assert.That( u.Friends != u2.Friends );
                Assert.That( u.Friends.Count == u2.Friends.Count );
                Assert.That( u.Friends[0] != u2.Friends[0] );
                Assert.That( u.Friends[0].FirstName == u2.Friends[0].FirstName );
                Assert.That( u.Friends[0].LastName == u2.Friends[0].LastName );
                Assert.That( u.Friends[1].FirstName == u2.Friends[1].FirstName );
                Assert.That( u.Friends[1].LastName == u2.Friends[1].LastName );

                Assert.That( u2.Friends[0].User == u2 );
                Assert.That( u2.Friends[1].User == u2 );

            }
        }


        [Serializable]
        class FriendAutonomous
        {
            string _firstName;
            string _lastName;
            
            [NonSerialized]
            readonly UserAutonomous _user;

            public FriendAutonomous( UserAutonomous u )
            {
                _user = u;
            }

            public UserAutonomous User { get { return _user; } }

            public string FirstName { get { return _firstName; } set { _firstName = value; } }

            public string LastName { get { return _lastName; } set { _lastName = value; } }

        }

        [Serializable]
        class UserAutonomous : IDeserializationCallback
        {
            readonly string _name;
            int _age;
            readonly List<FriendAutonomous> _friends;

            public UserAutonomous( string name )
            {
                _name = name;
                _friends = new List<FriendAutonomous>();
            }

            public List<FriendAutonomous> Friends { get { return _friends; } }

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

            void IDeserializationCallback.OnDeserialization( object sender )
            {
                foreach( var f in Friends )
                {
                    // f.User = this;
                    typeof( FriendAutonomous )
                        .GetField( "_user", BindingFlags.Instance | BindingFlags.NonPublic )
                        .SetValue( f, this );
                }
            }
        }

        T CloneSerializableObject<T>( T o, IFormatter formatter = null )
        {
            using( var s = new MemoryStream() )
            {
                if( formatter == null ) formatter = new BinaryFormatter();
                formatter.Serialize( s, o );
                s.Position = 0;
                return (T)formatter.Deserialize( s );
            }
        }

        [Test]
        public void saving_autonomous_friend()
        {
            UserAutonomous u = new UserAutonomous( "John" ) { Age = 58 };
            var f1 = new FriendAutonomous( u ) { FirstName = "F1", LastName = "Alesi" };
            u.Friends.Add( f1 );
            u.Friends.Add( new FriendAutonomous( u ) { FirstName = "F2", LastName = "Alesi2" } );

            var f1Bis = CloneSerializableObject( f1 );
            Assert.That( f1Bis.FirstName == "F1" && f1Bis.LastName == "Alesi" );
            Assert.IsNull( f1Bis.User );
        }

        [Test]
        public void saving_autonomous_user()
        {
            UserAutonomous u = new UserAutonomous( "John" ) { Age = 58 };
            var f1 = new FriendAutonomous( u ) { FirstName = "F1", LastName = "Alesi" };
            u.Friends.Add( f1 );
            u.Friends.Add( new FriendAutonomous( u ) { FirstName = "F2", LastName = "Alesi2" } );

            var u2 = CloneSerializableObject( u );
            Assert.That( u2.Friends.Count == 2 );
            Assert.IsNotNull( u2.Friends[0].User );
        }


    }
}

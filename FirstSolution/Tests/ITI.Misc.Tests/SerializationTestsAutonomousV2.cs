using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Misc.Tests
{

    [TestFixture]
    public class SerializationTestsAutonomousV2
    {
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

        class UserAutonomous : ISerializable    
        {
            readonly string _name;
            int _age;
            readonly List<FriendAutonomous> _friends;

            public UserAutonomous( string name )
            {
                _name = name;
                _friends = new List<FriendAutonomous>();
            }

            protected UserAutonomous(SerializationInfo info, StreamingContext context)
            {
                _name = info.GetString( "n" );
                _age = info.GetInt32( "a" );
                FriendAutonomous[] f = (FriendAutonomous[])info.GetValue( "f", typeof(FriendAutonomous) );
            }

            public void GetObjectData( SerializationInfo info, StreamingContext context )
            {
                info.AddValue( "n", _name );
                info.AddValue( "a", _age );
                info.AddValue( "f", _friends.ToArray() );
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


        }

        T CloneSerializableObject<T>( T o )
        {
            using( var s = new MemoryStream() )
            {
                BinaryFormatter f = new BinaryFormatter();
                f.Serialize( s, o );
                s.Position = 0;
                return (T)f.Deserialize( s );
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
            Assert.IsNull( u2.Friends[0].User );
        }


    }
}

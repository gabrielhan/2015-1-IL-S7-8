using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Bottle
{
    public class ListSimpleBottle
    {
        SimpleBottle[] _tab;
        int _count;

        public ListSimpleBottle()
        {
            _tab = new SimpleBottle[4];
        }

        public int Count 
        {
            get { return _count; } 
        }

        public SimpleBottle this[ int i ]
        {
            get 
            {
                if( i >= _count ) throw new IndexOutOfRangeException();
                return _tab[i];
            }
            set 
            {
                if( i >= _count ) throw new IndexOutOfRangeException();
                _tab[i] = value; 
            }
        } 
        
        public void Add( SimpleBottle b )
        {
            Debug.Assert( _count <= _tab.Length );
            if( ++_count > _tab.Length )
            {
                //var newOne = new SimpleBottle[_tab.Length * 2];
                //Array.Copy( _tab, newOne, _tab.Length );
                Array.Resize( ref _tab, _tab.Length * 2 );
            }
            _tab[_count - 1] = b;
        }

        public void RemoveAt( int i )
        {
            if( i < 0 || i >= _count ) throw new IndexOutOfRangeException();
            int lenToCopy = --_count - i;
            if( lenToCopy > 0 ) Array.Copy( _tab, i + 1, _tab, i, lenToCopy );
            _tab[_count] = null;
        }


        /// <summary>
        /// Returns the index (zero based) of the first occurence of the given item. -1 if not found.
        /// </summary>
        /// <param name="item">The bottle to find.</param>
        /// <returns>Positive if found, -1 otherwise.</returns>
        public int IndexOf( SimpleBottle item )
        {
            return 0;
        }



        //public bool Remove( SimpleBottle b )
        //{

        //}


    }
}

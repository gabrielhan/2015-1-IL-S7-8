using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ITI.Bottle
{
    public class ITIDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey,TValue>>
    {
        class Node
        {
            public readonly TKey Key;
            public TValue Value;
            public Node Next;

            public Node( TKey k, TValue initialValue )
            {
                Key = k;
                Value = initialValue;
            }
        }

        Node[] _buckets;
        int _count;
        int _version;

        public ITIDictionary()
        {
            _buckets = new Node[5];
        }

        public int Count
        {
            get { return _count; }
        }

        public TValue this[ TKey key ]
        {
            get
            {
                int idx = GetBucketIndex( key );
                Node found = FindInBucket( idx, key );
                if( found == null ) throw new KeyNotFoundException();
                return found.Value;
            }
            set
            {
                int idx = GetBucketIndex( key );
                Node n = FindInBucket( idx, key );
                if( n != null ) n.Value = value;
                else DoAdd( key, value, idx );
                ++_version;
            }
        }

        public void Clear()
        {
            Array.Clear( _buckets, 0, _buckets.Length );
            _count = 0;
            ++_version;
        }

        public bool ContainsKey( TKey key )
        {
            return FindInBucket( GetBucketIndex( key ), key ) != null;
        }

        public bool TryGetValue( TKey key, out TValue value )
        {
            Node n = FindInBucket( GetBucketIndex( key ), key );
            if( n == null )
            {
                value = default( TValue );
                return false;
            }
            value = n.Value;
            return true;
        }



        public bool Remove( TKey key )
        {
            int idx = GetBucketIndex( key );
            Node head = _buckets[idx];
            if( head == null ) return false;
            Node prev = null;
            while( head != null )
            {
                if( head.Key.Equals( key ) )
                {
                    if( prev == null )
                    {
                        _buckets[idx] = head.Next;
                    }
                    else
                    {
                        prev.Next = head.Next;
                    }
                    --_count;
                    ++_version;
                    return true;
                }
                prev = head;
                head = head.Next;
            }
            return false;
        }

        public void Add( TKey key, TValue value )
        {
            int idx = GetBucketIndex( key );
            if( FindInBucket( idx, key ) != null ) throw new ArgumentException( "Key already exists!" );
            DoAdd( key, value, idx );
        }

        void DoAdd( TKey key, TValue value, int idx )
        {
            Node newOne = new Node( key, value );
            newOne.Next = _buckets[idx];
            _buckets[idx] = newOne;
            ++_count;
            ++_version;
        }

        int GetBucketIndex( TKey key )
        {
            int h = key.GetHashCode();
            int idxBucket = Math.Abs( h % _buckets.Length );
            return idxBucket;
        }

        Node FindInBucket( int idxBucket, TKey key )
        {
            Node head = _buckets[idxBucket];
            while( head != null )
            {
                if( head.Key.Equals( key ) ) return head;
                head = head.Next;
            }
            return null;
        }

        class E : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            readonly ITIDictionary<TKey,TValue> _owner;
            KeyValuePair<TKey, TValue> _current;
            int _originalVersion;

            int _idxBucket;
            Node _nextNode;

            public E( ITIDictionary<TKey,TValue> owner )
            {
                _owner = owner;
                _idxBucket = -1;
                _originalVersion = _owner._version;
            }

            public KeyValuePair<TKey, TValue> Current
            {
                get { return _current; }
            }

            public bool MoveNext()
            {
                if( _owner._version != _originalVersion ) throw new InvalidOperationException( "The dictionary has been modified." );
                while( _nextNode == null )
                {
                    if( ++_idxBucket >= _owner._buckets.Length ) return false;
                    _nextNode = _owner._buckets[_idxBucket];
                }
                Debug.Assert( _nextNode != null );
                _current = new KeyValuePair<TKey, TValue>( _nextNode.Key, _nextNode.Value );
                _nextNode = _nextNode.Next;
                return true;
            }

            object System.Collections.IEnumerator.Current
            {
                get { return _current; }
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public void Dispose()
            {
            }

        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumeratorHandMande()
        {
            return new E( this );
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for( int i = 0; i < _buckets.Length; i++ )
            {
                Node n = _buckets[i];
                while( n != null )
                {
                    yield return new KeyValuePair<TKey, TValue>( n.Key, n.Value );
                    n = n.Next;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

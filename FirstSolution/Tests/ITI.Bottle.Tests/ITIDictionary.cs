using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITI.Bottle
{
    public class ITIDictionary<TKey, TValue>
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

        public ITIDictionary()
        {
            _buckets = new Node[5];
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
            }
        }

        public void Clear()
        {
            Array.Clear( _buckets, 0, _buckets.Length );
            _count = 0;
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

    }
}

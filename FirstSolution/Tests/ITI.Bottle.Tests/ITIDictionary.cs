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
        }

        public void Add( TKey key, TValue value )
        {
            int idx = GetBucketIndex( key );
            if( FindInBucket( idx, key ) != null ) throw new ArgumentException( "Key already exists!" );
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

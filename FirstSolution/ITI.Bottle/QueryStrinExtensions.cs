using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Bottle
{
    public static class QueryStrinExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> ExpandedValues( this Dictionary<string, List<string>> @this )
        {
            yield return new KeyValuePair<string, string>( "Hello!", "From ExpandedValues (string version)" );

            foreach( var kv in @this )
                foreach( var val in kv.Value )
                    yield return new KeyValuePair<string, string>( kv.Key, val );

            yield return new KeyValuePair<string, string>( "Bye bye", "From ExpandedValues" );
        }
        
        public static IEnumerable<KeyValuePair<TKey, TValue>> ExpandedValues<TKey,TValue>( this Dictionary<TKey, List<TValue>> @this )
        {
            foreach( var kv in @this )
                foreach( var val in kv.Value )
                    yield return new KeyValuePair<TKey, TValue>( kv.Key, val );
        }
    }
}

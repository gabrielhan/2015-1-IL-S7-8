using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Misc
{
    public interface IPushable<T>
    {
        bool IsOpen { get; }
        void Open();
        void Close();
        void Push( T e );
    }

    public static class PushableExtensions
    {
        public static void SimplePush<T>( this IPushable<T> @this, T e )
        {
            bool mustClose = !@this.IsOpen;
            if( mustClose ) @this.Open();
            @this.Push( e );
            if( mustClose ) @this.Close();
        }
    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Misc
{
    public enum KrabouilleMode
    {
        Krabouille,
        Dekrabouille
    }

    public class KrabouilleStream : Stream
    {
        readonly Stream _stream;
        readonly KrabouilleMode _mode;
        byte[] _cryptData;
        int _currentCryptDataIndex;
        readonly Random _random;

        public KrabouilleStream( Stream s, KrabouilleMode mode, string passPhrase )
        {
            _stream = s;
            _mode = mode;
            _cryptData = Encoding.UTF8.GetBytes( passPhrase );
            _random = new Random( passPhrase.GetHashCode() );
        }

        public override bool CanRead
        {
            get { return _mode == KrabouilleMode.Dekrabouille; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return _mode == KrabouilleMode.Krabouille; }
        }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override int Read( byte[] buffer, int offset, int count )
        {
            if( _mode == KrabouilleMode.Krabouille ) throw new InvalidOperationException();
            
            int lenRead = _stream.Read( buffer, offset, count );
            for( int i = 0; i < lenRead; ++i )
            {
                var b = buffer[offset + i];
                //buffer[offset + i] = (byte)(b ^ _cryptData[_currentCryptDataIndex]);
                buffer[offset + i] ^= _cryptData[_currentCryptDataIndex];
                if( ++_currentCryptDataIndex == _cryptData.Length ) _currentCryptDataIndex = 0;
                unchecked
                {
                    _cryptData[_currentCryptDataIndex] += b;
                }
            }
            return lenRead;
        }

        public override void Write( byte[] buffer, int offset, int count )
        {
            if( _mode == KrabouilleMode.Dekrabouille ) throw new InvalidOperationException();

            for( int i = 0; i < count; ++i )
            {
                var b = buffer[offset + i] ^= _cryptData[_currentCryptDataIndex];
                if( ++_currentCryptDataIndex == _cryptData.Length ) _currentCryptDataIndex = 0;
                unchecked
                {
                    _cryptData[_currentCryptDataIndex] += b;
                }
            }
            _stream.Write( buffer, offset, count );
        }

        public override long Seek( long offset, SeekOrigin origin )
        {
            throw new NotSupportedException();
        }

        public override void SetLength( long value )
        {
            throw new NotSupportedException();
        }

    }
}

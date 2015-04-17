using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            int totalLenRead = 0;
            int totalSaltCount = 0;
            int saltCount = count;
            for(;;)
            {
                int lenRead = _stream.Read( buffer, offset, saltCount );
                saltCount = 0;
                for( int i = 0; i < lenRead; ++i )
                {
                    var b = buffer[offset + i + saltCount];
                    buffer[offset + i] = (byte)(b ^ _cryptData[_currentCryptDataIndex]);
                    if( ++_currentCryptDataIndex == _cryptData.Length ) _currentCryptDataIndex = 0;
                    unchecked
                    {
                        _cryptData[_currentCryptDataIndex] += b;
                    }
                    if( _random.NextDouble() < 0.02 )
                    {
                        _random.Next();
                        ++saltCount;
                    }
                }
                if( saltCount == 0 ) break;
                totalSaltCount += saltCount;
                lenRead -= saltCount;
                totalLenRead += lenRead;
                offset += lenRead;
            }
            return totalLenRead;
        }

        byte[] _localBuffer = new byte[256];

        public override void Write( byte[] buffer, int offset, int count )
        {
            if( _mode == KrabouilleMode.Dekrabouille ) throw new InvalidOperationException();

            while( count > 0 )
            {
                int len = Math.Min( count, _localBuffer.Length-1 );
                Array.Copy( buffer, offset, _localBuffer, 0, len );
                offset += len;
                count -= len;
                WriteLocalBuffer( len );
            }
            WriteLocalBuffer( count );
        }

        private void WriteLocalBuffer( int count )
        {
            Debug.Assert( count < _localBuffer.Length );
            int writtenCount = 0;
            for( int i = 0; i < count; ++i )
            {
                var b = _localBuffer[i] ^= _cryptData[_currentCryptDataIndex];
                if( ++_currentCryptDataIndex == _cryptData.Length ) _currentCryptDataIndex = 0;
                unchecked
                {
                    _cryptData[_currentCryptDataIndex] += b;
                }
                // Salt
                if( _random.NextDouble() < 0.02 )
                {
                    var saved = _localBuffer[i + 1];
                    _localBuffer[i + 1] = (byte)_random.Next();
                    _stream.Write( _localBuffer, writtenCount, i + 2 - writtenCount );
                    writtenCount = i + 1;
                    _localBuffer[i + 1] = saved;
                }
            }
            _stream.Write( _localBuffer, writtenCount, count - writtenCount );
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

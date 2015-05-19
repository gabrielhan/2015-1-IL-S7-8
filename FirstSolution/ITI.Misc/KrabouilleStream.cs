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

        const double _saltRate = 1.0;

        public KrabouilleStream( Stream s, KrabouilleMode mode, string passPhrase )
        {
            _stream = s;
            _mode = mode;
            _cryptData = Encoding.UTF8.GetBytes( passPhrase );
            // DO NOT USE GetHashCode: it is not stable!
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

        bool _isSaltPreview;

        public override int Read( byte[] buffer, int offset, int count )
        {
            if( _mode == KrabouilleMode.Krabouille ) throw new InvalidOperationException();
            int bufferLimit = offset + count;
            int totalLenRead = 0;
            int saltCount = count;
            for( ; ; )
            {
                int lenRead = _stream.Read( buffer, offset, saltCount );
                if( totalLenRead + lenRead < count )
                    bufferLimit = offset + lenRead;
                saltCount = 0;
                for( int i = 0; i < lenRead; ++i )
                {
                    if( (offset + i + saltCount) >= bufferLimit ) break;
                    if( !_isSaltPreview )
                    {
                        if( _random.NextDouble() < _saltRate )
                        {
                            _random.Next();
                            _isSaltPreview = true;
                            ++saltCount;
                        }
                    }
                    if( (offset + i + saltCount) >= bufferLimit ) break;
                    _isSaltPreview = false;
                    _cryptData[_currentCryptDataIndex] = 0;
                    var b = buffer[offset + i + saltCount];
                    buffer[offset + i] = (byte)(b ^ _cryptData[_currentCryptDataIndex]);
                    if( ++_currentCryptDataIndex == _cryptData.Length ) _currentCryptDataIndex = 0;
                    unchecked
                    {
                        _cryptData[_currentCryptDataIndex] += b;
                    }
                }
                lenRead -= saltCount;
                totalLenRead += lenRead;
                if( saltCount == 0 ) break;
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
                int len = Math.Min( count, _localBuffer.Length - 1 );
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
                // Salt
                if( _random.NextDouble() < _saltRate )
                {
                    var saved = _localBuffer[i];
                    _localBuffer[i] = (byte)_random.Next();
                    _localBuffer[i] = 255;
                    _stream.Write( _localBuffer, writtenCount, i + 1 - writtenCount );
                    writtenCount = i;
                    _localBuffer[i] = saved;
                }
                _cryptData[_currentCryptDataIndex] = 0;
                var b = _localBuffer[i] ^= _cryptData[_currentCryptDataIndex];
                if( ++_currentCryptDataIndex == _cryptData.Length ) _currentCryptDataIndex = 0;
                unchecked
                {
                    _cryptData[_currentCryptDataIndex] += b;
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




namespace gaby.Misc
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
        int _jcount;
        readonly Random _randy;
        readonly byte[] _pwByte;
        int _salt;
        int _buffersize;


        public KrabouilleStream(Stream s, KrabouilleMode mode, string passPhrase)
        {

            _stream = s;
            _mode = mode;
            _jcount = 0;
            _randy = new Random(hashy(passPhrase));
            // todo hashy
            _pwByte = Encoding.UTF8.GetBytes(passPhrase);
            // salt config
            _salt = 15;
            _buffersize = 98;
        }

        private void WriteWithKey(byte[] buffy, int i, int operand)
        {
            buffy[i] = (byte)(operand ^ _pwByte[_jcount % _pwByte.Length]);
            var b = buffy[i];
            unchecked
            {
                _jcount++;
                _pwByte[_jcount % _pwByte.Length] += b;
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {

            var randint = 0;
            int rCursor = offset;
            int i = 0;
            byte[] buffy = new byte[_buffersize];

            for (; ; )
            {

                randint = _randy.Next(255);
                if (randint < _salt)
                {
                    randint = _randy.Next(255);
                    WriteWithKey(buffy, i, randint);
                }
                else
                {
                    WriteWithKey(buffy, i, randint ^ (int)buffer[rCursor]);
                    rCursor++;
                }

                if (rCursor >= (offset + count))
                {
                    _stream.Write(buffy, 0, i + 1);
                    break;
                }
                else
                {
                    i++;
                    if (i >= _buffersize) { _stream.Write(buffy, 0, _buffersize); i = 0; }
                }

            }
        }



        public override int Read(byte[] buffer, int offset, int count)
        {
            //init
            int wcursor = offset;
            var randint = 0;
            int currentoffset = offset;
            //  bool stop = false;

            // reading loop
            for (; ; )
            {
                var lenread = _stream.Read(buffer, wcursor, count + offset - wcursor);
                //wcursor = where we write the good bytes; is between current offset and (lenread + current offset)
                //offset = first byte, needed for return 
                // currentoffset = start of the current dekrabouille loop = last pos of the wcursor from the previous loop

                // stop when lenread < 1 ? but need one more reading ;c
                // if (lenread < (count + offset - wcursor)) { stop = true; }
                currentoffset = wcursor;

                //dekrabouille last reading
                for (int i = wcursor; i < (lenread + currentoffset); ++i)
                {

                    //key stuff
                    var b = buffer[i];
                    buffer[i] ^= _pwByte[_jcount % _pwByte.Length];
                    unchecked
                    {
                        _jcount++;
                        _pwByte[_jcount % _pwByte.Length] += b;
                    }

                    //salt stuff
                    randint = _randy.Next(255);

                    if (randint < _salt)
                    {
                        randint = _randy.Next(255);
                        // wcursor doesnt move, i will.
                    }
                    else
                    {
                        buffer[wcursor] = (byte)(randint ^ buffer[i]);
                        wcursor++;
                        //a good byte was added wcursor moves up. 
                    }


                }
                if (wcursor >= (lenread + currentoffset)) { break; }
            }
            return wcursor - offset;
        }




        public override bool CanRead
        {
            get { if (_mode == KrabouilleMode.Krabouille) { return false; } else return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { if (_mode == KrabouilleMode.Krabouille) { return true; } else return false; }
        }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override long Length
        {
            get { return _stream.Length; }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        private int hashy(string input)
        {

            // LOLOLOLOL use something better
            return input.GetHashCode();
        }
    }
}
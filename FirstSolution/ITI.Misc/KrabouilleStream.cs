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

        public KrabouilleStream( Stream s, KrabouilleMode mode, string passPhrase )
        {
            _stream = s;
            _mode = mode;
            _cryptData = Encoding.UTF8.GetBytes( passPhrase ); 
        }

        public override bool CanRead
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanSeek
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanWrite
        {
            get { throw new NotImplementedException(); }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
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

        public override int Read( byte[] buffer, int offset, int count )
        {
            throw new NotImplementedException();
        }

        public override long Seek( long offset, SeekOrigin origin )
        {
            throw new NotImplementedException();
        }

        public override void SetLength( long value )
        {
            throw new NotImplementedException();
        }

        public override void Write( byte[] buffer, int offset, int count )
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.IO;
using System.Text;

namespace Antix.IO.Entities
{
    public class IOFileEntityWriter :
        StreamWriter
    {
        readonly IOFileEntity _entity;

        public IOFileEntity Entity
        {
            get { return _entity; }
        }

        IOFileEntityWriter(
            Stream stream, Encoding encoding, IOFileEntity entity)
            : base(stream, encoding)
        {
            _entity = entity;
        }

        public static IOFileEntityWriter Create(
            Stream stream, 
            Encoding encoding,
            IOFileEntity entity)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (encoding == null) throw new ArgumentNullException("encoding");
            if (entity == null) throw new ArgumentNullException("entity");

            return new IOFileEntityWriter(stream, encoding, entity);
        }
    }
}
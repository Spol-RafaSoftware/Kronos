﻿using System;
using System.Buffers;
using System.IO;
using Kronos.Core.Serialization;

namespace Kronos.Core.Storage
{
    public class BufferedStream : Stream
    {
        public byte[] Pool => _pool;

        private byte[] _pool;
        private int _length;
        private readonly int _reserve = 4 * 1024;

        public BufferedStream()
        {
            _pool = ArrayPool<byte>.Shared.Rent(_reserve);

            // Skip first sizeof(int) bytes, because they are reserved for the size
            _length = sizeof(int);
            Position = sizeof(int);
        }

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => true;
        public override long Length => _length;
        public override long Position { get; set; }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int size = Math.Min(_length, count);

            Buffer.BlockCopy(_pool, (int)Position, buffer, offset, size);

            Position += size;

            return size;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // Reallocate bytes if array is too small
            CheckSize(count);

            // Write data to the buffer
            Buffer.BlockCopy(buffer, offset, _pool, (int)Position, count);

            _length += count;
            Position += count;

            WriteNewSize();
        }

        private void CheckSize(int incomingBytes)
        {
            long expectedSize = Position + incomingBytes;
            if (expectedSize > _pool.Length)
            {
                // rent new, bigger array with expected size and some reserve
                byte[] newArray = Rent((int)expectedSize + _reserve);

                // copy bytes to new array
                Buffer.BlockCopy(_pool, 0, newArray, 0, _length);

                // return old bytes to the pool
                Return(_pool);

                _pool = newArray;
            }
        }

        private void WriteNewSize()
        {
            // Package size without first bytes reserved for size
            int packageSize = _length - sizeof(int);

            // Write size to the reserved bytes without allocation
            NoAllocBitConverter.GetBytes(packageSize, _pool);
        }

        protected override void Dispose(bool disposing)
        {
            Return(_pool);
        }

        private static byte[] Rent(int count) => ArrayPool<byte>.Shared.Rent(count);

        private static void Return(byte[] bytes) => ArrayPool<byte>.Shared.Return(bytes);
    }
}
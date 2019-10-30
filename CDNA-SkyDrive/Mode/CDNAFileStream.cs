using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32.SafeHandles;

namespace CDNA_SkyDrive.Mode
{
    public class CDNAFileStream : FileStream
    {
        private bool isread = true;

        public bool ISREAD
        {
            get { return isread; }
            set { isread = value; }
        }


        public CDNAFileStream(SafeFileHandle handle, FileAccess access) : base(handle, access)
        {
        }

        public CDNAFileStream(IntPtr handle, FileAccess access) : base(handle, access)
        {
        }

        public CDNAFileStream(string path, FileMode mode) : base(path, mode)
        {
        }

        public CDNAFileStream(SafeFileHandle handle, FileAccess access, int bufferSize) : base(handle, access, bufferSize)
        {
        }

        public CDNAFileStream(IntPtr handle, FileAccess access, bool ownsHandle) : base(handle, access, ownsHandle)
        {
        }

        public CDNAFileStream(string path, FileMode mode, FileAccess access) : base(path, mode, access)
        {
        }

        public CDNAFileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) : base(handle, access, bufferSize, isAsync)
        {
        }

        public CDNAFileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize) : base(handle, access, ownsHandle, bufferSize)
        {
        }

        public CDNAFileStream(string path, FileMode mode, FileAccess access, FileShare share) : base(path, mode, access, share)
        {
        }

        public CDNAFileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync) : base(handle, access, ownsHandle, bufferSize, isAsync)
        {
        }

        public CDNAFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize) : base(path, mode, access, share, bufferSize)
        {
        }

        public CDNAFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync) : base(path, mode, access, share, bufferSize, useAsync)
        {
        }

        public CDNAFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options) : base(path, mode, access, share, bufferSize, options)
        {
        }
    }
}

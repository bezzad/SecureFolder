using System;

namespace SecureFolder
{
    public class ProgressChangedEventArg : EventArgs
    {
        public string FileName { get; set; }
        public long TotalBytes { get; set; }
        public long ProgressedBytes { get; set; }
    }
}

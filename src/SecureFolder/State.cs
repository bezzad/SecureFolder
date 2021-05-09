using System;

namespace SecureFolder
{
    [Flags]
    public enum State
    {
        Help=0,
        Encryption=1,
        Decryption=2,
        Remove=4
    }
}

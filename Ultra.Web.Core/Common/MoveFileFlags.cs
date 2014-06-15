using System;

namespace Ultra.Web.Core.Common
{
    [Flags]
    public enum MoveFileFlags
    {
        MOVEFILE_COPY_ALLOWED = 2,
        MOVEFILE_CREATE_HARDLINK = 0x10,
        MOVEFILE_DELAY_UNTIL_REBOOT = 4,
        MOVEFILE_FAIL_IF_NOT_TRACKABLE = 0x20,
        MOVEFILE_REPLACE_EXISTING = 1,
        MOVEFILE_WRITE_THROUGH = 8
    }
}


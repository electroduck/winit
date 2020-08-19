Namespace WIMInternal
    <Flags>
    Enum WIMFlags As Integer
        None = 0
        Verify = 2
        Index = 4
        NoApply = 8
        ShareWrite = &H40
        FileInfo = &H80
    End Enum

    Enum CompressType As Integer
        None = 0
        XPress = 1
        LZX = 2
    End Enum

    Enum WIMMessage As Integer
        WIM_MSG_SUCCESS = 0
        WIM_MSG_DONE = &HFFFF_FFF0
        WIM_MSG_SKIP_ERROR = &HFFFF_FFFE
        WIM_MSG_ABORT_IMAGE = &HFFFF_FFFF
        WIM_MSG = &H8000 + &H1476
        WIM_MSG_TEXT
        WIM_MSG_PROGRESS
        WIM_MSG_PROCESS
        WIM_MSG_SCANNING
        WIM_MSG_SETRANGE
        WIM_MSG_SETPOS
        WIM_MSG_STEPIT
        WIM_MSG_COMPRESS
        WIM_MSG_ERROR
        WIM_MSG_ALIGNMENT
        WIM_MSG_RETRY
        WIM_MSG_SPLIT
        WIM_MSG_FILEINFO
        WIM_MSG_INFO
        WIM_MSG_WARNING
        WIM_MSG_CHK_PROCESS
        WIM_MSG_WARNING_OBJECTID
        WIM_MSG_STALE_MOUNT_DIR
        WIM_MSG_STALE_MOUNT_FILE
        WIM_MSG_MOUNT_CLEANUP_PROGRESS
        WIM_MSG_CLEANUP_SCANNING_DRIVE
        WIM_MSG_IMAGE_ALREADY_MOUNTED
        WIM_MSG_CLEANUP_UNMOUNTING_IMAGE
        WIM_MSG_QUERY_ABORT
        WIM_MSG_IO_RANGE_START_REQUEST_LOOP
        WIM_MSG_IO_RANGE_END_REQUEST_LOOP
        WIM_MSG_IO_RANGE_REQUEST
        WIM_MSG_IO_RANGE_RELEASE
        WIM_MSG_VERIFY_PROGRESS
        WIM_MSG_COPY_BUFFER
        WIM_MSG_METADATA_EXCLUDE
        WIM_MSG_GET_APPLY_ROOT
        WIM_MSG_MDPAD
        WIM_MSG_STEPNAME
        WIM_MSG_PERFILE_COMPRESS
        WIM_MSG_CHECK_CI_EA_PREREQUISITE_NOT_MET
        WIM_MSG_JOURNALING_ENABLED
    End Enum
End Namespace
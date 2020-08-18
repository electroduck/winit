Imports System.Management
Imports System.Runtime.InteropServices
Imports Electroduck.WinIt.DiskInternal

Public Class Partition
    Inherits FileLike

    Private Const NTSTATUS_PROBLEM_BIT As Integer = &H8000_0000

    Private Declare Unicode Function NtCreateSymbolicLinkObject Lib "ntdll.dll" _
        (ByRef rhObject As IntPtr, access As AccessLevel, attr As ObjectAttributes, usDestName As UnicodeStringAlt) As Integer

    Private Declare Unicode Function RtlNtStatusToDosError Lib "ntoskrnl.exe" (nStatus As Integer) As Integer

    Private mDisk As Harddisk
    Private mPart As Integer
    Private mPartWMI As ManagementObject
    Private mLogicalDisk As ManagementObject
    Private mVolume As ManagementObject

    Public Sub New(disk As Harddisk, nPart As Integer)
        MyBase.New(String.Format("\\.\Harddisk{0}Partition{1}", disk.Number, nPart), AccessLevel.Read Or AccessLevel.Write,
                   ShareMode.Read Or ShareMode.Write, CreationDisposition.OpenExisting)
        mDisk = disk

        Dim strQueryPart As String = String.Format("SELECT * FROM Win32_DiskPartition WHERE DiskIndex={0} AND Index={1}", disk.Number, nPart - 1)
        Using mosPart As New ManagementObjectSearcher(strQueryPart)
            For Each mo As ManagementObject In mosPart.Get
                mPartWMI = mo
            Next
        End Using

        Dim strQueryLogical As String = String.Format("ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{0}'}} WHERE ResultClass=Win32_LogicalDisk",
                                                      mPartWMI("DeviceID"))
        Using mosLogical As New ManagementObjectSearcher(strQueryLogical)
            For Each mo As ManagementObject In mosLogical.Get
                mLogicalDisk = mo
            Next
        End Using

        Dim strQueryVolume As String = String.Format("SELECT * FROM Win32_Volume WHERE DriveLetter='{0}'", mLogicalDisk("DeviceID"))
        Using mosVolume As New ManagementObjectSearcher(strQueryVolume)
            For Each mo As ManagementObject In mosVolume.Get
                mVolume = mo
            Next
        End Using
    End Sub

    Public Sub Format(strFileSystem As String, Optional strLabel As String = "", Optional nClusterSize As Integer = 4096)
        mVolume.InvokeMethod("Format", New Object() {strFileSystem, True, nClusterSize, strLabel, False})
    End Sub

    'Public Sub FormatNTFS(Optional strLabel As String = "")
    '    DiscUtils.Ntfs.NtfsFileSystem.Format(Stream, strLabel, mDisk.GeometryDU, 0, Length \ mDisk.GeometryDU.BytesPerSector)
    'End Sub

    Public Overrides ReadOnly Property Length As Long
        Get
            Dim infLength As New LengthInfo
            IOControl(IOCTL_DISK_GET_LENGTH_INFO, Nothing, infLength)
            Return infLength.nLength
        End Get
    End Property

    Private Function GetPartInfo() As PartitionInformationReference
        Dim infPart As New PartitionInformationReference
        IOControl(IOCTL_DISK_GET_PARTITION_INFO, Nothing, infPart)
        Return infPart
    End Function

    Public Property PartitionType As Byte
        Get
            Return GetPartInfo.nPartitionType
        End Get
        Set(bValue As Byte)
            Dim infPart As New PartitionInformationLimited With {.nPartitionType = bValue}
            IOControl(IOCTL_DISK_SET_PARTITION_INFO, infPart, Nothing)
        End Set
    End Property

    Public Sub MountSymlink(strMountPoint As String)
        Dim attr As New ObjectAttributesHelper(strMountPoint)
        Dim usTargetName As New UnicodeStringAlt With {
            .nLength = strMountPoint.Length,
            .nMaxLength = strMountPoint.Length,
            .strBuffer = strMountPoint
        }

        Dim hSymlink As IntPtr
        Dim nStatus As Integer = NtCreateSymbolicLinkObject(hSymlink, AccessLevel.All, attr.Attributes, usTargetName)
        If nStatus And NTSTATUS_PROBLEM_BIT Then
            Throw New ComponentModel.Win32Exception(RtlNtStatusToDosError(nStatus))
        End If
    End Sub
End Class

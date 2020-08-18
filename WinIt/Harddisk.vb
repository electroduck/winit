Imports System.Runtime.InteropServices
Imports Electroduck.WinIt.DiskInternal

Public Class Harddisk
    Inherits FileLike

    Public Structure PartitionPrototype
        Dim nLength As DataQuantity
        'Dim nType As Byte
        Dim bBootable As Boolean

        'Public Const PARTTYPE_EXTENDED As Byte = &H5
        'Public Const PARTTYPE_RAW As Byte = &H6
        'Public Const PARTTYPE_NTFS As Byte = &H7
        'Public Const PARTTYPE_FAT32_CHS As Byte = &HC
        'Public Const PARTTYPE_FAT32 As Byte = &HC
        'Public Const PARTTYPE_FAT16 As Byte = &HE
        'Public Const PARTTYPE_NTFS_RECOVERY As Byte = &H27
        'Public Const PARTTYPE_DYNAMICVOL As Byte = &H42
        'Public Const PARTTYPE_GPT As Byte = &HEE
        'Public Const PARTTYPE_EFI As Byte = &HEF
    End Structure

    Private mNumber As Integer
    Private mGeometry As DiskGeometry
    Private mInfoStringsRead As Boolean = False
    Private mVendor As String
    Private mProductID As String
    Private mProductRev As String
    Private mSerialNum As String

    Public Sub New(nDisk As Integer)
        MyBase.New(String.Format("\\.\PhysicalDrive{0}", nDisk), AccessLevel.Read Or AccessLevel.Write,
                   ShareMode.Read Or ShareMode.Write, CreationDisposition.OpenExisting)
        mNumber = nDisk
    End Sub

    Public ReadOnly Property Number As Integer
        Get
            Return mNumber
        End Get
    End Property

    Public Shared Function GetDiskList() As Harddisk()
        Dim lstDisks As New List(Of Harddisk)
        Dim bDone As Boolean = False
        Dim nDisk As Integer = 0

        Do
            Try
                lstDisks.Add(New Harddisk(nDisk))
            Catch ex As Exception
                Debug.WriteLine("Got error message """ & ex.Message & """, disk list complete.")
                bDone = True
            End Try

            nDisk += 1
        Loop Until bDone

        Return lstDisks.ToArray
    End Function

    Private Sub CheckGeometry()
        If mGeometry Is Nothing Then
            mGeometry = New DiskGeometry
            IOControl(IOCTL_DISK_GET_DRIVE_GEOMETRY, Nothing, mGeometry)
        End If
    End Sub

    Public ReadOnly Property Size As DataQuantity
        Get
            CheckGeometry()
            Return New DataQuantity(mGeometry.nCylinders * mGeometry.nTracksPerCylinder _
                                    * mGeometry.nSectorsPerTrack * mGeometry.nBytesPerSector)
        End Get
    End Property

    Public ReadOnly Property IsRemovable As Boolean
        Get
            CheckGeometry()
            Return mGeometry.typeMedia <> MediaType.FixedMedia
        End Get
    End Property

    Public ReadOnly Property GeometryDU As DiscUtils.Geometry
        Get
            CheckGeometry()
            Return New DiscUtils.Geometry(CInt(mGeometry.nCylinders), CInt(mGeometry.nTracksPerCylinder),
                                          CInt(mGeometry.nSectorsPerTrack), CInt(mGeometry.nBytesPerSector))
        End Get
    End Property

    Private Sub CheckInfoStrings()
        If Not mInfoStringsRead Then
            Dim sddh As New StorageDeviceDescriptorHeader
            Dim query As New StoragePropertyQuery With {
                .prop = StoragePropertyID.StorageDeviceProperty,
                .qtype = StorageQueryType.PropertyStandardQuery
            }

            Using blkQuery As New StructMemoryBlock(query)
                Using blkResult As New StructMemoryBlock(sddh, 1024)
                    IOControl(IOCTL_STORAGE_QUERY_PROPERTY, blkQuery, blkResult)

                    blkResult.BlockToStructure(sddh)

                    mVendor = If(sddh.nVendorIDOffset, blkResult.ExtractASCIIZString(sddh.nVendorIDOffset), "")
                    mProductID = If(sddh.nProductIDOffset, blkResult.ExtractASCIIZString(sddh.nProductIDOffset), "")
                    mProductRev = If(sddh.nProductRevisionOffset, blkResult.ExtractASCIIZString(sddh.nProductRevisionOffset), "")
                    mSerialNum = If(sddh.nSerialNumberOffset, blkResult.ExtractASCIIZString(sddh.nSerialNumberOffset), "")
                End Using
            End Using

            mInfoStringsRead = True
        End If
    End Sub

    Public ReadOnly Property VendorID As String
        Get
            CheckInfoStrings()
            Return mVendor
        End Get
    End Property

    Public ReadOnly Property ProductID As String
        Get
            CheckInfoStrings()
            Return mProductID
        End Get
    End Property

    Public ReadOnly Property ProductRevision As String
        Get
            CheckInfoStrings()
            Return mProductRev
        End Get
    End Property

    Public ReadOnly Property SerialNumber As String
        Get
            CheckInfoStrings()
            Return mSerialNum
        End Get
    End Property

    Public ReadOnly Property Name As String
        Get
            If ProductID.Length > 0 Then
                Return ProductID.Trim
            ElseIf ProductRevision.Length > 0 Then
                Return If(VendorID.Length, VendorID.Trim & " ", "Generic rev.") & ProductRevision.Trim
            ElseIf SerialNumber.Length > 0 Then
                Return If(VendorID.Length, VendorID.Trim & " ", "Generic SN ") & SerialNumber.Trim
            ElseIf VendorID.Length Then
                Return VendorID.Trim & " brand disk"
            Else
                Return "Generic disk"
            End If
        End Get
    End Property

    Private Function GenerateSignature() As Integer
        Return CInt(Date.Now.ToBinary And &H7FFF_FFFF)
    End Function

    Public Sub ReinitializeMBR(Optional nSignature As Integer = -1)
        If nSignature = -1 Then
            nSignature = GenerateSignature()
        End If

        Dim cdMBR As New CreateDiskMBR With {.nSignature = nSignature}
        IOControl(IOCTL_DISK_CREATE_DISK, cdMBR, Nothing)
    End Sub

    Public Sub RepartitionMBR(arrParts() As PartitionPrototype, Optional nSignature As Integer = -1)
        If nSignature = -1 Then
            nSignature = GenerateSignature()
        End If

        Dim infLayout As New DriveLayoutInformationMBR With {
            .nPartitionCount = arrParts.Length,
            .nSignature = nSignature
        }

        Dim nMBRMax As Long = DataQuantity.FromTerabytes(2).Bytes
        Dim nSpacing As Long = DataQuantity.FromMegabytes(2).Bytes

        Dim nDiskSize As Long = Size.Bytes
        If nDiskSize > nMBRMax Then
            nDiskSize = nMBRMax
        End If

        Dim nCurPos As Long = nSpacing
        For nPart = 0 To arrParts.Length - 1
            infLayout.arrPartitionEntries(nPart).nStartingOffset = nCurPos
            infLayout.arrPartitionEntries(nPart).nPartitionLength _
                = If(arrParts(nPart).nLength IsNot Nothing, arrParts(nPart).nLength, nDiskSize - (nCurPos + nSpacing))
            infLayout.arrPartitionEntries(nPart).nPartitionNumber = nPart + 1
            infLayout.arrPartitionEntries(nPart).nPartitionType = 6 'arrParts(nPart).nType
            infLayout.arrPartitionEntries(nPart).bBootIndicator = arrParts(nPart).bBootable
            infLayout.arrPartitionEntries(nPart).bRecognizedPartition = True
            infLayout.arrPartitionEntries(nPart).bRewritePartition = True

            nCurPos += infLayout.arrPartitionEntries(nPart).nPartitionLength + nSpacing
            If nCurPos > nDiskSize Then
                Throw New ArgumentOutOfRangeException("arrParts", "Not enough space on disk for all partitions")
            End If
        Next

        IOControl(IOCTL_DISK_SET_DRIVE_LAYOUT, infLayout, Nothing)
    End Sub

    Public ReadOnly Property Partition(nPart As Integer) As Partition
        Get
            Return New Partition(Me, nPart)
        End Get
    End Property

    Public Overrides ReadOnly Property Length As Long
        Get
            Dim infLength As New LengthInfo
            IOControl(IOCTL_DISK_GET_LENGTH_INFO, Nothing, infLength)
            Return infLength.nLength
        End Get
    End Property
End Class

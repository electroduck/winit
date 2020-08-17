Imports System.Runtime.InteropServices

Public Class Harddisk
    Inherits FileLike

    Private Const IOCTL_DISK_GET_DRIVE_GEOMETRY As Integer = &H0007_0000
    Private Const IOCTL_DISK_SET_DRIVE_LAYOUT As Integer = &H0007_C010
    Private Const IOCTL_DISK_CREATE_DISK As Integer = &H0007_C058
    Private Const IOCTL_DISK_GET_LENGTH_INFO As Integer = &H0007_405C
    Private Const IOCTL_STORAGE_QUERY_PROPERTY As Integer = &H002D_1400

    Private Enum MediaType As UInteger
        Unknown
        F5_1Pt2_512
        F3_1Pt44_512
        F3_2Pt88_512
        F3_20Pt8_512
        F3_720_512
        F5_360_512
        F5_320_512
        F5_320_1024
        F5_180_512
        F5_160_512
        RemovableMedia
        FixedMedia
        F3_120M_512
        F3_640_512
        F5_640_512
        F5_720_512
        F3_1Pt2_512
        F3_1Pt23_1024
        F5_1Pt23_1024
        F3_128Mb_512
        F3_230Mb_512
        F8_256_128
        F3_200Mb_512
        F3_240M_512
        F3_32M_512
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Private Class DiskGeometry
        Public nCylinders As ULong
        Public typeMedia As MediaType
        Public nTracksPerCylinder As UInteger
        Public nSectorsPerTrack As UInteger
        Public nBytesPerSector As UInteger
    End Class

    Private Enum StoragePropertyID As UInteger
        StorageDeviceProperty
        StorageAdapterProperty
        StorageDeviceIdProperty
        StorageDeviceUniqueIdProperty
        StorageDeviceWriteCacheProperty
        StorageMiniportProperty
        StorageAccessAlignmentProperty
        StorageDeviceSeekPenaltyProperty
        StorageDeviceTrimProperty
        StorageDeviceWriteAggregationProperty
        StorageDeviceDeviceTelemetryProperty
        StorageDeviceLBProvisioningProperty
        StorageDevicePowerProperty
        StorageDeviceCopyOffloadProperty
        StorageDeviceResiliencyProperty
        StorageDeviceMediumProductType
        StorageAdapterRpmbProperty
        StorageAdapterCryptoProperty
        StorageDeviceIoCapabilityProperty
        StorageAdapterProtocolSpecificProperty
        StorageDeviceProtocolSpecificProperty
        StorageAdapterTemperatureProperty
        StorageDeviceTemperatureProperty
        StorageAdapterPhysicalTopologyProperty
        StorageDevicePhysicalTopologyProperty
        StorageDeviceAttributesProperty
        StorageDeviceManagementStatus
        StorageAdapterSerialNumberProperty
        StorageDeviceLocationProperty
        StorageDeviceNumaProperty
        StorageDeviceZonedDeviceProperty
        StorageDeviceUnsafeShutdownCount
        StorageDeviceEnduranceProperty
    End Enum

    Private Enum StorageQueryType As UInteger
        PropertyStandardQuery
        PropertyExistsQuery
        PropertyMaskQuery
        PropertyQueryMaxDefined
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Private Class StoragePropertyQuery
        Public prop As StoragePropertyID
        Public qtype As StorageQueryType
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=1)> Public arrExtraData(1) As Byte
    End Class

    Private Enum StorageBusType As UInteger
        BusTypeUnknown
        BusTypeScsi
        BusTypeAtapi
        BusTypeAta
        BusType1394
        BusTypeSsa
        BusTypeFibre
        BusTypeUsb
        BusTypeRAID
        BusTypeiScsi
        BusTypeSas
        BusTypeSata
        BusTypeSd
        BusTypeMmc
        BusTypeVirtual
        BusTypeFileBackedVirtual
        BusTypeSpaces
        BusTypeNvme
        BusTypeSCM
        BusTypeUfs
        BusTypeMax
        BusTypeMaxReserved
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Private Class StorageDeviceDescriptorHeader
        Public nVersion As UInteger
        Public nSize As UInteger
        Public nDeviceType As Byte
        Public nDeviceTypeModifier As Byte
        <MarshalAs(UnmanagedType.I1)> Public bRemovableMedia As Boolean
        <MarshalAs(UnmanagedType.I1)> Public bCommandQueueing As Boolean
        Public nVendorIDOffset As UInteger
        Public nProductIDOffset As UInteger
        Public nProductRevisionOffset As UInteger
        Public nSerialNumberOffset As UInteger
        Public bustype As StorageBusType
        Public nRawPropertiesLength As UInteger
    End Class

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
End Class

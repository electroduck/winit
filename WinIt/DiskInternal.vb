Imports System.Runtime.InteropServices

Namespace DiskInternal
    Module IOControlCodes
        Public Const IOCTL_DISK_GET_DRIVE_GEOMETRY As Integer = &H0007_0000
        Public Const IOCTL_DISK_SET_DRIVE_LAYOUT As Integer = &H0007_C010
        Public Const IOCTL_DISK_CREATE_DISK As Integer = &H0007_C058
        Public Const IOCTL_DISK_GET_LENGTH_INFO As Integer = &H0007_405C
        Public Const IOCTL_DISK_GET_PARTITION_INFO As Integer = &H0007_4004
        Public Const IOCTL_DISK_SET_PARTITION_INFO As Integer = &H0007_C008
        Public Const IOCTL_STORAGE_QUERY_PROPERTY As Integer = &H002D_1400
    End Module

    Enum MediaType As UInteger
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
    Class DiskGeometry
        Public nCylinders As ULong
        Public typeMedia As MediaType
        Public nTracksPerCylinder As UInteger
        Public nSectorsPerTrack As UInteger
        Public nBytesPerSector As UInteger
    End Class

    Enum StoragePropertyID As UInteger
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

    Enum StorageQueryType As UInteger
        PropertyStandardQuery
        PropertyExistsQuery
        PropertyMaskQuery
        PropertyQueryMaxDefined
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Class StoragePropertyQuery
        Public prop As StoragePropertyID
        Public qtype As StorageQueryType
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=1)> Public arrExtraData(1) As Byte
    End Class

    Enum StorageBusType As UInteger
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
    Class StorageDeviceDescriptorHeader
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

    Enum PartitionStyle As UInteger
        MBR
        GPT
        RAW
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Class CreateDiskMBR
        Public style As PartitionStyle = PartitionStyle.MBR
        Public nSignature As Integer
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=16)>
        Public ReadOnly arrDummy(15) As Byte
    End Class

    <StructLayout(LayoutKind.Sequential)>
    Class CreateDiskGPT
        Public style As PartitionStyle = PartitionStyle.GPT
        Public idDisk As Guid
        Public nMaxPartCount As Integer
    End Class

    <StructLayout(LayoutKind.Sequential)>
    Class PartitionInformationReference
        Public nStartingOffset As Long
        Public nPartitionLength As Long
        Public nHiddenSectors As Integer
        Public nPartitionNumber As Integer
        Public nPartitionType As Byte
        <MarshalAs(UnmanagedType.I1)> Public bBootIndicator As Boolean
        <MarshalAs(UnmanagedType.I1)> Public bRecognizedPartition As Boolean
        <MarshalAs(UnmanagedType.I1)> Public bRewritePartition As Boolean
    End Class

    <StructLayout(LayoutKind.Sequential)>
    Structure PartitionInformationValue
        Public nStartingOffset As Long
        Public nPartitionLength As Long
        Public nHiddenSectors As Integer
        Public nPartitionNumber As Integer
        Public nPartitionType As Byte
        <MarshalAs(UnmanagedType.I1)> Public bBootIndicator As Boolean
        <MarshalAs(UnmanagedType.I1)> Public bRecognizedPartition As Boolean
        <MarshalAs(UnmanagedType.I1)> Public bRewritePartition As Boolean
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Class DriveLayoutInformationMBR
        Public nPartitionCount As Integer
        Public nSignature As Integer
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)>
        Public arrPartitionEntries(3) As PartitionInformationValue
    End Class

    <StructLayout(LayoutKind.Sequential)>
    Class LengthInfo
        Public nLength As Long
    End Class

    <StructLayout(LayoutKind.Sequential)>
    Class PartitionInformationLimited
        Public nPartitionType As Byte
    End Class

    <StructLayout(LayoutKind.Sequential)>
    Class UnicodeString
        Public nLength As UShort
        Public nMaxLength As UShort
        Public pBuffer As IntPtr
    End Class

    <StructLayout(LayoutKind.Sequential)>
    Class UnicodeStringAlt
        Public nLength As UShort
        Public nMaxLength As UShort
        <MarshalAs(UnmanagedType.LPWStr)>
        Public strBuffer As String
    End Class

    <StructLayout(LayoutKind.Sequential)>
    Class ObjectAttributes
        Public nLength As UInteger
        Public hRootDir As IntPtr
        Public pObjectName As IntPtr
        Public nAttributes As Integer
        Public pSecurityDescriptor As IntPtr
        Public pSecurityQOS As IntPtr
    End Class

    Class ObjectAttributesHelper
        Public ReadOnly Property Attributes As ObjectAttributes
        Private mString As UnicodeString
        Private mStringBlock As MemoryBlock
        Private mStringStructBlock As MemoryBlock

        Public Sub New(strName As String)
            Attributes = New ObjectAttributes
            mStringBlock = MemoryBlock.FromString(strName, Text.Encoding.Unicode)

            mString = New UnicodeString With {
                .nLength = strName.Length,
                .nMaxLength = mStringBlock.Size,
                .pBuffer = mStringBlock.Pointer
            }

            mStringStructBlock = New StructMemoryBlock(mString)
            Attributes.pObjectName = mStringStructBlock.Pointer
        End Sub
    End Class
End Namespace
Imports System.Runtime.InteropServices

Public Class FileLike
    Implements IDisposable

    <Flags>
    Public Enum ShareMode As UInteger
        None = 0
        Read = 1
        Write = 2
        Delete = 4
    End Enum

    Public Enum CreationDisposition As UInteger
        CreateNew = 1
        CreateAlways = 2
        OpenExisting = 3
        OpenAlways = 4
        TruncateExisting = 5
    End Enum

    <Flags>
    Public Enum AccessLevel As Integer
        Read = &H8000_0000
        Write = &H4000_0000
        Execute = &H2000_0000
        All = &H1000_0000
        None = 0
    End Enum

    Private Shared ReadOnly INVALID_HANDLE_VALUE As IntPtr = &HFFFF_FFFF

    Private Declare Ansi Function CloseHandle Lib "kernel32.dll" (hFile As IntPtr) As Boolean

    Private Declare Auto Function CreateFile Lib "kernel32.dll" _
        (strPath As String, access As AccessLevel, sm As ShareMode, pSecAttr As IntPtr,
         cd As CreationDisposition, hTemplate As IntPtr) As IntPtr

    Private Declare Ansi Function DeviceIoControl Lib "kernel32.dll" _
        (hDevice As IntPtr, nCode As Integer, pInBuf As IntPtr, nInBufSize As UInteger,
         pOutBuf As IntPtr, nOutBufSize As UInteger, ByRef rnBytesRet As UInteger,
         pOverlapped As IntPtr) As Boolean

    Private mPath As String
    Private mHandle As IntPtr

    Protected Overridable ReadOnly Property Path As String
        Get
            Return mPath
        End Get
    End Property

    Protected Overridable ReadOnly Property Handle As IntPtr
        Get
            Return mHandle
        End Get
    End Property

    Public Sub New(strPath As String, access As AccessLevel, sm As ShareMode, cd As CreationDisposition)
        mPath = strPath

        mHandle = CreateFile(strPath, access, sm, IntPtr.Zero, cd, IntPtr.Zero)
        If mHandle = IntPtr.Zero Or mHandle = INVALID_HANDLE_VALUE Then
            Throw New ComponentModel.Win32Exception(Err.LastDllError)
        End If
    End Sub

#If 0 Then
    Public Sub IOControl(nCode As Integer, objIn As Object, objOut As Object)
        Dim blkIn As MemoryBlock = Nothing
        If objIn IsNot Nothing Then
            blkIn = New MemoryBlock(Marshal.SizeOf(objIn))
            Marshal.StructureToPtr(objIn, blkIn.Pointer, False)
        End If

        Try
            Dim blkOut As MemoryBlock = Nothing
            If objOut IsNot Nothing Then
                blkOut = New MemoryBlock(Marshal.SizeOf(objOut))
                Marshal.StructureToPtr(objOut, blkOut.Pointer, False)
            End If

            Try
                IOControl(nCode, blkIn, blkOut)
            Finally
                If objOut IsNot Nothing Then
                    Marshal.PtrToStructure(blkOut.Pointer, objOut)
                    Marshal.DestroyStructure(blkOut.Pointer, objOut.GetType)
                End If
            End Try

        Finally
            If objIn IsNot Nothing Then
                Marshal.DestroyStructure(blkIn.Pointer, objIn)
            End If
        End Try
    End Sub
#End If

    Public Sub IOControl(nCode As Integer, objIn As Object, objOut As Object)
        Dim blkIn As StructMemoryBlock = Nothing
        Dim blkOut As StructMemoryBlock = Nothing

        If objIn IsNot Nothing Then
            blkIn = New StructMemoryBlock(objIn)
        End If

        If objOut IsNot Nothing Then
            blkOut = New StructMemoryBlock(objOut)
        End If

        IOControl(nCode, blkIn, blkOut)

        If objOut IsNot Nothing Then
            blkOut.BlockToStructure(objOut)
        End If
    End Sub

    Public Function IOControl(nCode As Integer, blkIn As MemoryBlock, blkOut As MemoryBlock) As UInteger
        Dim nBytesRet As UInteger

        If Not DeviceIoControl(mHandle, nCode,
                               If(blkIn Is Nothing, IntPtr.Zero, blkIn.Pointer),
                               If(blkIn Is Nothing, 0, blkIn.Size),
                               If(blkOut Is Nothing, IntPtr.Zero, blkOut.Pointer),
                               If(blkOut Is Nothing, 0, blkOut.Size),
                               nBytesRet, IntPtr.Zero) Then
            Throw New ComponentModel.Win32Exception(Err.LastDllError)
        End If

        Return nBytesRet
    End Function

#Region "IDisposable Support"
    Private mDisposed As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(bDisposing As Boolean)
        If Not mDisposed Then
            CloseHandle(mHandle)
            mHandle = IntPtr.Zero
        End If
        mDisposed = True
    End Sub

    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class

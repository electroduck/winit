Imports System.IO
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

    Private Declare Ansi Function FlushFileBuffers Lib "kernel32.dll" (hFile As IntPtr) As Boolean

    Private Declare Ansi Function SetFilePointerEx Lib "kernel32.dll" _
        (hFile As IntPtr, nDistMove As Long, ByRef rnNewPointer As Long, mthd As SeekOrigin) As Boolean

    Private Declare Ansi Function ReadFile Lib "kernel32.dll" _
        (hFile As IntPtr, pBuffer As IntPtr, nToRead As Integer, ByRef rnRead As Integer, pOverlapped As IntPtr) As Boolean

    Private Declare Ansi Function WriteFile Lib "kernel32.dll" _
        (hFile As IntPtr, pBuffer As IntPtr, nToWrite As Integer, ByRef rnWritten As Integer, pOverlapped As IntPtr) As Boolean

    Private mPath As String
    Private mHandle As IntPtr
    Private mAccess As AccessLevel
    Private mStream As FileLikeStream

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

    Public ReadOnly Property Access As AccessLevel
        Get
            Return mAccess
        End Get
    End Property

    Public Overridable ReadOnly Property Length As Long
        Get
            Throw New NotImplementedException
        End Get
    End Property

    Public Overridable ReadOnly Property Stream As Stream
        Get
            Return mStream
        End Get
    End Property

    Public Sub New(strPath As String, access As AccessLevel, sm As ShareMode, cd As CreationDisposition)
        mPath = strPath
        mAccess = access

        mHandle = CreateFile(strPath, access, sm, IntPtr.Zero, cd, IntPtr.Zero)
        If mHandle = IntPtr.Zero Or mHandle = INVALID_HANDLE_VALUE Then
            Throw New ComponentModel.Win32Exception(Err.LastDllError)
        End If

        mStream = New FileLikeStream(Me)
    End Sub

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

    Protected Overridable Sub Flush()
        If Not FlushFileBuffers(mHandle) Then
            Throw New ComponentModel.Win32Exception(Err.LastDllError)
        End If
    End Sub

    Private Class FileLikeStream
        Inherits Stream

        Private mFile As FileLike

        Public Overrides ReadOnly Property CanRead As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property CanSeek As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property CanWrite As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property Length As Long
            Get
                Return mFile.Length
            End Get
        End Property

        Public Overrides Property Position As Long
            Get
                Return Seek(0, SeekOrigin.Current)
            End Get
            Set(nValue As Long)
                Seek(nValue, SeekOrigin.Begin)
            End Set
        End Property

        Public Sub New(f As FileLike)
            mFile = f
        End Sub

        Public Overrides Sub Flush()
            mFile.Flush()
        End Sub

        Public Overrides Sub SetLength(value As Long)
            Throw New NotImplementedException()
        End Sub

        Public Overrides Sub Write(arrBuffer() As Byte, nOffset As Integer, nCount As Integer)
            Using blkTemp As New MemoryBlock(nCount)
                Marshal.Copy(arrBuffer, nOffset, blkTemp.Pointer, nCount)
                If Not WriteFile(mFile.mHandle, blkTemp.Pointer, nCount, Nothing, IntPtr.Zero) Then
                    Throw New ComponentModel.Win32Exception(Err.LastDllError)
                End If
            End Using
        End Sub

        Public Overrides Function Seek(nOffset As Long, origin As SeekOrigin) As Long
            Dim nNewPos As Long
            If Not SetFilePointerEx(mFile.mHandle, nOffset, nNewPos, origin) Then
                Throw New ComponentModel.Win32Exception(Err.LastDllError)
            End If
            Return nNewPos
        End Function

        Public Overrides Function Read(arrBuffer() As Byte, nOffset As Integer, nCount As Integer) As Integer
            Using blkTemp As New MemoryBlock(nCount)
                Dim nRead As Integer
                If Not ReadFile(mFile.mHandle, blkTemp.Pointer, nCount, nRead, IntPtr.Zero) Then
                    Throw New ComponentModel.Win32Exception(Err.LastDllError)
                End If
                Marshal.Copy(blkTemp.Pointer, arrBuffer, nOffset, nRead)
                Return nRead
            End Using
        End Function
    End Class

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

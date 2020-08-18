Imports System.Runtime.InteropServices

Public Class MemoryBlock
    Implements IDisposable

    Private mPointer As IntPtr
    Private mSize As Integer

    Public ReadOnly Property Pointer As IntPtr
        Get
            Return mPointer
        End Get
    End Property

    Public ReadOnly Property Size As Integer
        Get
            Return mSize
        End Get
    End Property

    Public Sub New(nSize As Integer)
        mSize = nSize
        mPointer = Marshal.AllocHGlobal(nSize)
        If mPointer = IntPtr.Zero Then
            Throw New InsufficientMemoryException("Unable to allocate " & nSize & " byte memory block")
        End If
    End Sub

    Public Shared Function FromString(str As String, fmt As Text.Encoding)
        Return FromArray(fmt.GetBytes(str))
    End Function

    Public Shared Function FromArray(arrData() As Byte)
        Dim blk As New MemoryBlock(arrData.Length)
        Marshal.Copy(arrData, 0, blk.Pointer, arrData.Length)
        Return blk
    End Function

    Public Sub BlockToStructure(objStruct As Object)
        Marshal.PtrToStructure(Pointer, objStruct)
    End Sub

    Public Function ExtractASCIIZString(Optional nOffset As Integer = 0)
        Dim nStringLength As Integer = 0

        Do
            If Marshal.ReadByte(mPointer, nOffset + nStringLength) = 0 Then
                Exit Do
            Else
                nStringLength += 1
            End If
        Loop

        Return Text.Encoding.ASCII.GetString(ExtractArray(nOffset, nStringLength))
    End Function

    Public Function ExtractArray(nOffset As Integer, nCount As Integer) As Byte()
        If nOffset < 0 Or nCount < 0 Or (nOffset + nCount) >= mSize Then
            Throw New IndexOutOfRangeException
        End If

        Dim arrData() As Byte = Array.CreateInstance(GetType(Byte), nCount)
        Marshal.Copy(mPointer + nOffset, arrData, 0, nCount)
        Return arrData
    End Function

#Region "IDisposable Support"
    Private mDisposed As Boolean ' To detect redundant calls

    Protected Overridable Sub DeleteMemoryBlock()
        Marshal.FreeHGlobal(mPointer)
        mPointer = IntPtr.Zero
        mSize = 0
    End Sub

    ' IDisposable
    Protected Sub Dispose(bDisposing As Boolean)
        If Not mDisposed Then
            DeleteMemoryBlock()
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

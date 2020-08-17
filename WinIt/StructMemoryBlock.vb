Imports System.Runtime.InteropServices

Public Class StructMemoryBlock
    Inherits MemoryBlock

    Private mStructType As Type

    Public Sub New(objStruct As Object, Optional nExtraBytes As Integer = 0)
        MyBase.New(Marshal.SizeOf(objStruct) + nExtraBytes)
        Marshal.StructureToPtr(objStruct, Pointer, False)
        mStructType = objStruct.GetType
    End Sub

    Public Function ToStructure() As Object
        Return Marshal.PtrToStructure(Pointer, mStructType)
    End Function

    Protected Overrides Sub DeleteMemoryBlock()
        Marshal.DestroyStructure(Pointer, mStructType)
        MyBase.DeleteMemoryBlock()
    End Sub
End Class

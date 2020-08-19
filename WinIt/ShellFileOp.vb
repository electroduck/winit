Imports System.Runtime.InteropServices

Module ShellFileOp
    Private Enum FileOpFunc As UInteger
        Move = 1
        Copy
        Delete
        Rename
    End Enum

    <Flags>
    Private Enum FileOpFlags As UInteger
        FOF_MULTIDESTFILES = &H1
        FOF_CONFIRMMOUSE = &H2
        FOF_SILENT = &H4
        FOF_RENAMEONCOLLISION = &H8
        FOF_NOCONFIRMATION = &H10
        FOF_WANTMAPPINGHANDLE = &H20
        FOF_ALLOWUNDO = &H40
        FOF_FILESONLY = &H80
        FOF_SIMPLEPROGRESS = &H100
        FOF_NOCONFIRMMKDIR = &H200
        FOF_NOERRORUI = &H400
        FOF_NOCOPYSECURITYATTRIBS = &H800
        FOF_NORECURSION = &H1000
        FOF_NO_CONNECTED_ELEMENTS = &H2000
        FOF_WANTNUKEWARNING = &H4000
        FOF_NORECURSEREPARSE = &H8000
    End Enum

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Private Class SHFileOp
        Public hWnd As IntPtr
        Public func As FileOpFunc
        Public pFrom As IntPtr
        Public pTo As IntPtr
        Public flags As FileOpFlags
        Public bAnyAborted As Boolean
        Public hNameMappings As IntPtr
        Public strProgressTitle As String
    End Class

    Private Declare Unicode Function SHFileOperationW Lib "shell32.dll" (op As SHFileOp) As Integer

    Private Class SHFileOpHelper
        Public mFileOp As New SHFileOp
        Private mFromBlock As MemoryBlock
        Private mToBlock As MemoryBlock

        Public Sub New(func As FileOpFunc, strFrom As String, strTo As String)
            mFileOp.func = func

            If strFrom IsNot Nothing Then
                mFromBlock = New MemoryBlock(strFrom.Length * 2 + 4)
                mFromBlock.Clear()
                Dim arrFrom() As Byte = Text.Encoding.Unicode.GetBytes(strFrom)
                Marshal.Copy(arrFrom, 0, mFromBlock.Pointer, arrFrom.Length)
                mFileOp.pFrom = mFromBlock.Pointer
            End If

            If strTo IsNot Nothing Then
                mToBlock = New MemoryBlock(strTo.Length * 2 + 4)
                mToBlock.Clear()
                Dim arrTo() As Byte = Text.Encoding.Unicode.GetBytes(strTo)
                Marshal.Copy(arrTo, 0, mToBlock.Pointer, arrTo.Length)
                mFileOp.pTo = mToBlock.Pointer
            End If
        End Sub

        Public Sub Execute()
            Dim nResult As Integer = SHFileOperationW(mFileOp)
            If nResult >= &H71 And nResult <= &H88 Then
                Throw New IO.IOException("Shell operation error " & nResult.ToString("X2"))
            ElseIf nResult = &HB7 Then
                Throw New IO.PathTooLongException
            ElseIf nResult = &H1_0000 Then
                Throw New IO.IOException("Unspecified error")
            ElseIf nResult = &H1_0074 Then
                Throw New InvalidOperationException("Root directories cannot be renamed")
            ElseIf nResult <> 0 Then
                Throw New ComponentModel.Win32Exception(nResult)
            End If
        End Sub
    End Class

    Public Sub ShellDelete(strPath As String)
        Dim op As New SHFileOpHelper(FileOpFunc.Delete, strPath, Nothing)
        op.mFileOp.flags = FileOpFlags.FOF_NOERRORUI Or FileOpFlags.FOF_NOCONFIRMATION
        op.Execute()
    End Sub

End Module

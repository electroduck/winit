Imports System.Runtime.InteropServices
Imports Electroduck.WinIt.WIMInternal

Public Class WindowsImageFile
    Inherits WIMHandle

    Private Delegate Function WIMMessageCallback(msg As WIMMessage, wParam As IntPtr, lParam As IntPtr, pUserData As IntPtr) As WIMMessage

    Private Declare Unicode Function WIMCreateFile Lib "wimgapi.dll" _
        (strPath As String, access As FileLike.AccessLevel, cd As FileLike.CreationDisposition, flags As WIMFlags,
         ct As CompressType, ByRef bOpenedExisting As Boolean) As IntPtr

    Private Declare Unicode Function WIMRegisterMessageCallback Lib "wimgapi.dll" _
        (hWIMFile As IntPtr, procCallback As WIMMessageCallback, pUserData As IntPtr) As Integer

    Private Declare Unicode Function WIMSetTemporaryPath Lib "wimgapi.dll" (hWIMFile As IntPtr, strPath As String) As Boolean

    Private Declare Unicode Function WIMGetImageCount Lib "wimgapi.dll" (hWIMFile As IntPtr) As Integer

    Private mCallback As WIMMessageCallback

    Public Event ProcessingFile(strFileName As String)
    Public Event [Error](strFile As String, ex As Exception)
    Public Event Warning(strFile As String, ex As Exception)
    Public Event Progress(nPercentDone As Integer, nMillisRemaining As Integer)

    Public Sub New(strPath As String)
        mHandle = WIMCreateFile(strPath, FileLike.AccessLevel.Read, FileLike.CreationDisposition.OpenExisting,
                                WIMFlags.None, CompressType.None, Nothing)
        If mHandle = IntPtr.Zero Then
            Throw New ComponentModel.Win32Exception(Err.LastDllError)
        End If

        mCallback = New WIMMessageCallback(AddressOf MessageCallbackProc)
        If WIMRegisterMessageCallback(mHandle, mCallback, IntPtr.Zero) = &HFFFF_FFFF Then
            Throw New ComponentModel.Win32Exception(Err.LastDllError)
        End If

        Dim strTempFolder As String = IO.Path.Combine(TempFolder, "WIM_" & Guid.NewGuid.ToString("N"))
        IO.Directory.CreateDirectory(strTempFolder)

        If Not WIMSetTemporaryPath(mHandle, strTempFolder) Then
            Throw New ComponentModel.Win32Exception(Err.LastDllError)
        End If
    End Sub

    Private Function MessageCallbackProc(msg As WIMMessage, wParam As IntPtr, lParam As IntPtr, pUserData As IntPtr) As WIMMessage
        Try
            Select Case msg
                Case WIMMessage.WIM_MSG_PROCESS
                    RaiseEvent ProcessingFile(Marshal.PtrToStringUni(wParam))

                Case WIMMessage.WIM_MSG_ERROR
                    RaiseEvent [Error](Marshal.PtrToStringUni(wParam), New ComponentModel.Win32Exception(CInt(lParam)))

                Case WIMMessage.WIM_MSG_WARNING
                    RaiseEvent Warning(Marshal.PtrToStringUni(wParam), New ComponentModel.Win32Exception(CInt(lParam)))

                Case WIMMessage.WIM_MSG_PROGRESS
                    RaiseEvent Progress(CInt(wParam), CInt(lParam))
            End Select
        Catch ex As Exception
            Debug.WriteLine("Exception in message callback: " & ex.ToString)
            Return WIMMessage.WIM_MSG_ABORT_IMAGE
        End Try

        Return WIMMessage.WIM_MSG_SUCCESS
    End Function

    Public ReadOnly Property ImageCount As Integer
        Get
            Return WIMGetImageCount(mHandle)
        End Get
    End Property
End Class

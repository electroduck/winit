Imports System.Runtime.InteropServices

Public Class WindowsImage
    Inherits WIMHandle

    Private Declare Unicode Function WIMLoadImage Lib "wimgapi.dll" (hWIMFile As IntPtr, nIndex As Integer) As IntPtr

    Private Declare Unicode Function WIMGetImageInformation Lib "wimgapi.dll" _
        (hImage As IntPtr, ByRef rpImageInfo As IntPtr, ByRef nImageInfoLength As Integer) As Boolean

    Private mInfoXML As String
    Private mDisplayName As String

    Public Sub New(wim As WindowsImageFile, nImageIndex As Integer)
        mHandle = WIMLoadImage(wim.Handle, nImageIndex)
        If mHandle = IntPtr.Zero Then
            Throw New ComponentModel.Win32Exception(Err.LastDllError)
        End If

        mInfoXML = GetInformationXML()
        ProcessInformationXML(mInfoXML)
    End Sub

    Private Function GetInformationXML() As String
        Dim pImageInfo As IntPtr
        Dim nImageInfoLen As Integer

        If Not WIMGetImageInformation(mHandle, pImageInfo, nImageInfoLen) Then
            Throw New ComponentModel.Win32Exception(Err.LastDllError)
        End If

        Dim arrInfo() As Byte = Array.CreateInstance(GetType(Byte), nImageInfoLen)
        Marshal.Copy(pImageInfo, arrInfo, 0, nImageInfoLen)

        Return Text.Encoding.Unicode.GetString(arrInfo)
    End Function

    Private Const XML_DISPLAYNAME As String = "<DISPLAYNAME>"

    Private Sub ProcessInformationXML(strXML As String)
        Dim nDispNameStart As Integer = strXML.IndexOf(XML_DISPLAYNAME) + XML_DISPLAYNAME.Length
        Dim nDispNameEnd As Integer = strXML.IndexOf("<"c, nDispNameStart)
        mDisplayName = strXML.Substring(nDispNameStart, nDispNameEnd - nDispNameStart)
    End Sub

    Public ReadOnly Property DisplayName As String
        Get
            Return mDisplayName
        End Get
    End Property
End Class

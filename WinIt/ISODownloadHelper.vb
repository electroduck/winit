Public MustInherit Class ISODownloadHelper
    Public MustOverride ReadOnly Property DownloadName As String
    Public MustOverride ReadOnly Property DownloadFileName As String
    Public MustOverride Function GetDownloadURL() As String

    Public Overridable Sub Download(strToFile As String, wndParent As IWin32Window)
        DownloadLarge(GetDownloadURL(), strToFile, DownloadName, wndParent)
    End Sub
End Class

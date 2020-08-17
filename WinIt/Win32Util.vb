Module Win32Util

    Private Declare Auto Function ExtractIconEx Lib "shell32.dll" _
        (strFile As String, nIconIndex As Integer, ByRef rhIconLarge As IntPtr,
         ByRef rhIconSmall As IntPtr, nIcons As UInteger) As UInteger

    Private Declare Ansi Function DestroyIcon Lib "user32.dll" (hIcon As IntPtr) As Boolean

    Public Function GetIcon(strFile As String, nIndex As Integer, bSmall As Boolean) As Icon
        Dim hIcon As IntPtr
        Dim nExtracted As UInteger

        If bSmall Then
            nExtracted = ExtractIconEx(strFile, nIndex, Nothing, hIcon, 1)
        Else
            nExtracted = ExtractIconEx(strFile, nIndex, hIcon, Nothing, 1)
        End If

        If nExtracted < 1 Or hIcon = IntPtr.Zero Then
            Throw New ComponentModel.Win32Exception(Err.LastDllError)
        End If

        ' Force the Icon object to own the handle (and delete it when disposed)
        Dim icnManaged As Icon = Icon.FromHandle(hIcon)
        ForceChangeValue(icnManaged, "ownHandle", True)
        Return icnManaged
    End Function
End Module

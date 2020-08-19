Module TempFiles
    Private mTempFolder As String = ""

    Private Sub InitTempFolder()
        SyncLock mTempFolder
            If mTempFolder.Length = 0 Then
                mTempFolder = IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString)
                IO.Directory.CreateDirectory(mTempFolder)
            End If
        End SyncLock
    End Sub

    Public Sub CleanTempFiles()
        SyncLock mTempFolder
            If mTempFolder.Length > 0 Then
                ShellDelete(mTempFolder)
                'IO.Directory.Delete(mTempFolder, True)
                mTempFolder = ""
            End If
        End SyncLock
    End Sub

    Public ReadOnly Property TempFolder As String
        Get
            InitTempFolder()
            Return mTempFolder
        End Get
    End Property
End Module

Class CommandFailedException
    Inherits Exception

    Private mResult As CommandResult
    Public ReadOnly Property Result As CommandResult
        Get
            Return mResult
        End Get
    End Property

    Public Sub New(res As CommandResult)
        MyBase.New("Command failed with code " & res.nExitCode & ".")
        mResult = res
    End Sub
End Class

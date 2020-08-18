Public Class InstallCancelledException
    Inherits Exception

    Public Sub New()
        MyBase.New("The install was cancelled by the user")
    End Sub
End Class

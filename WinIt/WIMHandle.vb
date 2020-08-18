Public Class WIMHandle
    Implements IDisposable

    Protected Declare Ansi Function WIMCloseHandle Lib "wimgapi.dll" (hObject As IntPtr) As Boolean

    Protected mHandle As IntPtr

    Public ReadOnly Property Handle As IntPtr
        Get
            Return mHandle
        End Get
    End Property

#Region "IDisposable Support"
    Private mDisposed As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(bDisposing As Boolean)
        If Not mDisposed And mHandle <> IntPtr.Zero Then
            WIMCloseHandle(mHandle)
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

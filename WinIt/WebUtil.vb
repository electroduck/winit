Module WebUtil
    Public Sub DownloadLarge(strURL As String, strTo As String, strName As String, wndParent As IWin32Window)
        Dim bCompleted As Boolean = False

        Dim page As New KPreisser.UI.TaskDialogPage With {
            .AllowCancel = True,
            .Instruction = "Downloading " & strName,
            .ProgressBar = New KPreisser.UI.TaskDialogProgressBar With {
                .Maximum = 100,
                .State = KPreisser.UI.TaskDialogProgressBarState.Marquee
            },
            .Title = "Downloading file",
            .Expander = New KPreisser.UI.TaskDialogExpander With {
                .Expanded = False,
                .Text = "Downloading " & strURL & " to " & strTo,
                .ExpandFooterArea = True
            }
        }

        page.StandardButtons.Add(KPreisser.UI.TaskDialogResult.Cancel)
        Dim dlgDownload As New KPreisser.UI.TaskDialog(page)

        Dim client As New Net.WebClient

        AddHandler client.DownloadProgressChanged,
            Sub(objSender2 As Object, e2 As Net.DownloadProgressChangedEventArgs)
                page.ProgressBar.Value = e2.ProgressPercentage
                page.ProgressBar.State = KPreisser.UI.TaskDialogProgressBarState.Normal
                'page.Text = String.Format("{0} of {0} KB received", e2.BytesReceived \ 1024, e2.TotalBytesToReceive \ 1024)
            End Sub

        AddHandler client.DownloadFileCompleted,
            Sub(objSender2 As Object, e2 As ComponentModel.AsyncCompletedEventArgs)
                bCompleted = True
                page.ProgressBar.Value = 100
                page.Text = "Download complete."
                dlgDownload.Close()
            End Sub

        AddHandler page.Created,
            Sub(objSender As Object, e As EventArgs)

                client.DownloadFileAsync(New Uri(strURL), strTo)
            End Sub

        If wndParent IsNot Nothing Then
            dlgDownload.Show(wndParent)
        Else
            dlgDownload.Show()
        End If

        If Not bCompleted Then
            Throw New DownloadCancelledException(strName)
        End If
    End Sub
End Module

Class DownloadCancelledException
    Inherits Exception

    Public Sub New(strDownloadName As String)
        MyBase.New("Download of " & strDownloadName & " was cancelled by the user.")
    End Sub
End Class

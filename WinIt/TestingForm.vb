Public Class TestingForm
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles LoadWIMBtn.Click
        If WIMDlg.ShowDialog = DialogResult.OK Then
            Using wim As New WindowsImageFile(WIMDlg.FileName)
                MessageBox.Show(Me, "Images: " & wim.ImageCount)
                For nImage As Integer = 1 To wim.ImageCount
                    Using img As New WindowsImage(wim, nImage)
                        MessageBox.Show(Me, "Image " & nImage & ": " & img.DisplayName)
                    End Using
                Next
            End Using
        End If
    End Sub
End Class

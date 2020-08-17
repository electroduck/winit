Imports KPreisser.UI

Public Class MainForm
    Private mISOMode As Boolean = False
    Private mSourcePath As String = ""

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AdjustPanelWidths()
    End Sub

    Private Sub AdjustPanelWidths()
        For Each ctl As Control In MainFlowPnl.Controls
            ctl.Width = MainFlowPnl.Width - (ctl.Margin.Left + ctl.Margin.Right)
        Next
    End Sub

    Private Sub MainForm_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        ResizeDebounceTmr.Enabled = True
    End Sub

    Private mWidthLast As Integer = 0
    Private Sub ResizeDebounceTmr_Tick(sender As Object, e As EventArgs) Handles ResizeDebounceTmr.Tick
        If mWidthLast = Width Then
            AdjustPanelWidths()
            ResizeDebounceTmr.Enabled = False
        Else
            mWidthLast = Width
        End If
    End Sub

    Private Sub ChooseISOBtn_Click(sender As Object, e As EventArgs) Handles ChooseISOBtn.Click
        If OpenISODlg.ShowDialog = DialogResult.OK Then
            mISOMode = True
            mSourcePath = OpenISODlg.FileName
            ImageSelLbl.Text = "Selected ISO file: " & IO.Path.GetFileName(mSourcePath)
        End If
    End Sub

    Private Sub ChooseDiskBtn_Click(sender As Object, e As EventArgs) Handles ChooseDiskBtn.Click
        If OpenInstDiskDlg.ShowDialog = DialogResult.OK Then
            mISOMode = False
            mSourcePath = OpenInstDiskDlg.SelectedPath
            ImageSelLbl.Text = "Selected install disk: " & mSourcePath
        End If
    End Sub

    Private Shared ReadOnly arrDownloaders() As ISODownloadHelper = {
        New Win10ConsumerDL(False),
        New Win10ConsumerDL(True)
    }

    Private Sub ChooseDLBtn_Click(sender As Object, e As EventArgs) Handles ChooseDLBtn.Click
        Dim pageChoices As New TaskDialogPage With {
            .AllowCancel = True,
            .Title = "Download ISO",
            .Instruction = "Choose an ISO file to download",
            .Text = "WinIt will automatically download and select the chosen ISO.",
            .CustomButtonStyle = TaskDialogCustomButtonStyle.CommandLinks
        }

        For Each dl As ISODownloadHelper In arrDownloaders
            pageChoices.CustomButtons.Add(New TaskDialogCustomButton With
                                          {.Text = dl.DownloadName, .Tag = dl})
        Next

        Dim dlgChoices As New TaskDialog(pageChoices)
        Dim btnResult As TaskDialogButton = dlgChoices.Show()

        If btnResult.GetType Is GetType(TaskDialogCustomButton) Then
            Enabled = False
            Try
                Dim dl As ISODownloadHelper = btnResult.Tag
                Dim strTempFile As String = IO.Path.Combine(TempFolder, dl.DownloadFileName & ".iso")
                Try
                    dl.Download(strTempFile, Me)
                    mSourcePath = strTempFile
                Catch exCancelled As DownloadCancelledException
                    MessageBox.Show(Me, "Download of " & dl.DownloadName & " cancelled.")
                Catch ex As Exception
                    ShowErrorMessage("Error downloading file", ex)
                End Try
            Finally
                Enabled = True
            End Try
        End If
    End Sub

    Private Sub ShowErrorMessage(strTitle As String, ex As Exception)
        Dim page As New TaskDialogPage With {
            .AllowCancel = True,
            .CanBeMinimized = False,
            .Expander = New TaskDialogExpander With {
                .CollapsedButtonText = "Details",
                .Text = ex.ToString
            },
            .Icon = SystemIcons.Error,
            .Text = ex.Message,
            .Title = "Error",
            .Instruction = strTitle
        }

        page.StandardButtons.Add(TaskDialogResult.OK)

        Dim dlgError As New TaskDialog(page)
        dlgError.Show()
    End Sub

    Private Sub MainForm_Closing(objSender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        CleanTempFiles()
    End Sub
End Class
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

    Private Sub ChooseDLBtn_Click(sender As Object, e As EventArgs) Handles ChooseDLBtn.Click
        Dim pageChoices As New KPreisser.UI.TaskDialogPage With {
            .AllowCancel = True,
            .Title = "Download ISO",
            .Instruction = "Choose an ISO file to download",
            .Text = "WinIt will automatically download and select the chosen ISO."
        }

        pageChoices.CustomButtons.Add("Windows 10 Home/Pro 64-bit")
        pageChoices.CustomButtons.Add("Windows 10 Home/Pro 32-bit")

        Dim dlgChoices As New KPreisser.UI.TaskDialog(pageChoices)
        dlgChoices.Show()
    End Sub
End Class
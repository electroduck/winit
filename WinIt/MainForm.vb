Imports KPreisser.UI

Public Class MainForm
    Private mISOMode As Boolean = False
    Private mSourcePath As String = ""
    Private mTargetDisk As Harddisk
    Private mSelDiskBrush As Brush
    Private mDiskIcon As Icon

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mSelDiskBrush = New SolidBrush(Color.FromArgb(128, SystemColors.Highlight))
        mDiskIcon = GetIcon("shell32.dll", 79, True)
        AdjustPanelWidths()
        BuildHDDList()
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
        InstallBtn.Enabled = CanInstall()
    End Sub

    Private Sub ChooseDiskBtn_Click(sender As Object, e As EventArgs) Handles ChooseDiskBtn.Click
        If OpenInstDiskDlg.ShowDialog = DialogResult.OK Then
            mISOMode = False
            mSourcePath = OpenInstDiskDlg.SelectedPath
            ImageSelLbl.Text = "Selected install disk: " & mSourcePath
        End If
        InstallBtn.Enabled = CanInstall()
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

        pageChoices.StandardButtons.Add(TaskDialogResult.Cancel)

        Dim dlgChoices As New TaskDialog(pageChoices)
        Dim btnResult As TaskDialogButton = dlgChoices.Show()

        If btnResult.GetType Is GetType(TaskDialogCustomButton) Then
            Dim dl As ISODownloadHelper = btnResult.Tag
            Enabled = False
            Try
                Throw New Exception("Test exception")
                Dim strTempFile As String = IO.Path.Combine(TempFolder, dl.DownloadFileName)
                dl.Download(strTempFile, Me)
                mSourcePath = strTempFile
            Catch exCancelled As DownloadCancelledException
                MessageBox.Show(Me, "Download of " & dl.DownloadName & " cancelled.")
            Catch ex As Exception
                ShowErrorMessage("Error downloading file", ex)
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
                .Text = ex.ToString,
                .ExpandFooterArea = True
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

    Private Sub BuildHDDList()
        TargetIconList.Images.Clear()
        TargetIconList.Images.Add(GetIcon("shell32.dll", 79, True))

        TargetListView.Clear()
        Dim arrDisks() As Harddisk = Harddisk.GetDiskList
        For Each disk As Harddisk In arrDisks
            Debug.WriteLine(String.Format("Disk V={0} PID={1} PR={2} SN={3} MB={4:N0}", disk.VendorID, disk.ProductID,
                disk.ProductRevision, disk.SerialNumber, disk.Size.Megabytes))

            Dim itmDisk As New ListViewItem With {
                .Text = String.Format("{0} ({1})", disk.Name, disk.Size),
                .ImageIndex = 0,
                .Tag = disk
            }

            TargetListView.Items.Add(itmDisk)
        Next
    End Sub

    Private Sub TargetListView_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TargetListView.SelectedIndexChanged
        If TargetListView.SelectedIndices.Count = 0 Then
            mTargetDisk = Nothing
        Else
            mTargetDisk = TargetListView.SelectedItems(0).Tag
        End If
        InstallBtn.Enabled = CanInstall()
    End Sub

    Private Sub TargetListView_DrawItem(sender As Object, e As DrawListViewItemEventArgs) Handles TargetListView.DrawItem
        If mTargetDisk IsNot Nothing AndAlso e.Item.Tag Is mTargetDisk Then
            e.Graphics.FillRectangle(mSelDiskBrush, e.Bounds)
        End If

        e.Graphics.DrawIcon(mDiskIcon, e.Bounds.X, e.Bounds.Y)
        e.Graphics.DrawString(e.Item.Text, TargetListView.Font, SystemBrushes.ControlText, e.Bounds.X + 18, e.Bounds.Y + 2)
    End Sub

    Private Function CanInstall() As Boolean
        Return mTargetDisk IsNot Nothing And mSourcePath.Length > 0
    End Function
End Class

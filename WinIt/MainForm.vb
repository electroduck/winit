Imports KPreisser.UI

Public Class MainForm
    Private mISOMode As Boolean = False
    Private mSourcePath As String = ""
    Private mTargetDisk As Harddisk
    Private mSelDiskBrush As Brush
    Private mDiskIcon As Icon
    Private mBlockage As String
    Private mBlockageTipShowing As Boolean

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mSelDiskBrush = New SolidBrush(Color.FromArgb(128, SystemColors.Highlight))
        mDiskIcon = GetIcon("shell32.dll", 79, True)
        AdjustPanelWidths()
        BuildHDDList()
        CheckCanInstall()
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
        CheckCanInstall()
    End Sub

    Private Sub ChooseDiskBtn_Click(sender As Object, e As EventArgs) Handles ChooseDiskBtn.Click
        If OpenInstDiskDlg.ShowDialog = DialogResult.OK Then
            mISOMode = False
            mSourcePath = OpenInstDiskDlg.SelectedPath
            ImageSelLbl.Text = "Selected install disk: " & mSourcePath
        End If
        CheckCanInstall()
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
                Dim strTempFile As String = IO.Path.Combine(TempFolder, dl.DownloadFileName)
                dl.Download(strTempFile, Me)
                mSourcePath = strTempFile
                mISOMode = True
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
            Try
                Debug.WriteLine(String.Format("Disk V={0} PID={1} PR={2} SN={3} MB={4:N0}", disk.VendorID, disk.ProductID,
                    disk.ProductRevision, disk.SerialNumber, disk.Size.Megabytes))

                Dim itmDisk As New ListViewItem With {
                    .Text = String.Format("{0} ({1})", disk.Name, disk.Size),
                    .ImageIndex = 0,
                    .Tag = disk
                }

                TargetListView.Items.Add(itmDisk)
            Catch ex As Exception
                'ShowErrorMessage("Problem with Disk " & disk.Number, ex)
                Debug.WriteLine("Exception accessing disk " & disk.Number & ": " & ex.ToString)
            End Try
        Next
    End Sub

    Private Sub TargetListView_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TargetListView.SelectedIndexChanged
        If TargetListView.SelectedIndices.Count = 0 Then
            mTargetDisk = Nothing
        Else
            mTargetDisk = TargetListView.SelectedItems(0).Tag
        End If
        CheckCanInstall()
    End Sub

    Private Sub TargetListView_DrawItem(sender As Object, e As DrawListViewItemEventArgs) Handles TargetListView.DrawItem
        If mTargetDisk IsNot Nothing AndAlso e.Item.Tag Is mTargetDisk Then
            e.Graphics.FillRectangle(mSelDiskBrush, e.Bounds)
        End If

        e.Graphics.DrawIcon(mDiskIcon, e.Bounds.X, e.Bounds.Y)
        e.Graphics.DrawString(e.Item.Text, TargetListView.Font, If(InstallWorker.IsBusy, SystemBrushes.ControlText, SystemBrushes.GrayText),
                              e.Bounds.X + 18, e.Bounds.Y + 2)
    End Sub

    Private Sub CheckCanInstall()
        Dim bCanInstall As Boolean

        If mTargetDisk Is Nothing Then
            bCanInstall = False
            If mSourcePath.Length = 0 Then
                mBlockage = "Choose a target disk and an install disk first."
            Else
                mBlockage = "Choose a target disk first."
            End If
        ElseIf mSourcePath.Length = 0 Then
            bCanInstall = False
            mBlockage = "Choose an install disk first."
        Else
            bCanInstall = True
        End If

        InstallBtn.Enabled = bCanInstall
    End Sub

    ' Can't use enter or hover events of install button when the button is disabled
    ' Can't use MainForm.MouseMove because we are hovering over another control
    Private Sub InstallHoverCheckTmr_Tick(sender As Object, e As EventArgs) Handles InstallHoverCheckTmr.Tick
        Dim rectInstallButtonClient As New Rectangle(0, 0, InstallBtn.Width, InstallBtn.Height)
        Dim rectInstallButtonScreen As Rectangle = InstallBtn.RectangleToScreen(rectInstallButtonClient)

        If rectInstallButtonScreen.Contains(MousePosition) And Not InstallBtn.Enabled Then
            If Not mBlockageTipShowing Then
                BlockageToolTip.Show(mBlockage, Me, PointToClient(InstallBtn.PointToScreen(
                                     New Point(InstallBtn.Width \ 2 - 12, InstallBtn.Height \ 2 - 12))))
                mBlockageTipShowing = True
            End If
        ElseIf mBlockageTipShowing Then
            BlockageToolTip.Hide(Me)
            mBlockageTipShowing = False
        End If
    End Sub

    Private Sub InstallBtn_Click(sender As Object, e As EventArgs) Handles InstallBtn.Click
        If InstallWorker.IsBusy Then
            ' TODO: Confirm cancel
        Else
            ConfirmInstall()
        End If
    End Sub

    Private Sub ConfirmInstall()
        Dim page As New TaskDialogPage With {
            .AllowCancel = True,
            .CanBeMinimized = False,
            .CheckBox = New TaskDialogCheckBox("I understand"),
            .Icon = TaskDialogIcon.Get(TaskDialogStandardIcon.SecurityWarningYellowBar),
            .Instruction = "Are you sure?",
            .Text = "You are about to install Windows on " & mTargetDisk.Name & ". " &
                "All existing files on this disk will be deleted.",
            .Title = "Confirm Installation"
        }

        page.StandardButtons.Add(TaskDialogResult.Cancel)
        page.StandardButtons.Add(TaskDialogResult.Continue)
        page.StandardButtons.Item(TaskDialogResult.Continue).Enabled = False

        AddHandler page.CheckBox.CheckedChanged,
            Sub(objSender As Object, e2 As EventArgs)
                page.StandardButtons.Item(TaskDialogResult.Continue).Enabled = page.CheckBox.Checked
            End Sub

        Dim dlgConfirm As New TaskDialog(page)
        If DirectCast(dlgConfirm.Show(Me), TaskDialogStandardButton).Result = TaskDialogResult.Continue Then
            StartInstall()
        End If
    End Sub

    Private Sub StartInstall()
        InputFilePnl.Enabled = False
        TargetDiskPnl.Enabled = False
        InstallWorker.RunWorkerAsync()
    End Sub
End Class

Imports KPreisser.UI
Imports System.ComponentModel

Public Class MainForm
    Private mISOMode As Boolean = False
    Private mSourcePath As String = ""
    Private mTargetDisk As Harddisk
    Private mSelDiskBrush As Brush
    Private mDiskIcon As Icon
    Private mBlockage As String
    Private mBlockageTipShowing As Boolean
    Private mCancelDialog As TaskDialog
    Private mFormHandle As IntPtr

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Icon = My.Resources.ProgramIcon
        mFormHandle = Handle
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
        dlgError.Show(mFormHandle)
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

    Private Sub TargetListView_SelectedIndexChanged(sender As Object, e As EventArgs)
        If TargetListView.SelectedIndices.Count = 0 Then
            mTargetDisk = Nothing
        Else
            mTargetDisk = TargetListView.SelectedItems(0).Tag
        End If
        CheckCanInstall()
    End Sub

    Private Sub TargetListView_DrawItem(sender As Object, e As DrawListViewItemEventArgs)
        If mTargetDisk IsNot Nothing AndAlso e.Item.Tag Is mTargetDisk Then
            e.Graphics.FillRectangle(mSelDiskBrush, e.Bounds)
        End If

        e.Graphics.DrawIcon(mDiskIcon, e.Bounds.X, e.Bounds.Y)
        e.Graphics.DrawString(e.Item.Text, TargetListView.Font, If(InstallWorker.IsBusy, SystemBrushes.GrayText, SystemBrushes.ControlText),
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
            ConfirmCancel()
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

    Private Sub ConfirmCancel()
        Dim page As New TaskDialogPage With {
            .AllowCancel = True,
            .CanBeMinimized = False,
            .Icon = TaskDialogIcon.Get(TaskDialogStandardIcon.Warning),
            .Instruction = "Stop installing?",
            .Text = "If you choose to stop installing Windows on this disk, " &
                "you will not be able to boot from it until the installation is restarted and completed. " &
                vbNewLine & vbNewLine &
                "Changes already made will not be undone. " &
                "Any old files that used to be on the disk will most likely still be gone.",
            .Title = "Confirm Installation"
        }

        Dim btnStop As New TaskDialogCustomButton With {
            .Text = "Stop installation",
            .DescriptionText = "Existing changes will not be undone"
        }

        Dim btnContinue As New TaskDialogCustomButton With {
            .Text = "Continue",
            .DescriptionText = "Finish installing Windows"
        }

        page.CustomButtons.Add(btnStop)
        page.CustomButtons.Add(btnContinue)

        mCancelDialog = New TaskDialog(page)
        If mCancelDialog.Show(Me) Is btnStop Then
            InstallWorker.CancelAsync()
        End If
        mCancelDialog = Nothing
    End Sub

    Private Sub StartInstall()
        InputFilePnl.Enabled = False
        TargetDiskPnl.Enabled = False
        InstallPbar.Style = ProgressBarStyle.Marquee
        InstallBtn.Text = "Cancel"
        ProgressText1Lbl.Text = "Starting installation..."

        InstallWorker.RunWorkerAsync()
        TargetListView.Invalidate()
    End Sub

    Private Const ISO_PATH_WIM As String = "sources\install.wim"

    Private Sub InstallWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles InstallWorker.DoWork
        e.Result = Nothing
        Try
            If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If

            Dim strWIMPath As String
            If mISOMode Then
                Using stmISO As IO.Stream = IO.File.OpenRead(mSourcePath)
                    Dim diskSource As New DiscUtils.Udf.UdfReader(stmISO)

                    If Not diskSource.FileExists(ISO_PATH_WIM) Then
                        Throw New FormatException("The given ISO file is not a Windows NT6+ install disk")
                    End If

                    Dim nWIMSize As Long = diskSource.GetFileLength(ISO_PATH_WIM)
                    strWIMPath = IO.Path.Combine(TempFolder, Guid.NewGuid.ToString("N") & ".iso")

                    'Dim nExtractedBytes As Long = 0
                    'Dim nRead As Long = 0
                    'Dim arrData(ISO_EXTRACT_BLOCKSIZE - 1) As Byte
                    'Using stmWIMOnISO As DiscUtils.Streams.SparseStream = diskSource.OpenFile(ISO_PATH_WIM, IO.FileMode.Open)
                    '    Using stmWIMFile As IO.Stream = IO.File.Create(strWIMPath)
                    '        While nExtractedBytes < nWIMSize
                    '            If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If

                    '            nRead = stmWIMOnISO.Read(arrData, 0, ISO_EXTRACT_BLOCKSIZE)
                    '            stmWIMFile.Write(arrData, 0, nRead)
                    '            nExtractedBytes += nRead
                    '            InstallWorker.ReportProgress((nExtractedBytes / nWIMSize) * 100.0,
                    '                                         "Extracting Windows image...")
                    '        End While
                    '    End Using
                    'End Using

                    Using stmWIMOnISO As DiscUtils.Streams.SparseStream = diskSource.OpenFile(ISO_PATH_WIM, IO.FileMode.Open)
                        Using stmWIMFile As IO.Stream = IO.File.Create(strWIMPath)
                            CopyStreamWithProgress(stmWIMOnISO, stmWIMFile,
                                Sub(nBytesCopied As Long, nBytesTotal As Long)
                                    If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If
                                    InstallWorker.ReportProgress((nBytesCopied / nBytesTotal) * 100, "Extracting Windows image...")
                                End Sub)
                        End Using
                    End Using
                End Using
            Else
                strWIMPath = IO.Path.Combine(mSourcePath, ISO_PATH_WIM)
                If Not IO.File.Exists(strWIMPath) Then
                    Throw New FormatException("The given location does not contain a Windows NT6+ install disk")
                End If
            End If

            If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If
            InstallWorker.ReportProgress(-1, "Initializing disk...")
            mTargetDisk.ReinitializeMBR()

            Dim arrParts() As Harddisk.PartitionPrototype = {
                New Harddisk.PartitionPrototype With { ' System (boot)
                    .bBootable = True,
                    .nLength = DataQuantity.FromMegabytes(100)
                },
                New Harddisk.PartitionPrototype With { ' Recovery
                    .bBootable = False,
                    .nLength = DataQuantity.FromMegabytes(500)
                },
                New Harddisk.PartitionPrototype With { ' Windows (main)
                    .bBootable = False,
                    .nLength = Nothing
                }
            }

            If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If
            InstallWorker.ReportProgress(-1, "Partitioning disk...")
            mTargetDisk.RepartitionMBR(arrParts)

            ' Wait 10 seconds for the change to be detected
            For n As Integer = 1 To 100
                InstallWorker.ReportProgress(n, "Partitioning disk...")
                Threading.Thread.Sleep(100)
            Next

            If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If
            InstallWorker.ReportProgress(-1, "Formatting boot partition...")
            Dim partBoot As Partition = mTargetDisk.Partition(1)
            partBoot.Format("NTFS", "Boot")

            If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If
            InstallWorker.ReportProgress(-1, "Formatting recovery partition...")
            Dim partRecovery As Partition = mTargetDisk.Partition(2)
            partRecovery.Format("NTFS", "Recovery")

            If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If
            InstallWorker.ReportProgress(-1, "Formatting main partition...")
            Dim partWindows As Partition = mTargetDisk.Partition(3)
            partWindows.Format("NTFS", "Windows")

            'If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If
            'InstallWorker.ReportProgress(-1, "Mounting boot partition...")
            'Dim strBootMount As String = IO.Path.Combine(TempFolder, "Boot_" & Guid.NewGuid.ToString("n"))
            'partBoot.Mount(strBootMount)

            'If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If
            'InstallWorker.ReportProgress(-1, "Mounting recovery partition...")
            'Dim strRecoveryMount As String = IO.Path.Combine(TempFolder, "Recovery_" & Guid.NewGuid.ToString("n"))
            'partRecovery.Mount(strRecoveryMount)

            'If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If
            'InstallWorker.ReportProgress(-1, "Mounting main partition...")
            'Dim strWindowsMount As String = IO.Path.Combine(TempFolder, "Windows_" & Guid.NewGuid.ToString("n"))
            'partWindows.Mount(strWindowsMount)

            Dim strBootMount As String = partBoot.DriveLetter & "\"
            Dim strRecoveryMount As String = partRecovery.DriveLetter & "\"
            Dim strWindowsMount As String = partWindows.DriveLetter & "\"

            If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If
            InstallWorker.ReportProgress(-1, "Reading image file...")
            Dim wim As New WindowsImageFile(strWIMPath)

            Dim imgToInstall As WindowsImage = Nothing
            If wim.ImageCount > 1 Then
                Dim pageChooseImage As New TaskDialogPage With {
                    .AllowCancel = False,
                    .CanBeMinimized = False,
                    .Instruction = "Choose Windows edition",
                    .Text = "The installer you selected contains several different editions of Windows. " &
                        "Select the one you would like to install.",
                    .Title = "Choose Edition"
                }

                pageChooseImage.StandardButtons.Add(TaskDialogResult.Cancel)
                pageChooseImage.StandardButtons.Add(TaskDialogResult.OK)
                pageChooseImage.StandardButtons.Item(TaskDialogResult.OK).Enabled = False

                For nImage = 1 To wim.ImageCount
                    Dim img As New WindowsImage(wim, nImage)
                    Dim rbCurImg As New TaskDialogRadioButton With {
                        .Text = img.DisplayName,
                        .Tag = img
                    }

                    AddHandler rbCurImg.CheckedChanged,
                        Sub()
                            pageChooseImage.StandardButtons.Item(TaskDialogResult.OK).Enabled = True
                            If rbCurImg.Checked Then
                                imgToInstall = rbCurImg.Tag
                            End If
                        End Sub

                    pageChooseImage.RadioButtons.Add(rbCurImg)
                Next

                Dim dlgChooseImage As New TaskDialog(pageChooseImage)
                If DirectCast(dlgChooseImage.Show(mFormHandle), TaskDialogStandardButton).Result <> TaskDialogResult.OK Then
                    Throw New InstallCancelledException
                End If

                For Each rb As TaskDialogRadioButton In pageChooseImage.RadioButtons
                    If rb.Tag IsNot imgToInstall Then
                        DirectCast(rb.Tag, IDisposable).Dispose()
                    End If
                Next
            Else
                imgToInstall = New WindowsImage(wim, 1)
            End If

            Dim exWIMError As Exception = Nothing
            Dim strProblemFile As String = ""

            AddHandler wim.Progress,
                Sub(nPercentDone As Integer, nMillisLeft As Integer)
                    'Dim tsLeft As TimeSpan = TimeSpan.FromMilliseconds(nMillisLeft)
                    InstallWorker.ReportProgress(nPercentDone, "Copying Windows files...")
                    'String.Format("Copying Windows files ({0:F0} {1}s left)...",
                    '    If(tsLeft.TotalMinutes >= 1.0, tsLeft.TotalMinutes, tsLeft.TotalSeconds),
                    '    If(tsLeft.TotalMinutes >= 1.0, "minute", "second")))
                End Sub

            AddHandler wim.Error,
                Sub(strFile As String, ex As Exception)
                    exWIMError = ex
                End Sub

            AddHandler wim.Warning,
                Sub(strFile As String, ex As Exception)
                    Dim pageWarning As New TaskDialogPage With {
                        .AllowCancel = False,
                        .Icon = TaskDialogIcon.Get(TaskDialogStandardIcon.Warning),
                        .Instruction = "Problem copying files",
                        .Text = ex.Message & vbNewLine & vbNewLine & "File: " & strFile,
                        .Title = "Warning"
                    }

                    pageWarning.StandardButtons.Add(TaskDialogResult.Abort)
                    pageWarning.StandardButtons.Add(TaskDialogResult.Continue)

                    Dim dlgWarning As New TaskDialog(pageWarning)
                    If DirectCast(dlgWarning.Show(mFormHandle), TaskDialogStandardButton).Result = TaskDialogResult.Abort Then
                        exWIMError = ex
                        strProblemFile = strFile
                        Throw ex
                    End If
                End Sub

            If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If
            InstallWorker.ReportProgress(-1, "Copying Windows files...")
            imgToInstall.Apply(strWindowsMount)
            imgToInstall.Dispose()
            wim.Dispose()

            If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If
            InstallWorker.ReportProgress(-1, "Copying boot files...")
            CopyBootFiles(strWindowsMount, strBootMount)

            If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If
            InstallWorker.ReportProgress(-1, "Copying recovery image...")
            Dim strWindowsREDir As String = IO.Path.Combine(strRecoveryMount, "Recovery\WindowsRE")
            IO.Directory.CreateDirectory(strWindowsREDir)

            Dim strRecoveryImagePath As String = IO.Path.Combine(strWindowsMount, "Windows\System32\Recovery\Winre.wim")
            Using stmRecoveryImageIn As IO.Stream = IO.File.OpenRead(strRecoveryImagePath)
                Using stmRecoveryImageOut As IO.Stream = IO.File.OpenWrite(IO.Path.Combine(strWindowsREDir, "Winre.wim"))
                    CopyStreamWithProgress(stmRecoveryImageIn, stmRecoveryImageOut,
                        Sub(nBytesCopied As Long, nBytesTotal As Long)
                            If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If
                            InstallWorker.ReportProgress((nBytesCopied / nBytesTotal) * 100, "Copying recovery image...")
                        End Sub)
                End Using
            End Using

            If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If
            InstallWorker.ReportProgress(-1, "Registering recovery image...")
            RegisterRecoveryImage(strWindowsMount, strRecoveryMount)

            If InstallWorker.CancellationPending Then : Throw New InstallCancelledException : End If
            InstallWorker.ReportProgress(-1, "Setting boot and recovery partitions as hidden...")
            partBoot.PartitionType = &H27
            partRecovery.PartitionType = &H27

            InstallWorker.ReportProgress(100, "Complete")
        Catch ex As Exception
            e.Result = ex
        End Try

        GC.Collect()
    End Sub

    Private Sub InstallWorker_Completed(sender As Object, e As RunWorkerCompletedEventArgs) Handles InstallWorker.RunWorkerCompleted
        If mCancelDialog IsNot Nothing Then
            mCancelDialog.Close()
            mCancelDialog = Nothing
        End If

        If e.Result Is Nothing Then
            MessageBox.Show(Me, "Installation succeeded.")
        ElseIf e.Result.GetType = GetType(InstallCancelledException) Then
            MessageBox.Show(Me, "Installation cancelled.")
        Else
            ShowErrorMessage("Error installing Windows", e.Result)
        End If

        InputFilePnl.Enabled = True
        TargetDiskPnl.Enabled = True
        InstallPbar.Style = ProgressBarStyle.Continuous
        InstallPbar.Value = 0
        InstallBtn.Text = "Install"
        ProgressText1Lbl.Text = "Press Install to begin installing Windows onto the selected disk."
        TargetListView.Invalidate()
    End Sub

    Private Sub InstallWorker_Progress(sender As Object, e As ProgressChangedEventArgs) Handles InstallWorker.ProgressChanged
        If e.ProgressPercentage >= 0 Then
            InstallPbar.Style = ProgressBarStyle.Continuous
            InstallPbar.Value = e.ProgressPercentage
        Else
            InstallPbar.Style = ProgressBarStyle.Marquee
        End If
        ProgressText1Lbl.Text = e.UserState
    End Sub

    Private Sub ShowDisclaimer()
        Dim page As New TaskDialogPage With {
            .Icon = TaskDialogStandardIcon.SecurityShield,
            .Instruction = "Disclaimer",
            .Text = My.Resources.Disclaimer,
            .Title = "WinIt",
            .Width = 200
        }

        page.StandardButtons.Add(TaskDialogResult.OK)

        Dim dlgDisclaimer As New TaskDialog(page)
        dlgDisclaimer.Show(Me)
    End Sub

End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.MainFlowPnl = New System.Windows.Forms.FlowLayoutPanel()
        Me.InputFilePnl = New Electroduck.UI.Panels.CollapsePanel()
        Me.ChooseDLBtn = New System.Windows.Forms.Button()
        Me.ImageSelLbl = New System.Windows.Forms.Label()
        Me.ChooseDiskBtn = New System.Windows.Forms.Button()
        Me.ChooseISOBtn = New System.Windows.Forms.Button()
        Me.DiskInstrLbl = New System.Windows.Forms.Label()
        Me.TargetDiskPnl = New Electroduck.UI.Panels.CollapsePanel()
        Me.TargetListView = New Electroduck.WinIt.ListViewEx()
        Me.TargetIconList = New System.Windows.Forms.ImageList(Me.components)
        Me.TargetInstrLbl = New System.Windows.Forms.Label()
        Me.ProgressPnl = New Electroduck.UI.Panels.CollapsePanel()
        Me.InstallBtn = New System.Windows.Forms.Button()
        Me.InstallPbar = New System.Windows.Forms.ProgressBar()
        Me.ProgressText1Lbl = New System.Windows.Forms.Label()
        Me.ResizeDebounceTmr = New System.Windows.Forms.Timer(Me.components)
        Me.HelpToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.OpenISODlg = New System.Windows.Forms.OpenFileDialog()
        Me.OpenInstDiskDlg = New System.Windows.Forms.FolderBrowserDialog()
        Me.InstallWorker = New System.ComponentModel.BackgroundWorker()
        Me.BlockageToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.InstallHoverCheckTmr = New System.Windows.Forms.Timer(Me.components)
        Me.OpenTmr = New System.Windows.Forms.Timer(Me.components)
        Me.MainFlowPnl.SuspendLayout()
        Me.InputFilePnl.SuspendLayout()
        Me.TargetDiskPnl.SuspendLayout()
        Me.ProgressPnl.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainFlowPnl
        '
        Me.MainFlowPnl.Controls.Add(Me.InputFilePnl)
        Me.MainFlowPnl.Controls.Add(Me.TargetDiskPnl)
        Me.MainFlowPnl.Controls.Add(Me.ProgressPnl)
        Me.MainFlowPnl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainFlowPnl.Location = New System.Drawing.Point(0, 0)
        Me.MainFlowPnl.Name = "MainFlowPnl"
        Me.MainFlowPnl.Size = New System.Drawing.Size(584, 462)
        Me.MainFlowPnl.TabIndex = 0
        '
        'InputFilePnl
        '
        Me.InputFilePnl.AnimationLength = 0
        Me.InputFilePnl.BorderColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(160, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.InputFilePnl.BorderMargin = New System.Windows.Forms.Padding(8)
        Me.InputFilePnl.BorderPadding = New System.Windows.Forms.Padding(2)
        Me.InputFilePnl.BorderThickness = CType(1, Short)
        Me.InputFilePnl.CanCollapse = True
        Me.InputFilePnl.Controls.Add(Me.ChooseDLBtn)
        Me.InputFilePnl.Controls.Add(Me.ImageSelLbl)
        Me.InputFilePnl.Controls.Add(Me.ChooseDiskBtn)
        Me.InputFilePnl.Controls.Add(Me.ChooseISOBtn)
        Me.InputFilePnl.Controls.Add(Me.DiskInstrLbl)
        Me.InputFilePnl.CornerRadius = 6
        Me.InputFilePnl.Expanded = True
        Me.InputFilePnl.HeaderFont = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InputFilePnl.HeaderHeight = 32
        Me.InputFilePnl.HeaderText = "1: Disk Image"
        Me.InputFilePnl.InnerBackgroundColor = System.Drawing.SystemColors.Window
        Me.InputFilePnl.InnerBackgroundTexture = Nothing
        Me.InputFilePnl.Location = New System.Drawing.Point(3, 3)
        Me.InputFilePnl.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
        Me.InputFilePnl.Name = "InputFilePnl"
        Me.InputFilePnl.Padding = New System.Windows.Forms.Padding(22, 40, 20, 20)
        Me.InputFilePnl.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.InputFilePnl.ShadowEnabled = True
        Me.InputFilePnl.ShadowOffset = New System.Windows.Forms.Padding(-3, 4, 3, 7)
        Me.InputFilePnl.ShadowSoftness = 8.0!
        Me.InputFilePnl.Size = New System.Drawing.Size(569, 144)
        Me.InputFilePnl.TabIndex = 0
        Me.InputFilePnl.Text = "1: Disk Image"
        '
        'ChooseDLBtn
        '
        Me.ChooseDLBtn.Location = New System.Drawing.Point(344, 65)
        Me.ChooseDLBtn.Margin = New System.Windows.Forms.Padding(0)
        Me.ChooseDLBtn.Name = "ChooseDLBtn"
        Me.ChooseDLBtn.Size = New System.Drawing.Size(150, 27)
        Me.ChooseDLBtn.TabIndex = 5
        Me.ChooseDLBtn.Text = "Download ISO"
        Me.HelpToolTip.SetToolTip(Me.ChooseDLBtn, "WinIt can automatically download the Windows 10 installer for you.")
        Me.ChooseDLBtn.UseVisualStyleBackColor = True
        '
        'ImageSelLbl
        '
        Me.ImageSelLbl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ImageSelLbl.AutoEllipsis = True
        Me.ImageSelLbl.BackColor = System.Drawing.SystemColors.Window
        Me.ImageSelLbl.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ImageSelLbl.Location = New System.Drawing.Point(25, 100)
        Me.ImageSelLbl.Margin = New System.Windows.Forms.Padding(3, 8, 3, 8)
        Me.ImageSelLbl.Name = "ImageSelLbl"
        Me.ImageSelLbl.Size = New System.Drawing.Size(521, 17)
        Me.ImageSelLbl.TabIndex = 4
        Me.ImageSelLbl.Text = "No disk image selected."
        '
        'ChooseDiskBtn
        '
        Me.ChooseDiskBtn.Location = New System.Drawing.Point(186, 65)
        Me.ChooseDiskBtn.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.ChooseDiskBtn.Name = "ChooseDiskBtn"
        Me.ChooseDiskBtn.Size = New System.Drawing.Size(150, 27)
        Me.ChooseDiskBtn.TabIndex = 3
        Me.ChooseDiskBtn.Text = "Choose install disk"
        Me.ChooseDiskBtn.UseVisualStyleBackColor = True
        '
        'ChooseISOBtn
        '
        Me.ChooseISOBtn.Location = New System.Drawing.Point(28, 65)
        Me.ChooseISOBtn.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.ChooseISOBtn.Name = "ChooseISOBtn"
        Me.ChooseISOBtn.Size = New System.Drawing.Size(150, 27)
        Me.ChooseISOBtn.TabIndex = 2
        Me.ChooseISOBtn.Text = "Choose ISO file"
        Me.ChooseISOBtn.UseVisualStyleBackColor = True
        '
        'DiskInstrLbl
        '
        Me.DiskInstrLbl.AutoSize = True
        Me.DiskInstrLbl.BackColor = System.Drawing.SystemColors.Window
        Me.DiskInstrLbl.Location = New System.Drawing.Point(25, 40)
        Me.DiskInstrLbl.Margin = New System.Windows.Forms.Padding(3, 0, 3, 8)
        Me.DiskInstrLbl.Name = "DiskInstrLbl"
        Me.DiskInstrLbl.Size = New System.Drawing.Size(492, 17)
        Me.DiskInstrLbl.TabIndex = 1
        Me.DiskInstrLbl.Text = "Choose an install disk or disk image (ISO file) for your desired version of Windo" &
    "ws."
        '
        'TargetDiskPnl
        '
        Me.TargetDiskPnl.AnimationLength = 0
        Me.TargetDiskPnl.BorderColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(160, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.TargetDiskPnl.BorderMargin = New System.Windows.Forms.Padding(8)
        Me.TargetDiskPnl.BorderPadding = New System.Windows.Forms.Padding(2)
        Me.TargetDiskPnl.BorderThickness = CType(1, Short)
        Me.TargetDiskPnl.CanCollapse = True
        Me.TargetDiskPnl.Controls.Add(Me.TargetListView)
        Me.TargetDiskPnl.Controls.Add(Me.TargetInstrLbl)
        Me.TargetDiskPnl.CornerRadius = 6
        Me.TargetDiskPnl.Expanded = True
        Me.TargetDiskPnl.HeaderFont = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TargetDiskPnl.HeaderHeight = 32
        Me.TargetDiskPnl.HeaderText = "2: Target Disk"
        Me.TargetDiskPnl.InnerBackgroundColor = System.Drawing.SystemColors.Window
        Me.TargetDiskPnl.InnerBackgroundTexture = Nothing
        Me.TargetDiskPnl.Location = New System.Drawing.Point(3, 147)
        Me.TargetDiskPnl.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.TargetDiskPnl.Name = "TargetDiskPnl"
        Me.TargetDiskPnl.Padding = New System.Windows.Forms.Padding(22, 40, 20, 20)
        Me.TargetDiskPnl.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.TargetDiskPnl.ShadowEnabled = True
        Me.TargetDiskPnl.ShadowOffset = New System.Windows.Forms.Padding(-3, 4, 3, 7)
        Me.TargetDiskPnl.ShadowSoftness = 8.0!
        Me.TargetDiskPnl.Size = New System.Drawing.Size(569, 144)
        Me.TargetDiskPnl.TabIndex = 1
        Me.TargetDiskPnl.Text = "2: Target Disk"
        '
        'TargetListView
        '
        Me.TargetListView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TargetListView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TargetListView.FullRowSelect = True
        Me.TargetListView.HideSelection = False
        Me.TargetListView.Location = New System.Drawing.Point(28, 68)
        Me.TargetListView.Name = "TargetListView"
        Me.TargetListView.OwnerDraw = True
        Me.TargetListView.Size = New System.Drawing.Size(518, 53)
        Me.TargetListView.SmallImageList = Me.TargetIconList
        Me.TargetListView.TabIndex = 2
        Me.TargetListView.UseCompatibleStateImageBehavior = False
        Me.TargetListView.View = System.Windows.Forms.View.SmallIcon
        '
        'TargetIconList
        '
        Me.TargetIconList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.TargetIconList.ImageSize = New System.Drawing.Size(16, 16)
        Me.TargetIconList.TransparentColor = System.Drawing.Color.Transparent
        '
        'TargetInstrLbl
        '
        Me.TargetInstrLbl.AutoSize = True
        Me.TargetInstrLbl.BackColor = System.Drawing.SystemColors.Window
        Me.TargetInstrLbl.Location = New System.Drawing.Point(25, 40)
        Me.TargetInstrLbl.Margin = New System.Windows.Forms.Padding(3, 0, 3, 8)
        Me.TargetInstrLbl.Name = "TargetInstrLbl"
        Me.TargetInstrLbl.Size = New System.Drawing.Size(418, 17)
        Me.TargetInstrLbl.TabIndex = 1
        Me.TargetInstrLbl.Text = "Choose the disk to install Windows on. All existing files will be deleted."
        '
        'ProgressPnl
        '
        Me.ProgressPnl.AnimationLength = 0
        Me.ProgressPnl.BorderColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(160, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.ProgressPnl.BorderMargin = New System.Windows.Forms.Padding(8)
        Me.ProgressPnl.BorderPadding = New System.Windows.Forms.Padding(2)
        Me.ProgressPnl.BorderThickness = CType(1, Short)
        Me.ProgressPnl.CanCollapse = True
        Me.ProgressPnl.Controls.Add(Me.InstallBtn)
        Me.ProgressPnl.Controls.Add(Me.InstallPbar)
        Me.ProgressPnl.Controls.Add(Me.ProgressText1Lbl)
        Me.ProgressPnl.CornerRadius = 6
        Me.ProgressPnl.Expanded = True
        Me.ProgressPnl.HeaderFont = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ProgressPnl.HeaderHeight = 32
        Me.ProgressPnl.HeaderText = "3: Install"
        Me.ProgressPnl.InnerBackgroundColor = System.Drawing.SystemColors.Window
        Me.ProgressPnl.InnerBackgroundTexture = Nothing
        Me.ProgressPnl.Location = New System.Drawing.Point(3, 291)
        Me.ProgressPnl.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.ProgressPnl.Name = "ProgressPnl"
        Me.ProgressPnl.Padding = New System.Windows.Forms.Padding(22, 40, 20, 20)
        Me.ProgressPnl.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ProgressPnl.ShadowEnabled = True
        Me.ProgressPnl.ShadowOffset = New System.Windows.Forms.Padding(-3, 4, 3, 7)
        Me.ProgressPnl.ShadowSoftness = 8.0!
        Me.ProgressPnl.Size = New System.Drawing.Size(569, 122)
        Me.ProgressPnl.TabIndex = 2
        Me.ProgressPnl.Text = "3: Install"
        '
        'InstallBtn
        '
        Me.InstallBtn.Enabled = False
        Me.InstallBtn.Location = New System.Drawing.Point(23, 68)
        Me.InstallBtn.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.InstallBtn.Name = "InstallBtn"
        Me.InstallBtn.Size = New System.Drawing.Size(100, 27)
        Me.InstallBtn.TabIndex = 6
        Me.InstallBtn.Text = "Install"
        Me.InstallBtn.UseVisualStyleBackColor = True
        '
        'InstallPbar
        '
        Me.InstallPbar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InstallPbar.Location = New System.Drawing.Point(134, 68)
        Me.InstallPbar.Name = "InstallPbar"
        Me.InstallPbar.Size = New System.Drawing.Size(412, 27)
        Me.InstallPbar.TabIndex = 2
        '
        'ProgressText1Lbl
        '
        Me.ProgressText1Lbl.AutoSize = True
        Me.ProgressText1Lbl.BackColor = System.Drawing.SystemColors.Window
        Me.ProgressText1Lbl.Location = New System.Drawing.Point(25, 40)
        Me.ProgressText1Lbl.Margin = New System.Windows.Forms.Padding(3, 0, 3, 8)
        Me.ProgressText1Lbl.Name = "ProgressText1Lbl"
        Me.ProgressText1Lbl.Size = New System.Drawing.Size(376, 17)
        Me.ProgressText1Lbl.TabIndex = 1
        Me.ProgressText1Lbl.Text = "Press Install to begin installing Windows onto the selected disk."
        '
        'ResizeDebounceTmr
        '
        '
        'OpenISODlg
        '
        Me.OpenISODlg.Filter = "ISO files|*.iso"
        Me.OpenISODlg.Title = "Choose ISO File"
        '
        'OpenInstDiskDlg
        '
        Me.OpenInstDiskDlg.Description = "Choose an install disk or folder containing the contents of one."
        Me.OpenInstDiskDlg.RootFolder = System.Environment.SpecialFolder.MyComputer
        '
        'InstallWorker
        '
        Me.InstallWorker.WorkerReportsProgress = True
        Me.InstallWorker.WorkerSupportsCancellation = True
        '
        'BlockageToolTip
        '
        Me.BlockageToolTip.IsBalloon = True
        Me.BlockageToolTip.ShowAlways = True
        '
        'InstallHoverCheckTmr
        '
        Me.InstallHoverCheckTmr.Enabled = True
        '
        'OpenTmr
        '
        Me.OpenTmr.Enabled = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 462)
        Me.Controls.Add(Me.MainFlowPnl)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(600, 38)
        Me.Name = "MainForm"
        Me.Text = "WinIt"
        Me.MainFlowPnl.ResumeLayout(False)
        Me.InputFilePnl.ResumeLayout(False)
        Me.InputFilePnl.PerformLayout()
        Me.TargetDiskPnl.ResumeLayout(False)
        Me.TargetDiskPnl.PerformLayout()
        Me.ProgressPnl.ResumeLayout(False)
        Me.ProgressPnl.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents MainFlowPnl As FlowLayoutPanel
    Friend WithEvents InputFilePnl As UI.Panels.CollapsePanel
    Friend WithEvents DiskInstrLbl As Label
    Friend WithEvents ChooseDiskBtn As Button
    Friend WithEvents ChooseISOBtn As Button
    Friend WithEvents ImageSelLbl As Label
    Friend WithEvents ResizeDebounceTmr As Timer
    Friend WithEvents ChooseDLBtn As Button
    Friend WithEvents HelpToolTip As ToolTip
    Friend WithEvents OpenISODlg As OpenFileDialog
    Friend WithEvents OpenInstDiskDlg As FolderBrowserDialog
    Friend WithEvents TargetDiskPnl As UI.Panels.CollapsePanel
    Friend WithEvents TargetInstrLbl As Label
    Friend WithEvents TargetListView As ListViewEx
    Friend WithEvents TargetIconList As ImageList
    Friend WithEvents ProgressPnl As UI.Panels.CollapsePanel
    Friend WithEvents ProgressText1Lbl As Label
    Friend WithEvents InstallWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents InstallBtn As Button
    Friend WithEvents InstallPbar As ProgressBar
    Friend WithEvents BlockageToolTip As ToolTip
    Friend WithEvents InstallHoverCheckTmr As Timer
    Friend WithEvents OpenTmr As Timer
End Class

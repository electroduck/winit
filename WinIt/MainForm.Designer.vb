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
        Me.ResizeDebounceTmr = New System.Windows.Forms.Timer(Me.components)
        Me.HelpToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.OpenISODlg = New System.Windows.Forms.OpenFileDialog()
        Me.OpenInstDiskDlg = New System.Windows.Forms.FolderBrowserDialog()
        Me.MainFlowPnl.SuspendLayout()
        Me.InputFilePnl.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainFlowPnl
        '
        Me.MainFlowPnl.Controls.Add(Me.InputFilePnl)
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
        Me.ImageSelLbl.AutoSize = True
        Me.ImageSelLbl.BackColor = System.Drawing.SystemColors.Window
        Me.ImageSelLbl.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ImageSelLbl.Location = New System.Drawing.Point(25, 100)
        Me.ImageSelLbl.Margin = New System.Windows.Forms.Padding(3, 8, 3, 8)
        Me.ImageSelLbl.Name = "ImageSelLbl"
        Me.ImageSelLbl.Size = New System.Drawing.Size(148, 17)
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
        Me.Text = "MainForm"
        Me.MainFlowPnl.ResumeLayout(False)
        Me.InputFilePnl.ResumeLayout(False)
        Me.InputFilePnl.PerformLayout()
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
End Class

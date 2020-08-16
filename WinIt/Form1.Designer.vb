<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Me.CollapsePanel1 = New Electroduck.UI.Panels.CollapsePanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.CollapsePanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'CollapsePanel1
        '
        Me.CollapsePanel1.AnimationLength = 0
        Me.CollapsePanel1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(180, Byte), Integer))
        Me.CollapsePanel1.BorderMargin = New System.Windows.Forms.Padding(8)
        Me.CollapsePanel1.BorderPadding = New System.Windows.Forms.Padding(2)
        Me.CollapsePanel1.BorderThickness = CType(1, Short)
        Me.CollapsePanel1.CanCollapse = True
        Me.CollapsePanel1.Controls.Add(Me.Label1)
        Me.CollapsePanel1.Controls.Add(Me.Button1)
        Me.CollapsePanel1.CornerRadius = 8
        Me.CollapsePanel1.Expanded = True
        Me.CollapsePanel1.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CollapsePanel1.HeaderHeight = 32
        Me.CollapsePanel1.HeaderText = "Test Panel 1"
        Me.CollapsePanel1.InnerBackgroundColor = System.Drawing.Color.White
        Me.CollapsePanel1.InnerBackgroundTexture = Nothing
        Me.CollapsePanel1.Location = New System.Drawing.Point(117, 21)
        Me.CollapsePanel1.Name = "CollapsePanel1"
        Me.CollapsePanel1.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.CollapsePanel1.ShadowEnabled = True
        Me.CollapsePanel1.ShadowOffset = New System.Windows.Forms.Padding(-4, 4, 4, 7)
        Me.CollapsePanel1.ShadowSoftness = 8.0!
        Me.CollapsePanel1.Size = New System.Drawing.Size(498, 132)
        Me.CollapsePanel1.TabIndex = 0
        Me.CollapsePanel1.Text = "Test Panel 1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(143, 58)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Label1"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(240, 41)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(91, 30)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.CollapsePanel1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.CollapsePanel1.ResumeLayout(False)
        Me.CollapsePanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents CollapsePanel1 As UI.Panels.CollapsePanel
    Friend WithEvents Label1 As Label
    Friend WithEvents Button1 As Button
End Class

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
        Me.SuspendLayout()
        '
        'CollapsePanel1
        '
        Me.CollapsePanel1.AnimationLength = 0
        Me.CollapsePanel1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(180, Byte), Integer))
        Me.CollapsePanel1.BorderMargin = New System.Windows.Forms.Padding(2)
        Me.CollapsePanel1.BorderPadding = New System.Windows.Forms.Padding(2)
        Me.CollapsePanel1.BorderThickness = CType(1, Short)
        Me.CollapsePanel1.CanCollapse = True
        Me.CollapsePanel1.CornerRadius = 8
        Me.CollapsePanel1.Expanded = True
        Me.CollapsePanel1.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CollapsePanel1.HeaderHeight = 32
        Me.CollapsePanel1.InnerBackgroundColor = System.Drawing.Color.White
        Me.CollapsePanel1.InnerBackgroundTexture = Nothing
        Me.CollapsePanel1.Location = New System.Drawing.Point(160, 49)
        Me.CollapsePanel1.Name = "CollapsePanel1"
        Me.CollapsePanel1.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.CollapsePanel1.ShadowEnabled = False
        Me.CollapsePanel1.ShadowOffset = New System.Windows.Forms.Padding(4)
        Me.CollapsePanel1.ShadowSoftness = 0!
        Me.CollapsePanel1.Size = New System.Drawing.Size(207, 100)
        Me.CollapsePanel1.TabIndex = 0
        Me.CollapsePanel1.Text = "Panel Header"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.CollapsePanel1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents CollapsePanel1 As UI.Panels.CollapsePanel
End Class

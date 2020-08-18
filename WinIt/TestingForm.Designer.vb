<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TestingForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.LoadWIMBtn = New System.Windows.Forms.Button()
        Me.WIMDlg = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'LoadWIMBtn
        '
        Me.LoadWIMBtn.Location = New System.Drawing.Point(12, 12)
        Me.LoadWIMBtn.Name = "LoadWIMBtn"
        Me.LoadWIMBtn.Size = New System.Drawing.Size(75, 23)
        Me.LoadWIMBtn.TabIndex = 0
        Me.LoadWIMBtn.Text = "Load WIM"
        Me.LoadWIMBtn.UseVisualStyleBackColor = True
        '
        'WIMDlg
        '
        Me.WIMDlg.FileName = "OpenFileDialog1"
        Me.WIMDlg.Filter = "WIM files|*.wim"
        '
        'TestingForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(344, 202)
        Me.Controls.Add(Me.LoadWIMBtn)
        Me.Name = "TestingForm"
        Me.Text = "TestingForm"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LoadWIMBtn As Button
    Friend WithEvents WIMDlg As OpenFileDialog
End Class

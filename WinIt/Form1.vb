Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim ctlPanel As New UI.Panels.BorderedPanel With {
            .Location = New Point(100, 100),
            .Size = New Size(320, 240),
            .BorderMargin = New Padding(4.0F),
            .InnerBackground = New Drawing2D.LinearGradientBrush(New PointF(0, 0), New PointF(320, 240), Color.AntiqueWhite, Color.Transparent),
            .CornerRadius = 16,
            .BackColor = Color.Transparent,
            .BorderPen = New Pen(New Drawing2D.LinearGradientBrush(New PointF(0, 0), New PointF(320, 240), Color.Black, Color.Green), 1.0F)
        }

        ctlPanel.Controls.Add(New Label With {.Location = New Point(20, 20), .Text = "Test Label", .BackColor = Color.Transparent})
        Controls.Add(ctlPanel)
        ctlPanel.Show()

        BackgroundImage = SystemIcons.Asterisk.ToBitmap
    End Sub
End Class

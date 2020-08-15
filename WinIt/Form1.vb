Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim ctlPanel1 As New UI.Panels.BorderedPanel With {
            .Location = New Point(50, 100),
            .Size = New Size(320, 240),
            .BorderMargin = New Padding(4.0F),
            .InnerBackground = New Drawing2D.LinearGradientBrush(New PointF(0, 0), New PointF(320, 240), Color.AntiqueWhite, Color.Transparent),
            .CornerRadius = 16,
            .BackColor = Color.Transparent,
            .BorderPen = New Pen(New Drawing2D.LinearGradientBrush(New PointF(0, 0), New PointF(320, 240), Color.Black, Color.Green), 1.0F)
        }

        ctlPanel1.Controls.Add(New Label With {.Location = New Point(20, 20), .Text = "Test Label 1", .BackColor = Color.Transparent})
        Controls.Add(ctlPanel1)
        ctlPanel1.Show()

        Dim ctlPanel2 As New UI.Panels.BorderedPanel With {
            .Location = New Point(400, 100),
            .Size = New Size(320, 240),
            .BorderMargin = New Padding(16.0F),
            .InnerBackground = New Drawing2D.LinearGradientBrush(New PointF(0, 0), New PointF(320, 240), Color.AntiqueWhite, Color.Aquamarine),
            .CornerRadius = 16,
            .BackColor = Color.Transparent,
            .BorderPen = New Pen(New Drawing2D.LinearGradientBrush(New PointF(0, 0), New PointF(320, 240), Color.Black, Color.Red), 1.0F),
            .ShadowEnabled = True,
            .ShadowSoftness = 4,
            .ShadowColor = Color.Black,
            .ShadowOffset = New Padding(8.0F)
        }

        ctlPanel2.Controls.Add(New Label With {.Location = New Point(20, 20), .Text = "Test Label 2", .BackColor = Color.Transparent})
        Controls.Add(ctlPanel2)
        ctlPanel2.Show()

        'BackgroundImage = SystemIcons.Asterisk.ToBitmap
    End Sub
End Class

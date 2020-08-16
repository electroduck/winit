Imports System.Windows.Forms
Imports System.Drawing

Class CollapsePanelHeader
    Inherits Label

    Private Const HEADER_ARROW_SIZE As Integer = 8
    Private Const HEADER_ARROW_PADDING_HORIZ As Integer = 4

    Private Shared ReadOnly HEADER_FORMAT As New StringFormat With {
        .Alignment = StringAlignment.Near,
        .LineAlignment = StringAlignment.Center
    }

    Public Sub New()
        MyBase.New
        Height = 32
        Cursor = Cursors.Hand
        Text = "Panel Header"
        BackColor = Color.Transparent
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim brFore As Brush = DirectCast(Parent, CollapsePanel).HeaderBrush
        Dim rectText As RectangleF

        If DirectCast(Parent, CollapsePanel).CanCollapse Then
            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            If DirectCast(Parent, CollapsePanel).Expanded Then
                DrawDownArrow(e.Graphics, brFore, HEADER_ARROW_PADDING_HORIZ, Height \ 2 - HEADER_ARROW_SIZE \ 2 - 1,
                              HEADER_ARROW_SIZE, HEADER_ARROW_SIZE)
            Else
                DrawRightArrow(e.Graphics, brFore, HEADER_ARROW_PADDING_HORIZ, Height \ 2 - HEADER_ARROW_SIZE \ 2 - 1,
                               HEADER_ARROW_SIZE, HEADER_ARROW_SIZE)
            End If
            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.Default

            rectText = New RectangleF(HEADER_ARROW_PADDING_HORIZ * 2 + HEADER_ARROW_SIZE, 0,
                                      Width - (HEADER_ARROW_PADDING_HORIZ * 2 + HEADER_ARROW_SIZE), Height)
        Else
            rectText = New RectangleF(0, 0, Width, Height)
        End If

        e.Graphics.DrawString(Text, DirectCast(Parent, CollapsePanel).HeaderFont, brFore, rectText, HEADER_FORMAT)
    End Sub

    Private Sub DrawDownArrow(gfx As Graphics, br As Brush, x As Integer, y As Integer, w As Integer, h As Integer)
        gfx.FillPolygon(br, New Point() {New Point(x, y), New Point(x + w, y), New Point(x + w / 2, y + h)})
    End Sub

    Private Sub DrawRightArrow(gfx As Graphics, br As Brush, x As Integer, y As Integer, w As Integer, h As Integer)
        gfx.FillPolygon(br, New Point() {New Point(x, y), New Point(x, y + h), New Point(x + w, y + h / 2)})
    End Sub

End Class

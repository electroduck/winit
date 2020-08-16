Imports System.Windows.Forms
Imports System.Drawing

Class CollapsePanelHeader
    Inherits Label

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
        e.Graphics.DrawString(Text, Parent.Font, New SolidBrush(Parent.ForeColor), New RectangleF(0, 0, Width, Height), HEADER_FORMAT)
    End Sub

End Class

Imports System.Drawing
Imports System.Windows.Forms

Public Class CollapsePanel
    Inherits BorderedPanel

    Private Const HEADER_HORIZ_MARGIN As Integer = 16

    Private mExpandedHeight As Integer
    Private WithEvents mHeader As New CollapsePanelHeader

    Public Sub New()
        MyBase.New
        Controls.Add(mHeader)
    End Sub

    Private mExpanded As Boolean = True
    Public Property Expanded As Boolean
        Get
            Return mExpanded
        End Get
        Set(bValue As Boolean)
            If bValue And Not mExpanded Then
                Expand()
            ElseIf Not bValue And mExpanded Then
                Collapse()
            End If
        End Set
    End Property

    Public Property HeaderHeight As Integer
        Get
            Return mHeader.Height
        End Get
        Set(nValue As Integer)
            mHeader.Height = nValue
            Invalidate()
        End Set
    End Property

    Public Overrides Property Text As String
        Get
            Return mHeader.Text
        End Get
        Set(strValue As String)
            mHeader.Text = strValue
        End Set
    End Property

    Private mHeaderFont As Font
    Public Property HeaderFont As Font
        Get
            Return mHeaderFont
        End Get
        Set(value As Font)
            mHeaderFont = value
            mHeader.Invalidate()
        End Set
    End Property

    Private mHeaderBrush As Brush
    Public Property HeaderBrush As Brush
        Get
            Return mHeaderBrush
        End Get
        Set(value As Brush)
            mHeaderBrush = value
            mHeader.Invalidate()
        End Set
    End Property

    Public Sub Collapse()
        If Not mExpanded Then
            Throw New InvalidOperationException("Already collapsed")
        End If

        Height = mHeader.Height + BorderMargin.Top + BorderMargin.Bottom
        mExpanded = False
        Invalidate()
    End Sub

    Public Sub Expand()
        If mExpanded Then
            Throw New InvalidOperationException("Already expanded")
        End If

        Height = mExpandedHeight
        mExpanded = False
        Invalidate()
    End Sub

    Private Sub UpdateSizes()
        If mExpanded Then
            mExpandedHeight = Height
        Else
            mHeader.Height = Height - (BorderMargin.Top + BorderMargin.Bottom)
        End If

        mHeader.Location = New Point(BorderMargin.Left + HEADER_HORIZ_MARGIN, BorderMargin.Top)
        mHeader.Width = Width - (BorderMargin.Left + BorderMargin.Right + HEADER_HORIZ_MARGIN * 2)
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        UpdateSizes()
    End Sub

    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        UpdateSizes()
        mHeader.Width = Width - (BorderMargin.Left + BorderMargin.Right)
    End Sub

    Protected Overrides Sub OnBorderChanged()
        MyBase.OnBorderChanged()
        UpdateSizes()
    End Sub

    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
        MyBase.OnVisibleChanged(e)
        mHeader.Visible = Visible
    End Sub

End Class

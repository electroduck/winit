Imports System.Drawing
Imports System.Windows.Forms

Public Class CollapsePanel
    Inherits BorderedPanel

    Private Const HEADER_HORIZ_MARGIN As Integer = 16

    Private mExpandedHeight As Integer
    Private WithEvents mHeader As New CollapsePanelHeader
    Private mInAnimation As Boolean = False

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

    Public Property HeaderText As String
        Get
            Return mHeader.Text
        End Get
        Set(strValue As String)
            mHeader.Text = strValue
        End Set
    End Property

    Public Property AnimationLength As Integer = 0

    Private mHeaderFont As Font = SystemFonts.DefaultFont
    Public Property HeaderFont As Font
        Get
            Return mHeaderFont
        End Get
        Set(value As Font)
            mHeaderFont = value
            mHeader.Invalidate()
        End Set
    End Property

    Private mHeaderBrush As Brush = SystemBrushes.ControlText
    Public Property HeaderBrush As Brush
        Get
            Return mHeaderBrush
        End Get
        Set(value As Brush)
            mHeaderBrush = value
            mHeader.Invalidate()
        End Set
    End Property

    Public ReadOnly Property Animating As Boolean
        Get
            Return mInAnimation
        End Get
    End Property

    Private mCanCollapse As Boolean = True
    Public Property CanCollapse As Boolean
        Get
            Return mCanCollapse
        End Get
        Set(value As Boolean)
            mCanCollapse = value
            mHeader.Cursor = If(value, Cursors.Hand, Cursors.Arrow)
        End Set
    End Property

    Public Sub Collapse()
        If Not mExpanded Then
            Throw New InvalidOperationException("Already collapsed")
        End If

        For Each ctl As Control In Controls
            If ctl IsNot mHeader Then
                ctl.Visible = False
            End If
        Next

        mExpanded = False
        If AnimationLength > 0 Then
            AnimateHeightChange(mHeader.Height + BorderMargin.Top + BorderMargin.Bottom, AnimationLength)
        Else
            Height = mHeader.Height + BorderMargin.Top + BorderMargin.Bottom
        End If
    End Sub

    Public Sub Expand()
        If mExpanded Then
            Throw New InvalidOperationException("Already expanded")
        End If

        For Each ctl As Control In Controls
            If ctl IsNot mHeader Then
                ctl.Visible = True
            End If
        Next

        mExpanded = True
        If AnimationLength > 0 Then
            AnimateHeightChange(mExpandedHeight, AnimationLength)
        Else
            Height = mExpandedHeight
        End If
    End Sub

    Private Declare Ansi Function GetTickCount Lib "kernel32.dll" () As Integer

    Private Sub AnimateHeightChange(nTargetHeight As Integer, nLengthMS As Integer)
        If mInAnimation Then
            Throw New InvalidOperationException("Already animating")
        End If
        mInAnimation = True

        Dim fAnimPercent As Single = 0.0F
        Dim nOrigHeight As Integer = Height
        Dim nStartTime As Integer = GetTickCount
        Dim nHeightDiff As Integer = nTargetHeight - nOrigHeight

        While fAnimPercent < 1.0F
            fAnimPercent = (GetTickCount - nStartTime) / nLengthMS
            If fAnimPercent > 1.0F Then
                fAnimPercent = 1.0F
            End If

            Height = fAnimPercent * nHeightDiff + nOrigHeight
            Invalidate()
            Application.DoEvents() ' TODO: Use timer instead
        End While

        Height = nTargetHeight
        mInAnimation = False
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
        If Not mInAnimation Then
            UpdateSizes()
            mHeader.Width = Width - (BorderMargin.Left + BorderMargin.Right)
        End If
    End Sub

    Protected Overrides Sub OnBorderChanged()
        MyBase.OnBorderChanged()
        UpdateSizes()
    End Sub

    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
        MyBase.OnVisibleChanged(e)
        mHeader.Visible = Visible
    End Sub

    Private Sub Header_Click(objSender As Object, e As EventArgs) Handles mHeader.Click
        If mCanCollapse And Not mInAnimation Then
            Expanded = Not Expanded
        End If
    End Sub

End Class

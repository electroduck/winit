Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Electroduck.UI.Panels

Public Class BorderedPanel
    Inherits Panel
    Implements IBordered

    Private Const SUPERSAMPLING As Integer = 4

    Private mBackBuf1 As New Bitmap(1, 1)

    Public Sub New()
        MyBase.New()

        BorderPen = SystemPens.ActiveBorder
        CornerRadius = 0.0F
        InnerBackground = SystemBrushes.Window
        BorderMargin = New Padding(2.0F)
        BorderPadding = New Padding(2.0F)
    End Sub

    Public Property BorderPen As Pen Implements IBordered.BorderPen
    Public Property CornerRadius As Integer Implements IBordered.CornerRadius
    Public Property InnerBackground As Brush Implements IBordered.InnerBackground
    Public Property BorderMargin As Padding Implements IBordered.BorderMargin
    Public Property BorderPadding As Padding Implements IBordered.BorderPadding

    Public Sub RefreshBackground()
        If Size <> mBackBuf1.Size Then
            mBackBuf1.Dispose()
            mBackBuf1 = New Bitmap(Width, Height)
        End If

        ' We don't keep the supersampled buffer around because it uses a lot of memory
        Using bmBackBuf2 As New Bitmap(Width * SUPERSAMPLING, Height * SUPERSAMPLING)
            Using gfxBuf2 As Graphics = Graphics.FromImage(bmBackBuf2)
                gfxBuf2.Clear(Color.Transparent)
                gfxBuf2.ScaleTransform(SUPERSAMPLING, SUPERSAMPLING)

                Dim rectInner As New Rectangle With {
                    .X = BorderMargin.Left,
                    .Y = BorderMargin.Top,
                    .Width = mBackBuf1.Width - (BorderMargin.Left + BorderMargin.Right),
                    .Height = mBackBuf1.Height - (BorderMargin.Top + BorderMargin.Bottom)
                }

                If CornerRadius = 0.0F Then
                    gfxBuf2.FillRectangle(InnerBackground, rectInner)
                    gfxBuf2.DrawRectangle(BorderPen, rectInner)
                Else
                    gfxBuf2.FillRectangle(InnerBackground, rectInner.Left + CornerRadius, rectInner.Top,
                        rectInner.Width - (CornerRadius * 2), rectInner.Height)
                    gfxBuf2.FillRectangle(InnerBackground, rectInner.Left, rectInner.Top + CornerRadius,
                        CornerRadius, rectInner.Height - (CornerRadius * 2))
                    gfxBuf2.FillRectangle(InnerBackground, rectInner.Right - CornerRadius, rectInner.Top + CornerRadius,
                        CornerRadius, rectInner.Height - (CornerRadius * 2))

                    gfxBuf2.FillPie(InnerBackground, rectInner.Left, rectInner.Top,
                                   CornerRadius * 2, CornerRadius * 2, 180.0F, 90.0F)
                    gfxBuf2.FillPie(InnerBackground, rectInner.Right - (CornerRadius * 2), rectInner.Top,
                                   CornerRadius * 2, CornerRadius * 2, 270.0F, 90.0F)
                    gfxBuf2.FillPie(InnerBackground, rectInner.Left, rectInner.Bottom - (CornerRadius * 2),
                                   CornerRadius * 2, CornerRadius * 2, 90.0F, 90.0F)
                    gfxBuf2.FillPie(InnerBackground, rectInner.Right - (CornerRadius * 2), rectInner.Bottom - (CornerRadius * 2),
                                   CornerRadius * 2, CornerRadius * 2, 0.0F, 90.0F)

                    gfxBuf2.DrawLine(BorderPen, rectInner.Left + CornerRadius - 1, rectInner.Top,
                        rectInner.Right - CornerRadius + 1, rectInner.Top)
                    gfxBuf2.DrawLine(BorderPen, rectInner.Left, rectInner.Top + CornerRadius - 1,
                        rectInner.Left, rectInner.Bottom - CornerRadius + 1)
                    gfxBuf2.DrawLine(BorderPen, rectInner.Right, rectInner.Top + CornerRadius - 1,
                        rectInner.Right, rectInner.Bottom - CornerRadius + 1)
                    gfxBuf2.DrawLine(BorderPen, rectInner.Left + CornerRadius - 1, rectInner.Bottom,
                        rectInner.Right - CornerRadius + 1, rectInner.Bottom)

                    gfxBuf2.DrawArc(BorderPen, rectInner.Left, rectInner.Top,
                                   CornerRadius * 2, CornerRadius * 2, 180.0F, 90.0F)
                    gfxBuf2.DrawArc(BorderPen, rectInner.Right - (CornerRadius * 2), rectInner.Top,
                                   CornerRadius * 2, CornerRadius * 2, 270.0F, 90.0F)
                    gfxBuf2.DrawArc(BorderPen, rectInner.Left, rectInner.Bottom - (CornerRadius * 2),
                                   CornerRadius * 2, CornerRadius * 2, 90.0F, 90.0F)
                    gfxBuf2.DrawArc(BorderPen, rectInner.Right - (CornerRadius * 2), rectInner.Bottom - (CornerRadius * 2),
                                   CornerRadius * 2, CornerRadius * 2, 0.0F, 90.0F)
                End If
            End Using

            Using gfxBuf1 As Graphics = Graphics.FromImage(mBackBuf1)
                Dim argsBase As New PaintEventArgs(gfxBuf1, New Rectangle(0, 0, mBackBuf1.Width, mBackBuf1.Height))
                MyBase.OnPaintBackground(argsBase)

                gfxBuf1.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
                gfxBuf1.DrawImage(bmBackBuf2, 0, 0, mBackBuf1.Width, mBackBuf1.Height)
            End Using
        End Using

        Invalidate()
    End Sub

    Protected Overrides Sub OnPaintBackground(args As PaintEventArgs)
        Using gfxScreen As Graphics = args.Graphics
            gfxScreen.DrawImageUnscaled(mBackBuf1, 0, 0)
        End Using
    End Sub

    Protected Overrides Sub OnClientSizeChanged(e As EventArgs)
        MyBase.OnClientSizeChanged(e)
        RefreshBackground()
    End Sub

    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
        If Visible Then
            RefreshBackground()
        End If
    End Sub

    Protected Overrides Sub OnBackColorChanged(e As EventArgs)
        MyBase.OnBackColorChanged(e)
        RefreshBackground()
    End Sub

    Protected Overrides Sub OnBackgroundImageChanged(e As EventArgs)
        MyBase.OnBackgroundImageChanged(e)
        RefreshBackground()
    End Sub

    Protected Overrides Sub OnBackgroundImageLayoutChanged(e As EventArgs)
        MyBase.OnBackgroundImageLayoutChanged(e)
        RefreshBackground()
    End Sub

    Protected Overrides Sub OnParentBackColorChanged(e As EventArgs)
        MyBase.OnParentBackColorChanged(e)
        RefreshBackground()
    End Sub

    Protected Overrides Sub OnParentBackgroundImageChanged(e As EventArgs)
        MyBase.OnParentBackgroundImageChanged(e)
        RefreshBackground()
    End Sub

    Protected Overrides Sub OnLocationChanged(e As EventArgs)
        MyBase.OnLocationChanged(e)
        RefreshBackground()
    End Sub
End Class

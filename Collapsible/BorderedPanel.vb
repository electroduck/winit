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
        ShadowEnabled = False
        ShadowOffset = New Padding(4)
        ShadowColor = Color.FromArgb(192, Color.Black)
        ShadowSoftness = 0
    End Sub

    Public Property BorderPen As Pen Implements IBordered.BorderPen
    Public Property CornerRadius As Integer Implements IBordered.CornerRadius
    Public Property InnerBackground As Brush Implements IBordered.InnerBackground
    Public Property BorderMargin As Padding Implements IBordered.BorderMargin
    Public Property BorderPadding As Padding Implements IBordered.BorderPadding
    Public Property ShadowEnabled As Boolean Implements IBordered.ShadowEnabled
    Public Property ShadowOffset As Padding Implements IBordered.ShadowOffset
    Public Property ShadowColor As Color Implements IBordered.ShadowColor
    Public Property ShadowSoftness As Single Implements IBordered.ShadowSoftness

    Public Sub RefreshBackground()
        If Size <> mBackBuf1.Size Then
            mBackBuf1.Dispose()
            mBackBuf1 = New Bitmap(Width, Height)
        End If

        ' We don't keep the supersampled buffer around because it uses a lot of memory
        Using bmBackBuf2 As New Bitmap(Width * SUPERSAMPLING, Height * SUPERSAMPLING)
            Using gfxBuf2 As Graphics = Graphics.FromImage(bmBackBuf2)
                gfxBuf2.Clear(Color.Transparent)
                gfxBuf2.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
                gfxBuf2.ScaleTransform(SUPERSAMPLING, SUPERSAMPLING)

                Dim rectInner As New Rectangle With {
                    .X = BorderMargin.Left,
                    .Y = BorderMargin.Top,
                    .Width = mBackBuf1.Width - (BorderMargin.Left + BorderMargin.Right),
                    .Height = mBackBuf1.Height - (BorderMargin.Top + BorderMargin.Bottom)
                }

                Dim rectShadow As New Rectangle With {
                    .X = rectInner.Left + ShadowOffset.Left,
                    .Y = rectInner.Top + ShadowOffset.Top,
                    .Width = rectInner.Width + ShadowOffset.Right - ShadowOffset.Left,
                    .Height = rectInner.Height + ShadowOffset.Bottom - ShadowOffset.Top
                }

                If CornerRadius = 0 Then
                    If ShadowEnabled Then
                        DrawSoftShadow(gfxBuf2, rectShadow, ShadowSoftness,
                            Sub(gfxShadow As Graphics)
                                gfxShadow.FillRectangle(New SolidBrush(ShadowColor), 0, 0, rectShadow.Width, rectShadow.Height)
                            End Sub)
                    End If

                    gfxBuf2.FillRectangle(InnerBackground, rectInner)
                    gfxBuf2.DrawRectangle(BorderPen, rectInner)
                Else
                    If ShadowEnabled Then
                        DrawSoftShadow(gfxBuf2, rectShadow, ShadowSoftness,
                            Sub(gfxShadow As Graphics)
                                FillRoundedRectangle(gfxShadow, New SolidBrush(ShadowColor),
                                                     New Rectangle(0, 0, rectShadow.Width, rectShadow.Height), CornerRadius)
                            End Sub, True)
                    End If

                    FillRoundedRectangle(gfxBuf2, InnerBackground, rectInner, CornerRadius)
                    DrawRoundedRectangle(gfxBuf2, BorderPen, rectInner, CornerRadius)
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
        GC.Collect()
    End Sub

    Private Shared Sub FillRoundedRectangle(gfx As Graphics, brushFill As Brush, rect As Rectangle, nCornerRadius As Integer)
        gfx.FillRectangle(brushFill, rect.Left + nCornerRadius, rect.Top,
            rect.Width - (nCornerRadius * 2), rect.Height)
        gfx.FillRectangle(brushFill, rect.Left, rect.Top + nCornerRadius,
            nCornerRadius, rect.Height - (nCornerRadius * 2))
        gfx.FillRectangle(brushFill, rect.Right - nCornerRadius, rect.Top + nCornerRadius,
            nCornerRadius, rect.Height - (nCornerRadius * 2))

        gfx.FillPie(brushFill, rect.Left, rect.Top,
                       nCornerRadius * 2, nCornerRadius * 2, 180.0F, 90.0F)
        gfx.FillPie(brushFill, rect.Right - (nCornerRadius * 2), rect.Top,
                       nCornerRadius * 2, nCornerRadius * 2, 270.0F, 90.0F)
        gfx.FillPie(brushFill, rect.Left, rect.Bottom - (nCornerRadius * 2),
                       nCornerRadius * 2, nCornerRadius * 2, 90.0F, 90.0F)
        gfx.FillPie(brushFill, rect.Right - (nCornerRadius * 2), rect.Bottom - (nCornerRadius * 2),
                       nCornerRadius * 2, nCornerRadius * 2, 0.0F, 90.0F)
    End Sub

    Private Shared Sub DrawRoundedRectangle(gfx As Graphics, penOutline As Pen, rect As Rectangle, nCorderRadius As Integer)
        gfx.DrawLine(penOutline, rect.Left + nCorderRadius - 1, rect.Top,
            rect.Right - nCorderRadius + 1, rect.Top)
        gfx.DrawLine(penOutline, rect.Left, rect.Top + nCorderRadius - 1,
            rect.Left, rect.Bottom - nCorderRadius + 1)
        gfx.DrawLine(penOutline, rect.Right, rect.Top + nCorderRadius - 1,
            rect.Right, rect.Bottom - nCorderRadius + 1)
        gfx.DrawLine(penOutline, rect.Left + nCorderRadius - 1, rect.Bottom,
            rect.Right - nCorderRadius + 1, rect.Bottom)

        gfx.DrawArc(penOutline, rect.Left, rect.Top,
                       nCorderRadius * 2, nCorderRadius * 2, 180.0F, 90.0F)
        gfx.DrawArc(penOutline, rect.Right - (nCorderRadius * 2), rect.Top,
                       nCorderRadius * 2, nCorderRadius * 2, 270.0F, 90.0F)
        gfx.DrawArc(penOutline, rect.Left, rect.Bottom - (nCorderRadius * 2),
                       nCorderRadius * 2, nCorderRadius * 2, 90.0F, 90.0F)
        gfx.DrawArc(penOutline, rect.Right - (nCorderRadius * 2), rect.Bottom - (nCorderRadius * 2),
                       nCorderRadius * 2, nCorderRadius * 2, 0.0F, 90.0F)
    End Sub

    Private Delegate Sub DrawShadowDelegate(gfxShadow As Graphics)

    Private Shared Sub DrawSoftShadow(gfx As Graphics, rectShadow As Rectangle, nSoftness As Single, procDrawShadow As DrawShadowDelegate,
                                      Optional bHighQuality As Boolean = False)
        If nSoftness = 0 Then
            procDrawShadow(gfx)
        ElseIf bHighQuality Then
            nSoftness /= 2
            Using bmShadowBuf As New Bitmap(rectShadow.Width, rectShadow.Height)
                Using gfxShadowBuf As Graphics = Graphics.FromImage(bmShadowBuf)
                    gfxShadowBuf.ScaleTransform((rectShadow.Width - nSoftness * 2) / rectShadow.Width,
                                                (rectShadow.Height - nSoftness * 2) / rectShadow.Height)
                    gfxShadowBuf.TranslateTransform(nSoftness, nSoftness)
                    procDrawShadow(gfxShadowBuf)
                End Using

                Using bmShadowSoft As Bitmap = Internal.ImageFX.BlurMethodB(bmShadowBuf, nSoftness)
                    gfx.DrawImageUnscaled(bmShadowSoft, rectShadow.Left, rectShadow.Top)
                End Using
            End Using
        Else
            Using bmShadowBuf As New Bitmap(CInt(rectShadow.Width / nSoftness) + 1,
                                            CInt(rectShadow.Height / nSoftness) + 1)
                Using gfxShadowBuf As Graphics = Graphics.FromImage(bmShadowBuf)
                    gfxShadowBuf.ScaleTransform(1 / nSoftness, 1 / nSoftness)
                    gfxShadowBuf.ScaleTransform((rectShadow.Width - 1) / rectShadow.Width, (rectShadow.Height - 1) / rectShadow.Height)
                    gfxShadowBuf.TranslateTransform(1, 1)
                    procDrawShadow(gfxShadowBuf)
                End Using

                gfx.DrawImage(bmShadowBuf, rectShadow.X - nSoftness, rectShadow.Y - nSoftness,
                    rectShadow.Width + (nSoftness * 2), rectShadow.Height + (nSoftness * 2))
            End Using
        End If
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

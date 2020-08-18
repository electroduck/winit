﻿Public Class ListViewEx
    Inherits ListView

    Public Property EnabledColor As Color
    Public Property DisabledColor As Color

    Protected Overrides Sub OnEnabledChanged(e As EventArgs)
        MyBase.OnEnabledChanged(e)
        BackColor = If(Enabled, EnabledColor, DisabledColor)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case &HA 'WM_ENABLE
                ' Prevent the message from reaching the control,
                ' so the colors don't get changed by the default procedure.
                Exit Sub ' <-- suppress WM_ENABLE message
            Case Else
                MyBase.WndProc(m)
                Exit Select
        End Select
    End Sub
End Class

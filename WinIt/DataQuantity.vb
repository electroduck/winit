Public Class DataQuantity
    Public Property Bytes As ULong

    Public Property Kilobytes As ULong
        Get
            Return Bytes \ 1024
        End Get
        Set(value As ULong)
            Bytes = value * 1024
        End Set
    End Property

    Public Property Megabytes As Double
        Get
            Return Bytes / Math.Pow(1024, 2)
        End Get
        Set(value As Double)
            Bytes = value * Math.Pow(1024, 2)
        End Set
    End Property

    Public Property Gigabytes As Double
        Get
            Return Bytes / Math.Pow(1024, 3)
        End Get
        Set(value As Double)
            Bytes = value * Math.Pow(1024, 3)
        End Set
    End Property

    Public Property Terabytes As Double
        Get
            Return Bytes / Math.Pow(1024, 4)
        End Get
        Set(value As Double)
            Bytes = value * Math.Pow(1024, 4)
        End Set
    End Property

    Public Sub New()
        Bytes = 0
    End Sub

    Public Sub New(nBytes As ULong)
        Bytes = nBytes
    End Sub

    Public Overrides Function ToString() As String
        Return FormatNicely()
    End Function

    Private Shared ReadOnly DQ_PREFIXES() As String = {
        "bytes", "KB", "MB", "GB", "TB", "PB"
    }

    Private Function FormatNicely() As String
        Dim fCurVal As Double = Bytes
        Dim nPrefix As Integer = 0

        Do
            If fCurVal < 1024.0 Or nPrefix = DQ_PREFIXES.Length - 1 Then
                Return fCurVal.ToString("N1") & " " & DQ_PREFIXES(nPrefix)
            Else
                fCurVal /= 1024.0
                nPrefix += 1
            End If
        Loop
    End Function

    Public Shared Widening Operator CType(dq As DataQuantity) As String
        Return dq.ToString
    End Operator

    Public Shared Widening Operator CType(dq As DataQuantity) As ULong
        Return dq.Bytes
    End Operator

    Public Shared Narrowing Operator CType(dq As DataQuantity) As Double
        Return dq.Bytes
    End Operator

    Public Shared Narrowing Operator CType(dq As DataQuantity) As Single
        Return dq.Bytes
    End Operator

    Public Shared Narrowing Operator CType(dq As DataQuantity) As Integer
        Return dq.Bytes And &HFFFF_FFFF
    End Operator

    Public Shared Function FromKilobytes(nKB As Long) As DataQuantity
        Return New DataQuantity(nKB * 1024)
    End Function

    Public Shared Function FromMegabytes(nMB As Long) As DataQuantity
        Return New DataQuantity(nMB * 1024 * 1024)
    End Function

    Public Shared Function FromGigabytes(nGB As Long) As DataQuantity
        Return New DataQuantity(nGB * 1024 * 1024 * 1024)
    End Function

    Public Shared Function FromTerabytes(nTB As Long) As DataQuantity
        Return New DataQuantity(nTB * 1024 * 1024 * 1024 * 1024)
    End Function
End Class

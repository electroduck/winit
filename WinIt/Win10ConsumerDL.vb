Public Class Win10ConsumerDL
    Inherits ISODownloadHelper

    Private mIs64Bit As Boolean

    Public Overrides ReadOnly Property DownloadName As String
        Get
            Return "Windows 10 Home/Pro (" & If(mIs64Bit, "64", "32") & "-bit)"
        End Get
    End Property

    Public Overrides ReadOnly Property DownloadFileName As String
        Get
            Return "Win10_Consumer_" & If(mIs64Bit, "64", "32") & "bit.iso"
        End Get
    End Property

    Public Sub New(b64Bit As Boolean)
        mIs64Bit = b64Bit
    End Sub

    Public Overrides Function GetDownloadURL() As String
        Dim client As New Net.WebClient
        Dim idSession As Guid = Guid.NewGuid

        client.Headers.Set(Net.HttpRequestHeader.UserAgent, "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_6; en-en) " &
                           "AppleWebKit/533.19.4 (KHTML, like Gecko) Version/5.0.3 Safari/533.19.4")

        Dim uriReq1 As New Uri(String.Format("https://www.microsoft.com/en-us/api/controls/contentinclude/html?" &
            "pageId=a8f8f489-4c7f-463a-9ca6-5cff94d8d041&" &
            "host=www.microsoft.com&segments=software-download%2cwindows10ISO&query=&action=getskuinformationbyproductedition&" &
            "sessionId={0}&productEditionId=1626&sdVersion=2", idSession.ToString("D")))
        Dim strResp1 As String = client.UploadString(uriReq1, "controlAttributeMapping=")

        Dim uriReq2 As New Uri(String.Format("https://www.microsoft.com/en-us/api/controls/contentinclude/html?" &
            "pageId=a224afab-2097-4dfa-a2ba-463eb191a285&" &
            "host=www.microsoft.com&segments=software-download%2cwindows10ISO&query=&action=GetProductDownloadLinksBySku&" &
            "sessionId={0}&skuId=9959&language=English&sdVersion=2", idSession.ToString("D")))
        Dim strResp2 As String = client.UploadString(uriReq2, "controlAttributeMapping=")

        Dim nURLStart As Integer = strResp2.IndexOf("https://software-download.microsoft.com/")
        Dim nURLEnd As Integer = strResp2.IndexOf(""""c, nURLStart + 1)

        Return strResp2.Substring(nURLStart, nURLEnd - nURLStart).Replace("&amp;", "&")
    End Function
End Class

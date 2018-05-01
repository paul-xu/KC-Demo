Imports System.IO
Imports System.Net
Imports System.Text

Public Class HttpUtils

    Public Shared Function GetData(ByVal url As String, ByVal data As String) As String
        Return GetData(url, data, Nothing)
    End Function

    Public Shared Function GetData(ByVal url As String, ByVal data As String, ByRef headers As Dictionary(Of String, String)) As String

        Dim request As HttpWebRequest = WebRequest.Create(url + "?" + data)
        request.Method = "GET"

        If Not (IsNothing(headers)) Then
            For Each header In headers
                request.Headers.Add(header.Key, header.Value)
            Next
        End If

        Dim sr As StreamReader = New StreamReader(request.GetResponse().GetResponseStream)
        Return sr.ReadToEnd
    End Function

    Public Shared Function PostData(ByVal url As String, ByVal data As String) As String

        ServicePointManager.Expect100Continue = False
        Dim request As HttpWebRequest = WebRequest.Create(url)
        '//Post请求方式  
        request.Method = "POST"

        '内容类型  
        request.ContentType = "application/x-www-form-urlencoded"
        '将URL编码后的字符串转化为字节  
        Dim encoding As New UTF8Encoding()
        Dim bys As Byte() = encoding.GetBytes(data)
        '设置请求的 ContentLength   
        request.ContentLength = bys.Length
        '获得请 求流  
        Dim newStream As Stream = request.GetRequestStream()
        newStream.Write(bys, 0, bys.Length)
        newStream.Close()
        '获得响应流  
        Dim sr As StreamReader = New StreamReader(request.GetResponse().GetResponseStream)
        Return sr.ReadToEnd
    End Function

    Public Shared Function GenerateSign(ByVal accessToken As String, ByVal nonce As String, ByVal timestamp As Long, ByVal userCode As String)
        'sign的生成方法：[access_token=<access_token>&nonce=<nonce>&timestamp=<timestamp>&userCode=<userCode><appSecret>].MD5
        'appSecret -服务端和客户端协商定义一个密钥， 类型为字符串， 值为12345

        Dim signBuilder As New StringBuilder
        signBuilder.Append("access_token=" & accessToken)
        signBuilder.Append("&nonce=" & nonce)
        signBuilder.Append("&timestamp=" & timestamp.ToString)
        signBuilder.Append("&userCode=" & userCode)
        signBuilder.Append("12345")

        Return MD5.MD5(signBuilder.ToString, 32)
    End Function
End Class

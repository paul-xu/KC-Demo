Imports System.Text
Imports Newtonsoft.Json.Linq

Public Class MainForm
    Private _loginForm As New LoginForm
    Private _userInfo As UserInfo
    Private _server As String
    Private _port As String

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _loginForm.ShowDialog() = DialogResult.OK Then
            Me._userInfo = _loginForm.UserInfo
            Me._server = _loginForm.Server
            Me._port = _loginForm.Port

            Me.LoadDevices(_userInfo)
        Else
            Me.Close()
        End If
    End Sub

    '获取当前登录用户的设备列表
    Private Sub LoadDevices(userInfo As UserInfo)
        Dim urlBuilder As New StringBuilder
        urlBuilder.Append("HTTP://" & _server & ":" & _port)
        urlBuilder.Append("/smarthome.IMCPlatform/device/v1.0/fetchDevices.action")

        Dim requestHeaders As New Dictionary(Of String, String)
        requestHeaders.Add("nonce", "ABCDEF")
        requestHeaders.Add("access_token", userInfo.AccessToken)
        requestHeaders.Add("userCode", userInfo.UserCode)
        Dim milliseconds = CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds)
        requestHeaders.Add("timestamp", milliseconds.ToString)

        Dim sign = HttpUtils.GenerateSign(userInfo.AccessToken, "ABCDEF", milliseconds, userInfo.UserCode)
        requestHeaders.Add("sign", sign)

        Dim strResponse = HttpUtils.GetData(urlBuilder.ToString(), "", requestHeaders)
        Dim jsonResponse = JObject.Parse(strResponse)
        Dim status = jsonResponse("status").ToObject(Of String)

        If status.Equals("0") Then
            Dim deviceResults = jsonResponse("result").Children().ToList()

            For Each result In deviceResults
                Dim device = result.ToObject(Of Device)()
                Dim item As New ListViewItem
                item.Text = device.DeviceAlias
                item.ImageIndex = 0
                item.Tag = device

                lvDevice.Items.Add(item)
            Next
        End If
    End Sub

    Private Sub lvDevice_DoubleClick(sender As Object, e As EventArgs) Handles lvDevice.DoubleClick
        Dim listItem = lvDevice.SelectedItems.Item(0)
        Dim device = CType(listItem.Tag, Device)

        Dim deviceForm As New DeviceForm(device, _userInfo, _server, _port)
        deviceForm.ShowDialog()
    End Sub
End Class

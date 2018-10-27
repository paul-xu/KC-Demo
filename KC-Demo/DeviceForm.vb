Imports System.Text

Public Class DeviceForm
    Private _device As Device
    Private _userInfo As UserInfo
    Private _server As String
    Private _port As String

    Public Sub New(ByVal device As Device, ByVal userInfo As UserInfo, ByVal server As String, ByVal port As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me._device = device
        Me._userInfo = userInfo
        Me._server = server
        Me._port = port
        Me.Width = _device.Operations.Count * 120
    End Sub

    Private Sub DeviceForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim nTop As Long = 20, nLeft As Long = 20

        Select Case _device.DeviceType
            Case "ZB-ST"
                Dim trackBar As New TrackBar
                With trackBar
                    .Left += nLeft
                    .Top = nTop
                    .Width = 360
                    .Visible = True
                    .Maximum = 100
                    .Minimum = 0
                    .Value = 20
                    .TickFrequency = 10

                    AddHandler .Scroll, AddressOf Tracker_Scroll
                End With

                Me.Width = 410
                Me.Controls.Add(trackBar)
            Case Else
                For Each operation In _device.Operations
                    Dim button As New Button
                    With button
                        .Left += nLeft
                        .Height = 30
                        .Top = nTop
                        .Visible = True
                        .Text = operation.Name
                        .Tag = operation.Code
                        nLeft += .Width + 30

                        AddHandler .Click, AddressOf OperationButton_Click
                    End With

                    Me.Controls.Add(button)
                Next
        End Select
    End Sub

    Private Sub OperationButton_Click(sender As Object, e As EventArgs)
        Dim urlBuilder As New StringBuilder
        urlBuilder.Append("HTTP://" & _server & ":" & _port)
        urlBuilder.Append("/smarthome.IMCPlatform/device/v1.0/control.action")

        Dim requestHeaders As New Dictionary(Of String, String)
        requestHeaders.Add("nonce", "ABCDEF")
        requestHeaders.Add("access_token", _userInfo.AccessToken)
        requestHeaders.Add("userCode", _userInfo.UserCode)
        Dim milliseconds = CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds)
        requestHeaders.Add("timestamp", milliseconds.ToString)

        Dim sign = HttpUtils.GenerateSign(_userInfo.AccessToken, "ABCDEF", milliseconds, _userInfo.UserCode)
        requestHeaders.Add("sign", sign)

        Dim dataBuilder As New StringBuilder
        dataBuilder.Append("hostCode=" & _device.HostCode)
        dataBuilder.Append("&addressCode=")
        dataBuilder.Append(_device.DeviceAddress)
        dataBuilder.Append("&state=" & CType(sender, Button).Tag.ToString())

        HttpUtils.GetData(urlBuilder.ToString, dataBuilder.ToString, requestHeaders)
    End Sub

    Private Sub Tracker_Scroll(sender As Object, e As EventArgs)
        Dim urlBuilder As New StringBuilder
        urlBuilder.Append("HTTP://" & _server & ":" & _port)
        urlBuilder.Append("/smarthome.IMCPlatform/device/v1.0/control.action")

        Dim requestHeaders As New Dictionary(Of String, String)
        requestHeaders.Add("nonce", "ABCDEF")
        requestHeaders.Add("access_token", _userInfo.AccessToken)
        requestHeaders.Add("userCode", _userInfo.UserCode)
        Dim milliseconds = CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds)
        requestHeaders.Add("timestamp", milliseconds.ToString)

        Dim sign = HttpUtils.GenerateSign(_userInfo.AccessToken, "ABCDEF", milliseconds, _userInfo.UserCode)
        requestHeaders.Add("sign", sign)

        Dim dataBuilder As New StringBuilder
        dataBuilder.Append("hostCode=" & _device.HostCode)
        dataBuilder.Append("&addressCode=")
        dataBuilder.Append(_device.DeviceAddress)
        dataBuilder.Append("&state=" & CType(sender, TrackBar).Value)

        HttpUtils.GetData(urlBuilder.ToString, dataBuilder.ToString, requestHeaders)
    End Sub
End Class
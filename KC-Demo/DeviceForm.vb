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
    End Sub

    Private Sub DeviceForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim nTop As Long = 20, nLeft As Long = 20

        Select Case _device.DeviceType
            Case DeviceType.RF_2262_315_33_CT, DeviceType.ZB_CT, DeviceType.ZB_DM
                Dim trackBar As New TrackBar
                With trackBar
                    .Left += nLeft
                    .Top = nTop
                    .Width = 360
                    .Visible = True
                    .Maximum = 100
                    .Minimum = 0
                    .TickFrequency = 10

                    AddHandler .Scroll, AddressOf Tracker_Scroll
                End With

                Me.Width = 410
                Me.Controls.Add(trackBar)
            Case Else
                Dim _iLoop As Integer = 0
                While _iLoop < _device.Operations.Count
                    Dim operation As DeviceOperation = _device.Operations(_iLoop)
                    Dim button As New Button
                    With button
                        .Left += nLeft
                        .Width = 80
                        .Height = 30
                        .Top = nTop
                        .Visible = True
                        .Text = operation.Name
                        .Tag = operation.Code
                        .Font = New Font("Microsoft YaHei", 9, FontStyle.Regular)

                        AddHandler .Click, AddressOf OperationButton_Click
                    End With

                    Me.Controls.Add(button)

                    _iLoop = _iLoop + 1
                    If _iLoop Mod 3 = 0 Then
                        nTop = nTop + 50
                        nLeft = 20
                    Else
                        nLeft = nLeft + 100
                    End If
                End While

                Me.Width = 340
                If _device.Operations.Count Mod 3 = 0 Then
                    Me.Height = _device.Operations.Count \ 3 * 50 + 60
                Else
                    Me.Height = (_device.Operations.Count \ 3 + 1) * 50 + 60
                End If
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
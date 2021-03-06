﻿Imports System.Text
Imports Newtonsoft.Json.Linq

Public Class DeviceForm
    Private Delegate Sub DelBindControls(obj As Object)

    Private _device As Device
    Private _userInfo As UserInfo
    Private _server As String
    Private _port As String
    Private _enclosureSwitches As List(Of EnclosureSwitch)

    Public Sub New(ByVal device As Device, ByVal userInfo As UserInfo, ByVal server As String, ByVal port As String)
        Me.New(device, userInfo, server, port, Nothing)
    End Sub

    Public Sub New(ByVal device As Device, ByVal userInfo As UserInfo, ByVal server As String, ByVal port As String, ByVal enclosureSwitches As List(Of EnclosureSwitch))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me._device = device
        Me._userInfo = userInfo
        Me._server = server
        Me._port = port
        Me._enclosureSwitches = enclosureSwitches
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
            Case DeviceType.CTL_ENCLOSURE_8, DeviceType.CTL_ENCLOSURE_32
                Me.Width = IIf(_enclosureSwitches.Count > 8, 620, 340)
                Dim NumOfSwitchesInRow = IIf(_enclosureSwitches.Count > 8, 8, 4)

                RenderEnclosureLayout(NumOfSwitchesInRow)

                If _enclosureSwitches.Count Mod NumOfSwitchesInRow = 0 Then
                    Me.Height = (_enclosureSwitches.Count \ NumOfSwitchesInRow) * 80 + 60
                Else
                    Me.Height = (_enclosureSwitches.Count \ NumOfSwitchesInRow + 1) * 80 + 60
                End If
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

    Private Sub RenderEnclosureLayout(ByVal numOfSwitchesInRow)
        Dim _startX As Integer = 30
        Dim _startY As Integer = 18

        Dim _counter As Integer = 0

        For Each switch In _enclosureSwitches
            Dim switchIcon As PictureBox = New PictureBox()
            Dim switchLabel As Label = New Label()

            With switchIcon
                If switch.State = 0 Then
                    .ImageLocation = "png\switch-off.png"
                Else
                    .ImageLocation = "png\switch-on.png"
                End If

                .Width = 42
                .Height = 42
                .Location = New Point(_startX, _startY)
                .SizeMode = PictureBoxSizeMode.StretchImage
                .Tag = switch
                .Parent = Me

                AddHandler .MouseLeave, AddressOf Me.DeviceIcon_MouseLeave
                AddHandler .MouseEnter, AddressOf Me.DeviceIcon_MouseEnter
                AddHandler .MouseDown, AddressOf Me.DeviceIcon_MouseDown
                AddHandler .MouseUp, AddressOf Me.DeviceIcon_MouseUp
            End With

            With switchLabel
                .Text = switch.NickName
                .Location = New Point(_startX + 3, _startY + 48)
                .TextAlign = ContentAlignment.TopCenter
                .AutoSize = True
                .Parent = Me
            End With

            _startX = _startX + 70
            _counter = _counter + 1

            If (_counter Mod numOfSwitchesInRow = 0) Then
                _startX = 30
                _startY = _startY + 80
            End If
        Next
    End Sub

    Private Sub ClickSwitch(ByVal switch As EnclosureSwitch, ByRef image As PictureBox)
        Dim urlBuilder As New StringBuilder
        urlBuilder.Append("HTTP://" & _server & ":" & _port)
        urlBuilder.Append("/smarthome.IMCPlatform/device/v1.0/controlEnclosure.action")

        Dim requestHeaders As New Dictionary(Of String, String)
        requestHeaders.Add("nonce", "ABCDEF")
        requestHeaders.Add("access_token", _userInfo.AccessToken)
        requestHeaders.Add("userCode", _userInfo.UserCode)
        Dim milliseconds = CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds)
        requestHeaders.Add("timestamp", milliseconds.ToString)

        Dim sign = HttpUtils.GenerateSign(_userInfo.AccessToken, "ABCDEF", milliseconds, _userInfo.UserCode)
        requestHeaders.Add("sign", sign)

        Dim dataBuilder As New StringBuilder
        dataBuilder.Append("deviceCode=" & switch.DeviceCode)
        dataBuilder.Append("&switchNum=" & switch.DeviceAddress)

        switch.State = IIf(switch.State = "0", "1", "0")
        dataBuilder.Append("&state=" & switch.State)

        HttpUtils.GetData(urlBuilder.ToString, dataBuilder.ToString, requestHeaders)

        image.ImageLocation = IIf(switch.State = "0", "png\switch-off.png", "png\switch-on.png")
        image.Tag = switch
    End Sub

    Private Sub DeviceIcon_MouseLeave(sender As Object, e As EventArgs)
        Dim Image = CType(sender, Control)
        Image.Location = New Point(Image.Location.X - 1, Image.Location.Y - 1)
    End Sub

    Private Sub DeviceIcon_MouseDown(sender As Object, e As MouseEventArgs)
        Dim Image = CType(sender, Control)
        Image.Location = New Point(Image.Location.X + 2, Image.Location.Y + 2)
    End Sub

    Private Sub DeviceIcon_MouseUp(sender As Object, e As MouseEventArgs)
        Dim Image = CType(sender, Control)
        Image.Location = New Point(Image.Location.X - 2, Image.Location.Y - 2)

        Dim switch As EnclosureSwitch = CType(Image.Tag, EnclosureSwitch)
        ClickSwitch(switch, CType(Image, PictureBox))
    End Sub

    Private Sub DeviceIcon_MouseEnter(sender As Object, e As EventArgs)
        Dim Image = CType(sender, Control)
        Image.Location = New Point(Image.Location.X + 1, Image.Location.Y + 1)
    End Sub
End Class
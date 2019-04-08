Imports System.Text
Imports Newtonsoft.Json.Linq

Public Class MainForm
    Private _loginForm As New LoginForm
    Private _loadingForm As New LoadingForm
    Private _deviceForm As DeviceForm
    Private _userInfo As UserInfo
    Private _server As String
    Private _port As String

    Private Delegate Function DelFetchEncolsureSwitches(ByVal deviceAddress As String) As List(Of EnclosureSwitch)

    Private _delFetchEnclosureSwitches As New DelFetchEncolsureSwitches(AddressOf FetchEncolsureSwitches)

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _loginForm.ShowDialog() = DialogResult.OK Then
            Me._userInfo = _loginForm.UserInfo
            Me._server = _loginForm.Server
            Me._port = _loginForm.Port

            tvConstruct.Nodes.Add(New TreeNode(_userInfo.UserName))

            Me.LoadFloors(_userInfo)
        Else
            Me.Close()
        End If
    End Sub

    '获取当前登录用户的设备列表
    Private Sub LoadDevicesByRoom(userInfo As UserInfo, roomCode As String)
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

        Dim strResponse = HttpUtils.GetData(urlBuilder.ToString(), "roomCode=" + roomCode, requestHeaders)
        Dim jsonResponse = JObject.Parse(strResponse)
        Dim status = jsonResponse("status").ToObject(Of String)

        gpbDevices.Controls.Clear()
        If status.Equals("0") Then
            Dim deviceResults As List(Of JToken) = jsonResponse("result").Children().ToList()

            Dim _startX As Integer = 30
            Dim _startY As Integer = 22

            Dim _counter As Integer = 0
            For Each _item As JToken In deviceResults
                Dim device = _item.ToObject(Of Device)
                Dim _deviceIcon As PictureBox = New PictureBox()
                Dim _deviceLabel As Label = New Label()

                _deviceIcon.Tag = device
                _deviceLabel.Text = device.DeviceAlias
                Select Case device.DeviceType
                    Case DeviceType.ZB_ST
                        If device.DeviceState.Equals("0") Then
                            _deviceIcon.ImageLocation = "png\light-off.png"
                            _deviceLabel.Text = _deviceLabel.Text + "(关)"
                        Else
                            _deviceIcon.ImageLocation = "png\light-on.png"
                            _deviceLabel.Text = _deviceLabel.Text + "(开)"
                        End If
                    Case DeviceType.RF_2262_315_33_ST
                        _deviceIcon.ImageLocation = "png\light-on.png"
                    Case DeviceType.ZB_CT
                        _deviceIcon.ImageLocation = "png\curtain.png"
                        _deviceLabel.Text = _deviceLabel.Text + "(" + device.DeviceState + "%)"
                    Case DeviceType.RF_2262_315_33_CT
                        _deviceIcon.ImageLocation = "png\curtain.png"
                    Case DeviceType.ZB_IF_TV
                        _deviceIcon.ImageLocation = "png\TV.png"
                    Case DeviceType.ZB_IF_AMP
                        _deviceIcon.ImageLocation = "png\AMP.png"
                    Case DeviceType.ZB_IF_AC
                        _deviceIcon.ImageLocation = "png\AC.png"
                    Case DeviceType.ZB_IF_BGMSC, DeviceType.ZB_IF_TVBOX
                        _deviceIcon.ImageLocation = "png\Other.png"
                    Case DeviceType.CTL_ENCLOSURE_8, DeviceType.CTL_ENCLOSURE_32
                        _deviceIcon.ImageLocation = "png\enclosure.png"
                    Case Else
                        _deviceIcon = Nothing
                        _deviceLabel = Nothing
                End Select

                If _deviceIcon IsNot Nothing Then
                    With _deviceIcon
                        .Width = 62
                        .Height = 62
                        .Location = New Point(_startX, _startY)
                        .SizeMode = PictureBoxSizeMode.StretchImage
                        .Parent = gpbDevices

                        AddHandler .MouseLeave, AddressOf Me.DeviceIcon_MouseLeave
                        AddHandler .MouseEnter, AddressOf Me.DeviceIcon_MouseEnter
                        AddHandler .MouseDown, AddressOf Me.DeviceIcon_MouseDown
                        AddHandler .MouseUp, AddressOf Me.DeviceIcon_MouseUp
                    End With
                End If

                If _deviceLabel IsNot Nothing Then
                    'Dim _offsetX = (_deviceIcon.Size.Width - _deviceLabel.Size.Width) / 2
                    With _deviceLabel
                        .Location = New Point(_startX, _startY + 68)
                        .TextAlign = ContentAlignment.TopCenter
                        .AutoSize = True
                        .Parent = gpbDevices
                    End With

                    _startX = _startX + 90
                    _counter = _counter + 1

                    If (_counter Mod 7 = 0) Then
                        _startX = 30
                        _startY = _startY + 120
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub LoadRooms(userInfo As UserInfo, floorCode As String, ByRef parentNode As TreeNode)
        Dim urlBuilder As New StringBuilder
        urlBuilder.Append("HTTP://" & _server & ":" & _port)
        urlBuilder.Append("/smarthome.IMCPlatform/construct/v1.0/fetchRooms.action")

        Dim requestHeaders As New Dictionary(Of String, String)
        requestHeaders.Add("nonce", "ABCDEF")
        requestHeaders.Add("access_token", userInfo.AccessToken)
        requestHeaders.Add("userCode", userInfo.UserCode)
        Dim milliseconds = CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds)
        requestHeaders.Add("timestamp", milliseconds.ToString)

        Dim sign = HttpUtils.GenerateSign(userInfo.AccessToken, "ABCDEF", milliseconds, userInfo.UserCode)
        requestHeaders.Add("sign", sign)

        Dim strResponse = HttpUtils.GetData(urlBuilder.ToString(), IIf(String.IsNullOrEmpty(floorCode), "", "floorCode=" + floorCode), requestHeaders)
        Dim jsonResponse = JObject.Parse(strResponse)
        Dim status = jsonResponse("status").ToObject(Of String)

        If status.Equals("0") Then
            Dim roomResults = jsonResponse("result").Children().ToList()

            For Each result In roomResults
                Dim room = result.ToObject(Of Room)()
                Dim item As New TreeNode
                item.Text = room.RoomAlias
                item.Tag = room
                item.ContextMenuStrip = cms_Room

                parentNode.Nodes.Add(item)
            Next
        End If
    End Sub

    Private Sub LoadFloors(userInfo As UserInfo)
        Dim urlBuilder As New StringBuilder
        urlBuilder.Append("HTTP://" & _server & ":" & _port)
        urlBuilder.Append("/smarthome.IMCPlatform/construct/v1.0/fetchFloors.action")

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
            Dim floorResults = jsonResponse("result").Children().ToList()

            For Each result In floorResults
                Dim floor = result.ToObject(Of Floor)()
                Dim item As New TreeNode
                item.Text = floor.FloorAlias
                item.Tag = floor

                LoadRooms(userInfo, floor.FloorCode, item)
                tvConstruct.TopNode.Nodes.Add(item)
            Next
        End If
    End Sub

    Private Function FetchEncolsureSwitches(ByVal deviceAddress As String) As List(Of EnclosureSwitch)
        Dim urlBuilder As New StringBuilder
        urlBuilder.Append("HTTP://" & _server & ":" & _port)
        urlBuilder.Append("/smarthome.IMCPlatform//device/v1.0/fetchEnclosureSwitches.action")

        Dim requestHeaders As New Dictionary(Of String, String)
        requestHeaders.Add("nonce", "ABCDEF")
        requestHeaders.Add("access_token", _userInfo.AccessToken)
        requestHeaders.Add("userCode", _userInfo.UserCode)
        Dim milliseconds = CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds)
        requestHeaders.Add("timestamp", milliseconds.ToString)

        Dim sign = HttpUtils.GenerateSign(_userInfo.AccessToken, "ABCDEF", milliseconds, _userInfo.UserCode)
        requestHeaders.Add("sign", sign)

        Dim dataBuilder As New StringBuilder
        dataBuilder.Append("deviceAddress=" & deviceAddress)

        Dim strResponse = HttpUtils.GetData(urlBuilder.ToString, dataBuilder.ToString, requestHeaders)
        Dim jsonResponse = JObject.Parse(strResponse)
        Dim status = jsonResponse("status").ToObject(Of String)

        If status.Equals("0") Then
            Dim switchResults As List(Of JToken) = jsonResponse("result").Children().ToList()
            Dim switchList As List(Of EnclosureSwitch) = New List(Of EnclosureSwitch)
            For Each _item As JToken In switchResults
                Dim switch = _item.ToObject(Of EnclosureSwitch)
                switchList.Add(switch)
            Next
            FetchEncolsureSwitches = switchList
        Else
            FetchEncolsureSwitches = Nothing
        End If
    End Function

    Private Sub tvConstruct_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tvConstruct.AfterSelect
        If TypeOf (e.Node.Tag) Is Room Then
            Dim room = CType(e.Node.Tag, Room)
            If room IsNot Nothing Then
                LoadDevicesByRoom(_userInfo, room.RoomCode)
            End If
        End If
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

        Dim device = CType(Image.Tag, Device)

        If device.DeviceType = DeviceType.CTL_ENCLOSURE_8 Or device.DeviceType = DeviceType.CTL_ENCLOSURE_32 Then
            _delFetchEnclosureSwitches.BeginInvoke(device.DeviceAddress, New AsyncCallback(
                                                   Sub(ar As IAsyncResult)
                                                       If Me.InvokeRequired Then
                                                           Me.Invoke(Sub()
                                                                         _loadingForm.Close()
                                                                     End Sub)
                                                       Else
                                                           _loadingForm.Close()
                                                       End If

                                                       Dim listEnclosureSwitches As List(Of EnclosureSwitch) = _delFetchEnclosureSwitches.EndInvoke(ar)
                                                       If Me.InvokeRequired Then
                                                           Me.Invoke(Sub()
                                                                         LaunchDeviceForm(device, listEnclosureSwitches)
                                                                     End Sub)
                                                       Else
                                                           LaunchDeviceForm(device, listEnclosureSwitches)
                                                       End If
                                                   End Sub), Nothing)
            _loadingForm.LoadingText = "正在读取控制盒开关状态……"
            _loadingForm.ShowDialog()
        Else
            _deviceForm = New DeviceForm(device, _userInfo, _server, _port)
            _deviceForm.ShowDialog()
        End If
    End Sub

    Private Sub DeviceIcon_MouseEnter(sender As Object, e As EventArgs)
        Dim Image = CType(sender, Control)
        Image.Location = New Point(Image.Location.X + 1, Image.Location.Y + 1)
    End Sub

    Private Sub cms_Room_Click(sender As Object, e As EventArgs) Handles cms_Room.Click
        Dim node As TreeNode = tvConstruct.SelectedNode

        If TypeOf (node.Tag) Is Room Then
            Dim room = CType(node.Tag, Room)
            If room IsNot Nothing Then
                LoadDevicesByRoom(_userInfo, room.RoomCode)
            End If
        End If
    End Sub

    Private Sub LaunchDeviceForm(ByVal device As Device, ByVal listEnclosureSwitches As List(Of EnclosureSwitch))
        If listEnclosureSwitches IsNot Nothing Then
            _deviceForm = New DeviceForm(device, _userInfo, _server, _port, listEnclosureSwitches)
            _deviceForm.ShowDialog()
        Else
            MessageBox.Show(Me, "无法读取控制盒开关状态，请检查控制盒是否连接正常……")
        End If
    End Sub
End Class

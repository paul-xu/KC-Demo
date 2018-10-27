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

        lvDevice.Items.Clear()
        If status.Equals("0") Then
            Dim deviceResults = jsonResponse("result").Children().ToList()

            For Each result In deviceResults
                Dim device = result.ToObject(Of Device)()
                Dim item As New ListViewItem
                item.Tag = device

                Select Case device.DeviceType
                    Case "ZB-ST"
                        If device.DeviceState.Equals("0") Then
                            item.ImageIndex = 1
                            item.Text = device.DeviceAlias + "(状态:关)"
                        Else
                            item.ImageIndex = 0
                            item.Text = device.DeviceAlias + "(状态:开)"
                        End If
                    Case "RF-2262-315-33-ST"
                        item.ImageIndex = 0
                        item.Text = device.DeviceAlias
                    Case "ZT-CT"
                        item.ImageIndex = 3
                        item.Text = device.DeviceAlias + "(" + device.DeviceState + "%)"
                    Case "RF-2262-315-33-CT"
                        item.ImageIndex = 3
                        item.Text = device.DeviceAlias
                End Select

                lvDevice.Items.Add(item)
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

    Private Sub lvDevice_DoubleClick(sender As Object, e As EventArgs) Handles lvDevice.DoubleClick
        Dim listItem = lvDevice.SelectedItems.Item(0)
        Dim device = CType(listItem.Tag, Device)

        Dim deviceForm As New DeviceForm(device, _userInfo, _server, _port)
        deviceForm.ShowDialog()
    End Sub

    Private Sub tvConstruct_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tvConstruct.AfterSelect
        If TypeOf (e.Node.Tag) Is Room Then
            Dim room = CType(e.Node.Tag, Room)
            If room IsNot Nothing Then
                LoadDevicesByRoom(_userInfo, room.RoomCode)
            End If
        End If
    End Sub
End Class

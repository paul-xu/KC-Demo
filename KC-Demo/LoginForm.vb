Imports System.Text
Imports KC_Demo
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class LoginForm
    Private _server As String
    Private _port As String
    Private _userInfo As UserInfo

    Property Server() As String
        Get
            Return _server
        End Get
        Set(ByVal value As String)
            _server = value
        End Set
    End Property

    Property Port() As String
        Get
            Return _port
        End Get
        Set(ByVal value As String)
            _port = value
        End Set
    End Property

    Public Property UserInfo As UserInfo
        Get
            Return _userInfo
        End Get
        Set(value As UserInfo)
            _userInfo = value
        End Set
    End Property

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim _urlBuilder As New StringBuilder
        Dim _dataBuilder As New StringBuilder

        _urlBuilder.Append("HTTP://")
        _urlBuilder.Append(Me.txtServer.Text.Trim())
        _urlBuilder.Append(":")
        _urlBuilder.Append(Me.txtPort.Text.Trim())
        _urlBuilder.Append("/smarthome.IMCPlatform/user/v1.0/login.action")

        _dataBuilder.Append("sn=")
        _dataBuilder.Append(Me.txtUserPhone.Text)
        _dataBuilder.Append("&userPwd=")
        _dataBuilder.Append(MD5.MD5(Me.txtUserPwd.Text, 32).ToUpper())

        Dim _response = HttpUtils.GetData(_urlBuilder.ToString, _dataBuilder.ToString)

        Dim _jsonResponse = JObject.Parse(_response)
        Dim _status = _jsonResponse("status").ToObject(Of String)

        If _status = "0" Then
            Me.UserInfo = _jsonResponse("result").ToObject(Of UserInfo)
            Me._server = Me.txtServer.Text.Trim()
            Me._port = Me.txtPort.Text.Trim()
            Me.DialogResult = DialogResult.OK
        Else
            Dim _message = _jsonResponse("message").ToObject(Of String)
            MessageBox.Show(_message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub
End Class
Imports Newtonsoft.Json

<JsonObject(MemberSerialization.OptIn)>
Public Class EnclosureSwitch
    Private _nickName As String
    Private _deviceCode As String
    Private _state As String
    Private _deviceAddress As String

    <JsonProperty("nickName")>
    Public Property NickName As String
        Get
            Return _nickName
        End Get
        Set(value As String)
            _nickName = value
        End Set
    End Property

    <JsonProperty("deviceCode")>
    Public Property DeviceCode As String
        Get
            Return _deviceCode
        End Get
        Set(value As String)
            _deviceCode = value
        End Set
    End Property

    <JsonProperty("state")>
    Public Property State As String
        Get
            Return _state
        End Get
        Set(value As String)
            _state = value
        End Set
    End Property

    <JsonProperty("deviceAddress")>
    Public Property DeviceAddress As String
        Get
            Return _deviceAddress
        End Get
        Set(value As String)
            _deviceAddress = value
        End Set
    End Property
End Class

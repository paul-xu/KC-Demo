Imports KC_Demo
Imports Newtonsoft.Json

<JsonObject(MemberSerialization.OptIn)>
Public Class Device
    Private _deviceAddress As String
    Private _deviceAlias As String
    Private _deviceType As String
    Private _deviceState As String
    Private _roomCode As String
    Private _hostCode As String
    Private _operations As List(Of DeviceOperation)

    <JsonProperty("deviceAddress")>
    Public Property DeviceAddress As String
        Get
            Return _deviceAddress
        End Get
        Set(value As String)
            _deviceAddress = value
        End Set
    End Property

    <JsonProperty("deviceAlias")>
    Public Property DeviceAlias As String
        Get
            Return _deviceAlias
        End Get
        Set(value As String)
            _deviceAlias = value
        End Set
    End Property

    <JsonProperty("deviceType")>
    Public Property DeviceType As String
        Get
            Return _deviceType
        End Get
        Set(value As String)
            _deviceType = value
        End Set
    End Property

    <JsonProperty("deviceState")>
    Public Property DeviceState As String
        Get
            Return _deviceState
        End Get
        Set(value As String)
            _deviceState = value
        End Set
    End Property

    <JsonProperty("roomCode")>
    Public Property RoomCode As String
        Get
            Return _roomCode
        End Get
        Set(value As String)
            _roomCode = value
        End Set
    End Property

    <JsonProperty("hostCode")>
    Public Property HostCode As String
        Get
            Return _hostCode
        End Get
        Set(value As String)
            _hostCode = value
        End Set
    End Property

    <JsonProperty("operations")>
    Public Property Operations As List(Of DeviceOperation)
        Get
            Return _operations
        End Get
        Set(value As List(Of DeviceOperation))
            _operations = value
        End Set
    End Property
End Class

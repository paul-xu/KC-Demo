Imports Newtonsoft.Json

<JsonObject(MemberSerialization.OptIn)>
Public Class Room
    Private _floorAlias As String
    Private _floorCode As String
    Private _roomAlias As String
    Private _roomCode As String

    <JsonProperty("floorAlias")>
    Public Property FloorAlias As String
        Get
            Return _floorAlias
        End Get
        Set(value As String)
            _floorAlias = value
        End Set
    End Property

    <JsonProperty("floorCode")>
    Public Property FloorCode As String
        Get
            Return _floorCode
        End Get
        Set(value As String)
            _floorCode = value
        End Set
    End Property

    <JsonProperty("roomAlias")>
    Public Property RoomAlias As String
        Get
            Return _roomAlias
        End Get
        Set(value As String)
            _roomAlias = value
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
End Class

Imports Newtonsoft.Json

<JsonObject(MemberSerialization.OptIn)>
Public Class Floor
    Private _floorAlias As String
    Private _floorCode As String

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
End Class

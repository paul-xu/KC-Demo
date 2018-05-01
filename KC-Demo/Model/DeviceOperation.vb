Imports Newtonsoft.Json

<JsonObject(MemberSerialization.OptIn)>
Public Class DeviceOperation
    Private _code As String
    Private _name As String

    <JsonProperty("code")>
    Public Property Code As String
        Get
            Return _code
        End Get
        Set(value As String)
            _code = value
        End Set
    End Property

    <JsonProperty("name")>
    Public Property Name As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
        End Set
    End Property
End Class

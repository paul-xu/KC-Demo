Imports Newtonsoft.Json

<JsonObject(MemberSerialization.OptIn)>
Public Class JsonResponse
    Private _message As String
    Private _status As String
    Private _result As String

    <JsonProperty("message")>
    Public Property Message As String
        Get
            Return _message
        End Get
        Set(value As String)
            _message = value
        End Set
    End Property

    <JsonProperty("status")>
    Public Property Status As String
        Get
            Return _status
        End Get
        Set(value As String)
            _status = value
        End Set
    End Property

    <JsonProperty("result")>
    Public Property Result As String
        Get
            Return _result
        End Get
        Set(value As String)
            _result = value
        End Set
    End Property
End Class

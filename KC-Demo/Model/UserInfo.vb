Imports Newtonsoft.Json

<JsonObject(MemberSerialization.OptIn)>
Public Class UserInfo
    Private _accessToken As String
    Private _authorizationUserCode As String
    Private _phoneType As String
    Private _userAddr As String
    Private _userAge As Int16?
    Private _userCode As String
    Private _userEmail As String
    Private _userId As Int32?
    Private _userName As String
    Private _userPhone As String
    Private _userSex As String

    <JsonProperty("accessToken")>
    Public Property AccessToken As String
        Get
            Return _accessToken
        End Get
        Set(value As String)
            _accessToken = value
        End Set
    End Property

    <JsonProperty("authorizationUserCode")>
    Public Property AuthorizationUserCode As String
        Get
            Return _authorizationUserCode
        End Get
        Set(value As String)
            _authorizationUserCode = value
        End Set
    End Property

    <JsonProperty("phoneType")>
    Public Property PhoneType As String
        Get
            Return _phoneType
        End Get
        Set(value As String)
            _phoneType = value
        End Set
    End Property

    <JsonProperty("userAddr")>
    Public Property UserAddr As String
        Get
            Return _userAddr
        End Get
        Set(value As String)
            _userAddr = value
        End Set
    End Property

    <JsonProperty("userAge")>
    Public Property UserAge As Short?
        Get
            Return _userAge
        End Get
        Set(value As Short?)
            _userAge = value
        End Set
    End Property

    <JsonProperty("userCode")>
    Public Property UserCode As String
        Get
            Return _userCode
        End Get
        Set(value As String)
            _userCode = value
        End Set
    End Property

    <JsonProperty("userEmail")>
    Public Property UserEmail As String
        Get
            Return _userEmail
        End Get
        Set(value As String)
            _userEmail = value
        End Set
    End Property

    <JsonProperty("userId")>
    Public Property UserId As Integer?
        Get
            Return _userId
        End Get
        Set(value As Integer?)
            _userId = value
        End Set
    End Property

    <JsonProperty("userName")>
    Public Property UserName As String
        Get
            Return _userName
        End Get
        Set(value As String)
            _userName = value
        End Set
    End Property

    <JsonProperty("userPhone")>
    Public Property UserPhone As String
        Get
            Return _userPhone
        End Get
        Set(value As String)
            _userPhone = value
        End Set
    End Property

    <JsonProperty("userSex")>
    Public Property UserSex As String
        Get
            Return _userSex
        End Get
        Set(value As String)
            _userSex = value
        End Set
    End Property
End Class

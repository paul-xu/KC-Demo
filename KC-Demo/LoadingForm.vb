Public Class LoadingForm
    Public Property LoadingText() As String
        Get
            Return Label1.Text
        End Get
        Set(ByVal value As String)
            Label1.Text = value
        End Set
    End Property
End Class
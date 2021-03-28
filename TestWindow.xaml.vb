Public Class TestWindow
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Message.Message("This is a test message.", vbOKOnly Or MsgBoxStyle.AbortRetryIgnore, "Test message")
    End Sub
End Class

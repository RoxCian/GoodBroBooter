Public Class RuntimeMonitor
    Public Property PreviousDevMode As DevMode? = Nothing
    Public Property ProcessToMonitor As Process
    Public Property ProcessExitCallback As Action

    Public Async Sub StartMonitoring()
        If Me.ProcessToMonitor IsNot Nothing Then
            Await Me.ProcessToMonitor.WaitForExitAsync()
            Me.ProcessExitCallback.Invoke
        End If
    End Sub
    Public Sub StopMonitoring()
        If Me.ProcessToMonitor IsNot Nothing Then Me.ProcessToMonitor.Kill()
    End Sub
End Class

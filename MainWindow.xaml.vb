Imports System.Security.Principal
Imports System.Threading

Class MainWindow
    Public Property Settings As SettingsModel
        Get
            Return Me.GetValue(SettingsProperty)
        End Get
        Set(value As SettingsModel)
            Me.SetValue(SettingsProperty, value)
        End Set
    End Property
    Public ReadOnly Property IsMonitoring As Boolean
        Get
            Return Me.GetValue(IsMonitoringProperty)
        End Get
    End Property
    Public ReadOnly Property IsRunAsAdministrator As Boolean
        Get
            Return Me.GetValue(IsRunAsAdministratorProperty)
        End Get
    End Property

    Private WithEvents NotifyIcon As New Forms.NotifyIcon(New ComponentModel.Container) With {
        .Icon = GetType(Forms.Form).GetProperty("DefaultIcon", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Static).GetValue(Nothing, Nothing),
        .Visible = True,
        .Text = "GoodBroBooter.exe",
        .BalloonTipText = "Just sit back and relax...",
        .BalloonTipTitle = "GoodBroBooter.exe"
    }
    Private ReadOnly monitor As New RuntimeMonitor With {
        .ProcessExitCallback = AddressOf Me.PostProcessing
    }

    Public Shared ReadOnly SettingsProperty As DependencyProperty = DependencyProperty.Register("Settings", GetType(SettingsModel), GetType(MainWindow), New PropertyMetadata(Nothing))
    Private Shared ReadOnly IsMonitoringPropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("IsMonitoring", GetType(Boolean), GetType(MainWindow), New PropertyMetadata(False))
    Public Shared ReadOnly IsMonitoringProperty = IsMonitoringPropertyKey.DependencyProperty
    Private Shared ReadOnly IsRunAsAdministratorPropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("IsRunAsAdministrator", GetType(Boolean), GetType(MainWindow), New PropertyMetadata(False))
    Public Shared ReadOnly IsRunAsAdministratorProperty = IsRunAsAdministratorPropertyKey.DependencyProperty

    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Dim settingsPath = AppDomain.CurrentDomain.BaseDirectory & If(AppDomain.CurrentDomain.BaseDirectory.EndsWith("\"), "", "\") & Process.GetCurrentProcess.ProcessName & ".json"
        Me.SetValue(SettingsProperty, New SettingsModel(settingsPath))

        Dim identity = WindowsIdentity.GetCurrent
        Dim principal As New WindowsPrincipal(identity)
        Me.SetValue(IsRunAsAdministratorPropertyKey, principal.IsInRole(WindowsBuiltInRole.Administrator))

        AddHandler CType(Windows.Application.Current, GoodBroBooter.Application).ToExit, AddressOf Me.HandleApplicationToExit

        Dim NotifyIconContextMenu As New Forms.ContextMenuStrip
        Dim NotifyIconContextMenuItemStop As New Forms.ToolStripMenuItem(My.Resources.Localization.MenuStop)
        AddHandler NotifyIconContextMenuItemStop.MouseUp, Sub(sender As Object, e As Forms.MouseEventArgs) If e.Button = Forms.MouseButtons.Left And Me.IsMonitoring Then Me.Stop()
        Dim NotifyIconContextMenuItemExit As New Forms.ToolStripMenuItem(My.Resources.Localization.MenuExit)
        AddHandler NotifyIconContextMenuItemExit.MouseUp, Sub(sender As Object, e As Forms.MouseEventArgs) If e.Button = Forms.MouseButtons.Left Then Application.Current.Shutdown()
        NotifyIconContextMenu.Items.Add(NotifyIconContextMenuItemStop)
        NotifyIconContextMenu.Items.Add(NotifyIconContextMenuItemExit)
        Me.NotifyIcon.ContextMenuStrip = NotifyIconContextMenu

        If Me.Settings.AutoBoot Then Me.Boot()
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs)
        Dim msg = Message.Message(My.Resources.Localization.QuitMessage, vbOKCancel Or vbQuestion, My.Resources.Localization.QuitMessageTitle)
        AddHandler msg.MessageReturned, Sub(sender2, e2) If e2.Result = MsgBoxResult.Ok Then Application.Current.Shutdown()
        Me.Show()
    End Sub

    Private Sub TrayButton_Click(sender As Object, e As RoutedEventArgs)
        Me.Hide()

        Me.NotifyIcon.ShowBalloonTip(3000)
    End Sub

    Private Sub OpenConfiguratorButton_Click(sender As Object, e As RoutedEventArgs)
        Try
            If FileIO.FileSystem.FileExists(Me.Settings.ConfiguratorPath) Then Process.Start(Me.Settings.ConfiguratorPath)
        Catch ex As ComponentModel.Win32Exception
            If ex.NativeErrorCode = 740 Then
                Message.Message(My.Resources.Localization.OpenConfiguratorAsAdminMessage, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, My.Resources.Localization.OpenConfiguratorFailedMessageTitle)
            Else
                Message.Message(My.Resources.Localization.OpenConfiguratorFailedMessage, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, My.Resources.Localization.OpenConfiguratorFailedMessageTitle)
            End If
        Catch ex As Exception
            Message.Message(My.Resources.Localization.OpenConfiguratorFailedMessage, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, My.Resources.Localization.OpenConfiguratorFailedMessageTitle)
        End Try
    End Sub

    Private Sub HandleNotifyIconDoubleClicked(sender As Object, e As Forms.MouseEventArgs) Handles NotifyIcon.DoubleClick
        If e.Button = Forms.MouseButtons.Left Then Me.Show()
    End Sub

    Private Sub HandleApplicationPathDialogReturned(sender As Object, e As RoutedEventArgs)
        If TypeOf sender Is OpenFileDialogControl Then
            Me.ApplicationPathTextBox.Text = sender.Result
            Me.ApplicationPathTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            Me.ProcessNameToMonitorTextBox.Text = sender.Result.Substring(sender.Result.LastIndexOf("\") + 1, sender.Result.LastIndexOf(".") - sender.Result.LastIndexOf("\") - 1)
            Me.ProcessNameToMonitorTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
        End If
    End Sub

    Private Sub HandleConfiguratorPathDialogReturned(sender As Object, e As RoutedEventArgs)
        If TypeOf sender Is OpenFileDialogControl Then
            Me.ConfiguratorPathTextBox.Text = sender.Result
            Me.ConfiguratorPathTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
        End If
    End Sub

    Private Shared Sub ResetFocus()
        Keyboard.ClearFocus()
    End Sub

    Private Sub BootButton_Click(sender As Object, e As RoutedEventArgs)
        Boot()
    End Sub

    Private Sub StopButton_Click(sender As Object, e As RoutedEventArgs)
        [Stop]()
    End Sub

    Private Shared Function StartStatement(statement As String) As Process
        Dim fileToken As String = ""
        Dim commandToken As String = ""
        Dim isInQuoteScope As Boolean = False
        For i = 0 To statement.Length - 1
            Dim c As Char = statement(i)
            Select Case c
                Case """"
                    isInQuoteScope = Not isInQuoteScope
                Case " "
                    If isInQuoteScope Then
                        fileToken &= " "
                    Else
                        commandToken = statement.Substring(i + 1)
                        Exit For
                    End If
                Case Else
                    fileToken &= c
            End Select
        Next
        If commandToken = "" Then Return Process.Start(fileToken) Else Return Process.Start(fileToken, commandToken)
    End Function

    Public Sub Boot()
        Me.SetValue(IsMonitoringPropertyKey, True)

        Dim dev As New DevMode
        Dim result = EnumDisplaySettingsA(Nothing, ENUM_CURRENT_SETTINGS, dev)
        If result <> 0 Then
            Dim processName = If(Settings.ProcessNameToMonitor IsNot Nothing And Settings.ProcessNameToMonitor <> "", Settings.ProcessNameToMonitor, If(Settings.ApplicationPath IsNot Nothing And Settings.ApplicationPath <> "", Settings.ApplicationPath.Substring(Settings.ApplicationPath.LastIndexOf("\") + 1, Settings.ApplicationPath.LastIndexOf(".") - Settings.ApplicationPath.LastIndexOf("\") - 1), Nothing))
            If processName Is Nothing Or processName = "" Or ((Settings.ApplicationPath Is Nothing Or Settings.ApplicationPath = "" Or Not FileIO.FileSystem.FileExists(Settings.ApplicationPath)) And (Settings.ShellStatement Is Nothing Or Settings.ShellStatement = "")) Then
                Me.SetValue(IsMonitoringPropertyKey, False)
                Message.Message(My.Resources.Localization.BootPathNeededMessage, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, My.Resources.Localization.BootFailedMessageTitle)
                Me.Show()
                Return
            End If
            ' Monitor settings
            If (Settings.ResolutionX IsNot Nothing And Settings.ResolutionY IsNot Nothing) Or Settings.ScreenOrientation <> ScreenOrientation.Null Or Settings.DisplayFrequency IsNot Nothing Then
                Dim prevDev = dev
                Dim prevWidth = dev.dmPelsWidth
                Dim prevHeight = dev.dmPelsHeight
                If Settings.ScreenOrientation <> ScreenOrientation.Null And Settings.ScreenOrientation - 1 <> dev.dmDisplayOrientation Then dev.dmDisplayOrientation = Settings.ScreenOrientation - 1
                If Settings.ResolutionX IsNot Nothing And Settings.ResolutionY IsNot Nothing Then
                    prevWidth = Settings.ResolutionX
                    prevHeight = Settings.ResolutionY
                End If

                If Settings.DisplayFrequency IsNot Nothing Then dev.dmDisplayFrequency = Settings.DisplayFrequency

                If Settings.ScreenOrientation = ScreenOrientation.Degree90Clockwise Or Settings.ScreenOrientation = ScreenOrientation.Degree270Clockwise Then
                    dev.dmPelsWidth = Math.Min(prevWidth, prevHeight)
                    dev.dmPelsHeight = Math.Max(prevWidth, prevHeight)
                Else
                    dev.dmPelsWidth = Math.Max(prevWidth, prevHeight)
                    dev.dmPelsHeight = Math.Min(prevWidth, prevHeight)
                End If
                If ChangeDisplaySettingsA(dev, 0) <> 0 Then
                    Me.SetValue(IsMonitoringPropertyKey, False)
                    Message.Message(My.Resources.Localization.BootFailedWhileSetDevModeMessage, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, My.Resources.Localization.BootFailedMessageTitle)
                    Me.Show()
                    Return
                End If
                Me.monitor.PreviousDevMode = prevDev
            End If
            ' End of monitor settings
            If Settings.ShellAnotherTask <> "" Then
                Process.Start(Settings.ShellAnotherTask)
            End If
            Try
                If Settings.ProcessNameToMonitor IsNot Nothing And Settings.ProcessNameToMonitor <> "" Then
                    Dim processListOld = Process.GetProcessesByName(processName)
                    StartStatement(If(Settings.ShellStatement IsNot Nothing And Settings.ShellStatement <> "", Settings.ShellStatement, Settings.ApplicationPath))
                    Dim processListNew As Process()
                    If Me.Settings.DelayToMonitorProcess IsNot Nothing AndAlso Me.Settings.DelayToMonitorProcess > 0 Then Threading.Thread.Sleep(Me.Settings.DelayToMonitorProcess)
                    If Me.Settings.MaxRetryCountForProcessMonitoring IsNot Nothing AndAlso Me.Settings.MaxRetryCountForProcessMonitoring > 0 AndAlso Me.Settings.RetryIntervalForProcessMonitoring IsNot Nothing AndAlso Me.Settings.RetryIntervalForProcessMonitoring > 0 Then
                        For i = 1 To Me.Settings.RetryIntervalForProcessMonitoring
                            processListNew = Process.GetProcessesByName(processName)
                            Dim flag As Boolean = True
                            For Each p In processListNew
                                For Each q In processListOld
                                    If p.Id = q.Id Then flag = False
                                Next
                                If flag Then
                                    Me.monitor.ProcessToMonitor = p
                                    Exit For
                                End If
                            Next
                            If flag Then Exit For
                            Threading.Thread.Sleep(Me.Settings.RetryIntervalForProcessMonitoring)
                        Next
                    Else
                        processListNew = Process.GetProcessesByName(processName)
                        For Each p In processListNew
                            Dim flag As Boolean = True
                            For Each q In processListOld
                                If p.Id = q.Id Then flag = False
                            Next
                            If flag Then
                                Me.monitor.ProcessToMonitor = p
                                Exit For
                            End If
                        Next
                    End If
                Else
                    Me.monitor.ProcessToMonitor = StartStatement(If(Settings.ShellStatement IsNot Nothing And Settings.ShellStatement <> "", Settings.ShellStatement, Settings.ApplicationPath))
                End If
            Catch ex As ComponentModel.Win32Exception
                PostProcessing()
                If ex.NativeErrorCode = 740 Then
                    Message.Message(My.Resources.Localization.BootAsAdminMessage, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, My.Resources.Localization.BootFailedMessageTitle)
                ElseIf ex.NativeErrorCode = 2 Then
                    Message.Message(My.Resources.Localization.BootFileNotFoundMessage, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, My.Resources.Localization.BootFailedMessageTitle)
                Else
                    Message.Message(My.Resources.Localization.BootFailedMessage, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, My.Resources.Localization.BootFailedMessageTitle)
                End If
                Me.Show()
                Return
            Catch ex As Exception
                PostProcessing()
                Message.Message(My.Resources.Localization.BootFailedMessage, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, My.Resources.Localization.BootFailedMessageTitle)
                Me.Show()
                Return
            End Try
            If Me.monitor.ProcessToMonitor IsNot Nothing Then
                Try
                    Me.monitor.ProcessToMonitor.PriorityClass = Me.Settings.ProcessPriorityClass
                Catch
                End Try
                Me.monitor.StartMonitoring()
            Else
                Me.SetValue(IsMonitoringPropertyKey, False)
                Return
            End If
        End If
        If Me.Settings.AutoHide Then Me.TrayButton_Click(Me, New RoutedEventArgs)
    End Sub
    Public Sub [Stop]()
        Me.monitor.StopMonitoring()
    End Sub
    Public Sub PostProcessing()
        Me.SetValue(IsMonitoringPropertyKey, False)
        If Me.monitor.PreviousDevMode IsNot Nothing Then
            If ChangeDisplaySettingsA(Me.monitor.PreviousDevMode, 0) <> 0 Then MsgBox(My.Resources.Localization.PostProcessingFailedWhileSetDevModeMessage, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, My.Resources.Localization.PostProcessingFailedMessageTitle)
            Me.monitor.PreviousDevMode = Nothing
        End If
        If Me.Settings.ShellPostTask IsNot Nothing And Me.Settings.ShellPostTask <> "" Then
            StartStatement(Me.Settings.ShellPostTask)
        End If
        If Me.Settings.AutoQuit Then Application.Current.Shutdown()
        Me.Show()
    End Sub

    Private Sub TestButton_Click(sender As Object, e As RoutedEventArgs)
        Me.Dispatcher.Invoke(Sub()
                                 Dim w As New TestWindow
                                 w.Show()
                             End Sub)
    End Sub
    Private Sub HandleApplicationToExit(sender As Object, e As HandableWindowEventArgs)
        If Not e.TargetWindow.Equals(Me) Then Return
        Dim msg = Message.Message(My.Resources.Localization.QuitMessage, vbOKCancel Or vbQuestion, My.Resources.Localization.QuitMessageTitle)
        AddHandler msg.MessageReturned, Sub(sender2, e2) If e2.Result = MsgBoxResult.Ok Then Application.Current.Shutdown()
        Me.Show()
        e.Handled = True
    End Sub
End Class
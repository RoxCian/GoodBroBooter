Public Class Message

    Public Property Title As String
        Get
            Return GetValue(TitleProperty)
        End Get

        Set(value As String)
            SetValue(TitleProperty, value)
        End Set
    End Property
    Public Property Detail As String
        Get
            Return GetValue(DetailProperty)
        End Get

        Set(value As String)
            SetValue(DetailProperty, value)
        End Set
    End Property
    Public Property MessageStyle As MsgBoxStyle
        Get
            Return GetValue(MessageStyleProperty)
        End Get

        Set(ByVal value As MsgBoxStyle)
            SetValue(MessageStyleProperty, value)
        End Set
    End Property
    Public ReadOnly Property DefaultButton As MsgBoxResult
        Get
            Return GetValue(DefaultButtonProperty)
        End Get
    End Property
    Public ReadOnly Property DefaultButtonControl As ColorizedButton
        Get
            Select Case Me.DefaultButton
                Case MsgBoxResult.Abort
                    Return Me.FindResource("AbortButton")
                Case MsgBoxResult.Cancel
                    Return Me.FindResource("CancelButton")
                Case MsgBoxResult.Ignore
                    Return Me.FindResource("IngoreButton")
                Case MsgBoxResult.No
                    Return Me.FindResource("NoButton")
                Case MsgBoxResult.Ok
                    Return Me.FindResource("OkButton")
                Case MsgBoxResult.Retry
                    Return Me.FindResource("RetryButton")
                Case MsgBoxResult.Yes
                    Return Me.FindResource("YesButton")
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property
    Public ReadOnly Property ButtonStyle As MessageButtonStyle
        Get
            Return GetValue(ButtonStyleProperty)
        End Get
    End Property
    Public ReadOnly Property MessageType As MessageType
        Get
            Return GetValue(MessageTypeProperty)
        End Get
    End Property
    Public ReadOnly Property Result As MsgBoxResult?
        Get
            Return GetValue(ResultProperty)
        End Get
    End Property
    Public ReadOnly Property IsShowed As Boolean
        Get
            Return GetValue(IsShowedProperty)
        End Get
    End Property
    Private Property TopLevelPanel As Panel
    Private Property ParentWindow As MainWindow
    Private MsgBoxReturnTask As Task(Of MsgBoxResult) = Nothing

    Public Event MessageReturned As MessageReturnedEventHandler

    Public Shared ReadOnly TitleProperty As DependencyProperty = DependencyProperty.Register("Title", GetType(String), GetType(Message), New PropertyMetadata("Message Title"))
    Public Shared ReadOnly DetailProperty As DependencyProperty = DependencyProperty.Register("Detail", GetType(String), GetType(Message), New PropertyMetadata("This is the detail of the message"))
    Public Shared ReadOnly MessageStyleProperty As DependencyProperty = DependencyProperty.Register("MessageStyle", GetType(MsgBoxStyle), GetType(Message), New PropertyMetadata(MsgBoxStyle.OkOnly, AddressOf HandleMessageStylePropertyChanged))
    Private Shared ReadOnly ResultPropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("Result", GetType(MsgBoxResult?), GetType(Message), New PropertyMetadata(Nothing))
    Public Shared ReadOnly ResultProperty As DependencyProperty = ResultPropertyKey.DependencyProperty

    Private Shared ReadOnly DefaultButtonPropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("DefaultButton", GetType(MsgBoxResult), GetType(Message), New PropertyMetadata(vbOK))
    Public Shared ReadOnly DefaultButtonProperty As DependencyProperty = DefaultButtonPropertyKey.DependencyProperty

    Private Shared ReadOnly ButtonStylePropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("ButtonStyle", GetType(MessageButtonStyle), GetType(Message), New PropertyMetadata(MessageButtonStyle.OkOnly))
    Public Shared ReadOnly ButtonStyleProperty As DependencyProperty = ButtonStylePropertyKey.DependencyProperty

    Private Shared ReadOnly MessageTypePropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("MessageType", GetType(MessageType), GetType(Message), New PropertyMetadata(MessageType.Null))
    Public Shared ReadOnly MessageTypeProperty As DependencyProperty = MessageTypePropertyKey.DependencyProperty

    Private Shared ReadOnly IsShowedPropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("IsShowed", GetType(Boolean), GetType(Message), New PropertyMetadata(False))
    Public Shared ReadOnly IsShowedProperty As DependencyProperty = IsShowedPropertyKey.DependencyProperty

    Public Shared Shadows ReadOnly VisibilityProperty As DependencyProperty = UIElement.VisibilityProperty.AddOwner(GetType(Message), New FrameworkPropertyMetadata(Visibility.Hidden, FrameworkPropertyMetadataOptions.Inherits, AddressOf HandleVisibilityChanged))

    Public Sub Show()
        Me.SetValue(IsShowedPropertyKey, True)
        Select Case Me.MessageType
            Case MessageType.Critical
                Media.SystemSounds.Hand.Play()
            Case MessageType.Exclamation, MessageType.InformationalExclamation
                Media.SystemSounds.Exclamation.Play()
            Case MessageType.Question
                Media.SystemSounds.Question.Play()
            Case Else
                Media.SystemSounds.Beep.Play()
        End Select
        If Me.MsgBoxReturnTask IsNot Nothing Then Me.MsgBoxReturnTask.Start()
    End Sub

    Private Shared Sub HandleMessageStylePropertyChanged(sender As Message, e As DependencyPropertyChangedEventArgs)
        If e.OldValue <> e.NewValue Then
            Dim v = DirectCast(e.NewValue, MsgBoxStyle)
            If (v Or MsgBoxStyle.RetryCancel) = v Then '101
                sender.SetValue(ButtonStylePropertyKey, MessageButtonStyle.RetryCancel)
                If (v Or MsgBoxStyle.DefaultButton2) = v Then sender.SetValue(DefaultButtonPropertyKey, vbCancel) Else sender.SetValue(DefaultButtonPropertyKey, vbRetry)
            ElseIf (v Or MsgBoxStyle.YesNoCancel) = v Then '11
                sender.SetValue(ButtonStylePropertyKey, MessageButtonStyle.YesNoCancel)
                If (v Or MsgBoxStyle.DefaultButton2) = v Then
                    sender.SetValue(DefaultButtonPropertyKey, vbNo)
                ElseIf (v Or MsgBoxStyle.DefaultButton3) = v Then
                    sender.SetValue(DefaultButtonPropertyKey, vbCancel)
                Else
                    sender.SetValue(DefaultButtonPropertyKey, vbYes)
                End If
            ElseIf (v Or MsgBoxStyle.YesNo) = v Then '100
                sender.SetValue(ButtonStylePropertyKey, MessageButtonStyle.YesNo)
                If (v Or MsgBoxStyle.DefaultButton2) = v Then sender.SetValue(DefaultButtonPropertyKey, vbNo) Else sender.SetValue(DefaultButtonPropertyKey, vbYes)
            ElseIf (v Or MsgBoxStyle.AbortRetryIgnore) = v Then '10
                sender.SetValue(ButtonStylePropertyKey, MessageButtonStyle.AbortRetryIgnore)
                If (v Or MsgBoxStyle.DefaultButton2) = v Then
                    sender.SetValue(DefaultButtonPropertyKey, vbRetry)
                ElseIf (v Or MsgBoxStyle.DefaultButton3) = v Then
                    sender.SetValue(DefaultButtonPropertyKey, vbIgnore)
                Else
                    sender.SetValue(DefaultButtonPropertyKey, vbAbort)
                End If
            ElseIf (v Or MsgBoxStyle.OkCancel) = v Then '1
                sender.SetValue(ButtonStylePropertyKey, MessageButtonStyle.OkCancel)
                If (v Or MsgBoxStyle.DefaultButton2) = v Then sender.SetValue(DefaultButtonPropertyKey, vbCancel) Else sender.SetValue(DefaultButtonPropertyKey, vbOK)
            Else '0
                sender.SetValue(ButtonStylePropertyKey, MessageButtonStyle.OkOnly)
                sender.SetValue(DefaultButtonPropertyKey, vbOK)
            End If

            If (v Or MsgBoxStyle.Information Or MsgBoxStyle.Exclamation) = v Then
                sender.SetValue(MessageTypePropertyKey, MessageType.InformationalExclamation)
            ElseIf (v Or MsgBoxStyle.Exclamation) = v Then
                sender.SetValue(MessageTypePropertyKey, MessageType.Exclamation)
            ElseIf (v Or MsgBoxStyle.Information) = v Then
                sender.SetValue(MessageTypePropertyKey, MessageType.Information)
            ElseIf (v Or MsgBoxStyle.Question) = v Then
                sender.SetValue(MessageTypePropertyKey, MessageType.Question)
            ElseIf (v Or MsgBoxStyle.Critical) = v Then
                sender.SetValue(MessageTypePropertyKey, MessageType.Critical)
            Else
                sender.SetValue(MessageTypePropertyKey, MessageType.Null)
            End If
        End If
    End Sub
    Private Shared Sub HandleVisibilityChanged(sender As Message, e As DependencyPropertyChangedEventArgs)
        If sender.Result IsNot Nothing And e.NewValue = Visibility.Hidden Then sender.OnMessageReturned(New MessageReturnedEventArgs(sender.Result))
    End Sub
    Protected Sub OnMessageReturned(e As MessageReturnedEventArgs)
        RaiseEvent MessageReturned(Me, e)
    End Sub
    Private Sub HandleButtonClick(sender As Object, e As RoutedEventArgs)
        Me.ReturnResult(CType(sender.TabIndex, MsgBoxResult))
    End Sub
    Private Sub ReturnResult(result As MsgBoxResult)
        Me.SetValue(ResultPropertyKey, result)
        Me.SetValue(IsShowedPropertyKey, False)
    End Sub

    Public Shared Function Message(Optional detail As String = Nothing, Optional style As MsgBoxStyle? = Nothing, Optional title As String = Nothing) As Message
        Dim msg As New Message
        If detail IsNot Nothing Then msg.Detail = detail
        If style IsNot Nothing Then msg.MessageStyle = style
        If title IsNot Nothing Then msg.Title = title

        Dim dispatcher = msg.Dispatcher
        Dim window As Windows.Window = Nothing
        For Each w As Windows.Window In Application.Current.Windows
            If w.Dispatcher.Thread.ManagedThreadId = dispatcher.Thread.ManagedThreadId Then
                window = w
                Exit For
            End If
        Next
        If window Is Nothing Then window = Application.Current.MainWindow
        msg.ParentWindow = window

        Dim toplevelpanel = GetChildOf(Of Panel)(window, 5)
        If toplevelpanel IsNot Nothing Then
            toplevelpanel.Children.Add(msg)
            msg.TopLevelPanel = toplevelpanel
            'Dim ad = GetChildOf(Of AdornerDecorator)(window)
            'If ad IsNot Nothing Then
            '    ad.AdornerLayer.Add(New MessageAdorner(ad, msg) With {.Visibility = Visibility.Visible})
            '    ad.Visibility = Visibility.Visible

            AddHandler msg.MessageReturned, AddressOf DisposeMessage
        Else
            Dim returnTask As New Task(Of MsgBoxResult)(Function() MsgBox(detail, style, title))
            returnTask.ContinueWith(Sub(t) msg.Dispatcher.BeginInvoke(Sub() msg.ReturnResult(t.Result)))
            msg.MsgBoxReturnTask = returnTask
        End If
        MessageManager.Enqueue(msg)
        Return msg
    End Function
    Private Shared Sub DisposeMessage(msg As Message, e As MessageReturnedEventArgs)
        msg.TopLevelPanel.Children.Remove(msg)
    End Sub
    Private Shared Function GetChildOf(Of T As DependencyObject)(root As DependencyObject, Optional MaxDepth As Integer = Integer.MaxValue) As T
        If MaxDepth < 0 Then Return Nothing
        If TypeOf root Is T Then Return root
        Dim count As Integer = VisualTreeHelper.GetChildrenCount(root)
        For i = 0 To count - 1
            Dim child = VisualTreeHelper.GetChild(root, i)
            If TypeOf child Is T Then Return child
            Dim result = GetChildOf(Of T)(child, MaxDepth - 1)
            If result IsNot Nothing Then Return result
        Next
        Return Nothing
    End Function
End Class
Public Enum MessageButtonStyle
    OkOnly = 0
    OkCancel = 1
    AbortRetryIgnore
    YesNoCancel
    YesNo
    RetryCancel
End Enum
Public Enum MessageType
    Null = 0
    Critical = 1
    Question
    Information
    Exclamation
    InformationalExclamation
End Enum

Public Class MessageReturnedEventArgs
    Inherits EventArgs
    Public ReadOnly Result As MsgBoxResult

    Public Sub New(result As MsgBoxResult)
        Me.Result = result
    End Sub
End Class
Public Delegate Sub MessageReturnedEventHandler(sender As Object, e As MessageReturnedEventArgs)

Public Class MessageManager
    Private Shared ReadOnly MessageQueue As New Queue(Of Message)

    Private Sub New() : End Sub

    Public Shared Sub Enqueue(m As Message)
        MessageQueue.Enqueue(m)
        If MessageQueue.Count < 2 Then
            AddHandler m.MessageReturned, AddressOf HandleMessageReturned
            m.Show()
        End If
    End Sub

    Private Shared Sub HandleMessageReturned(sender As Object, e As MessageReturnedEventArgs)
        RemoveHandler MessageQueue.Peek.MessageReturned, AddressOf HandleMessageReturned
        Dim m = MessageQueue.Dequeue()
        If MessageQueue.Count > 0 Then
            AddHandler MessageQueue.Peek.MessageReturned, AddressOf HandleMessageReturned
            MessageQueue.Peek.Show()
        End If
    End Sub
End Class

Public Class MessageAdorner
    Inherits Adorner

    Private ReadOnly Property Element As UIElement
    Private ReadOnly Property Message As Message
    Public Sub New(element As UIElement, message As Message)
        MyBase.New(element)
        Me.Element = element
        Me.Message = message
    End Sub

    Protected Overrides ReadOnly Property VisualChildrenCount As Integer
        Get
            If Me.message Is Nothing Then Return 0 Else Return 1
        End Get
    End Property
    Protected Overrides Function GetVisualChild(index As Integer) As Visual
        If index = 0 And Me.Message IsNot Nothing Then Return Me.Message Else Return Nothing
    End Function
    Protected Overrides Function ArrangeOverride(finalSize As Size) As Size
        Me.Message.Arrange(New Rect(finalSize))
        MyBase.ArrangeOverride(finalSize)
        Return finalSize
    End Function
    Protected Overrides Function MeasureOverride(constraint As Size) As Size
        Me.Message.Measure(constraint)
        MyBase.MeasureOverride(constraint)
        Return constraint
    End Function
End Class
Imports System.ComponentModel

Public Class OpenFileDialogControl
    Inherits UserControl
    Implements ICommand

    <Category("Data")>
    <Description("The file path that open file dialog returned")>
    Public Property Result As String
        Get
            Return Me.GetValue(ResultProperty)
        End Get
        Set(value As String)
            Me.SetValue(ResultProperty, value)
        End Set
    End Property
    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), Bindable(True)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    <Description("Title for open file dialog"), Category("Appearance")>
    Public Property Title As String
        Get
            Return Me.GetValue(TitleProperty)
        End Get
        Set(value As String)
            Me.SetValue(TitleProperty, value)
        End Set
    End Property
    Public Property DefaultExt As String
        Get
            Return Me.GetValue(DefaultExtProperty)
        End Get
        Set(value As String)
            Me.SetValue(DefaultExtProperty, value)
        End Set
    End Property
    Public Property Filter As String
        Get
            Return Me.GetValue(FilterProperty)
        End Get
        Set(value As String)
            Me.SetValue(FilterProperty, value)
        End Set
    End Property
    Public Property FilterDescription As String
        Get
            Return Me.GetValue(FilterDescriptionProperty)
        End Get
        Set(value As String)
            Me.SetValue(FilterDescriptionProperty, value)
        End Set
    End Property


    Public Shadows ReadOnly Property Visibility As Visibility = Visibility.Hidden

    Private ReadOnly OpenFileDialog As New Microsoft.Win32.OpenFileDialog

    Public Shared ReadOnly ResultProperty As DependencyProperty = DependencyProperty.Register("Result", GetType(String), GetType(OpenFileDialogControl), New PropertyMetadata(""))
    Public Shared ReadOnly TitleProperty As DependencyProperty = DependencyProperty.Register("Title", GetType(String), GetType(OpenFileDialogControl), New PropertyMetadata(""))
    Public Shared ReadOnly DefaultExtProperty As DependencyProperty = DependencyProperty.Register("DefaultExt", GetType(String), GetType(OpenFileDialogControl), New PropertyMetadata(""))
    Public Shared ReadOnly FilterProperty As DependencyProperty = DependencyProperty.Register("Filter", GetType(String), GetType(OpenFileDialogControl), New PropertyMetadata(""))
    Public Shared ReadOnly FilterDescriptionProperty As DependencyProperty = DependencyProperty.Register("FilterDescription", GetType(String), GetType(OpenFileDialogControl), New PropertyMetadata(""))
    Public Shared Shadows ReadOnly VisibilityProperty As DependencyProperty = UserControl.VisibilityProperty.AddOwner(GetType(OpenFileDialogControl), New PropertyMetadata(Visibility.Hidden))
    Public Shared ReadOnly ReturnedEvent As RoutedEvent = EventManager.RegisterRoutedEvent("Returned", RoutingStrategy.Direct, GetType(RoutedEventHandler), GetType(OpenFileDialogControl))

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
    Public Custom Event Returned As RoutedEventHandler
        AddHandler(value As RoutedEventHandler)
            Me.AddHandler(ReturnedEvent, value)
        End AddHandler
        RemoveHandler(value As RoutedEventHandler)
            Me.RemoveHandler(ReturnedEvent, value)
        End RemoveHandler
        RaiseEvent(sender As Object, e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event

    Public Function GetFullFilter() As String
        Dim fullFilter As String = ""
        Dim fs = Me.Filter.Split("|")
        Dim fds = Me.FilterDescription.Split("|")
        If fs.Count = fds.Count And fs.Count > 0 Then
            For i = 0 To fs.Count - 1
                fullFilter &= fds(i) & " (" & fs(i).Replace(";", ", ") & ")|" & fs(i) & "|"
            Next
            If fullFilter.EndsWith("|") Then fullFilter = fullFilter.Remove(fullFilter.Length - 1)
        End If
        Return fullFilter
    End Function

    Public Sub New()
        Me.OpenFileDialog.CheckFileExists = True
    End Sub
    Public Sub Show()
        Me.OpenFileDialog.Title = Me.Title
        Me.OpenFileDialog.DefaultExt = Me.DefaultExt
        Me.OpenFileDialog.Filter = Me.GetFullFilter
        Me.OpenFileDialog.InitialDirectory = If(Me.Result = "" Or Not Me.Result.Contains(":"), AppDomain.CurrentDomain.BaseDirectory, Me.Result.Remove(Me.Result.LastIndexOf("\")))
        Dim result = Me.OpenFileDialog.ShowDialog
        If result Then
            Me.Result = If(Me.OpenFileDialog.FileName.StartsWith(AppDomain.CurrentDomain.BaseDirectory), Me.OpenFileDialog.FileName.Replace(AppDomain.CurrentDomain.BaseDirectory, ""), Me.OpenFileDialog.FileName)
            RaiseEvent Returned(Me, New RoutedEventArgs(ReturnedEvent))
        End If
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Show()
    End Sub
End Class

Imports System.ComponentModel
Imports System.Globalization

<Serializable>
Public Class SettingsModel
    Inherits Freezable
    Implements INotifyDataErrorInfo

    <Text.Json.Serialization.JsonInclude>
    Public Property ApplicationPath As String
        Get
            Return Me.GetValue(ApplicationPathProperty)
        End Get
        Set(value As String)
            Me.SetValue(ApplicationPathProperty, value)
        End Set
    End Property
    <Text.Json.Serialization.JsonInclude>
    Public Property ShellStatement As String
        Get
            Return Me.GetValue(ShellStatementProperty)
        End Get
        Set(value As String)
            Me.SetValue(ShellStatementProperty, value)
        End Set
    End Property
    <Text.Json.Serialization.JsonInclude>
    Public Property ConfiguratorPath As String
        Get
            Return Me.GetValue(ConfiguratorPathProperty)
        End Get
        Set(value As String)
            Me.SetValue(ConfiguratorPathProperty, value)
        End Set
    End Property
    <Text.Json.Serialization.JsonInclude>
    Public Property ProcessNameToMonitor As String
        Get
            Return Me.GetValue(ProcessNameToMonitorProperty)
        End Get
        Set(value As String)
            Me.SetValue(ProcessNameToMonitorProperty, value)
        End Set
    End Property
    <Text.Json.Serialization.JsonInclude>
    Public Property ProcessPriorityClass As ProcessPriorityClass
        Get
            Return GetValue(ProcessPriorityClassProperty)
        End Get

        Set(value As ProcessPriorityClass)
            SetValue(ProcessPriorityClassProperty, value)
        End Set
    End Property

    <Text.Json.Serialization.JsonInclude>
    Public Property DelayToMonitorProcess As Integer?
        Get
            Return GetValue(DelayToMonitorProcessProperty)
        End Get

        Set(value As Integer?)
            SetValue(DelayToMonitorProcessProperty, value)
        End Set
    End Property
    <Text.Json.Serialization.JsonInclude>
    Public Property MaxRetryCountForProcessMonitoring As Integer?
        Get
            Return GetValue(MaxRetryCountForProcessMonitoringProperty)
        End Get
        Set(value As Integer?)
            SetValue(MaxRetryCountForProcessMonitoringProperty, value)
        End Set
    End Property
    <Text.Json.Serialization.JsonInclude>
    Public Property RetryIntervalForProcessMonitoring As Integer?
        Get
            Return GetValue(RetryIntervalForProcessMonitoringProperty)
        End Get

        Set(value As Integer?)
            SetValue(RetryIntervalForProcessMonitoringProperty, value)
        End Set
    End Property
    <Text.Json.Serialization.JsonInclude>
    Public Property AutoBoot As Boolean
        Get
            Return Me.GetValue(AutoBootProperty)
        End Get
        Set(value As Boolean)
            Me.SetValue(AutoBootProperty, value)
        End Set
    End Property
    <Text.Json.Serialization.JsonInclude>
    Public Property AutoHide As Boolean
        Get
            Return Me.GetValue(AutoHideProperty)
        End Get
        Set(value As Boolean)
            Me.SetValue(AutoHideProperty, value)
        End Set
    End Property
    <Text.Json.Serialization.JsonInclude>
    Public Property AutoQuit As Boolean
        Get
            Return Me.GetValue(AutoQuitProperty)
        End Get
        Set(value As Boolean)
            Me.SetValue(AutoQuitProperty, value)
        End Set
    End Property
    <Text.Json.Serialization.JsonInclude>
    Public Property ScreenOrientation As ScreenOrientation
        Get
            Return Me.GetValue(ScreenOrientationProperty)
        End Get
        Set(value As ScreenOrientation)
            Me.SetValue(ScreenOrientationProperty, value)
        End Set
    End Property
    <Text.Json.Serialization.JsonInclude>
    Public Property ResolutionX As Integer?
        Get
            Return GetValue(ResolutionXProperty)
        End Get

        Set(value As Integer?)
            SetValue(ResolutionXProperty, value)
        End Set
    End Property
    <Text.Json.Serialization.JsonInclude>
    Public Property ResolutionY As Integer?
        Get
            Return GetValue(ResolutionYProperty)
        End Get

        Set(value As Integer?)
            SetValue(ResolutionYProperty, value)
        End Set
    End Property
    <Text.Json.Serialization.JsonInclude>
    Public Property DisplayFrequency As Integer?
        Get
            Return GetValue(DisplayFrequencyProperty)
        End Get

        Set(value As Integer?)
            SetValue(DisplayFrequencyProperty, value)
        End Set
    End Property
    <Text.Json.Serialization.JsonInclude>
    Public Property ShellAnotherTask As String
        Get
            Return GetValue(ShellAnotherTaskProperty)
        End Get

        Set(value As String)
            SetValue(ShellAnotherTaskProperty, value)
        End Set
    End Property
    <Text.Json.Serialization.JsonInclude>
    Public Property ShellPostTask As String
        Get
            Return GetValue(ShellPostTaskProperty)
        End Get

        Set(value As String)
            SetValue(ShellPostTaskProperty, value)
        End Set
    End Property
    <Text.Json.Serialization.JsonInclude>
    Public Property LanguageCultureCode As String
        Get
            Return GetValue(LanguageCultureTagProperty)
        End Get

        Set(value As String)
            SetValue(LanguageCultureTagProperty, value)
        End Set
    End Property

    Private ReadOnly Property SettingsPath As String

    Public Shared ReadOnly ApplicationPathProperty As DependencyProperty = DependencyProperty.Register("ApplicationPath", GetType(String), GetType(SettingsModel))
    Public Shared ReadOnly ShellStatementProperty As DependencyProperty = DependencyProperty.Register("ShellStatement", GetType(String), GetType(SettingsModel))
    Public Shared ReadOnly ConfiguratorPathProperty As DependencyProperty = DependencyProperty.Register("ConfiguratorPathPath", GetType(String), GetType(SettingsModel))
    Public Shared ReadOnly ProcessNameToMonitorProperty As DependencyProperty = DependencyProperty.Register("ProcessNameToMonitorPath", GetType(String), GetType(SettingsModel))
    Public Shared ReadOnly ProcessPriorityClassProperty As DependencyProperty = DependencyProperty.Register("ProcessPriorityClass", GetType(ProcessPriorityClass), GetType(SettingsModel), New PropertyMetadata(ProcessPriorityClass.Normal))
    Public Shared ReadOnly DelayToMonitorProcessProperty As DependencyProperty = DependencyProperty.Register("DelayToMonitorProcess", GetType(Integer?), GetType(SettingsModel), New PropertyMetadata(Nothing))
    Public Shared ReadOnly MaxRetryCountForProcessMonitoringProperty As DependencyProperty = DependencyProperty.Register("MaxRetryCountForProcessMonitoring", GetType(Integer?), GetType(SettingsModel), New PropertyMetadata(Nothing))
    Public Shared ReadOnly RetryIntervalForProcessMonitoringProperty As DependencyProperty = DependencyProperty.Register("RetryIntervalForProcessMonitoring", GetType(Integer?), GetType(SettingsModel), New PropertyMetadata(Nothing))
    Public Shared ReadOnly AutoBootProperty As DependencyProperty = DependencyProperty.Register("AutoBoot", GetType(Boolean), GetType(SettingsModel))
    Public Shared ReadOnly AutoHideProperty As DependencyProperty = DependencyProperty.Register("AutoHide", GetType(Boolean), GetType(SettingsModel))
    Public Shared ReadOnly AutoQuitProperty As DependencyProperty = DependencyProperty.Register("AutoQuit", GetType(Boolean), GetType(SettingsModel))
    Public Shared ReadOnly ScreenOrientationProperty As DependencyProperty = DependencyProperty.Register("ScreenOrientation", GetType(ScreenOrientation), GetType(SettingsModel), New PropertyMetadata(ScreenOrientation.Null))
    Public Shared ReadOnly ResolutionXProperty As DependencyProperty = DependencyProperty.Register("ResolutionX", GetType(Integer?), GetType(SettingsModel), New PropertyMetadata(Nothing))
    Public Shared ReadOnly ResolutionYProperty As DependencyProperty = DependencyProperty.Register("ResolutionY", GetType(Integer?), GetType(SettingsModel), New PropertyMetadata(Nothing))
    Public Shared ReadOnly DisplayFrequencyProperty As DependencyProperty = DependencyProperty.Register("DisplayFrequency", GetType(Integer?), GetType(SettingsModel), New PropertyMetadata(Nothing))
    Public Shared ReadOnly ShellAnotherTaskProperty As DependencyProperty = DependencyProperty.Register("ShellAnotherTask", GetType(String), GetType(SettingsModel), New PropertyMetadata(""))
    Public Shared ReadOnly ShellPostTaskProperty As DependencyProperty = DependencyProperty.Register("ShellPostTask", GetType(String), GetType(SettingsModel), New PropertyMetadata(""))
    Public Shared ReadOnly LanguageCultureTagProperty As DependencyProperty = DependencyProperty.Register("LanguageCultureTag", GetType(String), GetType(SettingsModel), New PropertyMetadata("-", AddressOf HandleLanguageCultureTagChanged))

    Public ReadOnly Property HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
        Get
            Return False
        End Get
    End Property

    Protected Overrides Sub OnPropertyChanged(e As DependencyPropertyChangedEventArgs)
        MyBase.OnPropertyChanged(e)
        If e.OldValue IsNot Nothing AndAlso e.NewValue IsNot Nothing AndAlso Not e.OldValue.Equals(e.NewValue) Then Save(Me)
    End Sub

    Public Event ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged

    Public Sub New(settingsPath As String)
        MyBase.New
        Me.SettingsPath = settingsPath
        If FileIO.FileSystem.FileExists(settingsPath) Then
            Populate(FileIO.FileSystem.ReadAllText(settingsPath, Text.Encoding.UTF8), Me)
        Else
            Save(Me)
        End If
    End Sub

    Public Function GetErrors(propertyName As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors
        Throw New NotImplementedException()
    End Function

    Protected Overrides Function CreateInstanceCore() As Freezable
        Return Text.Json.JsonSerializer.Deserialize(Of SettingsModel)(Text.Json.JsonSerializer.Serialize(Me, JsonSerializationOptions), JsonSerializationOptions)
    End Function

    Private Shared ReadOnly JsonSerializationOptions As New Text.Json.JsonSerializerOptions With {
        .WriteIndented = True,
        .IgnoreReadOnlyFields = True,
        .IgnoreReadOnlyProperties = True
    }
    Private Shared Sub Populate(info As String, ByRef settings As SettingsModel)
        Dim setter = Text.Json.JsonSerializer.Deserialize(Of Text.Json.JsonDocument)(info, JsonSerializationOptions).RootElement.EnumerateObject
        For Each i In setter
            Dim p = settings.GetType.GetProperty(i.Name)
            If p IsNot Nothing And p.CanWrite Then p.SetValue(settings, Text.Json.JsonSerializer.Deserialize(i.Value.GetRawText, p.PropertyType))
        Next
    End Sub
    Private Shared Sub Save(settings As SettingsModel)
        Try
            If settings.SettingsPath IsNot Nothing And settings.SettingsPath <> "" Then FileIO.FileSystem.WriteAllText(settings.SettingsPath, Text.Json.JsonSerializer.Serialize(settings, JsonSerializationOptions), False, Text.Encoding.UTF8)
        Catch
            MsgBox("Cannot write settings to """ & settings.SettingsPath & """", MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, "Save settings failed")
        End Try
    End Sub
    Private Shared Sub HandleLanguageCultureTagChanged(sender As SettingsModel, e As DependencyPropertyChangedEventArgs)
        If e.NewValue = "-" Then LocalizationHelper.Service.SetCulture(CultureInfo.InstalledUICulture) _
        Else LocalizationHelper.Service.SetCulture(LocalizationHelper.CultureList(LocalizationHelper.CultureTagList.IndexOf(e.NewValue)))
    End Sub
End Class

Public Enum ScreenOrientation As Integer
    Null = 0
    Degree0 = 1
    Degree90Clockwise
    Degree180Clockwise
    Degree270Clockwise
End Enum
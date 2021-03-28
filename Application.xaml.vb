Imports System.Globalization
Imports Microsoft.Win32

Class Application

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

    Public Event ToHide As HandableEventHandler
    Public Event ToExit As HandableEventHandler

    Public ReadOnly Property SystemTheme As SystemTheme
        Get
            If OperatingSystem.IsWindowsVersionAtLeast(10) Then
                Return If(Registry.GetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 0) = 0, SystemTheme.Dark, SystemTheme.Light)
            Else

            End If
        End Get
    End Property

    Public Event SystemThemeChangedEvent As SystemThemeChangedEventHandler

    Dim tc As New ThemedColor With {
        .LightColor = Color.FromArgb(255, 0, 0, 0),
        .DarkColor = Color.FromArgb(255, 255, 255, 255)
    }

    Public Sub New()
        MyBase.New
        Dim darkTheme As New ResourceDictionary With {.Source = New Uri("/Resources/ColorThemes/DarkColorTheme.xaml", UriKind.Relative)}
        Dim lightTheme As New ResourceDictionary With {.Source = New Uri("/Resources/ColorThemes/LightColorTheme.xaml", UriKind.Relative)}
        Me.Resources.MergedDictionaries.Add(darkTheme)
        Me.Resources.MergedDictionaries.Add(lightTheme)

        Me.HandleSystemUserPreferenceChanged(Me, New UserPreferenceChangedEventArgs(UserPreferenceCategory.Color))
        Me.OnSystemThemeChanged()
        AddHandler SystemEvents.UserPreferenceChanged, AddressOf Me.HandleSystemUserPreferenceChanged
    End Sub

    Private Sub HandleToHideButtonClicked(sender As Object, e As RoutedEventArgs)
        Dim window = GetParentOf(Of MainWindow)(sender)
        If window IsNot Nothing Then Me.OnToHide(New HandableWindowEventArgs(window))
    End Sub
    Private Sub HandleToExitButtonClicked(sender As Object, e As RoutedEventArgs)

        Dim Window = GetParentOf(Of Window)(sender)
        If Window IsNot Nothing Then Me.OnToExit(New HandableWindowEventArgs(Window))
    End Sub

    Protected Sub OnToHide(e As HandableWindowEventArgs)
        RaiseEvent ToHide(Me, e)
        If Not e.Handled Then If e.TargetWindow.Equals(Application.Current.MainWindow) Then e.TargetWindow.Hide() Else e.TargetWindow.WindowState = WindowState.Minimized
    End Sub
    Protected Sub OnToExit(e As HandableWindowEventArgs)
        RaiseEvent ToExit(Me, e)
        If Not e.Handled Then If e.TargetWindow.Equals(Application.Current.MainWindow) Then Application.Current.Shutdown() Else e.TargetWindow.Close()
    End Sub

    Private Sub HandleSystemUserPreferenceChanged(sender As Object, e As UserPreferenceChangedEventArgs)
        Me.OnSystemThemeChanged()
    End Sub

    Private Sub OnSystemThemeChanged()
        Static lastTheme As SystemTheme? = Nothing
        If lastTheme Is Nothing OrElse lastTheme <> Me.SystemTheme Then
            RaiseEvent SystemThemeChangedEvent(Me, New SystemThemeChangedEventArgs(Me.SystemTheme))
            lastTheme = Me.SystemTheme
        End If
    End Sub

    Private Shared Function GetParentOf(Of T As DependencyObject)(element As DependencyObject) As T
        Dim parent = element
        Do While parent IsNot Nothing
            If TypeOf parent Is T Then Return parent
            parent = VisualTreeHelper.GetParent(parent)
        Loop
        Return Nothing
    End Function
End Class

Public Class HandableWindowEventArgs
    Inherits EventArgs
    Public Property Handled As Boolean = False
    Public ReadOnly Property TargetWindow As Window

    Public Sub New(target As Window)
        Me.TargetWindow = target
    End Sub
End Class

Public Enum SystemTheme
    Light = 0
    Dark = 1
End Enum
Public Class SystemThemeChangedEventArgs
    Public ReadOnly Property SystemTheme As SystemTheme

    Public Sub New(theme As SystemTheme)
        Me.SystemTheme = theme
    End Sub
End Class

Public Delegate Sub SystemThemeChangedEventHandler(sender As Object, e As SystemThemeChangedEventArgs)

Public Delegate Sub HandableEventHandler(sender As Object, e As HandableWindowEventArgs)

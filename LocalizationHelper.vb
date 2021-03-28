Imports System.ComponentModel
Imports System.Globalization

Public Class LocalizationHelper
    Public Shared ReadOnly Property Service As New LocalizationService
    Public Shared ReadOnly Property CultureList As List(Of CultureInfo)
        Get
            Dim result As New List(Of CultureInfo)
            Dim allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
            For Each c In allCultures
                If c.LCID = 127 Then Continue For
                If My.Resources.Localization.ResourceManager.GetResourceSet(c, True, False) IsNot Nothing Then result.Add(c)
            Next
            Return result
        End Get
    End Property
    Public Shared ReadOnly Property CultureNameList As List(Of String)
        Get
            Dim result As New List(Of String)
            For Each c In CultureList
                result.Add(c.NativeName)
            Next
            Return result
        End Get
    End Property
    Public Shared ReadOnly Property CultureTagList As List(Of String)
        Get
            Dim result As New List(Of String)
            For Each c In CultureList
                result.Add(c.IetfLanguageTag)
            Next
            Return result
        End Get
    End Property
    Public Shared ReadOnly Property CultureTagToCultureIndexConverter As New ValueConverter(Of String, Integer)(Function(t, p, c) If(t = "-", 0, CultureTagList.IndexOf(t) + 1), Function(s, p, c) If(s = 0, "-", CultureTagList(s - 1)), 0, "-")
End Class
Public Class LocalizationService
    Implements INotifyPropertyChanged

    Public ReadOnly Property Localization As New My.Resources.Localization

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Sub SetCulture(culture As CultureInfo)
        My.Resources.Localization.Culture = culture
        Me.OnPropertyChanged(New PropertyChangedEventArgs("Localization"))
    End Sub
    Protected Sub OnPropertyChanged(e As PropertyChangedEventArgs)
        RaiseEvent PropertyChanged(Me, e)
    End Sub
End Class
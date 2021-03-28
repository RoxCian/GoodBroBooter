Imports System.Globalization

Public Class CueBannerNumericBox
    Inherits CueBannerTextBox


    Public Property Value As Integer?
        Get
            Return GetValue(ValueProperty)
        End Get
        Set(value As Integer?)
            SetValue(ValueProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Integer?), GetType(CueBannerNumericBox), New PropertyMetadata(Nothing, AddressOf h_valueChanged))
    Public Shared Shadows ReadOnly TextProperty As DependencyProperty = TextBox.TextProperty.AddOwner(GetType(CueBannerNumericBox), New FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.Inherits, AddressOf h_textChanged))

    Protected Shadows Function GetValue(dp As DependencyProperty)
        Return MyBase.GetValue(dp)
    End Function

    Protected Shadows Sub SetValue(dp As DependencyProperty, value As Object)
        If dp.Equals(TextProperty) Then

        End If
        MyBase.SetValue(dp, value)
    End Sub

    Public Sub New()
        MyBase.New
        'Dim b As New Binding
        'b.Source = Me
        'b.Path = New PropertyPath("Text")
        'b.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus

        'b.Converter = New ValueConverter(Of String, Integer?)(Function(s, o, c) If(s = "" Or s Is Nothing Or Not IsNumeric(s), Nothing, CInt(s)), Function(n, o, c) If(n Is Nothing, "", n.ToString))
        'Me.SetBinding(ValueProperty, b)
    End Sub

    Protected Overrides Sub OnPreviewTextInput(e As TextCompositionEventArgs)
        If e.Text = "" Or System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^\d|([1-9]\d+)") Then
            MyBase.OnPreviewTextInput(e)
        Else
            e.Handled = True
            Media.SystemSounds.Beep.Play()
        End If
    End Sub
    Private Shared Sub h_valueChanged(sender As CueBannerNumericBox, e As DependencyPropertyChangedEventArgs)
        If e.NewValue <> e.OldValue Then sender.Text = If(e.NewValue Is Nothing, "", e.NewValue.ToString)
    End Sub
    Private Shared Sub h_textChanged(sender As CueBannerNumericBox, e As DependencyPropertyChangedEventArgs)
        If e.NewValue <> e.OldValue Then
            If e.NewValue = "" Or e.NewValue Is Nothing Then
                sender.Value = Nothing
            ElseIf Not System.Text.RegularExpressions.Regex.IsMatch(e.NewValue, "^\d|([1-9]\d+)") Then
                sender.Value = Nothing
                sender.Text = ""
                Media.SystemSounds.Beep.Play()
            Else
                Try
                    sender.Value = CType(CInt(e.NewValue), Integer?)
                Catch
                    Media.SystemSounds.Beep.Play()
                End Try
                sender.Text = sender.Value
            End If
        End If
    End Sub
End Class

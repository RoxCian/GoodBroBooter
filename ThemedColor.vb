Imports System.Windows.Media.Animation

Public Class ThemedColor
    Inherits Freezable

    Public Property LightColor As Color
        Get
            Return GetValue(LightColorProperty)
        End Get

        Set(ByVal value As Color)
            SetValue(LightColorProperty, value)
        End Set
    End Property
    Public Property DarkColor As Color
        Get
            Return GetValue(DarkColorProperty)
        End Get

        Set(ByVal value As Color)
            SetValue(DarkColorProperty, value)
        End Set
    End Property
    Public Overridable ReadOnly Property Color As Color
        Get
            Return GetValue(ColorProperty)
        End Get
    End Property
    Public Overridable ReadOnly Property Brush As SolidColorBrush
        Get
            Return GetValue(BrushProperty)
        End Get
    End Property

    Public Shared ReadOnly LightColorProperty As DependencyProperty = DependencyProperty.Register("LightColor", GetType(Color), GetType(ThemedColor), New PropertyMetadata(Colors.White))
    Public Shared ReadOnly DarkColorProperty As DependencyProperty = DependencyProperty.Register("DarkColor", GetType(Color), GetType(ThemedColor), New PropertyMetadata(Colors.Black))
    Private Shared ReadOnly ColorPropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("Color", GetType(Color), GetType(ThemedColor), New PropertyMetadata(Colors.White))
    Public Shared ReadOnly ColorProperty As DependencyProperty = ColorPropertyKey.DependencyProperty
    Private Shared ReadOnly BrushPropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("Brush", GetType(SolidColorBrush), GetType(ThemedColor), New PropertyMetadata(New SolidColorBrush(Colors.White)))
    Public Shared ReadOnly BrushProperty As DependencyProperty = BrushPropertyKey.DependencyProperty

    Public Sub New()
        MyBase.New()
        Me.HandleSystemThemeChanged(Me, New SystemThemeChangedEventArgs(CType(Application.Current, Application).SystemTheme))
        AddHandler CType(Application.Current, Application).SystemThemeChangedEvent, AddressOf Me.HandleSystemThemeChanged
    End Sub
    Public Sub New(lightColor As Color, darkColor As Color)
        Me.New()
        Me.LightColor = lightColor
        Me.DarkColor = darkColor
    End Sub

    Private Sub HandleSystemThemeChanged(sender As Object, e As SystemThemeChangedEventArgs)
        Try
            If e.SystemTheme = SystemTheme.Dark Then
                Me.SetValue(BrushPropertyKey, New SolidColorBrush(Me.DarkColor))
                Me.SetValue(ColorPropertyKey, Me.DarkColor)
            Else
                Me.SetValue(BrushPropertyKey, New SolidColorBrush(Me.LightColor))
                Me.SetValue(ColorPropertyKey, Me.LightColor)
            End If
        Catch
        End Try
    End Sub

    Public Shared Narrowing Operator CType(tc As ThemedColor) As Color
        Return tc.Color
    End Operator

    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New ThemedColor(Me.LightColor, Me.DarkColor)
    End Function
End Class

Public Class ThemedColorFreezable
    Inherits ThemedColor

    Public ReadOnly Property Color As Color
        Get
            If CType(Application.Current, Application).SystemTheme = SystemTheme.Dark Then Return Me.DarkColor Else Return Me.LightColor
        End Get
    End Property

    Public Sub New(lightColor As Color, darkColor As Color)
        MyBase.New()
        Me.LightColor = lightColor
        Me.DarkColor = darkColor
    End Sub
End Class


Public Class ThemedColorConverter
    Inherits ValueConverter(Of ThemedColor, Color)
    Public Sub New()
        MyBase.New(Function(tc, p, c) tc.Color)
    End Sub
End Class

Public Class ThemedColorAnimation
    Inherits ColorAnimationBase


    Public Property From As ThemedColor
        Get
            Return GetValue(FromProperty)
        End Get

        Set(ByVal value As ThemedColor)
            SetValue(FromProperty, value)
        End Set
    End Property
    Public Property [To] As ThemedColor
        Get
            Return GetValue(ToProperty)
        End Get

        Set(ByVal value As ThemedColor)
            SetValue(ToProperty, value)
        End Set
    End Property
    Public Property EasingFunction As IEasingFunction
        Get
            Return GetValue(EasingFunctionProperty)
        End Get
        Set(value As IEasingFunction)
            SetValue(EasingFunctionProperty, value)
        End Set
    End Property

    Public Shared ReadOnly FromProperty As DependencyProperty =
                           DependencyProperty.Register("From",
                           GetType(ThemedColor), GetType(ThemedColorAnimation),
                           New PropertyMetadata(Nothing))

    Public Shared ReadOnly ToProperty As DependencyProperty =
                           DependencyProperty.Register("To",
                           GetType(ThemedColor), GetType(ThemedColorAnimation),
                           New PropertyMetadata(Nothing))
    Public Shared ReadOnly EasingFunctionProperty As DependencyProperty =
                           DependencyProperty.Register("EasingFunction",
                           GetType(IEasingFunction), GetType(ThemedColorAnimation),
                           New PropertyMetadata(Nothing))

    Public Sub New()
        MyBase.New
    End Sub
    Public Sub New(Optional from As ThemedColor = Nothing, Optional [to] As ThemedColor = Nothing)
        Me.From = from
        Me.To = [to]
    End Sub

    Protected Overrides Function GetCurrentValueCore(defaultOriginValue As Color, defaultDestinationValue As Color, animationClock As AnimationClock) As Color
        Dim o = If(Me.From IsNot Nothing, Me.From.Color, defaultOriginValue)
        Dim d = If(Me.To IsNot Nothing, Me.To.Color, defaultDestinationValue)
        Return Color.FromArgb(o.A + (d.A - o.A) * animationClock.CurrentProgress, o.R + (d.R - o.R) * animationClock.CurrentProgress, o.G + (d.G - o.G) * animationClock.CurrentProgress, o.B + (d.B - o.B) * animationClock.CurrentProgress)
    End Function

    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New ThemedColorAnimation(Me.From, Me.To)
    End Function
End Class
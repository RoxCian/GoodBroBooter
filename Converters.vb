Imports System.Globalization

Public Class ValueConverter(Of TSource, TTarget)
    Implements IValueConverter

    Public Property Converter As Func(Of TSource, Object, CultureInfo, TTarget)
    Public Property ConvertBackConverter As Func(Of TTarget, Object, CultureInfo, TSource)
    Public Property FallbackTarget As TTarget
    Public Property FallbackSource As TSource

    Public Sub New(Optional converter As Func(Of TSource, Object, CultureInfo, TTarget) = Nothing, Optional convertBackConverter As Func(Of TTarget, Object, CultureInfo, TSource) = Nothing, Optional fallbackTarget As TTarget = Nothing, Optional fallbackSource As TSource = Nothing)
        Me.Converter = converter
        Me.ConvertBackConverter = convertBackConverter
        Me.FallbackTarget = fallbackTarget
        Me.FallbackSource = fallbackSource
    End Sub


    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If Me.Converter IsNot Nothing Then
            If targetType.IsAssignableTo(GetType(TTarget)) Then Return Me.Converter.Invoke(value, parameter, culture) Else Return Me.FallbackTarget
        End If
        Return value
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        If Me.ConvertBackConverter IsNot Nothing Then
            If targetType.IsAssignableTo(GetType(TSource)) Then Return Me.ConvertBackConverter.Invoke(value, parameter, culture) Else Return Me.FallbackSource
        End If
        Return value
    End Function
End Class

Public Class AddValueConverter
    Inherits DependencyObject
    Implements IValueConverter

    Public Property Operand As Double
        Get
            Return GetValue(OperandProperty)
        End Get

        Set(ByVal value As Double)
            SetValue(OperandProperty, value)
        End Set
    End Property

    Public Shared ReadOnly OperandProperty As DependencyProperty =
                           DependencyProperty.Register("Operand",
                           GetType(Double), GetType(AddValueConverter),
                           New PropertyMetadata(0.0))


    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Return value + Me.Operand
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return value - Me.Operand
    End Function
End Class

Public Class InvertConverter
    Inherits ValueConverter(Of Boolean, Boolean)

    Public Sub New()
        MyBase.New(Function(v, p, c) Not v, Function(v, p, c) Not v)
    End Sub
End Class

Public Class EnumValueConverter
    Implements IValueConverter

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        If targetType.IsAssignableTo(GetType([Enum])) And (IsNumeric(value) Or TypeOf value Is String) Then Return [Enum].Parse(targetType, value)
        Return Nothing
    End Function

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If TypeOf value Is [Enum] Then Return System.Convert.ChangeType(value, [Enum].GetUnderlyingType(value.GetType))
        Return Nothing
    End Function
End Class

Public Class PriorityIndexToPriorityClassEnumConverter
    Inherits ValueConverter(Of ProcessPriorityClass, Integer)
    Public Sub New()
        MyBase.New(Function(t, p, c)
                       If t And ProcessPriorityClass.RealTime = t Then
                           Return 0
                       ElseIf t And ProcessPriorityClass.High = t Then
                           Return 1
                       ElseIf t And ProcessPriorityClass.AboveNormal = t Then
                           Return 2
                       ElseIf t And ProcessPriorityClass.Normal = t Then
                           Return 3
                       ElseIf t And ProcessPriorityClass.BelowNormal = t Then
                           Return 4
                       Else
                           Return 5
                       End If
                   End Function,
                   Function(s, p, c)
                       Select Case s
                           Case 0 : Return ProcessPriorityClass.RealTime
                           Case 1 : Return ProcessPriorityClass.High
                           Case 2 : Return ProcessPriorityClass.AboveNormal
                           Case 3 : Return ProcessPriorityClass.Normal
                           Case 4 : Return ProcessPriorityClass.BelowNormal
                           Case Else : Return ProcessPriorityClass.Idle
                       End Select
                   End Function)
    End Sub
End Class


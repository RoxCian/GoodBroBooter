Public Class ColorizedButton
    Public Property MouseOverBackground As Brush
        Get
            Return GetValue(MouseOverBackgroundProperty)
        End Get

        Set(value As Brush)
            SetValue(MouseOverBackgroundProperty, value)
        End Set
    End Property

    Public Property MouseOverForeground As Brush
        Get
            Return GetValue(MouseOverForegroundProperty)
        End Get

        Set(value As Brush)
            SetValue(MouseOverForegroundProperty, value)
        End Set
    End Property

    Public Property MouseOverBorderBrush As Brush
        Get
            Return GetValue(MouseOverBorderBrushProperty)
        End Get

        Set(value As Brush)
            SetValue(MouseOverBorderBrushProperty, value)
        End Set
    End Property

    Public Property PressedBackground As Brush
        Get
            Return GetValue(PressedBackgroundProperty)
        End Get

        Set(value As Brush)
            SetValue(PressedBackgroundProperty, value)
        End Set
    End Property

    Public Property PressedForeground As Brush
        Get
            Return GetValue(PressedForegroundProperty)
        End Get

        Set(value As Brush)
            SetValue(PressedForegroundProperty, value)
        End Set
    End Property

    Public Property PressedBorderBrush As Brush
        Get
            Return GetValue(PressedBorderBrushProperty)
        End Get

        Set(value As Brush)
            SetValue(PressedBorderBrushProperty, value)
        End Set
    End Property

    Public Property ContentMargin As Thickness
        Get
            Return GetValue(ContentMarginProperty)
        End Get

        Set(value As Thickness)
            SetValue(ContentMarginProperty, value)
        End Set
    End Property

    Public Shared ReadOnly MouseOverBackgroundProperty As DependencyProperty = DependencyProperty.Register("MouseOverBackground", GetType(Brush), GetType(ColorizedButton), New PropertyMetadata(New SolidColorBrush(Colors.SkyBlue)))
    Public Shared ReadOnly MouseOverForegroundProperty As DependencyProperty = DependencyProperty.Register("MouseOverForeground", GetType(Brush), GetType(ColorizedButton), New PropertyMetadata(New SolidColorBrush(Colors.White)))
    Public Shared ReadOnly MouseOverBorderBrushProperty As DependencyProperty = DependencyProperty.Register("MouseOverBorderBrush", GetType(Brush), GetType(ColorizedButton), New PropertyMetadata(New SolidColorBrush(Colors.White)))
    Public Shared ReadOnly PressedBackgroundProperty As DependencyProperty = DependencyProperty.Register("PressedBackground", GetType(Brush), GetType(ColorizedButton), New PropertyMetadata(New SolidColorBrush(Color.FromRgb(103, 174, 203))))
    Public Shared ReadOnly PressedForegroundProperty As DependencyProperty = DependencyProperty.Register("PressedForeground", GetType(Brush), GetType(ColorizedButton), New PropertyMetadata(New SolidColorBrush(Colors.White)))
    Public Shared ReadOnly PressedBorderBrushProperty As DependencyProperty = DependencyProperty.Register("PressedBorderBrush", GetType(Brush), GetType(ColorizedButton), New PropertyMetadata(New SolidColorBrush(Colors.White)))
    Public Shared ReadOnly ContentMarginProperty As DependencyProperty = DependencyProperty.Register("ContentMargin", GetType(Thickness), GetType(ColorizedButton), New PropertyMetadata(New Thickness))
End Class

Public Class CueBannerTextBox
    Public Property CueBannerText As String
        Get
            Return GetValue(CueBannerTextProperty)
        End Get

        Set(ByVal value As String)
            SetValue(CueBannerTextProperty, value)
        End Set
    End Property

    Public Property CueBannerForeground As Brush
        Get
            Return GetValue(CueBannerForegroundProperty)
        End Get

        Set(ByVal value As Brush)
            SetValue(CueBannerForegroundProperty, value)
        End Set
    End Property

    Public Property CueBannerFontFamily As FontFamily
        Get
            Return GetValue(CueBannerFontFamilyProperty)
        End Get

        Set(ByVal value As FontFamily)
            SetValue(CueBannerFontFamilyProperty, value)
        End Set
    End Property

    Public Property CueBannerFontSize As Double
        Get
            Return GetValue(CueBannerFontSizeProperty)
        End Get

        Set(ByVal value As Double)
            SetValue(CueBannerFontSizeProperty, value)
        End Set
    End Property

    Public Property CueBannerFontStyle As FontStyle
        Get
            Return GetValue(CueBannerFontStyleProperty)
        End Get

        Set(ByVal value As FontStyle)
            SetValue(CueBannerFontStyleProperty, value)
        End Set
    End Property
    Public Shared ReadOnly CueBannerTextProperty As DependencyProperty = DependencyProperty.Register("CueBannerText", GetType(String), GetType(CueBannerTextBox), New PropertyMetadata("Banner"))
    Public Shared ReadOnly CueBannerForegroundProperty As DependencyProperty = DependencyProperty.Register("CueBannerForeground", GetType(Brush), GetType(CueBannerTextBox), New PropertyMetadata(New SolidColorBrush(Colors.Gray)))
    Public Shared ReadOnly CueBannerFontFamilyProperty As DependencyProperty = DependencyProperty.Register("CueBannerFontFamily", GetType(FontFamily), GetType(CueBannerTextBox), New PropertyMetadata(TextBox.FontFamilyProperty.DefaultMetadata.DefaultValue))
    Public Shared ReadOnly CueBannerFontSizeProperty As DependencyProperty = DependencyProperty.Register("CueBannerFontSize", GetType(Double), GetType(CueBannerTextBox), New PropertyMetadata(TextBox.FontSizeProperty.DefaultMetadata.DefaultValue))
    Public Shared ReadOnly CueBannerFontStyleProperty As DependencyProperty = DependencyProperty.Register("CueBannerFontStyle", GetType(FontStyle), GetType(CueBannerTextBox), New PropertyMetadata(TextBox.FontStyleProperty.DefaultMetadata.DefaultValue))

End Class

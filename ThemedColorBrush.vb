Imports System.Reflection

Public Class ThemedColorBrush
    Inherits Brush

    Public Property Color As ThemedColor
        Get
            Return GetValue(ColorProperty)
        End Get

        Set(ByVal value As ThemedColor)
            SetValue(ColorProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ColorProperty As DependencyProperty = DependencyProperty.Register("Color", GetType(ThemedColor), GetType(ThemedColorBrush), New PropertyMetadata(New ThemedColor))

    Public Sub New()
        MyBase.New
    End Sub
    Public Sub New(color As ThemedColor)
        Me.Color = color
    End Sub

    Protected Overrides Function CreateInstanceCore() As Freezable
        Return New ThemedColorBrush(Me.Color)
    End Function

    Private Shared Sub BuildType(b As ThemedColorBrush)

    End Sub

    Friend Sub UpdateResourceOverride(channel As Object, skipOnChannelCheck As Boolean) ' channel As System.Windows.Media.DUCE.Channel
        If skipOnChannelCheck Or _duceResource.IsOnChannel(channel)) Then
            {
                CType(Me, Object).UpdateResource(channel, skipOnChannelCheck)
            Dim vTransform As Transform = Me.Transform
            Dim vRelativeTransform As Transform = RelativeTransform

            Dim hTransform As Object ' As DUCE.ResourceHandle
            If vTransform Is Nothing Or Object.ReferenceEquals(vTransform, Transform.Identity) Then
                hTransform = Convert.ChangeType(0, DUCENs.GetType("ResourceHandle"))
            Else
                hTransform = Convert.ChangeType(vTransform, DUCENs.GetType("IResource")).GetHandle(channel)
            End If

            Dim hRelativeTransform As Object ' As DUCE.ResourceHandle 
            If vRelativeTransform Is Nothing Or Object.ReferenceEquals(vRelativeTransform, Transform.Identity) Then
                hRelativeTransform = DUCE.ResourceHandle.Null;
                
                Else
                {
                    hRelativeTransform = ((DUCE.IResource)vRelativeTransform).GetHandle(channel);
                }

                // Obtain handles for animated properties
                DUCE.ResourceHandle hOpacityAnimations = GetAnimationResourceHandle(OpacityProperty, channel);
                DUCE.ResourceHandle hColorAnimations = GetAnimationResourceHandle(ColorProperty, channel);

                // Pack & send command packet
                DUCE.MILCMD_SOLIDCOLORBRUSH Data;
                unsafe
                {
                    Data.Type = MILCMD.MilCmdSolidColorBrush;
                    Data.Handle = _duceResource.GetHandle(channel);
                    If (hOpacityAnimations.IsNull) Then
                                            {
                        Data.Opacity = Opacity;
                    }
                    Data.hOpacityAnimations = hOpacityAnimations;
                    Data.hTransform = hTransform;
                    Data.hRelativeTransform = hRelativeTransform;
                    If (hColorAnimations.IsNull) Then
                                                {
                        Data.Color = CompositionResourceManager.ColorToMilColorF(Color);
                    }
                    Data.hColorAnimations = hColorAnimations;

                    // Send packed command structure
                    channel.SendCommand(
                        (Byte *) & Data,
                        sizeof(DUCE.MILCMD_SOLIDCOLORBRUSH));
                }
            End If
        End Sub
        internal override DUCE.ResourceHandle AddRefOnChannelCore(DUCE.Channel channel)
        {
                If (_duceResource.CreateOrAddRefOnChannel(this, channel, System.Windows.Media.Composition.DUCE.ResourceType.TYPE_SOLIDCOLORBRUSH))
                {
                    Transform vTransform = Transform;
                    If (vTransform! = null)((DUCE.IResource)vTransform).AddRefOnChannel(channel);
                    Transform vRelativeTransform = RelativeTransform;
                    If (vRelativeTransform! = null)((DUCE.IResource)vRelativeTransform).AddRefOnChannel(channel);

                    AddRefOnChannelAnimations(channel);


                    UpdateResource(channel, true /* skip "on channel" check - we already know that we're on channel */ );
                }

                Return _duceResource.GetHandle(channel);
}
        internal override void ReleaseOnChannelCore(DUCE.Channel channel)
        {
                Debug.Assert(_duceResource.IsOnChannel(channel));

                If (_duceResource.ReleaseOnChannel(channel))
                {
                    Transform vTransform = Transform;
                    If (vTransform! = null)((DUCE.IResource)vTransform).ReleaseOnChannel(channel);
                    Transform vRelativeTransform = RelativeTransform;
                    If (vRelativeTransform! = null)((DUCE.IResource)vRelativeTransform).ReleaseOnChannel(channel);

                    ReleaseOnChannelAnimations(channel);
}
}
        internal override DUCE.ResourceHandle GetHandleCore(DUCE.Channel channel)
        {
            // Note that we are in a lock here already.
            Return _duceResource.GetHandle(channel);
        }
        internal override int GetChannelCountCore()
        {
            // must already be in composition lock here
            Return _duceResource.GetChannelCount();
        }
        internal override DUCE.Channel GetChannelCore(int index)
        {
            Return _duceResource.GetChannel(index)
        }
        Dim _duceResource = DUCENs.GetType("System.Windows.Media.Composition.DUCE.MultiChannelResource").GetConstructor(Array.Empty(Of Object)).Invoke(Array.Empty(Of Object))


    Private Shared DUCENs As Assembly = Assembly.Load("System.Windows.Media.Composition.DUCE")
End Class

Public Class BrushMixin(Of T)
    Inherits Mixin<T>

End Class

Imports System.Runtime.InteropServices

Public Module Win32
    <StructLayout(LayoutKind.Sequential)>
    Public Structure DevMode
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)>
        Public dmDeviceName As String
        Public dmSpecVersion As Short

        Public dmDriverVersion As Short
        Public dmSize As Short
        Public dmDriverExtra As Short
        Public dmFields As Integer
        Public dmPositionX As Integer
        Public dmPositionY As Integer
        Public dmDisplayOrientation As Integer
        Public dmDisplayFixedOutput As Integer
        Public dmColor As Short
        Public dmDuplex As Short
        Public dmYResolution As Short
        Public dmTTOption As Short
        Public dmCollate As Short

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)>
        Public dmFormName As String

        Public dmLogPixels As Short
        Public dmBitsPerPel As Short
        Public dmPelsWidth As Integer
        Public dmPelsHeight As Integer
        Public dmDisplayFlags As Integer
        Public dmDisplayFrequency As Integer
        Public dmICMMethod As Integer
        Public dmICMIntent As Integer
        Public dmMediaType As Integer
        Public dmDitherType As Integer
        Public dmReserved1 As Integer
        Public dmReserved2 As Integer
        Public dmPanningWidth As Integer
        Public dmPanningHeight As Integer
    End Structure

    Public Declare Function EnumDisplaySettingsA Lib "user32" (deviceName As String, modeNum As Integer, ByRef devMode As DevMode) As Integer
    Public Declare Function ChangeDisplaySettingsA Lib "user32" (ByRef devMode As DevMode, flags As Integer) As Integer

    Public Const ENUM_CURRENT_SETTINGS = -1
    Public Const CDS_UPDATEREGISTRY = 1
    Public Const CDS_TEST = 2
    Public Const DISP_CHANGE_SUCCESSFUL = 0
    Public Const DISP_CHANGE_RESTART = 1
    Public Const DISP_CHANGE_FAILED = -1
    Public Const DMDO_DEFAULT = 0
    Public Const DMDO_90 = 1
    Public Const DMDO_180 = 2
    Public Const DMDO_270 = 3
End Module

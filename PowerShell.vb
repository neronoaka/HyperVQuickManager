Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Management.Automation
Imports System.Management.Automation.Runspaces
Imports Microsoft.HyperV.PowerShell

Public Class PowerShellHelper
    Private Shared Function InvokeScript(ByVal Cmdlet As String, ByVal Args As Collection(Of CommandParameter)) As Collection(Of PSObject)
        Dim ps As PowerShell = PowerShell.Create()
        ps.AddCommand(Cmdlet)
        If IsNothing(Args) = False Then
            For Each param As CommandParameter In Args
                ps.AddParameter(param.Name, param.Value)
            Next
        End If
        Dim InvokedObject As Collection(Of PSObject) = ps.Invoke
        If (ps.HadErrors) Then
            Dim ss As String = Nothing
            For Each s As ErrorRecord In ps.Streams.Error
                ss += s.Exception.Message & vbCrLf
            Next
            Throw New Exception(ss)
        End If
        Return InvokedObject
    End Function

    Public Shared Function GetList() As List(Of VirtualMachine)
        Dim out As New List(Of VirtualMachine)
        Try
            Dim i As Collection(Of PSObject) = InvokeScript("Get-VM", Nothing)
            For Each obj As PSObject In i
                Dim vm As VirtualMachine = obj.BaseObject
                out.Add(vm)
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "获取虚拟机错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return out
    End Function

    Public Shared Sub VMOptions(ByVal name As String, ByVal opt As Options)
        Dim cmdlet As String = "Stop-VM"
        Dim param As New Collection(Of CommandParameter)
        param.Add(New CommandParameter("-Name", name))
        Select Case opt
            Case Options.Save
                param.Add(New CommandParameter("-Save", Nothing))
            Case Options.TurnOff
                param.Add(New CommandParameter("-TurnOff", Nothing))
            Case Options.Restart
                cmdlet = "Restart-VM"
                param.Add(New CommandParameter("-Force", Nothing))
            Case Options.Start
                cmdlet = "Start-VM"
            Case Options.Suspend
                cmdlet = "Suspend-VM"
            Case Options.ResumeVM
                cmdlet = "Resume-VM"
        End Select
        Try
            InvokeScript(cmdlet, param)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "虚拟机电源错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Shared Sub NewVM(ByVal name As String, ByVal mem As Long, ByVal cpu As Integer, ByVal vmpath As String, ByVal diskpath As String, ByVal sw As String, ByVal gen2 As Boolean)
        Dim params1 As New Collection(Of CommandParameter), params2 As New Collection(Of CommandParameter)
        Dim membyte As Int64 = mem * 1024 * 1024
        Dim gen As Integer = 1
        If gen2 Then gen = 2
        params1.Add(New CommandParameter("-VHDPath", diskpath))
        params1.Add(New CommandParameter("-Path", vmpath))
        params1.Add(New CommandParameter("-Name", name))
        params1.Add(New CommandParameter("-MemoryStartupBytes", membyte))
        If IsNothing(sw) = False Then params1.Add(New CommandParameter("-SwitchName", sw))
        params1.Add(New CommandParameter("-Generation", gen))
        params2.Add(New CommandParameter("-VMName", name))
        params2.Add(New CommandParameter("-Count", cpu))
        InvokeScript("New-VM", params1)
        Try
            InvokeScript("Set-VMProcessor", params2)
        Catch ex As Exception
            MessageBox.Show("虚拟机已成功创建，但是在添加中央处理器时发生错误，" &
                            "因此该虚拟机的默认中央处理器数量为1但不影响运行，" &
                            "错误原因：" & ex.Message, "Warring", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Public Shared Function GetSW() As List(Of String)
        Dim l As New List(Of String)
        Try
            Dim obj As Collection(Of PSObject) = InvokeScript("Get-VMSwitch", Nothing)
            For Each p As PSObject In obj
                l.Add(p.Properties("Name").Value)
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "获取交换机信息错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return l
    End Function
End Class
Public Enum Options
    Start = 0
    PowerOff = 1
    TurnOff = 2
    Save = 3
    Suspend = 4
    ResumeVM = 5
    Restart = 6
End Enum
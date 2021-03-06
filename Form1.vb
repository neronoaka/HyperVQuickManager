Imports Microsoft.HyperV.PowerShell

Public Class Form1
    Private VM As VirtualMachine
    Private Lists As List(Of VirtualMachine)

    Public Sub RefreshInfo()
        ListBox1.Items.Clear()
        Lists = PowerShellHelper.GetList()
        For Each vm As VirtualMachine In Lists
            ListBox1.Items.Add(vm.Name)
        Next
        VM = Nothing
        GetInfo()
    End Sub

    Private Sub GetInfo()
        For Each i As ToolStripItem In ToolStrip1.Items
            i.Enabled = False
        Next
        Dim btns(0) As Integer
        If IsNothing(VM) = False Then
            Select Case VM.State
                Case VMState.Running
                    btns = {6, 4, 8, 2, 5}
                Case VMState.Paused
                    btns = {3, 6, 8}
                Case VMState.Off
                    btns = {1}
                Case VMState.Saved
                    btns = {1}
            End Select
            For Each tag As Integer In btns
                For Each i As ToolStripItem In ToolStrip1.Items
                    If i.Tag = tag Then i.Enabled = True
                Next
            Next
            PropertyGrid1.SelectedObject = New VMInfo(VM)
        Else
            PropertyGrid1.SelectedObject = Nothing
        End If
    End Sub

    Private Sub NewVMToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewVMToolStripMenuItem.Click
        Form2.Show()
        Me.Enabled = False
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RefreshInfo()
    End Sub

    Private Sub 刷新ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 刷新ToolStripMenuItem.Click
        RefreshInfo()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim i As Integer = ListBox1.SelectedIndex
        If i > -1 Then VM = Lists.Item(i)
        GetInfo()
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        PowerShellHelper.VMOptions(VM.Name, Options.Start)
        GetInfo()
    End Sub

    Private Sub ToolStripButton6_Click(sender As Object, e As EventArgs) Handles ToolStripButton6.Click
        PowerShellHelper.VMOptions(VM.Name, Options.TurnOff)
        GetInfo()
    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        PowerShellHelper.VMOptions(VM.Name, Options.PowerOff)
        GetInfo()
    End Sub

    Private Sub ToolStripButton8_Click(sender As Object, e As EventArgs) Handles ToolStripButton8.Click
        PowerShellHelper.VMOptions(VM.Name, Options.Save)
        GetInfo()
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        PowerShellHelper.VMOptions(VM.Name, Options.Suspend)
        GetInfo()
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        PowerShellHelper.VMOptions(VM.Name, Options.ResumeVM)
        GetInfo()
    End Sub

    Private Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click
        PowerShellHelper.VMOptions(VM.Name, Options.Restart)
        GetInfo()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If IsNothing(VM) = False Then
            Label1.Text = "CPU Usage:" & VM.CPUUsage & "%"
        Else
            Label1.Text = Nothing
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        OpenFileDialog1.ShowDialog()
    End Sub
End Class

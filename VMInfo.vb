Imports Microsoft.HyperV.PowerShell

Public Class VMInfo
    Private vm As VirtualMachine
    Public ReadOnly Property Name As String
        Get
            Return vm.Name
        End Get
    End Property

    Public ReadOnly Property Notes As String
        Get
            Return vm.Notes
        End Get
    End Property

    Public ReadOnly Property Network As String()
        Get
            Dim lists As New List(Of String)
            For Each nic As VMNetworkAdapter In vm.NetworkAdapters
                If IsNothing(nic) = False Then
                    lists.Add("Adapter:" & nic.Name)
                    lists.Add("Switch:" & nic.SwitchName)
                    lists.Add("MACAddress:" & nic.MacAddress)
                    For Each ip As String In nic.IPAddresses
                        lists.Add("IPAddress:" & ip)
                    Next
                    lists.Add(Nothing)
                End If
            Next
            Return lists.ToArray
        End Get
    End Property

    Public ReadOnly Property State As String
        Get
            Return vm.State.ToString()
        End Get
    End Property

    Public ReadOnly Property MemoryStatus As String
        Get
            Return vm.MemoryStatus
        End Get
    End Property

    Public ReadOnly Property Memory As String
        Get
            Return vm.MemoryStartup / 1048576 & "MB"
        End Get
    End Property

    Public ReadOnly Property CPUCount As Long
        Get
            Return vm.ProcessorCount
        End Get
    End Property

    Public Sub New(ByVal arg As VirtualMachine)
        vm = arg
    End Sub
End Class

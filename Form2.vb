Public Class Form2
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim vhd As String = TextBox2.Text
            If IO.File.Exists(vhd) = False Then Throw New Exception("VHD file not found")
            Dim sw As String = ComboBox1.Text
            If sw.Length < 1 Then sw = Nothing
            PowerShellHelper.NewVM(TextBox1.Text, NumericUpDown1.Value, NumericUpDown2.Value, TextBox3.Text, vhd, sw, CheckBox1.Checked)
            Form1.RefreshInfo()
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "创建虚拟机错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Form2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Form1.Enabled = True
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox3.Text = Application.StartupPath & "\VM"
        Dim sw As List(Of String) = PowerShellHelper.GetSW
        For Each s As String In sw
            ComboBox1.Items.Add(s)
        Next
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox2.Text = OpenFileDialog1.FileName
    End Sub
End Class
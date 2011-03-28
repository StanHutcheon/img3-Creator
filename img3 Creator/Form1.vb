Public Class Form1
    Private Property winstyle As ProcessWindowStyle
    Public temp = My.Computer.FileSystem.SpecialDirectories.Temp
    Public key As String = ""
    Public iv As String = ""

    Public Sub imagetoolInject(ByVal png As String, ByVal destinationimg3 As String, ByVal templateimg3 As String, ByVal iv As String, ByVal key As String)
        ShellWait(temp & "\imgtool\imagetool.exe", "inject " & png & " " & destinationimg3 & " " & templateimg3 & " " & iv & " " & key)
    End Sub

    Public Function killProc(ByVal processToKill As String)
        For Each p As Process In Process.GetProcessesByName(processToKill)
            p.Kill()
        Next
        Return 0
    End Function

    Public Sub ShellWait(ByVal file As String, ByVal arg As String)
        Dim procNlite As New Process
        winstyle = 1
        procNlite.StartInfo.FileName = file
        procNlite.StartInfo.Arguments = " " & arg
        procNlite.StartInfo.WindowStyle = winstyle
        Application.DoEvents()
        procNlite.Start()
        Do Until procNlite.HasExited
            Application.DoEvents()
            For i = 0 To 5000000
                Application.DoEvents()
            Next
        Loop
        procNlite.WaitForExit()
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ChDir(temp)
        killProc("7za")
        System.IO.File.WriteAllBytes(temp & "\imgtool.zip", My.Resources.imgtool)
        System.IO.File.WriteAllBytes(temp & "\7za.exe", My.Resources._7za)
        System.IO.File.WriteAllBytes(temp & "\extract.bat", My.Resources.extractimgtool)
        'ShellWait(temp & "\7za.exe", "e " & temp & "\imgtool.zip")
        If System.IO.Directory.Exists(temp & "\imgtool") Then
            Try
                My.Computer.FileSystem.DeleteDirectory(temp & "\imgtool", FileIO.DeleteDirectoryOption.DeleteAllContents)
            Catch ex As Exception
                Exit Sub
            End Try
        End If
        ExecCmd(temp & "\extract.bat")
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Button1.Enabled = False
        img3.ShowDialog()
        If img3.FileName = "" Then
            Exit Sub
        End If
        If System.IO.File.Exists(temp & "\template.img3") Then
            System.IO.File.Delete(temp & "\template.img3")
        Else
            System.IO.File.Copy(img3.FileName, temp & "\template.img3")
        End If

        If System.IO.File.Exists(temp & "\bootlogo.img3") Then
            System.IO.File.Delete(temp & "\bootlogo.img3")
        Else
            System.IO.File.Copy(img3.FileName, temp & "\bootlogo.img3")
        End If
        tick1.Visible = True
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Button2.Enabled = False
        png.ShowDialog()
        If png.FileName = "" Then
            Exit Sub
        End If
        If System.IO.File.Exists(temp & "\picture.png") Then
            System.IO.File.Delete(temp & "\picture.png")
        Else
            System.IO.File.Copy(png.FileName, temp & "\picture.png")
        End If
        PictureBox1.Image.FromFile(temp & "\picture.png")
        tick1.Visible = True
        Button3.Enabled = True
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Button3.Enabled = False
        'ShellWait(temp & "\imgtool\imagetool.exe", "inject " & temp & "\picture.png " & temp & "\bootlogo.img3 " & temp & "\template.img3 " & key & " " & iv)
        iv = TextBox2.Text
        key = TextBox1.Text
        imagetoolInject(temp & "\picture.png", temp & "\bootlogo.img3", temp & "\template.img3", iv, key)
        saveimg3.ShowDialog()
        If saveimg3.FileName = "" Then
            MsgBox("your img3 has been created but because you didn't select a save area, it is located at:" & vbCrLf & _
                   "%temp%\bootlogo.img3", MsgBoxStyle.Information, "img3 Creator | Help")
            Exit Sub
        End If
        System.IO.File.WriteAllBytes(saveimg3.FileName, temp & "\bootlogo.img3")
        MsgBox("Your newly created img3 is located at:" & vbCrLf & saveimg3.FileName)
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        If TextBox2.Text = "" Then
            Button2.Enabled = False
        ElseIf Not TextBox1.Text = "" And Not TextBox2.Text = "" Then
            Button2.Enabled = True
        End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text = "" Then
            Button2.Enabled = False
        ElseIf Not TextBox1.Text = "" And Not TextBox2.Text = "" Then
            Button2.Enabled = True
        End If
    End Sub

    Private Sub TextBox3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.Click
        label1.Select()
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged
        [Select]()
    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged
        label1.Select()
    End Sub
End Class

Module InstallUtil
    Public Structure CommandResult
        Dim nExitCode As Integer
        Dim strOutput As String
        Dim strError As String
    End Structure

    Public Function RunCommand(strExecutable As String, strArgs As String, Optional strInput As String = "") As CommandResult
        Dim psiCommand As New ProcessStartInfo With {
            .Arguments = strArgs,
            .FileName = strExecutable,
            .CreateNoWindow = True,
            .UseShellExecute = False,
            .RedirectStandardInput = True,
            .RedirectStandardOutput = True,
            .RedirectStandardError = True
        }

        Dim pcssCommand As Process = Process.Start(psiCommand)
        pcssCommand.StandardInput.Write(strInput)
        pcssCommand.WaitForExit()

        Dim res As CommandResult
        res.nExitCode = pcssCommand.ExitCode
        res.strOutput = pcssCommand.StandardOutput.ReadToEnd
        res.strError = pcssCommand.StandardError.ReadToEnd
        Return res
    End Function

    Public Sub CopyBootFiles(strWindowsMount As String, strBootMount As String)
        Dim res As CommandResult = RunCommand(IO.Path.Combine(strWindowsMount, "Windows\System32\bcdboot.exe"),
                                              String.Format("{0} /s {1}", IO.Path.Combine(strWindowsMount, "Windows"), strBootMount))

        If res.nExitCode <> 0 Then
            Throw New CommandFailedException(res)
        End If
    End Sub

    Public Sub RegisterRecoveryImage(strWindowsMount As String, strRecoveryMount As String)
        Dim res As CommandResult = RunCommand(IO.Path.Combine(strWindowsMount, "Windows\System32\Reagentc.exe"),
                                              String.Format("/Setreimage /Path {0} /Target {1}",
                                                            IO.Path.Combine(strRecoveryMount, "Recovery\WindowsRE"),
                                                            IO.Path.Combine(strWindowsMount, "Windows")))

        If res.nExitCode <> 0 Then
            Throw New CommandFailedException(res)
        End If
    End Sub

    Public Delegate Sub ProgressReportDelegate(nBytesCopied As Long, nBytesTotal As Long)

    Public Sub CopyStreamWithProgress(stmInput As IO.Stream, stmOutput As IO.Stream, procProgressReport As ProgressReportDelegate,
                                      Optional nBlockSize As Long = 1024 * 512, Optional nTotalLength As Long = -1)
        Dim nCopiedBytes As Long = 0
        Dim nRead As Long = 0
        Dim arrData(nBlockSize - 1) As Byte

        If nTotalLength = -1 Then
            nTotalLength = stmInput.Length
        End If

        While nCopiedBytes < nTotalLength
            nRead = stmInput.Read(arrData, 0, arrData.Length)
            stmOutput.Write(arrData, 0, nRead)
            nCopiedBytes += nRead
            procProgressReport(nCopiedBytes, nTotalLength)
        End While
    End Sub
End Module

﻿' Copyright (c) Microsoft Corporation.  All rights reserved.
'The following code was generated by Microsoft Visual Studio 2005.
'The test owner should check each test for validity.
Imports System
Imports System.Text
Imports System.Collections.Generic
Imports PInvoke
Imports Xunit

'''<summary>
'''This is a test class for PInvoke.ProcedureFinder and is intended
'''to contain all PInvoke.ProcedureFinder Unit Tests
'''</summary>
Public Class ProcedureFinderTest

    <Fact>
    Public Sub Find1()
        Using finder As New ProcedureFinder()
            Dim name As String = Nothing

            Assert.True(finder.TryFindDllNameExact("GetProcAddress", name))
            Assert.Equal("kernel32.dll", name, True)
            Assert.True(finder.TryFindDllNameExact("SendMessageW", name))
            Assert.Equal("user32.dll", name, True)
        End Using
    End Sub

    <Fact>
    Public Sub Find2()
        Using finder As New ProcedureFinder()
            Dim name As String = Nothing

            Assert.False(finder.TryFindDllNameExact("DoesNotExistFunc", name))
        End Using
    End Sub

    <Fact>
    Public Sub Find3()
        Using finder As New ProcedureFinder()
            Dim name As String = Nothing

            Assert.False(finder.TryFindDllNameExact("SendMessage", name))
            Assert.True(finder.TryFindDllNameExact("SendMessageW", name))
            Assert.Equal("user32.dll", name, True)
            Assert.True(finder.TryFindDllName("SendMessage", name))
            Assert.Equal("user32.dll", name, True)
        End Using
    End Sub

End Class

﻿' Copyright (c) Microsoft Corporation.  All rights reserved.
'The following code was generated by Microsoft Visual Studio 2005.
'The test owner should check each test for validity.
Imports System
Imports System.Text
Imports System.Collections.Generic
Imports PInvoke.Parser
Imports Xunit

'''<summary>
'''This is a test class for PInvoke.Parser.Macro and is intended
'''to contain all PInvoke.Parser.Macro Unit Tests
'''</summary>
Public Class MacroTest

    <Fact>
    Public Sub CreateMethod1()
        Dim m As MethodMacro = Nothing
        Assert.True(MethodMacro.TryCreateFromDeclaration("m1", "(x) x + 2", m))
        Assert.Equal("m1", m.Name)
    End Sub

    <Fact>
    Public Sub CreateMethod2()
        Dim m As MethodMacro = Nothing
        Assert.False(MethodMacro.TryCreateFromDeclaration("m1", "2", m))
    End Sub

    ''' <summary>
    ''' Make sure that whitespace is expactly preserved
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub CreateMethod3()
        Dim m As MethodMacro = Nothing
        Dim sig As String = "(x) ""foo""#x"
        Assert.True(MethodMacro.TryCreateFromDeclaration("m1", sig, m))
        Assert.Equal(sig, m.MethodSignature)
    End Sub

    <Fact>
    Public Sub CreateMethod4()
        Dim m As MethodMacro = Nothing
        Dim sig As String = "(x) ""foo""#x           +    5"
        Assert.True(MethodMacro.TryCreateFromDeclaration("m1", sig, m))
        Assert.Equal(sig, m.MethodSignature)
    End Sub

End Class

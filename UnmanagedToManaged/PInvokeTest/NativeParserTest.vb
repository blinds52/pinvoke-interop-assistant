﻿' Copyright (c) Microsoft Corporation.  All rights reserved.
'The following code was generated by Microsoft Visual Studio 2005.
'The test owner should check each test for validity.
Imports System
Imports System.Text
Imports System.Collections.Generic
Imports PInvoke
Imports PInvoke.Parser
Imports PInvokeTest
Imports System.IO
Imports Xunit

'''<summary>
'''This is a test class for PInvoke.Parser.NativeParser and is intended
'''to contain all PInvoke.Parser.NativeParser Unit Tests
'''</summary>
Public Class ParseEngineTest

    Private Function ParseString(ByVal data As String) As ParseResult
        Dim parser As New ParseEngine()
        Using stream As New StringReader(data)
            Return parser.Parse(stream)
        End Using
    End Function

    Private Function ParseFile(ByVal filePath As String) As ParseResult
        Dim parser As New ParseEngine()
        Using reader As New StreamReader(filePath)
            Return parser.Parse(New TextReaderBag(reader))
        End Using

    End Function

    Private Function FullParseFile(ByVal filePath As String) As ParseResult
        Dim opts As New PreProcessorOptions()
        Dim pre As New PreProcessorEngine(opts)

        Using stream As New StreamReader(filePath)
            Dim data As String = pre.Process(New TextReaderBag(stream))
            Dim tempPath As String = Path.GetTempFileName()
            Try
                File.WriteAllText(tempPath, data)
                Return ParseFile(tempPath)
            Finally
                File.Delete(tempPath)
            End Try
        End Using
    End Function

    ''' <summary>
    ''' Parse a data.  Import the sal semantics
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SalParse(ByVal text As String) As ParseResult
        Dim salText As String = File.ReadAllText("Sal.txt")
        text = salText & vbCrLf & text

        Dim opts As New PreProcessorOptions()
        Dim pre As New PreProcessorEngine(opts)
        Dim data As String = pre.Process(New TextReaderBag(New StringReader(text)))
        Dim tempPath As String = Path.GetTempFileName()
        Try
            File.WriteAllText(tempPath, data)
            Return ParseFile(tempPath)
        Finally
            File.Delete(tempPath)
        End Try

    End Function

    Private Sub VerifyStruct(ByVal nt As NativeType, ByVal name As String, ByVal ParamArray args As String())
        Dim ntStruct As NativeStruct = TryCast(nt, NativeStruct)
        Assert.NotNull(ntStruct)
        VerifyMembers(ntStruct, name, args)
    End Sub

    Private Sub VerifyUnion(ByVal nt As NativeType, ByVal name As String, ByVal ParamArray args As String())
        Dim ntUnion As NativeUnion = TryCast(nt, NativeUnion)
        Assert.NotNull(ntUnion)
        VerifyMembers(ntUnion, name, args)
    End Sub

    Private Sub VerifyMembers(ByVal container As NativeDefinedType, ByVal name As String, ByVal ParamArray args As String())
        If container.IsAnonymous Then
            Assert.True(String.IsNullOrEmpty(name))
        Else
            Assert.Equal(name, container.Name)
        End If

        Dim index As Integer = 0
        For Each cur As NativeMember In container.Members
            Dim definedType As NativeDefinedType = TryCast(cur.NativeType, NativeDefinedType)
            If definedType IsNot Nothing AndAlso definedType.IsAnonymous Then
                Assert.True(String.IsNullOrEmpty(args(index)))
            Else
                Assert.Equal(args(index), cur.NativeType.DisplayName)
            End If
            Assert.Equal(args(index + 1), cur.Name)
            index += 2
        Next
    End Sub

    Private Sub VerifyProc(ByVal result As ParseResult, ByVal index As Integer, ByVal str As String)
        Assert.NotNull(result)

        Assert.True(index < result.NativeProcedures.Count, "Invalid procedure index")
        Dim proc As NativeProcedure = result.NativeProcedures(index)
        Assert.Equal(str, proc.DisplayName)
    End Sub

    Private Sub VerifyFuncPtr(ByVal result As ParseResult, ByVal index As Integer, ByVal str As String)
        Assert.NotNull(result)
        Assert.True(index < result.NativeDefinedTypes.Count, "Invalid index")
        Dim fptr As NativeFunctionPointer = DirectCast(result.NativeDefinedTypes(index), NativeFunctionPointer)
        Assert.Equal(str, fptr.DisplayName)
    End Sub

    Private Sub VerifyProcSal(ByVal result As ParseResult, ByVal index As Integer, ByVal str As String)
        Assert.NotNull(result)

        Assert.True(index < result.NativeProcedures.Count, "Invalid procedure index")
        Dim proc As NativeProcedure = result.NativeProcedures(index)
        Assert.Equal(str, proc.Signature.CalculateSignature(proc.Name, True))
    End Sub

    Private Sub VerifyTypeDef(ByVal nt As NativeType, ByVal name As String, ByVal targetName As String)
        Dim td As NativeTypeDef = TryCast(nt, NativeTypeDef)
        Assert.NotNull(td)
        Assert.Equal(name, td.DisplayName)
        Assert.Equal(targetName, td.RealType.DisplayName)
    End Sub

    Private Sub VerifyPointer(ByVal nt As NativeType, ByVal fullName As String)
        Dim pt As NativePointer = TryCast(nt, NativePointer)
        Assert.NotNull(pt)
        Assert.Equal(pt.DisplayName, fullName)
    End Sub

    Private Sub VerifyEnum(ByVal nt As NativeType, ByVal name As String, ByVal ParamArray args As String())
        Dim ntEnum As NativeEnum = TryCast(nt, NativeEnum)
        Assert.NotNull(ntEnum)
        Assert.Equal(name, ntEnum.Name)

        Dim list As New List(Of NativeEnumValue)(ntEnum.Values)
        For i As Integer = 0 To args.Length - 1 Step 2
            Assert.True(list.Count > 0, "No more values")
            Dim valueName As String = args(i)
            Dim valueValue As String = args(i + 1)
            Dim ntValue As NativeEnumValue = list(0)
            list.RemoveAt(0)
            Assert.Equal(valueName, ntValue.Name)
            Assert.Equal(valueValue, ntValue.Value.Expression)
        Next
    End Sub

    Private Sub VerifyPrint(ByVal result As ParseResult, ByVal index As Integer, ByVal str As String)
        Assert.NotNull(result)

        Dim realStr As String = SymbolPrinter.Convert(result.ParsedTypes(index))
        Assert.Equal(str, realStr)
    End Sub

    ''' <summary>
    ''' Single member struct
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub TestStruct1()
        Dim result As ParseResult = ParseFile("Struct1.txt")
        Dim nt As NativeStruct = DirectCast(result.ParsedTypes(0), NativeStruct)
        VerifyMembers(nt, "foo", "int", "i")
    End Sub

    ''' <summary>
    ''' Single member struct with comments
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub TestStruct2()
        Dim result As ParseResult = ParseFile("Struct2.txt")
        Dim nt As NativeStruct = DirectCast(result.ParsedTypes(0), NativeStruct)
        VerifyMembers(nt, "bar", "double", "j")
    End Sub

    ''' <summary>
    ''' Pointers inside of the struct
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub TestStruct3()
        Dim result As ParseResult = ParseFile("Struct3.txt")
        Dim nt As NativeStruct = DirectCast(result.ParsedTypes(0), NativeStruct)
        VerifyMembers(nt, "bar2", "bar**", "i", "foo", "j")
    End Sub

    <Fact>
    Public Sub TestStruct4()
        Dim result As ParseResult = ParseFile("Struct4.txt")
        Dim nt1 As NativeStruct = DirectCast(result.ParsedTypes(0), NativeStruct)
        Dim nt2 As NativeStruct = DirectCast(result.ParsedTypes(1), NativeStruct)

        VerifyMembers(nt1, "s1", "int", "i", "double*", "j")
        VerifyMembers(nt2, "s2", "s1", "parent")
    End Sub

    ''' <summary>
    ''' Pointers to full nested struct references
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub TestStruct5()
        Dim result As ParseResult = ParseFile("Struct5.txt")
        Dim nt As NativeStruct = DirectCast(result.ParsedTypes(0), NativeStruct)
        VerifyMembers(nt, "s1", "int", "i", "s1*", "next")
    End Sub

    ''' <summary>
    ''' Verify the Type defs that occur after the struct definition
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub TestStruct6()
        Dim result As ParseResult = ParseFile("Struct6.txt")
        VerifyStruct(result.ParsedTypes(0), "s1", "int", "i")
        VerifyTypeDef(result.ParsedTypes(1), "t1", "s1")
        VerifyStruct(result.ParsedTypes(2), "s2", "int", "i")
        VerifyTypeDef(result.ParsedTypes(3), "t2", "s2")
        VerifyTypeDef(result.ParsedTypes(4), "t3", "s2")
    End Sub

    ''' <summary>
    ''' Verify the Type defs that occur after the struct definition when they
    ''' contain pointer references
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub TestStruct7()
        Dim result As ParseResult = ParseFile("Struct7.txt")
        VerifyStruct(result.ParsedTypes(0), "s1", "int", "i")
        VerifyTypeDef(result.ParsedTypes(1), "t1", "s1*")
        VerifyStruct(result.ParsedTypes(2), "s2", "int", "i")
        VerifyTypeDef(result.ParsedTypes(3), "t2", "s2*")
        VerifyTypeDef(result.ParsedTypes(4), "t3", "s2**")
    End Sub

    <Fact>
    Public Sub TestStruct8()
        Dim result As ParseResult = ParseFile("Struct8.txt")
        VerifyStruct(result.ParsedTypes(0), String.Empty, "int", "i")
        VerifyStruct(result.ParsedTypes(1), "s1", String.Empty, "j", "int", "i")
        VerifyStruct(result.ParsedTypes(2), String.Empty, "s2*", "i")
        VerifyStruct(result.ParsedTypes(3), "s2", "int", "i", String.Empty, "j")
    End Sub

    <Fact>
    Public Sub TestStruct9()
        Dim result As ParseResult = ParseFile("Struct9.txt")
        VerifyStruct(result.ParsedTypes(0), "s1", "int[]", "i")
        VerifyStruct(result.ParsedTypes(1), "s2", "int[5]", "i")
        VerifyStruct(result.ParsedTypes(2), "s3", "int[5]", "i")
    End Sub

    <Fact>
    Public Sub TestStruct10()
        Dim result As ParseResult = ParseFile("Struct10.txt")
        VerifyStruct(result.ParsedTypes(0), "s1", "unsigned int", "i")
        VerifyStruct(result.ParsedTypes(1), "s2", "unsigned int", "i", "unsigned int", "j")
    End Sub

    <Fact>
    Public Sub TestStruct11()
        Dim result As ParseResult = ParseFile("Struct11.txt")
        VerifyStruct(result.ParsedTypes(0), "_s1", "unsigned int", "i")
        VerifyTypeDef(result.ParsedTypes(1), "s1", "_s1")
        VerifyStruct(result.ParsedTypes(2), "_s2", "unsigned int", "i", "unsigned int", "j")
        VerifyTypeDef(result.ParsedTypes(3), "s2", "_s2")
        VerifyTypeDef(result.ParsedTypes(4), "ps2", "_s2*")

    End Sub

    <Fact>
    Public Sub TestStruct12()
        Dim result As ParseResult = ParseFile("Struct12.txt")
        VerifyStruct(result.ParsedTypes(0), "s1", "<bitvector 5>", "i")
        VerifyStruct(result.ParsedTypes(1), "s2", "<bitvector 5>", "i", "<bitvector 6>", "j")
        VerifyStruct(result.ParsedTypes(2), "s3", "<bitvector 5>", "i", "<bitvector 6>", "j", "int", "k")
    End Sub

    <Fact>
    Public Sub TestStruct13()
        Dim result As ParseResult = ParseFile("Struct13.txt")
        VerifyStruct(result.ParsedTypes(0), "s1", "int", "AnonymousMember1", "char", "c")
    End Sub

    <Fact>
    Public Sub TestStruct14()
        Dim result As ParseResult = ParseFile("Struct14.txt")
        VerifyStruct(result.ParsedTypes(1), "s1", "int", "AnonymousMember1", "void* (*s1_pFPtr)(int)", "AnonymousMember2")
    End Sub

    <Fact>
    Public Sub TestStruct15()
        Dim result As ParseResult = ParseFile("Struct15.txt")
        VerifyStruct(result.ParsedTypes(0), "s1", "char", "m1", "char", "m2")
        VerifyStruct(result.ParsedTypes(1), "s2", "char", "m1", "int", "m2", "int", "m3")
    End Sub

    <Fact>
    Public Sub TestStruct16()
        Dim result As ParseResult = ParseFile("Struct16.txt")
        VerifyStruct(result.ParsedTypes(0), "s1", "int[20]", "m1")
        VerifyStruct(result.ParsedTypes(1), "s2", "int[20]", "m1")
        VerifyStruct(result.ParsedTypes(2), "s3", "int[]", "m1", "int[]", "m2")
    End Sub

    <Fact>
    Public Sub TestStruct17()
        Dim result As ParseResult = ParseFile("Struct17.txt")
        VerifyStruct(result.ParsedTypes(0), "s1", "int[15]", "m1")
        VerifyStruct(result.ParsedTypes(1), "s2", "int[5]", "m1")
        VerifyStruct(result.ParsedTypes(2), "s3", "int[40]", "m1")
    End Sub

    ''' <summary>
    ''' Structs with __ptr32 and __ptr64 members
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub TestStruct18()
        Dim result As ParseResult = ParseFile("Struct18.txt")
        VerifyStruct(result.ParsedTypes(0), "s1", "int*", "m1", "char*", "m2")
        VerifyStruct(result.ParsedTypes(1), "s2", "int*", "m1", "char*", "m2")
        VerifyStruct(result.ParsedTypes(2), "s3", "int*", "m1", "char*", "m2")
    End Sub

    <Fact>
    Public Sub TestStruct19()
        Dim result As ParseResult = ParseFile("Struct19.txt")
        VerifyStruct(result.ParsedTypes(0), "s1", "<bitvector 8>", "m1", "<bitvector 10>", "m2")
        VerifyStruct(result.ParsedTypes(1), "s2", "int", "m1", "int", "m2")
    End Sub

    <Fact>
    Public Sub TestStruct20()
        Dim result As ParseResult = ParseFile("Struct20.txt")
        VerifyStruct(result.ParsedTypes(0), "s1", "unsigned char", "m1", "double", "m2", "char", "m3")
    End Sub

    <Fact>
    Public Sub TestStruct21()
        Dim result As ParseResult = ParseFile("Struct21.txt")
        VerifyStruct(result.ParsedTypes(0), "s1", "int", "m1", "char", "m2", "char", "m3")
        VerifyStruct(result.ParsedTypes(1), "s2", "int", "m1", "char", "m2", "char", "m3")
    End Sub

    <Fact>
    Public Sub TestClass1()
        Dim result As ParseResult = ParseFile("class1.txt")
        VerifyStruct(result.ParsedTypes(0), "c1", "int", "m1")
        VerifyStruct(result.ParsedTypes(1), "c2", "char*", "m1", "char", "m2")
    End Sub

    <Fact>
    Public Sub TestUnion1()
        Dim result As ParseResult = ParseFile("Union1.txt")
        VerifyUnion(result.ParsedTypes(0), "u1", "int", "i")
        VerifyUnion(result.ParsedTypes(1), "u2", "char", "i", "int", "j")
    End Sub

    <Fact>
    Public Sub TestUnion2()
        Dim result As ParseResult = ParseFile("Union2.txt")
        VerifyUnion(result.ParsedTypes(0), "u1", "int", "i")
        VerifyTypeDef(result.ParsedTypes(1), "t1", "u1")
        VerifyUnion(result.ParsedTypes(2), "u2", "char", "i", "int", "j")
        VerifyTypeDef(result.ParsedTypes(3), "t2", "u2")
        VerifyTypeDef(result.ParsedTypes(4), "t3", "u2*")
    End Sub

    <Fact>
    Public Sub Mixed1()
        Dim result As ParseResult = ParseFile("Mixed1.txt")
        VerifyUnion(result.ParsedTypes(0), String.Empty, "int", "i", "int", "j")
        VerifyStruct(result.ParsedTypes(1), "s1", String.Empty, "i", "char", "j")
    End Sub

    <Fact>
    Public Sub Mixed2()
        Dim result As ParseResult = ParseFile("Mixed2.txt")
        VerifyStruct(result.ParsedTypes(0), String.Empty, "int", "i")
        VerifyUnion(result.ParsedTypes(1), String.Empty, String.Empty, "i", "int", "j")
        VerifyStruct(result.ParsedTypes(2), "s1", String.Empty, "i", "char", "j")
    End Sub

    <Fact>
    Public Sub Mixed3()
        Dim result As ParseResult = ParseFile("Mixed3.txt")
        VerifyUnion(result.ParsedTypes(0), String.Empty, "int", "j", "float", "i")
        VerifyStruct(result.ParsedTypes(1), "s1", String.Empty, "Union1", "char", "k")
    End Sub

    <Fact>
    Public Sub TypeDef1()
        Dim result As ParseResult = ParseFile("TypeDef1.txt")
        VerifyTypeDef(result.ParsedTypes(0), "foo", "int")
        VerifyTypeDef(result.ParsedTypes(1), "bar", "char")
    End Sub

    <Fact>
    Public Sub TypeDef2()
        Dim result As ParseResult = ParseFile("TypeDef2.txt")
        VerifyTypeDef(result.ParsedTypes(0), "foo1", "int")
        VerifyTypeDef(result.ParsedTypes(1), "foo2", "int")
        VerifyTypeDef(result.ParsedTypes(2), "bar1", "char")
        VerifyTypeDef(result.ParsedTypes(3), "bar2", "char")
    End Sub

    <Fact>
    Public Sub TypeDef3()
        Dim result As ParseResult = ParseFile("TypeDef3.txt")
        VerifyTypeDef(result.ParsedTypes(0), "foo1", "int")
        VerifyTypeDef(result.ParsedTypes(1), "foo2", "int*")
        VerifyTypeDef(result.ParsedTypes(2), "bar1", "char")
        VerifyTypeDef(result.ParsedTypes(3), "bar2", "char*")
    End Sub

    <Fact>
    Public Sub Typedef4()
        Dim result As ParseResult = ParseFile("TypeDef4.txt")
        Assert.Equal("foo1(int)", SymbolPrinter.Convert(result.NativeTypedefs(0)))
        Assert.Equal("foo2(*(int))", SymbolPrinter.Convert(result.NativeTypedefs(1)))
        Assert.Equal("LPWSTR(*(wchar))", SymbolPrinter.Convert(result.NativeTypedefs(2)))
        Assert.Equal("FOO(*(wchar))", SymbolPrinter.Convert(result.NativeTypedefs(3)))
    End Sub

    <Fact>
    Public Sub Typedef5()
        Dim result As ParseResult = ParseFile("TypeDef5.txt")
        Assert.Equal("CINT(int(int))", SymbolPrinter.Convert(result.NativeTypedefs(0)))
        Assert.Equal("LPCSTR(*(WCHAR))", SymbolPrinter.Convert(result.NativeTypedefs(1)))
    End Sub

    <Fact>
    Public Sub Typedef6()
        Dim result As ParseResult = ParseFile("TypeDef6.txt")
        Assert.Equal("intarray([](int))", SymbolPrinter.Convert(result.NativeTypedefs(0)))
        Assert.Equal("chararray([](char))", SymbolPrinter.Convert(result.NativeTypedefs(1)))
    End Sub

    <Fact>
    Public Sub Typedef7()
        Dim result As ParseResult = ParseFile("TypeDef7.txt")
        Assert.Equal("s1(_s1(m1(int)))", SymbolPrinter.Convert(result.NativeTypedefs(0)))
    End Sub

    <Fact>
    Public Sub Typedef8()
        Dim result As ParseResult = ParseFile("TypeDef8.txt")
        Assert.Equal("f1(f1(Sig(int)(Sal)))", SymbolPrinter.Convert(result.NativeTypedefs(0)))
        Assert.Equal("f2(f2(Sig(*(int))(Sal)(param1(int)(Sal))))", SymbolPrinter.Convert(result.NativeTypedefs(1)))
    End Sub

    <Fact>
    Public Sub Typedef9()
        Dim result As ParseResult = ParseFile("TypeDef9.txt")
        Assert.Equal("td1(char)", SymbolPrinter.Convert(result.NativeTypedefs(0)))
        Assert.Equal("td2(char)", SymbolPrinter.Convert(result.NativeTypedefs(1)))
    End Sub

    <Fact>
    Public Sub Enum1()
        Dim result As ParseResult = ParseFile("Enum1.txt")
        VerifyEnum(result.ParsedTypes(0), "e1", "v1", "", "v2", "")
        VerifyEnum(result.ParsedTypes(1), "e2", "v1", "", "v2", "")
        VerifyEnum(result.ParsedTypes(2), "e3", "v1", "")
    End Sub

    <Fact>
    Public Sub Enum2()
        Dim result As ParseResult = ParseFile("Enum2.txt")
        VerifyEnum(result.ParsedTypes(0), "e1", "v1", "", "v2", "")
        VerifyTypeDef(result.ParsedTypes(1), "t1_e1", "e1")
        VerifyEnum(result.ParsedTypes(2), "e2", "v1", "", "v2", "")
        VerifyTypeDef(result.ParsedTypes(3), "t1_e2", "e2")
        VerifyTypeDef(result.ParsedTypes(4), "pt2_e2", "e2*")
    End Sub

    <Fact>
    Public Sub Enum3()
        Dim result As ParseResult = ParseFile("Enum3.txt")
        VerifyEnum(result.ParsedTypes(0), "e1", "v1", "1", "v2", "")
        VerifyEnum(result.ParsedTypes(1), "e2", "v1", "2", "v2", "v1+1")
    End Sub

    <Fact>
    Public Sub Enum4()
        Dim result As ParseResult = ParseFile("Enum4.txt")
        VerifyEnum(result.ParsedTypes(0), "_e1", "v1", "", "v2", "")
        VerifyTypeDef(result.ParsedTypes(1), "e1", "_e1")
        VerifyEnum(result.ParsedTypes(2), "_e2", "v1", "", "v2", "")
        VerifyTypeDef(result.ParsedTypes(3), "e2", "_e2")
        VerifyEnum(result.ParsedTypes(4), "_e3", "v1", "")
        VerifyTypeDef(result.ParsedTypes(5), "e3", "_e3")
        VerifyEnum(result.ParsedTypes(6), "e4")
    End Sub

    <Fact>
    Public Sub Proc1()
        Dim result As ParseResult = FullParseFile("Proc1.txt")
        VerifyProc(result, 0, "void p1()")
        VerifyProc(result, 1, "void p2(int i)")
        VerifyProc(result, 2, "void p3(int i, int j)")
        VerifyProc(result, 3, "int p4(int i, int j)")
    End Sub

    <Fact>
    Public Sub Proc2()
        Dim result As ParseResult = FullParseFile("Proc2.txt")
        VerifyProc(result, 0, "void p1()")
        VerifyProc(result, 1, "void p2(int i)")
        VerifyProc(result, 2, "void p3(int i, int j)")
        VerifyProc(result, 3, "int p4(int i, int j)")
    End Sub

    <Fact>
    Public Sub Proc3()
        Dim result As ParseResult = FullParseFile("Proc3.txt")
        VerifyProc(result, 0, "void p1(int* i)")
        VerifyProc(result, 1, "void p2(int** i)")
        VerifyProc(result, 2, "void p3(int** i)")
        VerifyProc(result, 3, "void p4(int[] i)")
    End Sub

    <Fact>
    Public Sub Proc4()
        Dim result As ParseResult = FullParseFile("Proc4.txt")
        VerifyProc(result, 0, "s1 p1(int* i)")
        VerifyProc(result, 1, "u1 p2(int** i)")
        VerifyProc(result, 2, "e1 p3(int** i)")
    End Sub

    <Fact>
    Public Sub Proc5()
        Dim result As ParseResult = FullParseFile("Proc5.txt")
        VerifyProc(result, 0, "void p1()")
        VerifyProc(result, 1, "int* p2()")
    End Sub

    <Fact>
    Public Sub Proc6()
        Dim result As ParseResult = FullParseFile("Proc6.txt")
        VerifyProc(result, 0, "void p1(int* p1)")
        VerifyProc(result, 1, "void p2(char** p1)")
        VerifyProc(result, 2, "void p3(char** p1)")
        VerifyProc(result, 3, "void p4(int*** p1)")
    End Sub

    ''' <summary>
    ''' Ignore calltype specifiers
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub Proc7()
        Dim result As ParseResult = FullParseFile("Proc7.txt")
        VerifyProc(result, 0, "void p1()")
        VerifyProc(result, 1, "void p2()")
        VerifyProc(result, 2, "void p3()")
        VerifyProc(result, 3, "void p4()")
        VerifyProc(result, 4, "void p5()")
        VerifyProc(result, 5, "void p6()")
        Assert.Equal(6, result.NativeProcedures.Count)
    End Sub

    ''' <summary>
    ''' Make sure that we are ignoring the volatile keyword
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub Proc8()
        Dim result As ParseResult = FullParseFile("Proc8.txt")
        VerifyProc(result, 0, "int p1(int a1)")
        VerifyProc(result, 1, "int p2(int a1)")
    End Sub

    ''' <summary>
    ''' Function pointers as parameters and return types
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub Proc9()
        Dim result As ParseResult = FullParseFile("Proc9.txt")
        VerifyProc(result, 0, "void p1(int (*anonymous)(int) fp1)")
        VerifyProc(result, 1, "void p2(int* (*anonymous)(int* a1) fp1)")
        VerifyProc(result, 2, "void p3(int* (*anonymous)())")
    End Sub

    ''' <summary>
    ''' Test the use of __ptr32 and __ptr64 in a procedure definition
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub Proc10()
        Dim result As ParseResult = FullParseFile("Proc10.txt")
        VerifyProc(result, 0, "void* p1(int* a1)")
        VerifyProc(result, 1, "void* p2(int* a1)")
        VerifyProc(result, 2, "void* p3(int* a1)")
        VerifyProc(result, 3, "void* p4(int* a1)")
    End Sub

    <Fact>
    Public Sub Proc11()
        Dim result As ParseResult = FullParseFile("Proc11.txt")
        VerifyProc(result, 0, "void p1(s1* a1)")
        VerifyProc(result, 1, "void p2(e1* a1)")
        VerifyProc(result, 2, "void p3(u1* a1)")
        VerifyProc(result, 3, "s1* p4()")
        VerifyProc(result, 4, "e1* p5()")
        VerifyProc(result, 5, "u1* p6()")
    End Sub

    <Fact>
    Public Sub Complex1()
        Dim result As ParseResult = FullParseFile("Complex1.txt")
        VerifyPrint(result, 0, "LPWSTR(*(wchar))")
        Assert.Equal(SymbolPrinter.Convert(result.NativeProcedures(0)), "p1(Sig(void)(Sal)(foo(LPWSTR)(Sal)))")
    End Sub

    <Fact>
    Public Sub Sal1()
        Dim text As String =
            "void p1(__in char c)"
        Dim result As ParseResult = SalParse(text)
        VerifyProcSal(result, 0, "void p1(Pre,Valid,Pre,Deref,ReadOnly char c)")
    End Sub

    ''' <summary>
    ''' Sal with a parameter
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub Sal2()
        Dim text As String =
            "void p1(__ecount(1) char c)"
        Dim result As ParseResult = SalParse(text)
        VerifyProcSal(result, 0, "void p1(NotNull,ElemWritableTo(1) char c)")
    End Sub

    ''' <summary>
    ''' __declspec with a non quoted string argument
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub Sal3()
        Dim text As String =
            "typdef __declspec(5) struct _s1 { int m1; } s1;"
        Dim result As ParseResult = SalParse(text)
        VerifyStruct(result.ParsedTypes(0), "_s1", "int", "m1")
    End Sub

    ''' <summary>
    ''' __declspec with a different macro call that is just not valid
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub Sal4()
        Dim text As String =
            "typedef __declspec(align(5)) struct _s1 { int m1; } s1;"
        Dim result As ParseResult = SalParse(text)
        VerifyStruct(result.ParsedTypes(0), "_s1", "int", "m1")
    End Sub

    <Fact>
    Public Sub Sal5()
        Dim text As String =
            "struct __declspec(5) s1 { int m1; };"
        Dim result As ParseResult = SalParse(text)
        VerifyStruct(result.ParsedTypes(0), "s1", "int", "m1")
    End Sub

    <Fact>
    Public Sub FuncPtr1()
        Dim result As ParseResult = FullParseFile("FuncPtr1.txt")
        VerifyFuncPtr(result, 0, "int (*f1)()")
        VerifyFuncPtr(result, 1, "int (*f2)(int)")
        VerifyFuncPtr(result, 2, "int (*f3)(char f)")
        VerifyFuncPtr(result, 3, "int (*f4)(char f, int j)")
        VerifyFuncPtr(result, 4, "int* (*f5)(int j)")
    End Sub

    <Fact>
    Public Sub FuncPtr2()
        Dim result As ParseResult = FullParseFile("FuncPtr2.txt")
        VerifyFuncPtr(result, 0, "int (*f1)()")
        VerifyFuncPtr(result, 1, "int (*f2)()")
        VerifyFuncPtr(result, 2, "int* (*f3)()")
        VerifyFuncPtr(result, 3, "int (*f4)()")
    End Sub

    <Fact>
    Public Sub FuncPtr3()
        Dim result As ParseResult = FullParseFile("FuncPtr3.txt")
        VerifyFuncPtr(result, 0, "int (*f1)(int a1)")
        VerifyFuncPtr(result, 1, "int* (*f2)(int a1)")
    End Sub

    <Fact>
    Public Sub FuncPtr4()
        Dim result As ParseResult = FullParseFile("FuncPtr4.txt")
        VerifyFuncPtr(result, 0, "int (*f1)(int a1)")
        VerifyFuncPtr(result, 1, "int* (*f2)(int a1)")
    End Sub

    ''' <summary>
    ''' List of unsupported scenarios that we are parsing
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub Errors1()
        Dim result As ParseResult = FullParseFile("Errors1.txt")

        ' C++ attribute
        Assert.Equal("C++ attributes are not supported: [uuid(55)]", result.ErrorProvider.Warnings(0))
        VerifyStruct(result.ParsedTypes(0), "s1", "int", "m1")

        ' Inline procedure 
        Assert.Equal("Ignoring Procedure p2 because it is defined inline.", result.ErrorProvider.Warnings(1))
        VerifyStruct(result.ParsedTypes(1), "s2", "int", "m1")

        ' Variable argument
        Assert.Equal("Procedure p3 has a variable argument signature which is unsupported.", result.ErrorProvider.Warnings(2))
        VerifyStruct(result.ParsedTypes(2), "s3", "int", "m1")

        ' Variable argument and inline
        Assert.Equal("Procedure p4 has a variable argument signature which is unsupported.", result.ErrorProvider.Warnings(3))
        VerifyStruct(result.ParsedTypes(3), "s4", "int", "m1")

        ' Member procedure
        Assert.Equal("Type member procedures are not supported: s5.p1", result.ErrorProvider.Warnings(4))
        VerifyStruct(result.ParsedTypes(4), "s5", "int", "m1")

        ' Member procedure inline
        Assert.Equal("Type member procedures are not supported: s6.p1", result.ErrorProvider.Warnings(5))
        VerifyStruct(result.ParsedTypes(5), "s6", "int", "m1")

        Assert.Equal("Type member procedures are not supported: s7.p1", result.ErrorProvider.Warnings(6))
        VerifyStruct(result.ParsedTypes(7), "s7", "int", "m1")

        ' Member procedure with const qualifier
        Assert.Equal("Type member procedures are not supported: s8.p1", result.ErrorProvider.Warnings(7))
        VerifyStruct(result.ParsedTypes(8), "s8", "int", "m1")

    End Sub
End Class

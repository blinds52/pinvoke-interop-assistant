﻿' Copyright (c) Microsoft Corporation.  All rights reserved.
'The following code was generated by Microsoft Visual Studio 2005.
'The test owner should check each test for validity.
Imports System
Imports System.Text
Imports System.Collections.Generic
Imports System.CodeDom
Imports PInvoke.Transform
Imports PInvoke
Imports Xunit

'''<summary>
'''This is a test class for PInvoke.Transform.MarshalTransform and is intended
'''to contain all PInvoke.Transform.MarshalTransform Unit Tests
'''</summary>
Public Class MarshalTransformTest
    ''' <summary>
    ''' Verify the use of a Win32 Bool type will be converted
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub BoolType1()
        VerifyProc(
            "BOOL p1()",
            "p1() As MarshalAsAttribute(UnmanagedType.Bool)Boolean")
        VerifyProc(
            "void p1(BOOL p1)",
            "p1(MarshalAsAttribute(UnmanagedType.Bool)In Boolean) As Void")
    End Sub

    ''' <summary>
    ''' Verify the use of the "boolean" type
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub BoolType2()
        VerifyProc(
            "boolean p1()",
            "p1() As MarshalAsAttribute(UnmanagedType.I1)Boolean")
        VerifyProc(
            "void p1(boolean b)",
            "p1(MarshalAsAttribute(UnmanagedType.I1)In Boolean) As Void")

    End Sub

    ''' <summary>
    ''' Pointer to primitives should be converted to ByRef's
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub PrimitivePointer1()
        VerifyProc(
            "void p1(__in DWORD *p)",
            "p1(InAttribute()Ref UInt32) As Void")
    End Sub

    ''' <summary>
    ''' Only convert single pointers to ByRefs
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub PrimitivePointer2()
        VerifyProc(
            "void p1(__inout_ecount(2) DWORD *p)",
            "p1(In IntPtr) As Void")
    End Sub

    ''' <summary>
    ''' Test Method with out string buffers
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub ConstantString1()
        VerifyProc(
            "void p1(LPCWSTR b)",
            "p1(InAttribute(),MarshalAsAttribute(UnmanagedType.LPWStr)In String) As Void")
    End Sub

    ''' <summary>
    ''' Convert return paramters
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub ConstantString2()
        VerifyProc(
            "LPCWSTR p1()",
            "p1() As MarshalAsAttribute(UnmanagedType.LPWStr)String")
    End Sub

    ''' <summary>
    ''' Should process SAL attributes even if the string is not constant 
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub ConstantString3()
        VerifyProc(
            "void p1(__in LPWSTR b)",
            "p1(InAttribute(),MarshalAsAttribute(UnmanagedType.LPWStr)In String) As Void")
    End Sub

    ''' <summary>
    ''' LPTSTR tests.  They are commonly typedef'd into LPCSTR so we need to make sure the 
    ''' distinction is preserved
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub ConstantString4()
        VerifyProc(
            "void p1(LPCTSTR b)",
            "p1(InAttribute(),MarshalAsAttribute(UnmanagedType.LPTStr)In String) As Void")
    End Sub

    ''' <summary>
    ''' __in_opt needs to be supported
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub ConstantString5()
        VerifyProc(
            "void p1(__in_opt LPWSTR b)",
            "p1(InAttribute(),MarshalAsAttribute(UnmanagedType.LPWStr)In String) As Void")
    End Sub

    ''' <summary>
    ''' String buffers with __out_ecount
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub MutableStringBuffer1()
        VerifyProc(
            "void p1(__out_ecount(notused) LPWSTR p1)",
            "p1(OutAttribute(),MarshalAsAttribute(UnmanagedType.LPWStr)In StringBuilder) As Void")
        VerifyProc(
            "void p1(__out_ecount_opt(notused) LPWSTR p1)",
            "p1(OutAttribute(),MarshalAsAttribute(UnmanagedType.LPWStr)In StringBuilder) As Void")
    End Sub

    ''' <summary>
    ''' String buffers with __out_ecount_part
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub MutableStringBuffer2()
        VerifyProc(
            "void p1(__out_ecount_part(notused,notused) LPWSTR p1)",
            "p1(OutAttribute(),MarshalAsAttribute(UnmanagedType.LPWStr)In StringBuilder) As Void")
        VerifyProc(
            "void p1(__out_ecount_part_opt(notused,notused) LPWSTR p1)",
            "p1(OutAttribute(),MarshalAsAttribute(UnmanagedType.LPWStr)In StringBuilder) As Void")
    End Sub

    ''' <summary>
    ''' Simple verification of a one way string buffer
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub OneWayStringBuffer1()
        VerifyProc(
            "void p1(__out_ecount(size) LPWSTR buf, int size)",
            "p1(OutAttribute(),MarshalAsAttribute(UnmanagedType.LPWStr)In StringBuilder,In Int32) As Void",
            "p1(Out String) As Void")
    End Sub

    ''' <summary>
    ''' One way string buffer with part buffers
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub OneWayStringBuffer2()
        VerifyProc(
            "void p1(__out_ecount_part(size,return+1) LPWSTR buf, int size)",
            "p1(OutAttribute(),MarshalAsAttribute(UnmanagedType.LPWStr)In StringBuilder,In Int32) As Void",
            "p1(Out String) As Void")
    End Sub

    ''' <summary>
    ''' One way string buffer with optional values
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub OneWayStringBuffer3()
        VerifyProc(
            "void p1(__out_ecount_part_opt(size,return+1) LPWSTR buf, int size)",
            "p1(OutAttribute(),MarshalAsAttribute(UnmanagedType.LPWStr)In StringBuilder,In Int32) As Void",
            "p1(Out String) As Void")
        VerifyProc(
            "void p1(__out_ecount_opt(size) LPWSTR buf, int size)",
            "p1(OutAttribute(),MarshalAsAttribute(UnmanagedType.LPWStr)In StringBuilder,In Int32) As Void",
            "p1(Out String) As Void")
    End Sub

    ''' <summary>
    ''' Recognize the two way string buffers
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub TwoWayStringBuffer1()
        VerifyProc(
            "void p1(__out_ecount_part(*size,*size+1) LPWSTR buf, __inout DWORD *size)",
            "p1(OutAttribute(),MarshalAsAttribute(UnmanagedType.LPWStr)In StringBuilder,Ref UInt32) As Void",
            "p1(Ref String) As Void")
    End Sub

    ''' <summary>
    ''' See a two way string buffer with optional SAL arguments
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub TwoWayStringBuffer2()
        VerifyProc(
            "void p1(__out_ecount_part_opt(*size,*size+1) LPWSTR buf, __inout DWORD *size)",
            "p1(Ref String) As Void")
    End Sub

    ''' <summary>
    ''' Recognize two way byte buffers
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub TwoWayByteBuffer1()
        VerifyProc(
            "void p1(__out_bcount_part(*size,*size) byte* b, __inout DWORD *size)",
            "p1(In IntPtr,Ref UInt32) As Void",
            "p1(Out PInvokePointer) As Void")
    End Sub

    ''' <summary>
    ''' Simple system int conversion
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub SystemInt1()
        VerifyProc(
            "void p1(WPARAM v)",
            "p1(MarshalAsAttribute(UnmanagedType.SysUInt)In UInt32) As Void")
    End Sub

    ''' <summary>
    ''' Should add an IntPtr wrapper
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub SystemInt2()
        VerifyProc(
            "void p1(WPARAM v)",
            "p1(In IntPtr) As Void")
    End Sub

    ''' <summary>
    ''' Return types should be converted as well
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub SystemInt3()
        VerifyProc(
            "WPARAM p1(WPARAM p)",
            "p1(In IntPtr) As IntPtr")
    End Sub

    <Fact>
    Public Sub SystemInt4()
        VerifyProc(
            "WPARAM p1(WPARAM p)",
            "p1(MarshalAsAttribute(UnmanagedType.SysUInt)In UInt32) As MarshalAsAttribute(UnmanagedType.SysUInt)UInt32")
    End Sub

    ''' <summary>
    ''' When only the return type is a System Int then don't create another method.  Languages don't
    ''' different on method return type so generating it will be a compile error
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub SystemInt5()
        VerifyNotProc(
            "WPARAM p1()",
            "p1() IntPrt")
    End Sub

    ''' <summary>
    ''' Pointers to known types should be transformed into ByRef
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub PointerToKnown1()
        VerifyProc(
            "void p1(S1 *s1)",
            "p1(Ref S1) As Void")
    End Sub

    ''' <summary>
    ''' Make sure that we don't turn a LPSTR into a ByRef char by the Pointer to known struct transform  
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub PointerToKnown2()
        VerifyProc(
            "void p1(char *v)",
            "p1(In IntPtr) As Void")
    End Sub

    ''' <summary>
    ''' If this is an array then make sure that we don't actually convert it to a single element
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub PointerToKnown3()
        ' NOTE: When array support is added this test will fail because it will be converted to something better 
        VerifyProc(
            "void p1(__out_ecount(4) S1 *s1)",
            "p1(In IntPtr) As Void")
    End Sub

    ''' <summary>
    ''' Make sure that we will convert a LP*STR into a StringBuilder
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub RawString1()
        VerifyProc(
            "void p1(LPWSTR p)",
            "p1(MarshalAsAttribute(UnmanagedType.LPWStr)In StringBuilder) As Void")
        VerifyProc(
            "void p1(LPSTR p)",
            "p1(MarshalAsAttribute(UnmanagedType.LPStr)In StringBuilder) As Void")
        VerifyProc(
            "void p1(LPTSTR p)",
            "p1(MarshalAsAttribute(UnmanagedType.LPTStr)In StringBuilder) As Void")
    End Sub

    ''' <summary>
    ''' Make sure that we will convert an LPC*STR into a String
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub RawString2()
        VerifyProc(
            "void p1(LPCWSTR p)",
            "p1(InAttribute(),MarshalAsAttribute(UnmanagedType.LPWStr)In String) As Void")
        VerifyProc(
            "void p1(LPCSTR p)",
            "p1(InAttribute(),MarshalAsAttribute(UnmanagedType.LPStr)In String) As Void")
        VerifyProc(
            "void p1(LPCTSTR p)",
            "p1(InAttribute(),MarshalAsAttribute(UnmanagedType.LPTStr)In String) As Void")
    End Sub

    ''' <summary>
    ''' Make sure that we get return types and convert them to strings (const or not)
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub RawString3()
        VerifyProc(
            "LPSTR p1()",
            "p1() As MarshalAsAttribute(UnmanagedType.LPStr)String")
        VerifyProc(
            "LPWSTR p1()",
            "p1() As MarshalAsAttribute(UnmanagedType.LPWStr)String")
        VerifyProc(
            "LPTSTR p1()",
            "p1() As MarshalAsAttribute(UnmanagedType.LPTStr)String")
    End Sub

    ''' <summary>
    ''' Array of int's
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub ArrayParam1()
        VerifyProc(
            "void p1(__in_ecount(4) int *p)",
            "p1(MarshalAsAttribute(UnmanagedType.LPArray,ArraySubType=UnmanagedType.I4,SizeConst=4)In Int32(1)) As Void")
    End Sub

    ''' <summary>
    ''' Array of pointers
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub ArrayParam2()
        VerifyProc(
            "void p1(__in_ecount(4) int**p)",
            "p1(MarshalAsAttribute(UnmanagedType.LPArray,ArraySubType=UnmanagedType.SysInt,SizeConst=4)In IntPtr(1)) As Void")
    End Sub

    ''' <summary>
    ''' Make sure that we don't genenarate an array of void[]
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub ArrayParam3()
        VerifyProc(
            "void p1(__in_ecount(2) void* param1)",
            "p1(InAttribute()In IntPtr) As Void")
    End Sub

    ''' <summary>
    ''' Double pointers should become Ref IntPtr
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub DoublePointer1()
        VerifyProc(
            "void p1(void** p)",
            "p1(Ref IntPtr) As Void")
    End Sub

    ''' <summary>
    ''' When the double pointer is __in then don't do anything special
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub DoublePointer2()
        VerifyProc(
            "void p1(__in char** p)",
            "p1(InAttribute()In IntPtr) As Void")
    End Sub

    ''' <summary>
    ''' When the double pointer is __out that is techinically invalid
    ''' so just ignore it 
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub DoublePointer3()
        VerifyProc(
            "void p1(__out char** p)",
            "p1(Ref IntPtr) As Void")
    End Sub

    ''' <summary>
    ''' __deref_out should be made Out IntPtr
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub DoublePointer4()
        VerifyProc(
            "void p1(__deref_out char**p );",
            "p1(Out IntPtr) As Void")
    End Sub

    <Fact>
    Public Sub BetterManagedTypeChar1()
        VerifyProc(
            "void p1(WCHAR c)",
            "p1(In Char) As Void")
    End Sub

    ''' <summary>
    ''' Don't convert TCHAR's because we don't know what their value will be
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub BetterManagedTypeChar2()
        VerifyProc(
            "void p1(TCHAR c)",
            "p1(In Byte) As Void")
    End Sub

    ''' <summary>
    ''' Return type should be converted as well
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub BetterManagedTypeChar3()
        VerifyProc(
            "WCHAR p1()",
            "p1() As Char")
    End Sub

    ''' <summary>
    ''' tagDEC structures should be convertible to decimal types
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub BetterManagedTypeDecimal1()
        VerifyProc(
            "void p1(DECIMAL d1)",
            "p1(In Decimal) As Void")
        VerifyProc(
            "void p1(tagDEC d1)",
            "p1(In Decimal) As Void")
    End Sub

    ''' <summary>
    ''' Decimal on the return type
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub BetterManagedTypeDecimal2()
        VerifyProc(
            "DECIMAL p1()",
            "p1() As Decimal")
        VerifyProc(
            "tagDEC p1()",
            "p1() As Decimal")
    End Sub

    ''' <summary>
    ''' Decimal Pointer parameter 
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub BetterManagedTypeDecimal3()
        VerifyProc(
            "void p1(DECIMAL *d1)",
            "p1(Ref Decimal) As Void")
        VerifyProc(
            "void p1(__in DECIMAL *d1)",
            "p1(InAttribute()Ref Decimal) As Void")
        VerifyProc(
            "void p1(__out DECIMAL *d1)",
            "p1(Out Decimal) As Void")
    End Sub

    <Fact>
    Public Sub BetterManagedTypeCurrency1()
        VerifyProc(
            "void p1(CURRENCY d1)",
            "p1(MarshalAsAttribute(UnmanagedType.Currency)In Decimal) As Void")
        VerifyProc(
            "void p1(tagCY d1)",
            "p1(MarshalAsAttribute(UnmanagedType.Currency)In Decimal) As Void")
    End Sub

    <Fact>
    Public Sub BetterManagedTypeCurrency2()
        VerifyProc(
            "CURRENCY p1()",
            "p1() As MarshalAsAttribute(UnmanagedType.Currency)Decimal")
        VerifyProc(
            "tagCY p1()",
            "p1() As MarshalAsAttribute(UnmanagedType.Currency)Decimal")
    End Sub

    ''' <summary>
    ''' Pointer to Currency parameter
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub BetterManagedTypeCurrency3()
        VerifyProc(
            "void p1(CURRENCY *d1)",
            "p1(MarshalAsAttribute(UnmanagedType.Currency)Ref Decimal) As Void")
        VerifyProc(
            "void p1(__in tagCY *d1)",
            "p1(MarshalAsAttribute(UnmanagedType.Currency),InAttribute()Ref Decimal) As Void")
        VerifyProc(
            "void p1(__out tagCY *d1)",
            "p1(MarshalAsAttribute(UnmanagedType.Currency)Out Decimal) As Void")
    End Sub

    ''' <summary>
    ''' BSTR as parameters
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub BStr1()
        VerifyProc(
            "void p1(BSTR p1)",
            "p1(MarshalAsAttribute(UnmanagedType.BStr)In String) As Void")
        VerifyProc(
            "void p1(__in BSTR p1)",
            "p1(MarshalAsAttribute(UnmanagedType.BStr),InAttribute()In String) As Void")
        VerifyProc(
            "void p1(__out BSTR p1)",
            "p1(MarshalAsAttribute(UnmanagedType.BStr),OutAttribute()Out String) As Void")
    End Sub

    ''' <summary>
    ''' BSTR as return type
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub Bstr2()
        VerifyProc(
            "BSTR p1()",
            "p1() As MarshalAsAttribute(UnmanagedType.BStr)String")
    End Sub

    ''' <summary>
    ''' We should not be doing anything with at __out BSTR because we don't understand
    ''' how to marshal that yet
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub Bstr3()
        VerifyProc(
            "void p1(__out_ecount(4) BSTR p1)",
            "p1(In IntPtr) As Void")
    End Sub

    ''' <summary>
    ''' Pointer to BSTR tests
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub Bstr4()
        VerifyProc(
            "void p1(BSTR *p)",
            "p1(MarshalAsAttribute(UnmanagedType.BStr)Ref String) As Void")
        VerifyProc(
            "void p1(__in BSTR *p)",
            "p1(MarshalAsAttribute(UnmanagedType.BStr),InAttribute()Ref String) As Void")
        VerifyProc(
            "void p1(__out BSTR *p)",
            "p1(MarshalAsAttribute(UnmanagedType.BStr),OutAttribute()Out String) As Void")
    End Sub

    ''' <summary>
    ''' Simple union definition
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub UnionAlignment1()
        VerifyTypeMembers("union u1 { int i; char j; };",
            "u1",
            "i", "FieldOffsetAttribute(0)System.Int32 i",
            "j", "FieldOffsetAttribute(0)System.Byte j")
    End Sub

    ''' <summary>
    ''' Make sure that obvious string pointers are not converted to a string in a union
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub UnionAlignment2()
        VerifyTypeMembers("union u1 { int i; wchar* j; };",
                "u1",
                "i", "FieldOffsetAttribute(0)System.Int32 i",
                "j", "FieldOffsetAttribute(0)System.IntPtr j")
    End Sub

    ''' <summary>
    ''' Make sure that a cBool is made blittable
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub UnionBool1()
        VerifyTypeMembers("union u1 { long l; bool b; };",
            "u1",
            "l", "FieldOffsetAttribute(0)System.Int32 l",
            "b", "FieldOffsetAttribute(0),MarshalAsAttribute(UnmanagedType.I1)System.Boolean b")
    End Sub

    <Fact>
    Public Sub UnionBool2()
        VerifyTypeMembers("union u1 { long l; BOOL b; };",
            "u1",
            "l", "FieldOffsetAttribute(0)System.Int32 l",
            "b", "FieldOffsetAttribute(0),MarshalAsAttribute(UnmanagedType.I1)System.Boolean b")
    End Sub

    <Fact>
    Public Sub StructStringMember()
        VerifyTypeMembers("struct s1 { wchar *j; };",
            "s1",
            "j", "MarshalAsAttribute(UnmanagedType.LPWStr)System.String j")
    End Sub

    ''' <summary>
    ''' Have string members of multiple types
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub StructStringMember2()
        VerifyTypeMembers("struct s1 { wchar *j; char *k; };",
            "s1",
            "j", "MarshalAsAttribute(UnmanagedType.LPWStr)System.String j",
            "k", "MarshalAsAttribute(UnmanagedType.LPStr)System.String k")
    End Sub

    ''' <summary>
    ''' Make sure that a BYTE[] is not converted to a string
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub StructArrayMember1()
        VerifyTypeMembers("struct s1 { BYTE[10] m1; };",
            "s1",
            "m1", "MarshalAsAttribute(UnmanagedType.ByValArray,SizeConst=10,ArraySubType=UnmanagedType.I1)System.Byte(1) m1")
    End Sub

    <Fact>
    Public Sub StructArrayMember2()
        VerifyTypeMembers("struct s1 { double[10] m1; };",
            "s1",
            "m1", "MarshalAsAttribute(UnmanagedType.ByValArray,SizeConst=10,ArraySubType=UnmanagedType.R8)System.Double(1) m1")
    End Sub

    <Fact>
    Public Sub StructArrayMember3()
        VerifyTypeMembers("struct s1 { int[10] m1; };",
            "s1",
            "m1", "MarshalAsAttribute(UnmanagedType.ByValArray,SizeConst=10,ArraySubType=UnmanagedType.I4)System.Int32(1) m1")
    End Sub

    ''' <summary>
    ''' Verify that a typedef pointing to an array will be marshalled correctly
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub TypedefToArray1()
        VerifyProc(
            "typedef int D[10];" & vbCrLf &
            "int p1(D param1);",
            "p1(In Int32(1)) As Int32")
    End Sub

    ''' <summary>
    ''' Typedef array as a member
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub TypedefToArray2()
        VerifyTypeMembers(
            "typedef int D[10];" & vbCrLf &
            "struct s1 { D m1; };",
            "s1",
            "m1", "System.Int32(1) m1")
    End Sub

    ''' <summary>
    ''' Verify default calling convention
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub CallingConvention1()
        VerifyProcCallingConvention(
            "void p1()",
            "p1",
            Runtime.InteropServices.CallingConvention.Winapi)
    End Sub

    ''' <summary>
    ''' Explicit calling convention 
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub CallingConvention2()
        VerifyProcCallingConvention("void __stdcall p1()", "p1", Runtime.InteropServices.CallingConvention.StdCall)
        VerifyProcCallingConvention("void __cdecl p1()", "p1", Runtime.InteropServices.CallingConvention.Cdecl)
        VerifyProcCallingConvention("void __winapi p1()", "p1", Runtime.InteropServices.CallingConvention.Winapi)
    End Sub

    ''' <summary>
    ''' Weird caling conventions should go back to winapi
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub CallingConvention3()
        VerifyProcCallingConvention("void __pascal p1()", "p1", Runtime.InteropServices.CallingConvention.Winapi)
        VerifyProcCallingConvention("void __inline p1()", "p1", Runtime.InteropServices.CallingConvention.Winapi)
        VerifyProcCallingConvention("void inline p1()", "p1", Runtime.InteropServices.CallingConvention.Winapi)
        VerifyProcCallingConvention("void __clrcall p1()", "p1", Runtime.InteropServices.CallingConvention.Winapi)
    End Sub

    ''' <summary>
    ''' Default for a function pointer is a CDECL
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub CallingConvention4()
        VerifyFPtrCallingConvention("typedef void (*f1)();", "f1", Runtime.InteropServices.CallingConvention.Winapi)
    End Sub

    ''' <summary>
    ''' Explicit convention
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub CallingConvention5()
        VerifyFPtrCallingConvention("typedef void (__cdecl *f1)();", "f1", Runtime.InteropServices.CallingConvention.Cdecl)
        VerifyFPtrCallingConvention("typedef void (__stdcall *f1)();", "f1", Runtime.InteropServices.CallingConvention.StdCall)
    End Sub

    ''' <summary>
    ''' Weird calling conventions should go to default
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub CallingConvention6()
        VerifyFPtrCallingConvention("typedef void (__pascal *f1)();", "f1", Runtime.InteropServices.CallingConvention.Winapi)
    End Sub

    ''' <summary>
    ''' Make sure we add the [In]
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub OptionalInPointer1()
        VerifyProc(
            "void p1(__in_opt int* param1);",
            "p1(InAttribute()In IntPtr) As Void")
    End Sub

    ''' <summary>
    ''' Don't add the [In] for non pointer types
    ''' </summary>
    ''' <remarks></remarks>
    <Fact>
    Public Sub OptionalInPointer2()
        VerifyProc(
            "void p1(__in_opt int param1);",
            "p1(In Int32) As Void")
    End Sub

End Class

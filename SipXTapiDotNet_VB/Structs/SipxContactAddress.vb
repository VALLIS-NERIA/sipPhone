'This file is part of SipXTapiDotNet.
'
'    SipXTapiDotNet is free software; you can redistribute it and/or modify
'    it under the terms of the GNU Lesser General Public License as published by
'    the Free Software Foundation; either version 2.1 of the License, or
'    (at your option) any later version.
'
'    SipXTapiDotNet is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU Lesser General Public License for more details.
'
'    You should have received a copy of the GNU Lesser General Public License
'    along with SipXTapiDotNet; if not, write to the Free Software
'    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
'
<StructLayout(LayoutKind.Sequential)> _
Public Structure SipxContactAddress
  Public ContactId As Integer
  Public ContactType As SipxContactType
  Public TransportType As SipxTransportType
  <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)> Public InterfaceName As String
  <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=28)> Public IpAddress As String
  Public Size As Integer
  Public Port As Integer
  <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)> Public CustomTransportName As String
  <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=64)> Public CustomRouteId As String
  Public Shared Function Create() As SipxContactAddress
    Dim rv As New SipxContactAddress
    rv.Size = Marshal.SizeOf(rv)
    Return rv
  End Function
  Public Shared Function Create(ByVal length As Integer) As SipxContactAddress()
    Dim rv(length - 1) As SipxContactAddress
    For i As Integer = 0 To length - 1
      rv(i).Size = Marshal.SizeOf(rv(i))
    Next
    Return rv
  End Function
  Public Shared Function FromPtr(ByVal ptr As IntPtr) As SipxContactAddress
    Dim rv As SipxContactAddress = SipxContactAddress.Create()
    'Private Function marshalContacts(ByVal ContactPtr As IntPtr, ByVal Count As Integer) As SipxContactAddress()
    rv = Marshal.PtrToStructure(ptr, rv.GetType)
    Return rv
  End Function

  ''' <summary>
  ''' Return SipxContactAddress() with copy of data referenced by ptr.
  ''' </summary>
  ''' <param name="ptr">Ptr to unmannaged memory</param>
  ''' <param name="Count">Count of elements in source</param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared Function FromPtr(ByVal ptr As IntPtr, ByVal Count As Integer) As SipxContactAddress()
    Dim rv(Count - 1) As SipxContactAddress
    For i As Integer = 0 To Count - 1
      'Dim i As Integer = 0
      rv(i) = fromPtr(IntPtr.op_Explicit(ptr.ToInt64 + i * Marshal.SizeOf(rv(i))))
    Next
    Return rv
  End Function
  ''' <summary> Fill unmanaged memory at contactPtr with copy of contacts()
  ''' </summary>
  ''' <param name="contacts"></param>
  ''' <param name="ContactPtr"></param>
  ''' <param name="count"></param>
  ''' <remarks>The memory must already be allocated</remarks>
  Public Shared Sub ToPtr(ByVal contacts() As SipxContactAddress, ByVal ContactPtr As IntPtr, ByVal count As Integer)
    For i As Integer = 0 To Math.Min(count, contacts.Length - 1)
      contacts(i).ToPtr(ContactPtr)
    Next
  End Sub
  ''' <summary>
  ''' Copies struct to unmannaged memory
  ''' </summary>
  ''' <param name="ptr">Pointer to unmanaged memory</param>
  ''' <remarks>The memory must already be allocated</remarks>
  Public Sub ToPtr(ByVal ptr As IntPtr)
    Marshal.StructureToPtr(Me, ptr, False)
  End Sub
End Structure

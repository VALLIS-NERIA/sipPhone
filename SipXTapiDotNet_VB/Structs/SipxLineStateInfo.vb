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
''' <summary>
''' Linestate event information structure.   This information is passed as part of the sipXtapi callback mechanism.  
''' </summary>
<StructLayout(LayoutKind.Sequential)> _
Public Structure SipxLineStateInfo
  ''' <summary>
  ''' The size of this structure
  ''' </summary>
  Public Size As UInteger
  ''' <summary>
  ''' Line handle associated with the linestate event.
  ''' </summary>
  Public LineHandle As IntPtr
  ''' <summary>
  ''' Identifies the linestate event.
  ''' </summary>
  Public StateEvent As SipxLineStateEvent
  ''' <summary>
  ''' Identifies the cause of the linestate event.
  ''' </summary>
  Public StateCause As SipxLineStateCause
End Structure


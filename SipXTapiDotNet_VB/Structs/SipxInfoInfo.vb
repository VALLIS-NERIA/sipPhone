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
''' An Info event signals the application layer that an Info message
''' was sent to this user agent.  If the Info message was sent to a 
''' call context (session) hCall will desiginate the call session. 
''' </summary>
Public Structure SipxInfoInfo
  ''' <summary>
  ''' The size of this structure.
  ''' </summary>
  Public Size As UInteger
  ''' <summary>
  ''' Call handle if available.
  ''' </summary>
  Public CallHandle As IntPtr
  ''' <summary>
  ''' Line handle if available.
  ''' </summary>
  Public LineHandle As IntPtr
  ''' <summary>
  ''' The URL of the host that originated the INFO message.
  ''' </summary>
  Public FromUrl As String
  ''' <summary>
  ''' The User Agent string of the source agent.
  ''' </summary>
  Public UserAgent As String
  ''' <summary>
  ''' String indicating the info content type.
  ''' </summary>
  Public ContentType As String
  ''' <summary>
  ''' Pointer to the Info message Content data.
  ''' </summary>
  Public ContentPtr As IntPtr
  ''' <summary>
  ''' Length of the Info message data pointed to by ContentPtr.
  ''' </summary>
  Public ContentLength As Integer
End Structure
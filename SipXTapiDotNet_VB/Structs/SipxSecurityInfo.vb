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
'''  An SipxSecurityInfo event informs that application layer of the status of a security operation.
''' </summary>
Public Structure SipxSecurityInfo
  ''' <summary>
  ''' The size of this structure in bytes 
  ''' </summary>
  Public Size As UInteger
  ''' <summary>
  ''' The negotiated SRTP key, if any.
  ''' </summary>
  Public Srtpkey As String
  ''' <summary>
  ''' Pointer to the certificate blob that was used to encrypt and/or sign.
  ''' </summary>
  Public Certificate As IntPtr
  ''' <summary>
  ''' Size of the certificate blob
  ''' </summary>
  Public CertificateSize As UInteger
  ''' <summary>
  ''' Event code for this SECURITY_INFO message
  ''' </summary>
  Public SecurityEvent As SipxSecurityEvent
  ''' <summary>
  ''' Cause code for this SECURITY_INFO message
  ''' </summary>
  Public SecurityCause As SipxSecurityCause
  ''' <summary>
  ''' Populated for SECURITY_CAUSE_SIGNATURE_NOTIFY.
  ''' </summary>
  Public SubjAltName As String
  ''' <summary>
  ''' Points to a call-id string associated with the event. Can be NULL.
  ''' </summary>
  Public CallId As String
  ''' <summary>
  ''' A call handle associated with the event.  Can be 0, to signify that the event is not associated with a call.
  ''' </summary>
  Public CallHandle As IntPtr
  ''' <summary>
  ''' A remote address associated with the event.  Can be NULL.
  ''' </summary>
  Public RemoteAddress As String
End Structure

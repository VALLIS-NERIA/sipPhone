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
''' A NotifyInfo event signifies that a Notify message was received for an active subscription.
''' </summary>
Public Structure SipxNotifyInfo
  ''' <summary>
  ''' The size of this structure
  ''' </summary>
  Public Size As UInteger
  ''' <summary>
  ''' A handle to the subscrption which caused this Notify event to be received.
  ''' </summary>
  Public SubHandle As IntPtr
  ''' <summary>
  ''' The User-Agent header field value from the SIP NOTIFY response (may be NULL)
  ''' </summary>
  Public NotifierUserAgent As String
  ''' <summary>
  ''' String indicating the content type
  ''' </summary>
  Public ContentType As String
  ''' <summary>
  ''' Pointer to the Notify message content
  ''' </summary>
  Public ContentPtr As IntPtr
  ''' <summary>
  ''' Length of the NOTIFY message content in bytes
  ''' </summary>
  Public ContentLength As Integer
End Structure

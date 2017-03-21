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
''' An SubStatus event informs that application layer of the status of an outbound Subscription requests.
''' </summary>
Public Structure SipxSubStatusInfo
  ''' <summary>
  ''' The size of this structure
  ''' </summary>
  Public Size As UInteger
  ''' <summary>
  ''' A handle to the subscription to which this state change occurred. 
  ''' </summary>
  Public SubHandle As IntPtr
  ''' <summary>
  ''' Enum state value indicating the current state of the subscription. 
  ''' </summary>
  Public SubState As SipxSubscriptionState
  ''' <summary>
  ''' Enum cause for the state change in this event.
  ''' </summary>
  Public SubCause As SipxSubscriptionCause
  ''' <summary>
  ''' The User Agent header field value from the SIP SUBSCRIBE response (may be NULL)
  ''' </summary>
  Public SubServerUserAgent As String
End Structure

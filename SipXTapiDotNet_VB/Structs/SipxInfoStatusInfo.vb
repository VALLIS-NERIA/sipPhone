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
''' An InfoStatus event informs that application layer of the status
''' of an outbound Info requests.   This information is passed as part of 
''' the sipXtapi callback mechanism.  
''' </summary>
Public Structure SipxInfoStatusInfo
  ''' <summary>
  ''' The size of this structure
  ''' </summary>
  Public Size As UInteger
  ''' <summary>
  ''' The handle used to make the outbound info request. 
  ''' </summary>
  Public InfoHandle As IntPtr
  ''' <summary>
  ''' Emumerated status for this request acknowledgement.
  ''' </summary>
  Public ResponseCode As Integer
  ''' <summary>
  ''' The text of the request acknowledgement.
  ''' </summary>
  Public ResponseText As String
  ''' <summary>
  ''' Event code for this InfoStatus message 
  ''' </summary>
  Public InfoStatusEvent As SipxInfoStatusEvent
End Structure

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
''' Media event information structure.  This information is passed as part of the sipXtapi callback mechanism.
''' </summary>
<StructLayout(LayoutKind.Sequential)> _
Friend Structure SipxMediaInfo
  ''' <summary>
  ''' The size of this structure
  ''' </summary>
  Public Size As UInteger
  ''' <summary>
  ''' Identifies the media event.
  ''' </summary>
  Public MediaEvent As SipxMediaEvent
  ''' <summary>
  ''' Identifies the cause of the media event.
  ''' </summary>
  Public MediaCause As SipxMediaCause
  ''' <summary>
  ''' Either Audio or Video.
  ''' </summary>
  Public MediaType As SipxMediaType
  ''' <summary>
  ''' Call handle associated with this event.
  ''' </summary>
  Public CallHandle As IntPtr
  ''' <summary>
  ''' Negotiated codec; only supplied on 
  ''' LocalStart and RemoteStart events.
  ''' </summary>
  Public CodecInfo As SipxCodecInfo
  ''' <summary>
  ''' Idle time (ms) for SILENT events; only 
  ''' supplied on RemoteSilent events.
  ''' </summary>
  Public IdleTime As Integer
End Structure

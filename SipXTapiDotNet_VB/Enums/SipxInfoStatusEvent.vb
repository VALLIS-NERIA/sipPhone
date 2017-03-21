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
''' Enumeration of possible INFO status events
''' </summary>
''' <remarks></remarks>
Public Enum SipxInfoStatusEvent
  ''' <summary>
  ''' This is the initial value for an InfoStatus event. 
  ''' </summary>
  Unknown = 0
  ''' <summary>
  ''' This event is fired if a response is received after an
  ''' INFO message has been sent
  ''' </summary>
  Response = 30000
  ''' <summary>
  ''' This event is fired in case a network error was encountered
  ''' while trying to send an Info event.
  ''' </summary>
  NetworkError = 31000
End Enum

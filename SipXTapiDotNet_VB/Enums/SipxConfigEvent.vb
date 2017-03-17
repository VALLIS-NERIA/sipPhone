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
''' Enumeration of possible configuration events
''' </summary>
Public Enum SipxConfigEvent
  ''' <summary>
  ''' Unknown configuration event 
  ''' </summary>
  Unknown = 0
  ''' <summary>
  ''' A STUN binding has been obtained for signaling purposes.
  ''' For a SipxConfigEvent type of StunSuccess, 
  ''' the pData pointer of the info structure will point to a
  ''' SipxContactAddress structure.
  ''' </summary>
  StunSuccess = 40000
  ''' <summary>
  ''' Unable to obtain a STUN binding for signaling purposes.
  ''' </summary>
  StunFailure = 41000
End Enum

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
''' SipxConfigInfo events signify that a change in configuration was observed.
''' </summary>
Public Structure SipxConfigInfo
  ''' <summary>
  ''' The size of this structure
  ''' </summary>
  Public Size As UInteger
  ''' <summary>
  ''' Event code -- see SipxConfigEvent for  details.
  ''' </summary>
  Public ConfigEvent As SipxConfigEvent
  ''' <summary>
  ''' Pointer to event data -- SEE SipxConfigEvent for details.
  ''' </summary>
  Public DataPtr As IntPtr
End Structure

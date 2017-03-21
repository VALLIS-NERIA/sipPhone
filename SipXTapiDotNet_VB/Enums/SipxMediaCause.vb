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
''' Enumeration of possible media event causes.
''' </summary>
Public Enum SipxMediaCause
  ''' <summary>
  ''' Normal cause; the call was likely torn down.
  ''' </summary>
  Normal
  ''' <summary>
  ''' Media state changed due to a local or remote hold operation.
  ''' </summary>
  Hold
  ''' <summary>
  ''' Media state changed due to a local or remote unhold operation
  ''' </summary>
  Unhold
  ''' <summary>
  ''' Media state changed due to an error condition. 
  ''' </summary>
  Failed
  ''' <summary>
  '''  Media state changed due to an error condition, (device was removed, already in use, etc).
  ''' </summary>
  DeviceUnavailable
  ''' <summary>
  ''' Incompatible destination -- We were unable to negotiate a codec
  ''' </summary>
  Incompatible
End Enum

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
''' Enumeration of possible linestate Events.
''' </summary>
Public Enum SipxLineStateEvent
  ''' <summary>
  ''' This is the initial Line event state.
  ''' </summary>
  Unknown = 0
  ''' <summary>
  ''' The Registering event is fired when sipXtapi
  ''' has successfully sent a Register message,
  ''' but has not yet received a success response 
  ''' from the registrar server.
  ''' </summary>
  Registering = 20000
  ''' <summary>
  ''' The Registered event is fired after sipXtapi has received
  ''' a response from the registrar server, indicating a successful
  ''' registration.
  ''' </summary>
  Registered = 21000
  ''' <summary>
  ''' The Unregistering event is fired when sipXtapi
  ''' has successfully sent a Register message with an expires=0
  ''' parameter, but has not yet received a success response from 
  ''' the registrar server.
  ''' </summary>
  Unregistering = 22000
  ''' <summary>
  ''' The Unregistered event is fired after sipXtapi has received
  ''' a response from the registrar server, indicating a successful
  ''' un-registration.
  ''' </summary>
  Unregistered = 23000
  ''' <summary>
  ''' The RegisterFailed event is fired to indicate a failure of Registration.
  ''' It is fired in the following cases:  
  ''' The client could not connect to the registrar server.
  ''' The registrar server challenged the client for authentication credentials,
  ''' and the client failed to supply valid credentials.
  ''' The registrar server did not generate a success response (status code == 200)
  ''' within a timeout period.
  ''' </summary>
  RegisterFailed = 24000
  ''' <summary>
  ''' The UnregisterFailed event is fired to indicate a failure of un-Registration.
  ''' It is fired in the following cases:  
  ''' The client could not connect to the registrar server.2
  ''' The registrar server challenged the client for authentication credentials,
  ''' and the client failed to supply valid credentials.
  ''' The registrar server did not generate a success response (status code == 200)
  ''' within a timeout period.
  ''' </summary>
  UnregisterFailed = 25000
  ''' <summary>
  ''' The Provisioned event is fired when a sipXtapi Line is added, and Registration is not 
  ''' requested (i.e. - sipxLineAdd is called with a bRegister parameter of false.
  ''' </summary>
  Provisioned = 26000
End Enum

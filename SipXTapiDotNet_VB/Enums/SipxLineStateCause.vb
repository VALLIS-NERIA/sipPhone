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
''' Enumeration of possible linestate Event causes.
''' </summary>
Public Enum SipxLineStateCause
  ''' <summary>
  '''  No cause specified.
  ''' </summary>
  Unknown = 0
  ''' <summary>
  ''' See SipxLineStateEvent.Registering.
  ''' </summary>
  RegisteringNormal = SipxLineStateEvent.Registering + 1
  ''' <summary>
  ''' See SipxLineStateEvent.Registered.
  ''' </summary>
  RegisteredNormal = SipxLineStateEvent.Registered + 1
  ''' <summary>
  ''' See SipxLineStateEvent.Unregistering
  ''' </summary>
  UnregisteringNormal = SipxLineStateEvent.Unregistering + 1
  ''' <summary>
  ''' See SipxLineStateEvent.Unregistered
  ''' </summary>
  UnregisteredNormal = SipxLineStateEvent.Unregistered + 1
  ''' <summary>
  ''' Failed to register becauseof a connectivity problem. 
  ''' </summary>
  RegisterFailedCouldNotConnect = SipxLineStateEvent.RegisterFailed + 1
  ''' <summary>
  ''' Failed to register because of an authorization / authentication failure.
  ''' </summary>
  RegisterFailedNotAuthorized = SipxLineStateEvent.RegisterFailed + 2
  ''' <summary>
  ''' Failed to register because of a timeout.
  ''' </summary>
  RegisterFailedTimeout = SipxLineStateEvent.RegisterFailed + 3
  ''' <summary>
  ''' Failed to unregister because of a connectivity problem.
  ''' </summary>
  UnregisterFailedCouldNotConnect = SipxLineStateEvent.UnregisterFailed + 1
  ''' <summary>
  ''' Failed to unregister because of of an authorization /  authentication failure.
  ''' </summary>
  UnregisterFailedNotAuthorized = SipxLineStateEvent.UnregisterFailed + 2
  ''' <summary>
  ''' Failed to register because of a timeout.
  ''' </summary>
  UnregisterFailedTimeout = SipxLineStateEvent.UnregisterFailed + 3
  ''' <summary>
  ''' See SipxLineStaeEvent.Provisioned
  ''' </summary>
  ProvisionedNormal = SipxLineStateEvent.Provisioned + 1
End Enum

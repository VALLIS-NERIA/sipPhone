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
''' Enum with all of the possible event types.
''' </summary>
''' <remarks></remarks>
Public Enum SipxEventCategory
  ''' <summary>
  ''' Signify a change in state of a call.  States 
  ''' range from the notification of a new call to 
  ''' ringing to connection established to changes in
  ''' audio state (starting sending, stop sending)
  ''' to termination of a call.
  ''' </summary>
  ''' <remarks></remarks>
  CallState
  ''' <summary>
  ''' LineState events indicate changes in the 
  ''' status of a line appearance.  Lines identify
  ''' inbound and outbound identities and can be 
  ''' either provisioned (hardcoded) or configured 
  ''' to automatically register with a registrar.  
  ''' Lines also encapsulate the authentication 
  ''' criteria needed for dynamic registrations.
  ''' </summary>
  ''' <remarks></remarks>
  LineState
  ''' <summary>
  ''' InfoStatus events are sent when the application 
  ''' requests sipXtapi to send an INFO message to 
  ''' another user agent.  The status event includes 
  ''' the response for the INFO method.  Application 
  ''' developers should look at this event to determine 
  ''' the outcome of the INFO message.
  ''' </summary>
  ''' <remarks></remarks>
  InfoStatus
  ''' <summary>
  ''' Info events are sent to the application whenever 
  ''' an INFO message is received by the sipXtapi user 
  ''' agent.  INFO messages are sent to a specific call.
  ''' sipXtapi will automatically acknowledges the INFO 
  ''' message at the protocol layer.
  ''' </summary>
  ''' <remarks></remarks>
  Info
  ''' <summary>
  ''' SubStatus events are sent to the application 
  ''' layer for information on the subscription state
  ''' (e.g. OK, Expired).
  ''' </summary>
  ''' <remarks></remarks>
  SubStatus
  ''' <summary>
  ''' Notify evens are send to the application layer
  ''' after a remote publisher has sent data to the 
  ''' application.  The application layer can retrieve
  ''' the data from this event.
  ''' </summary>
  ''' <remarks></remarks>
  Notify
  ''' <summary>
  ''' Config events signify changes in configuration.
  ''' For example, when requesting STUN support, a 
  ''' notification is sent with the STUN outcome (either
  ''' SUCCESS or FAILURE)
  ''' </summary>
  ''' <remarks></remarks>
  Config
  ''' <summary>
  ''' Security events signify occurences in call security 
  ''' processing.  These events are only sent when using
  ''' S/MIME or TLS.
  ''' </summary>
  ''' <remarks></remarks>
  Security
  ''' <summary>
  ''' Media events signify changes in the audio state for
  ''' sipXtapi or a particular call.
  ''' </summary>
  ''' <remarks></remarks>
  Media
End Enum

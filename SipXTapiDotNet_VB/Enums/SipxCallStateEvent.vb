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
''' Major call state events identify significant changes in the state of a call.
''' </summary>
Public Enum SipxCallStateEvent
  ''' <summary>
  ''' An Unknown event is generated when the state for a call 
  ''' is no longer known.  This is generally an error 
  ''' condition; see the minor event for specific causes.
  ''' </summary>
  Unknown = 0
  ''' <summary>
  ''' The NewCall event indicates that a new call has been 
  ''' created automatically by the sipXtapi.  This event is 
  ''' most frequently generated in response to an inbound 
  ''' call request.
  ''' </summary>
  NewCall = 1000
  ''' <summary>
  ''' The Dialtone event indicates that a new call has been
  ''' created for the purpose of placing an outbound call.  
  ''' The application layer should determine if it needs to 
  ''' simulate dial tone for the end user.
  ''' </summary>
  Dialtone = 2000
  ''' <summary>
  ''' The RemoteOffering event indicates that a call setup 
  ''' invitation has been sent to the remote party.  The 
  ''' invitation may or may not every receive a response.  If
  ''' a response is not received in a timely manor, sipXtapi 
  ''' will move the call into a disconnected state.  If 
  ''' calling another sipXtapi user agent, the reciprocal 
  ''' state is Offer.
  ''' </summary>
  RemoteOffering = 2500
  ''' <summary>
  ''' The RemoteOffering event indicates that a call setup 
  ''' invitation has been accepted and the end user is in the
  ''' alerting state (ringing).  Depending on the SIP 
  ''' configuration, end points, and proxy servers involved, 
  ''' this event should only last for 3 minutes.  Afterwards,
  ''' the state will automatically move to Disconnected.  If 
  ''' calling another sipXtapi user agent, the reciprocate 
  ''' state is Alerting. 
  ''' 
  ''' Pay attention to the cause code for this event.  If
  ''' the cause code is SipxCallStateCause.EarlyMedia, the 
  ''' remote the party is sending early media (e.g. gateway is
  ''' producing ringback or audio feedback).  In this case, the
  ''' user agent should not produce local ringback.
  ''' </summary>
  RemoteAlerting = 3000
  ''' <summary>
  ''' The Connected state indicates that call has been setup 
  ''' between the local and remote party.  Network audio should be 
  ''' flowing provided and the microphone and speakers should
  ''' be engaged.
  ''' </summary>
  Connected = 4000
  ''' <summary>
  ''' The Bridged state indicates that a call is active,
  ''' however, the local microphone/speaker are not engaged.  If
  ''' this call is part of a conference, the party will be able
  ''' to talk with other Bridged conference parties.  Application
  ''' developers can still play and record media.
  ''' </summary>
  Bridged = 5000
  ''' <summary>
  ''' The Held state indicates that a call is
  ''' both locally and remotely held.  No network audio is flowing 
  ''' and the local microphone and speaker are not engaged.
  ''' </summary>
  Held = 6000
  ''' <summary>
  ''' The RemoteHeld state indicates that the remote 
  ''' party is on hold.  Locally, the microphone and speaker are
  ''' still engaged, however, no network audio is flowing.
  ''' </summary>
  RemoteHeld = 7000
  ''' <summary>
  ''' The Disconnected state indicates that a call was 
  ''' disconnected or failed to connect.  A call may move 
  ''' into the Disconnected states from almost every other 
  ''' state.  Please review the Disconnected minor events to
  ''' understand the cause.
  ''' </summary>
  Disconnected = 8000
  ''' <summary>
  ''' An Offering state indicates that a new call invitation 
  ''' has been extended this user agent.  Application 
  ''' developers should invoke sipxCallAccept(), 
  ''' sipxCallReject() or sipxCallRedirect() in response.  
  ''' Not responding will result in an implicit call 
  ''' sipXcallReject().
  ''' </summary>
  Offering = 9000
  ''' <summary>
  ''' An Alerting state indicates that an inbound call has 
  ''' been accepted and the application layer should alert 
  ''' the end user.  The alerting state is limited to 3 
  ''' minutes in most configurations; afterwards the call 
  ''' will be canceled.  Applications will generally play 
  ''' some sort of ringing tone in response to this event.
  ''' </summary>
  Allerting = 10000
  ''' <summary>
  ''' The Destroyed event indicates the underlying resources 
  ''' have been removed for a call.  This is the last event 
  ''' that the application will receive for any call.  The 
  ''' call handle is invalid after this event is received.
  ''' </summary>
  Destroyed = 11000
  ''' <summary>
  ''' The transfer state indicates a state change in a 
  ''' transfer attempt.  Please see the SipxCallStateCause
  ''' codes for details on each state transition.
  ''' </summary>
  Transfer = 12000
End Enum

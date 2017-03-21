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
''' Callstate cuase events identify the reason for a Callstate event or provide more detail.
''' </summary>
Public Enum SipxCallStateCause
  ''' <summary>
  ''' Unknown cause 
  ''' </summary>
  Unknown
  ''' <summary>
  ''' The stage changed due to normal operation
  ''' </summary>
  Normal
  ''' <summary>
  ''' A call is being transferred to this user agent from another user agent.
  ''' </summary>
  Transferred
  ''' <summary>
  '''  A call on this user agent is being transferred to another user agent.
  ''' </summary>
  Transfer
  ''' <summary>
  ''' A conference operation caused a stage change
  ''' </summary>
  Conference
  ''' <summary>
  ''' The remote party is alerting and providing ringback audio (early media)
  ''' </summary>
  EarlyMedia
  ''' <summary>
  ''' The callee rejected a request (e.g. hold) 
  ''' </summary>
  RequestNotAccepted
  ''' <summary>
  ''' The state changed due to a bad address.  This can be caused by a 
  ''' malformed URL or network problems with your DNS server.
  ''' </summary>
  BadAddress
  ''' <summary>
  ''' The state cahnged because the remote party is busy
  ''' </summary>
  Busy
  ''' <summary>
  ''' Not enough resources are available to complete the desired operation.
  ''' </summary>
  ResourceLimit
  ''' <summary>
  ''' A network error caused the desired operation to fail
  ''' </summary>
  Network
  ''' <summary>
  ''' The stage changed due to a redirection of a call. 
  ''' </summary>
  Redirected
  ''' <summary>
  ''' No response was received from the remote party or network node
  ''' </summary>
  NoReponse
  ''' <summary>
  ''' Unable to authenticate due to either bad or missing credentials 
  ''' </summary>
  Auth
  ''' <summary>
  ''' A transfer attempt has been initiated.  This event
  ''' is sent when a user agent attempts either a blind
  ''' or consultative transfer.
  ''' </summary>
  TransferInitiated
  ''' <summary>
  ''' A transfer attempt has been accepted by the remote
  ''' transferee.  This event indicates that the 
  ''' transferee supports transfers (REFER method).  The
  ''' event is fired upon a 2xx class response to the SIP
  ''' REFER request.
  ''' </summary>
  TransferAccepted
  ''' <summary>
  ''' The transfer target is attempting the transfer.  
  ''' This event is sent when transfer target (or proxy /
  ''' B2BUA) receives the call invitation, but before the
  ''' the tranfer target accepts is.
  ''' </summary>
  TransferTrying
  ''' <summary>
  ''' The transfer target is ringing.  This event is 
  ''' generally only sent during blind transfer.  
  ''' Consultative transfer should proceed directly to 
  ''' TransferSuccess or TransferFailure. */
  ''' </summary>
  TransferRinging
  ''' <summary>
  ''' The transfer was completed successfully.  The
  ''' original call to transfer target will
  ''' automatically disconnect.
  ''' </summary>
  TransferSuccess
  ''' <summary>
  ''' The transfer failed.  After a transfer fails,
  ''' the application layer is responsible for 
  ''' recovering original call to the transferee. 
  ''' That call is left on hold. 
  ''' </summary>
  TransferFailure
  ''' <summary>
  ''' Fired if the remote party's user-agent does not support S/MIME.
  ''' </summary>
  RemoteSmimeUnsupported
  ''' <summary>
  ''' Fired if a local S/MIME operation failed. 
  ''' For more information, applications should 
  ''' process the Security event.
  ''' </summary>
  SmimeFailure
  ''' <summary>
  ''' The even was fired as part of sipXtapi shutdown
  ''' </summary>
  Shutdown
End Enum
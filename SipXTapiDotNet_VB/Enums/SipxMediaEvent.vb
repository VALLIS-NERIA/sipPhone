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
''' Enumeration of possible media events
''' </summary>
Public Enum SipxMediaEvent
  ''' <summary>
  ''' Unknown or undefined media event, this is
  ''' generally the sign of an internal error in 
  ''' sipXtapi
  ''' </summary>
  Unknown = 0
  ''' <summary>
  ''' Local media (audio or video) is being
  ''' sent to the remote party
  ''' </summary>
  LocalStart = 50000
  ''' <summary>
  ''' Local media (audio or video) is no longer
  ''' being sent to the remote party.  This may
  ''' be caused by a local/remote hold 
  ''' operation, call tear down, or error.  See
  ''' the SipxMediaCause enumeration for more
  ''' information.
  ''' </summary>
  LocalStop
  ''' <summary>
  ''' Remote media (audio or video) is ready to
  ''' be received.  If no audio/video is 
  ''' received for longer then the idle period,
  ''' a RemoteSilent event will be fired.
  ''' See sipxConfigSetConnectionIdleTimeout.
  ''' </summary>
  RemoteStart
  ''' <summary>
  ''' Remote media (audio or video) has been 
  ''' stopped due to a hold or call tear down.
  ''' </summary>
  RemoteStop
  ''' <summary>
  ''' Remote media has not been received for 
  ''' some configured period.  This generally
  ''' indicates a network problem and/or a
  ''' problem with the remote party.  See 
  ''' sipxConfigSetConnectionIdleTimeout for
  ''' more information.
  ''' </summary>
  RemoteSilent
  ''' <summary>
  ''' A file is being played to local and/or
  ''' remote parties.  This event will be
  ''' followed by a PlayFileStop when
  ''' the file is manually stopped or 
  ''' finished playing.
  ''' </summary>
  PlayFileStart
  ''' <summary>
  ''' A file has completed playing or was aborted.
  ''' </summary>
  PlayFileStop
  ''' <summary>
  ''' A buffer is being played to local and/or 
  ''' remote parties.  This event will be
  ''' followed by a PlayBufferStop when
  ''' the file is manually stopped or 
  ''' finished playing.
  ''' </summary>
  PlayBufferStart
  ''' <summary>
  ''' A buffer has completed playing or was aborted.
  ''' </summary>
  PlayBufferStop
  ''' <summary>
  ''' Not yet implemented 
  ''' </summary>
  RemoteDtmf
  ''' <summary>
  ''' Fired if the media device is not present or already in use.
  ''' </summary>
  DeviceFailure
End Enum

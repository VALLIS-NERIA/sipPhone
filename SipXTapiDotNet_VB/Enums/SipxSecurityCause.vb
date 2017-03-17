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
''' Enumeration of possible security causes
''' </summary>
Public Enum SipxSecurityCause
  ''' <summary>
  ''' An UNKNOWN cause code is generated when the state
  ''' for the security operation 
  ''' is no longer known.  This is generally an error 
  ''' condition; see the info structure for details.
  ''' </summary>
  Unknown = 0
  ''' <summary>
  ''' Event was fired as part of the normal encryption / decryption process. 
  ''' </summary>
  Normal
  ''' <summary>
  ''' An S/MIME encryption succeeded.
  ''' </summary>
  EncryptSuccess
  ''' <summary>
  ''' An S/MIME encryption failed because the
  ''' security library could not start.
  ''' </summary>
  EncryptFailureLibInit
  ''' <summary>
  ''' An S/MIME encryption failed because of a bad certificate / public key. 
  ''' </summary>
  EncryptFailureBadPublicKey
  ''' <summary>
  ''' An S/MIME encryption failed because of an invalid parameter.
  ''' </summary>
  EncryptFailureInvalidParameter
  ''' <summary>
  ''' An S/MIME decryption succeeded.
  ''' </summary>
  DecryptSuccess
  ''' <summary>
  ''' An S/MIME decryption failed due to a failure to initialize the certificate database.
  ''' </summary>
  DecryptFailureDBInit
  ''' <summary>
  ''' An S/MIME decryption failed due to an invalid certificate database password.
  ''' </summary>
  DecryptFailureBadDBPassword
  ''' <summary>
  ''' An S/MIME decryption failed due to an invalid parameter.
  ''' </summary>
  DecryptFailureInvalidParameter
  ''' <summary>
  ''' An S/MIME decryption operation aborted due to a bad signature.
  ''' </summary>
  DecryptBadSignature
  ''' <summary>
  ''' An S/MIME decryption operation aborted due to a missing signature.
  ''' </summary>
  DecryptMissingSignature
  ''' <summary>
  ''' An S/MIME decryption operation aborted because the signature was rejected.
  ''' </summary>
  DecryptSignatureRejected
  ''' <summary>
  ''' A TLS server certificate is being presented to the application for possible rejection.
  ''' The application must respond to this message.
  ''' If the application returns false, the certificate is rejected and the call will not
  ''' complete.  If the application returns true, the certificate is accepted. 
  ''' </summary>
  TlsServerCertificate
  ''' <summary>
  ''' A TLS operation failed due to a bad password.
  ''' </summary>
  TlsBadPassword
  ''' <summary>
  ''' A TLS operation failed.
  ''' </summary>
  TlsLibraryFailure
  ''' <summary>
  ''' The remote host is not reachable.
  ''' </summary>
  RemoteHostUnreachable
  ''' <summary>
  ''' A TLS connection to the remote party failed.
  ''' </summary>
  TlsConnectionFailure
  ''' <summary>
  ''' A failure occured during the TLS handshake.
  ''' </summary>
  TlsHandshakeFailure
  ''' <summary>
  ''' The SignatureNotify event is fired when the user-agent
  ''' receives a SIP message with signed SMIME as its content.
  ''' The signer's certificate will be located in the info structure
  ''' associated with this event.  The application can choose to accept
  ''' the signature, by returning 'true' in response to this message
  ''' or can choose to reject the signature
  ''' by returning 'false' in response to this message.
  ''' </summary>
  SignatureNotify
  ''' <summary>
  ''' The application has rejected the server's TLS certificate.
  ''' </summary>
  TlsCertificateRejected
End Enum

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
Public Enum SipxResult
  Success = 0          '/**< Success */
  Failure              '/**< Generic Failure*/
  NotImplemented       '/**< Method/API not implemented */
  OutOfMemory          '/**< Unable to allocate enough memory to perform operation*/
  InvalidArgs          '/**< Invalid arguments; bad handle, argument out of range, etc.*/
  BadAddress           '/**< Invalid SIP address */
  OutOfResources       '/**< Out of resources (hit some max limit) */
  InsufficientBuffer   '/**< Buffer too short for this operation */
  EvalTimeout          '/**< The evaluation version of this product has expired */
  Busy                 '/**< The operation failed because the system was busy */
  InvalidState         '/**< The operation failed because the object was in
  '                          the wrong state.  For example, attempting to split
  '                          a call from a conference before that call is 
  '                          connected. */
  MissingRuntimeFiles  '/**< The operation failed because required runtime dependencies are missing. */
  TlsDatabaseFailure   '/**< The operation failed because the certificate database did not initialize. */
  TlsBadPassword       '/**< The operation failed because the certificate database did not accept the password.*/
  TlsTcpImportFailure  '/**< The operation failed because a TCP socket could not be imported by the SSL/TLS module. */
  NssFailure           '/**< The operation failed due to an NSS failure. */
End Enum

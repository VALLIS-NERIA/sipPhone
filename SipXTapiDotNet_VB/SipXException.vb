'Copyright (c) 2006 ProfitFuel, Inc (Authors: Charlie Hedlin & Joshua Garvin) 
'
'This file is part of SipXTapiDotNet.

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
<Serializable()> Public Class SipxException
  Inherits System.Exception
  Private m_SipxResult As SipxResult

  Public Overrides ReadOnly Property Message() As String
    Get
      Dim rv As String
      If String.IsNullOrEmpty(MyBase.Message) Then
        rv = MyBase.Message
      Else
        Select Case m_SipxResult
          Case SipxResult.BadAddress
            rv = "SipXTapi returned error: Bad Address"
          Case SipxResult.Failure
            rv = "SipXTapi returned error: Failure"
          Case SipxResult.InvalidArgs
            rv = "SipXTapi returned error: Invalid Args"
          Case SipxResult.NotImplemented
            rv = "SipXTapi returned error: Not Implemented"
          Case SipxResult.OutOfMemory
            rv = "SipXTapi returned error: Out of Memory"
          Case SipxResult.OutOfResources
            rv = "SipXTapi returned error: Out of Resources"
          Case SipxResult.Success
            rv = "SipXTapi returned status: Success"
          Case Else
            rv = "SipXTapi returned unknown status: " & m_SipxResult.ToString
        End Select
      End If
      Return rv
    End Get
  End Property
  Public Sub New()
    MyBase.New()
  End Sub
  Public Sub New(ByVal result As SipxResult)
    MyBase.New()
    m_SipxResult = result
  End Sub
  Public Sub New(ByVal message As String)
    MyBase.New(message)
  End Sub
  Public Sub New(ByVal message As String, ByVal result As SipxResult)
    MyBase.New(message)
    m_SipxResult = result
  End Sub
  Public Sub New(ByVal message As String, ByVal innerException As Exception)
    MyBase.New(message, innerException)
  End Sub
  Public Sub New(ByVal message As String, ByVal innerException As Exception, ByVal result As SipxResult)
    MyBase.New(message, innerException)
    m_SipxResult = result
  End Sub
  Protected Sub New(ByVal info As Runtime.Serialization.SerializationInfo, ByVal context As Runtime.Serialization.StreamingContext)
    MyBase.New(info, context)
  End Sub
  Protected Sub New(ByVal info As Runtime.Serialization.SerializationInfo, ByVal context As Runtime.Serialization.StreamingContext, ByVal result As SipxResult)
    MyBase.New(info, context)
    m_SipxResult = result
  End Sub
End Class

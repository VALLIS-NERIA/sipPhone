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
<Serializable()> Public Class SipxLineEventArgs
  Inherits SipxEventArgs


  Private m_LineStateInfo As SipxLineStateInfo

  Public ReadOnly Property LineStateinfo() As SipxLineStateInfo
    Get
      Return m_LineStateInfo
    End Get
  End Property

  Friend Sub New(ByVal lineStateInfo As SipxLineStateInfo)
    m_EventCategory = SipxEventCategory.LineState
    m_LineStateInfo = lineStateInfo
  End Sub
End Class

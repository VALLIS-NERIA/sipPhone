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
Public Enum SipxToneId
  Dtmf_0 = Microsoft.VisualBasic.Asc("0")
  Dtmf_1 = Microsoft.VisualBasic.Asc("1")
  Dtmf_2 = Microsoft.VisualBasic.Asc("2")
  Dtmf_3 = Microsoft.VisualBasic.Asc("3")
  Dtmf_4 = Microsoft.VisualBasic.Asc("4")
  Dtmf_5 = Microsoft.VisualBasic.Asc("5")
  Dtmf_6 = Microsoft.VisualBasic.Asc("6")
  Dtmf_7 = Microsoft.VisualBasic.Asc("7")
  Dtmf_8 = Microsoft.VisualBasic.Asc("8")
  Dtmf_9 = Microsoft.VisualBasic.Asc("9")
  Dtmf_Star = Microsoft.VisualBasic.Asc("*")
  Dtmf_Pound = Microsoft.VisualBasic.Asc("#")
  Dtmf_Flash = Microsoft.VisualBasic.Asc("!")
  Tone_Dialtone = 512
  Tone_Busy
  Tone_Ringback
  Tone_Ringtone
  Tone_CallFailed
  Tone_Silence
  Tone_Backspace
  Tone_CallWaiting
  Tone_CallHeld
  Tone_LoudFastBusy
End Enum

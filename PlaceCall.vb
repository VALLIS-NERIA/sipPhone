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
Public Class PlaceCall
  Private WithEvents m_Instance As SipXTapiDotNet.SipxInstance
  Private m_LineNum As Integer
  Private m_CallNum As Integer
  Private WithEvents m_Config As SipXTapiDotNet.SipxConfig
  Private m_Event As SipXTapiDotNet.SipxEvent

  Private Sub PlaceCallButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlaceCallButton.Click
    PlaceCallButton.Enabled = False
    SettingsGroupBox.Enabled = False
    Me.Cursor = Cursors.WaitCursor
    If Not m_Instance.IsConnected Then
      Try
        m_Instance.Identity = """" & IdentityDisplayTextBox.Text & """ <" & IdentityAddressTextBox.Text & ">"
        m_Instance.Connect(True)

        Console.WriteLine("Set Log Level...")
        m_Config = SipXTapiDotNet.SipxConfig.Create(m_Instance)

        Console.WriteLine("Set Input Device...")
        m_Instance.Audio.SetInputDevice(AudioInputComboBox.Text)

        Console.WriteLine("Set Output Device...")
        m_Instance.Audio.SetOutputDevice(AudioOutputComboBox.Text)

        Console.WriteLine("Dump Local Contacts...")
        m_Config.DumpLocalContacts()

        Console.WriteLine("Creating line...")
        m_LineNum = m_Instance.LineCreate()
        Console.WriteLine("m_LineNum=" & m_LineNum)

        Console.WriteLine("Add credentials...")
        m_Instance.Lines(m_LineNum).AddCreditial(UsernameTextBox.Text, PasswordTextBox.Text, RealmTextBox.Text)

        Console.WriteLine("Call line...")
        m_CallNum = m_Instance.Lines(m_LineNum).CallCreate()

        Console.WriteLine("Call Connect...")
        m_Instance.Lines(m_LineNum).Calls(m_CallNum).Connect(Me.SipAddressTextBox.Text)

        Console.WriteLine("Done!")
      Catch ex As System.Exception
        MsgBox(ex.ToString, MsgBoxStyle.OkOnly, ex.Message)
      Finally
        Me.Cursor = Cursors.Default
        If m_Instance.IsConnected Then
          SettingsGroupBox.Enabled = False
          PlaceCallButton.Text = "Disconnect"
        End If
        PlaceCallButton.Enabled = True
      End Try
    Else
      Try
        'Try
        Console.WriteLine("Disconnecting...")
        m_Instance.Disconnect(True)
      Catch ex As System.Exception
        MsgBox(ex.ToString, MsgBoxStyle.OkOnly, ex.Message)
      Finally
        Me.Cursor = Cursors.Default
        If Not m_Instance.IsConnected Then
          SettingsGroupBox.Enabled = True
          PlaceCallButton.Text = "Place Call"
        End If
        PlaceCallButton.Enabled = True
      End Try
    End If
  End Sub

  Private Sub m_Instance_InstanceEvent(ByVal sender As Object, ByVal e As SipXTapiDotNet.SipxLineEventArgs) Handles m_Instance.LineEvent
    Console.WriteLine("Event: " & e.EventCategory.ToString & ", " & e.LineStateinfo.StateCause.ToString & ", " & e.LineStateinfo.StateEvent.ToString)
  End Sub

  Private Sub m_Instance_InstanceEvent(ByVal sender As Object, ByVal e As SipXTapiDotNet.SipxCallEventArgs) Handles m_Instance.CallEvent
    Console.WriteLine("Event: " & e.EventCategory.ToString & ", " & e.CallStateinfo.StateEvent.ToString & ", " & e.CallStateinfo.StateCause.ToString)
  End Sub

  Private Sub PlaceCall_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    m_Instance.Dispose()
  End Sub

  Private Sub PlaceCall_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Console.WriteLine("Set Log Level...")
        'SipXTapiDotNet.SipxConfig.LogFile = "SipxTapiPhone.log"
        'SipXTapiDotNet.SipxConfig.LogFile = SipXTapiDotNet.SipxLogLevel.Debug

        Console.WriteLine("Instance...")
    m_Instance = SipXTapiDotNet.SipxInstance.Create("", tlsPort:=-1)
    m_Instance.Connect(True)
    AudioInputComboBox.Items.AddRange(m_Instance.Audio.GetDevices(SipXTapiDotNet.SipxMediaDeviceType.Input).ToArray)
    AudioOutputComboBox.Items.AddRange(m_Instance.Audio.GetDevices(SipXTapiDotNet.SipxMediaDeviceType.Output).ToArray)
    m_Instance.Disconnect(True)
  End Sub
End Class

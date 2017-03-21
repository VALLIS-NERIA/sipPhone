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
Imports System.Threading

Public Class EventQueueMonitor

  Private m_MonitorThread As Thread
  Private m_Shutdown As Boolean
  Private m_SipXEventQueue As Queue(Of SipxEventArgs)
  Private Shared m_InstanceObjects As New Generic.Dictionary(Of SipxInstance, EventQueueMonitor)

  Private WithEvents m_Instance As SipxInstance
  Private m_NotificationEvent As AutoResetEvent

  Public Event SipxCallEvent As EventHandler(Of SipxCallEventArgs)
  Public Event SipxMediaEvent As EventHandler(Of SipxEventArgs)
  Public Event SipxLineEvent As EventHandler(Of SipxLineEventArgs)

  Private Sub New()
    m_SipXEventQueue = New Queue(Of SipxEventArgs)
    m_NotificationEvent = New AutoResetEvent(False)
  End Sub

  Public Shared Function GetMonitor(ByVal instance As SipxInstance) As EventQueueMonitor
    Dim rv As EventQueueMonitor
    If m_InstanceObjects.ContainsKey(instance) Then
      rv = m_InstanceObjects(instance)
    Else
      rv = New EventQueueMonitor
      m_InstanceObjects.Add(instance, rv)
      rv.m_Shutdown = False
      rv.m_Instance = instance
      rv.StartEventQueueMonitorThread()
    End If
    Return rv
  End Function
  Public Sub Shutdown()
    m_Shutdown = True
    If m_MonitorThread.ThreadState And ThreadState.Stopped <> ThreadState.Stopped Then
      m_MonitorThread.Join(3000)
    End If
  End Sub

  Public Sub EnqueueSipxEvent(ByVal myEventArgs As EventArgs)
    m_SipXEventQueue.Enqueue(myEventArgs)
    m_NotificationEvent.Set()
  End Sub
  Public Sub HandleSipXInstanceEvent(ByVal sender As Object, ByVal myEventArgs As EventArgs) Handles m_Instance.SipxInstanceEvent
    'Console.WriteLine("EventMonitorThread: " & Now.TimeOfDay.ToString & " Enqueing: " & mySipxEventArgs.EventCategory.ToString & " Event, " & mySipxEventArgs.StateEvent.ToString & " " & mySipxEventArgs.StateCause.ToString)
    EnqueueSipxEvent(myEventArgs)
  End Sub

  Private Sub handleEvent(ByVal mySipxEventArgs As SipxEventArgs)
    Console.WriteLine("EventMonitorThread: " & Now.TimeOfDay.ToString & " Handling: " & mySipxEventArgs.EventCategory.ToString & " Event.")
    Select Case mySipxEventArgs.EventCategory
      Case SipxEventCategory.CallState
        RaiseEvent SipxCallEvent(m_Instance, mySipxEventArgs)
      Case SipxEventCategory.Config
      Case SipxEventCategory.Info
      Case SipxEventCategory.InfoStatus
      Case SipxEventCategory.LineState
        RaiseEvent SipxLineEvent(m_Instance, mySipxEventArgs)
      Case SipxEventCategory.Media
        RaiseEvent SipxMediaEvent(m_Instance, mySipxEventArgs)
      Case SipxEventCategory.Notify
      Case SipxEventCategory.Security
      Case SipxEventCategory.SubStatus
    End Select
    Console.WriteLine("EventMonitorThread: " & Now.TimeOfDay.ToString & " Handled: " & mySipxEventArgs.EventCategory.ToString & " Event")
  End Sub

  Private Sub StartEventQueueMonitorThread()
    If Not (m_MonitorThread Is Nothing) Then
      If m_MonitorThread.ThreadState And ThreadState.Stopped <> ThreadState.Stopped Then
        If Not m_MonitorThread.Join(500) Then
          Throw New Exception("Notify thread was not stopped!")
        End If
      End If
      m_MonitorThread = Nothing
    End If
    m_Shutdown = False
    m_MonitorThread = New Thread(New ThreadStart(AddressOf EventQueueMonitorThread))
    m_MonitorThread.Name = "Event Queue Monitor"
    m_MonitorThread.Start()
  End Sub

  Private Sub EventQueueMonitorThread()
    While Not m_Shutdown
      m_NotificationEvent.WaitOne(1000, True)
      Do While m_SipXEventQueue.Count > 0
        handleEvent(m_SipXEventQueue.Dequeue)
      Loop
    End While
  End Sub

End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PlaceCall
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing AndAlso components IsNot Nothing Then
      components.Dispose()
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.PlaceCallButton = New System.Windows.Forms.Button
    Me.SipAddressTextBox = New System.Windows.Forms.TextBox
    Me.SipAddressLabel = New System.Windows.Forms.Label
    Me.SettingsGroupBox = New System.Windows.Forms.GroupBox
    Me.AudioOutputLabel = New System.Windows.Forms.Label
    Me.AudioInputLabel = New System.Windows.Forms.Label
    Me.AudioOutputComboBox = New System.Windows.Forms.ComboBox
    Me.AudioInputComboBox = New System.Windows.Forms.ComboBox
    Me.IdentityAddressTextBox = New System.Windows.Forms.TextBox
    Me.IdentityDisplayTextBox = New System.Windows.Forms.TextBox
    Me.RealmTextBox = New System.Windows.Forms.TextBox
    Me.PasswordTextBox = New System.Windows.Forms.TextBox
    Me.UsernameTextBox = New System.Windows.Forms.TextBox
    Me.IdentityAddressLabel = New System.Windows.Forms.Label
    Me.IdentityDisplayLabel = New System.Windows.Forms.Label
    Me.RealmLabel = New System.Windows.Forms.Label
    Me.PasswordLabel = New System.Windows.Forms.Label
    Me.UsernameLabel = New System.Windows.Forms.Label
    Me.SettingsGroupBox.SuspendLayout()
    Me.SuspendLayout()
    '
    'PlaceCallButton
    '
    Me.PlaceCallButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.PlaceCallButton.Location = New System.Drawing.Point(198, 264)
    Me.PlaceCallButton.Name = "PlaceCallButton"
    Me.PlaceCallButton.Size = New System.Drawing.Size(75, 23)
    Me.PlaceCallButton.TabIndex = 0
    Me.PlaceCallButton.Text = "Place Call"
    Me.PlaceCallButton.UseVisualStyleBackColor = True
    '
    'SipAddressTextBox
    '
    Me.SipAddressTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.SipAddressTextBox.Location = New System.Drawing.Point(12, 238)
    Me.SipAddressTextBox.Name = "SipAddressTextBox"
    Me.SipAddressTextBox.Size = New System.Drawing.Size(261, 20)
    Me.SipAddressTextBox.TabIndex = 1
    '
    'SipAddressLabel
    '
    Me.SipAddressLabel.AutoSize = True
    Me.SipAddressLabel.Location = New System.Drawing.Point(12, 222)
    Me.SipAddressLabel.Name = "SipAddressLabel"
    Me.SipAddressLabel.Size = New System.Drawing.Size(102, 13)
    Me.SipAddressLabel.TabIndex = 2
    Me.SipAddressLabel.Text = "Sip Address To Call:"
    '
    'SettingsGroupBox
    '
    Me.SettingsGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.SettingsGroupBox.Controls.Add(Me.AudioOutputLabel)
    Me.SettingsGroupBox.Controls.Add(Me.AudioInputLabel)
    Me.SettingsGroupBox.Controls.Add(Me.AudioOutputComboBox)
    Me.SettingsGroupBox.Controls.Add(Me.AudioInputComboBox)
    Me.SettingsGroupBox.Controls.Add(Me.IdentityAddressTextBox)
    Me.SettingsGroupBox.Controls.Add(Me.IdentityDisplayTextBox)
    Me.SettingsGroupBox.Controls.Add(Me.RealmTextBox)
    Me.SettingsGroupBox.Controls.Add(Me.PasswordTextBox)
    Me.SettingsGroupBox.Controls.Add(Me.UsernameTextBox)
    Me.SettingsGroupBox.Controls.Add(Me.IdentityAddressLabel)
    Me.SettingsGroupBox.Controls.Add(Me.IdentityDisplayLabel)
    Me.SettingsGroupBox.Controls.Add(Me.RealmLabel)
    Me.SettingsGroupBox.Controls.Add(Me.PasswordLabel)
    Me.SettingsGroupBox.Controls.Add(Me.UsernameLabel)
    Me.SettingsGroupBox.Location = New System.Drawing.Point(12, 3)
    Me.SettingsGroupBox.Name = "SettingsGroupBox"
    Me.SettingsGroupBox.Size = New System.Drawing.Size(261, 209)
    Me.SettingsGroupBox.TabIndex = 13
    Me.SettingsGroupBox.TabStop = False
    Me.SettingsGroupBox.Text = "Settings"
    '
    'AudioOutputLabel
    '
    Me.AudioOutputLabel.AutoSize = True
    Me.AudioOutputLabel.Location = New System.Drawing.Point(21, 179)
    Me.AudioOutputLabel.Name = "AudioOutputLabel"
    Me.AudioOutputLabel.Size = New System.Drawing.Size(72, 13)
    Me.AudioOutputLabel.TabIndex = 26
    Me.AudioOutputLabel.Text = "Audio Output:"
    '
    'AudioInputLabel
    '
    Me.AudioInputLabel.AutoSize = True
    Me.AudioInputLabel.Location = New System.Drawing.Point(29, 152)
    Me.AudioInputLabel.Name = "AudioInputLabel"
    Me.AudioInputLabel.Size = New System.Drawing.Size(64, 13)
    Me.AudioInputLabel.TabIndex = 25
    Me.AudioInputLabel.Text = "Audio Input:"
    '
    'AudioOutputComboBox
    '
    Me.AudioOutputComboBox.FormattingEnabled = True
    Me.AudioOutputComboBox.Location = New System.Drawing.Point(100, 176)
    Me.AudioOutputComboBox.Name = "AudioOutputComboBox"
    Me.AudioOutputComboBox.Size = New System.Drawing.Size(155, 21)
    Me.AudioOutputComboBox.TabIndex = 24
    '
    'AudioInputComboBox
    '
    Me.AudioInputComboBox.FormattingEnabled = True
    Me.AudioInputComboBox.Location = New System.Drawing.Point(100, 149)
    Me.AudioInputComboBox.Name = "AudioInputComboBox"
    Me.AudioInputComboBox.Size = New System.Drawing.Size(155, 21)
    Me.AudioInputComboBox.TabIndex = 23
    '
    'IdentityAddressTextBox
    '
    Me.IdentityAddressTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.IdentityAddressTextBox.Location = New System.Drawing.Point(100, 122)
    Me.IdentityAddressTextBox.Name = "IdentityAddressTextBox"
    Me.IdentityAddressTextBox.Size = New System.Drawing.Size(155, 20)
    Me.IdentityAddressTextBox.TabIndex = 22
    '
    'IdentityDisplayTextBox
    '
    Me.IdentityDisplayTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.IdentityDisplayTextBox.Location = New System.Drawing.Point(100, 96)
    Me.IdentityDisplayTextBox.Name = "IdentityDisplayTextBox"
    Me.IdentityDisplayTextBox.Size = New System.Drawing.Size(155, 20)
    Me.IdentityDisplayTextBox.TabIndex = 21
    '
    'RealmTextBox
    '
    Me.RealmTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.RealmTextBox.Location = New System.Drawing.Point(100, 70)
    Me.RealmTextBox.Name = "RealmTextBox"
    Me.RealmTextBox.Size = New System.Drawing.Size(155, 20)
    Me.RealmTextBox.TabIndex = 20
    '
    'PasswordTextBox
    '
    Me.PasswordTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.PasswordTextBox.Location = New System.Drawing.Point(100, 45)
    Me.PasswordTextBox.Name = "PasswordTextBox"
    Me.PasswordTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
    Me.PasswordTextBox.Size = New System.Drawing.Size(155, 20)
    Me.PasswordTextBox.TabIndex = 19
    '
    'UsernameTextBox
    '
    Me.UsernameTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.UsernameTextBox.Location = New System.Drawing.Point(100, 19)
    Me.UsernameTextBox.Name = "UsernameTextBox"
    Me.UsernameTextBox.Size = New System.Drawing.Size(155, 20)
    Me.UsernameTextBox.TabIndex = 18
    '
    'IdentityAddressLabel
    '
    Me.IdentityAddressLabel.AutoSize = True
    Me.IdentityAddressLabel.Location = New System.Drawing.Point(8, 125)
    Me.IdentityAddressLabel.Name = "IdentityAddressLabel"
    Me.IdentityAddressLabel.Size = New System.Drawing.Size(85, 13)
    Me.IdentityAddressLabel.TabIndex = 17
    Me.IdentityAddressLabel.Text = "Identity Address:"
    '
    'IdentityDisplayLabel
    '
    Me.IdentityDisplayLabel.AutoSize = True
    Me.IdentityDisplayLabel.Location = New System.Drawing.Point(12, 99)
    Me.IdentityDisplayLabel.Name = "IdentityDisplayLabel"
    Me.IdentityDisplayLabel.Size = New System.Drawing.Size(81, 13)
    Me.IdentityDisplayLabel.TabIndex = 16
    Me.IdentityDisplayLabel.Text = "Identity Display:"
    '
    'RealmLabel
    '
    Me.RealmLabel.AutoSize = True
    Me.RealmLabel.Location = New System.Drawing.Point(53, 73)
    Me.RealmLabel.Name = "RealmLabel"
    Me.RealmLabel.Size = New System.Drawing.Size(40, 13)
    Me.RealmLabel.TabIndex = 15
    Me.RealmLabel.Text = "Realm:"
    '
    'PasswordLabel
    '
    Me.PasswordLabel.AutoSize = True
    Me.PasswordLabel.Location = New System.Drawing.Point(38, 48)
    Me.PasswordLabel.Name = "PasswordLabel"
    Me.PasswordLabel.Size = New System.Drawing.Size(56, 13)
    Me.PasswordLabel.TabIndex = 14
    Me.PasswordLabel.Text = "Password:"
    '
    'UsernameLabel
    '
    Me.UsernameLabel.AutoSize = True
    Me.UsernameLabel.Location = New System.Drawing.Point(35, 22)
    Me.UsernameLabel.Name = "UsernameLabel"
    Me.UsernameLabel.Size = New System.Drawing.Size(58, 13)
    Me.UsernameLabel.TabIndex = 13
    Me.UsernameLabel.Text = "Username:"
    '
    'PlaceCall
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(285, 296)
    Me.Controls.Add(Me.SettingsGroupBox)
    Me.Controls.Add(Me.SipAddressLabel)
    Me.Controls.Add(Me.SipAddressTextBox)
    Me.Controls.Add(Me.PlaceCallButton)
    Me.Name = "PlaceCall"
    Me.Text = "Place Call Demo"
    Me.SettingsGroupBox.ResumeLayout(False)
    Me.SettingsGroupBox.PerformLayout()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents PlaceCallButton As System.Windows.Forms.Button
  Friend WithEvents SipAddressTextBox As System.Windows.Forms.TextBox
  Friend WithEvents SipAddressLabel As System.Windows.Forms.Label
  Friend WithEvents SettingsGroupBox As System.Windows.Forms.GroupBox
  Friend WithEvents IdentityAddressTextBox As System.Windows.Forms.TextBox
  Friend WithEvents IdentityDisplayTextBox As System.Windows.Forms.TextBox
  Friend WithEvents RealmTextBox As System.Windows.Forms.TextBox
  Friend WithEvents PasswordTextBox As System.Windows.Forms.TextBox
  Friend WithEvents UsernameTextBox As System.Windows.Forms.TextBox
  Friend WithEvents IdentityAddressLabel As System.Windows.Forms.Label
  Friend WithEvents IdentityDisplayLabel As System.Windows.Forms.Label
  Friend WithEvents RealmLabel As System.Windows.Forms.Label
  Friend WithEvents PasswordLabel As System.Windows.Forms.Label
  Friend WithEvents UsernameLabel As System.Windows.Forms.Label
  Friend WithEvents AudioOutputLabel As System.Windows.Forms.Label
  Friend WithEvents AudioInputLabel As System.Windows.Forms.Label
  Friend WithEvents AudioOutputComboBox As System.Windows.Forms.ComboBox
  Friend WithEvents AudioInputComboBox As System.Windows.Forms.ComboBox

End Class

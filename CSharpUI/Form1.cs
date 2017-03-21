using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SipXTapiDotNet;

namespace CSharpUI {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        private SipxInstance sipInstance;
        public void Init() {
            sipInstance = SipxInstance.Create("", tlsPort: -1);
            sipInstance.Connect(true);
            AudioInputComboBox.Items.AddRange(sipInstance.Audio.GetDevices(SipXTapiDotNet.SipxMediaDeviceType.Input).ToArray());
            AudioOutputComboBox.Items.AddRange(sipInstance.Audio.GetDevices(SipXTapiDotNet.SipxMediaDeviceType.Output).ToArray());
            sipInstance.Disconnect(true);
        }
        public void Call() {
            PlaceCallButton.Enabled = false;
            SettingsGroupBox.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            if (!sipInstance.IsConnected) {
                try {
                    sipInstance.Identity = $"\"{IdentityDisplayTextBox.Text} \"<{IdentityAddressTextBox.Text}>";
                    sipInstance.Connect(true);


                }
                catch (Exception ex){
                    MessageBox.Show(ex.ToString(),ex.Message);
                }
            }


        }

        private void Form1_Load(object sender, EventArgs e) {
            MessageBox.Show(Helper.waveInGetNumDevs().ToString());
            Helper.GetWaveInDevL();
        }
    }
}

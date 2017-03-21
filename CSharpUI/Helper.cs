using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace CSharpUI {
    class Helper {
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct WaveInCaps {
            public short wMid;
            public short wPid;
            public int vDriverVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szPname;
            public uint dwFormats;
            public short wChannels;
            public short wReserved1;
        }

        [DllImport("winmm.dll")]
        public static extern int waveInGetNumDevs();
        [DllImport("winmm.dll", EntryPoint = "waveInGetDevCaps")]
        public static extern int waveInGetDevCaps(int uDeviceId, ref WaveInCaps lpCaps, int uSize);
        public static List<string> GetWaveInDevL() {
            List<string> ls = new List<string>();
            int count = waveInGetNumDevs();
            if (count < 1) return ls;
            for(int i = 0; i < count; i++) {
                WaveInCaps cap = new WaveInCaps();
                waveInGetDevCaps(i, ref cap, Marshal.SizeOf(cap));
            }
            return ls;
        }
    }
}

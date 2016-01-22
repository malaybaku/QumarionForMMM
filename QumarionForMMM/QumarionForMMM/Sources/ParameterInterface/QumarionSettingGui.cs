using System;
using System.ComponentModel;
using System.Windows.Forms;

using Baku.Quma.Pdk;

namespace QumarionForMMM
{
    public partial class QumarionSettingGui : UserControl, INotifyPropertyChanged
    {
        public QumarionSettingGui()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool UseAccelerometer { get; private set; } = false;
        public bool UseAccelerometerFilter { get; private set; } = false;
        public AccelerometerRestrictMode AccelerometerRestrictMode { get; private set; } = AccelerometerRestrictMode.None;
        public bool BindFootToGround { get; private set; } = false;
        public uint LegIKScaleFactor { get; private set; } = 10;
        public float ArmAngle { get; private set; } = 35.0f;

        private void checkBoxUseAccelerometer_CheckedChanged(object sender, EventArgs e)
        {
            UseAccelerometer = (bool)Invoke(new UseAccelerometerDelegate(() =>
                checkBoxUseAccelerometer.Checked
                ));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseAccelerometer)));
        }
        private delegate bool UseAccelerometerDelegate();

        private void checkBoxUseAccelFilter_CheckedChanged(object sender, EventArgs e)
        {
            UseAccelerometerFilter = (bool)Invoke(new UseAccelerometerDelegate(() =>
                checkBoxUseAccelFilter.Checked
                ));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseAccelerometerFilter)));
        }
        private delegate bool UseAccelerometerFilterDelegate();

        private void radioButtonAccelLimitNone_CheckedChanged(object sender, EventArgs e)
        {
            bool isOn = (bool)Invoke(new UseAccelerometerDelegate(() => radioButtonAccelLimitNone.Checked));
            if(isOn)
            {
                AccelerometerRestrictMode = AccelerometerRestrictMode.None;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccelerometerRestrictMode)));
            }
        }
        private delegate bool AccelLimitNoneDelegate();
        private void radioButtonAccelLimitX_CheckedChanged(object sender, EventArgs e)
        {
            bool isOn = (bool)Invoke(new UseAccelerometerDelegate(() => radioButtonAccelLimitX.Checked));
            if (isOn)
            {
                AccelerometerRestrictMode = AccelerometerRestrictMode.AxisX;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccelerometerRestrictMode)));
            }
        }
        private delegate bool AccelLimitXDelegate();
        private void radioButtonAccelLimitZ_CheckedChanged(object sender, EventArgs e)
        {
            bool isOn = (bool)Invoke(new UseAccelerometerDelegate(() => radioButtonAccelLimitZ.Checked));
            if (isOn)
            {
                AccelerometerRestrictMode = AccelerometerRestrictMode.AxisZ;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccelerometerRestrictMode)));
            }
        }
        private delegate bool AccelLimitZDelegate();

        private void checkBoxBindToGround_CheckedChanged(object sender, EventArgs e)
        {
            BindFootToGround = (bool)Invoke(new BindFootToGroundDelegate(() =>
                checkBoxBindToGround.Checked
                ));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BindFootToGround)));
        }
        private delegate bool BindFootToGroundDelegate();

        private void textBoxLegIKScaleFactor_TextChanged(object sender, EventArgs e)
        {
            string text = (string)Invoke(new LegIKScaleFactorDelegate(() => textBoxLegIKScaleFactor.Text));
            uint scaleFactor;
            if(uint.TryParse(text, out scaleFactor))
            {
                LegIKScaleFactor = scaleFactor;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LegIKScaleFactor)));
            }
        }
        private delegate string LegIKScaleFactorDelegate();

        private void textBoxArmAngle_TextChanged(object sender, EventArgs e)
        {
            string text = (string)Invoke(new LegIKScaleFactorDelegate(() => textBoxLegIKScaleFactor.Text));
            float armAngle;
            if (float.TryParse(text, out armAngle))
            {
                ArmAngle = armAngle;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ArmAngle)));
            }
        }
        private delegate string ArmAngleDelegate();
    }
}

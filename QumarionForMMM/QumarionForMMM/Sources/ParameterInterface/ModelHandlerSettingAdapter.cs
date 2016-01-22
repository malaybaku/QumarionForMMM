using System;
using System.ComponentModel;
using Baku.Quma.Pdk;

namespace QumarionForMMM
{

    /// <summary>プラグイン設定を<see cref="ModelHandler"/>に反映させるアダプタを表します。</summary>
    class ModelHandlerSettingAdapter
    {
        public ModelHandlerSettingAdapter(ModelHandler modelHandler)
        {
            _modelHandler = modelHandler;
        }

        public void Connect(QumarionSettingHolder setting)
        {
            Sync(setting);
            setting.PropertyChanged += OnSettingPropertyChanged;
        }

        public void Sync(QumarionSettingHolder setting)
        {
            _modelHandler.QumarionModel.AttachedQumarion.EnableAccelerometer = setting.UseAccelerometer;
            _modelHandler.QumarionModel.AccelerometerMode =
                setting.UseAccelerometerFilter ? AccelerometerMode.Relative : AccelerometerMode.Direct;
            _modelHandler.QumarionModel.AccelerometerRestrictMode = setting.AccelerometerRestrictMode;
            _modelHandler.BindFootToGround = setting.BindFootToGround;
            _modelHandler.ArmAngleRad = (float)(setting.ArmAngle * Math.PI / 180.0f);
            _modelHandler.LegIKScaleFactor = setting.LegIKScaleFactor * 0.01f;
        }

        private readonly ModelHandler _modelHandler;

        private void OnSettingPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is QumarionSettingHolder)) return;
            var setting = sender as QumarionSettingHolder;

            if (_modelHandler.QumarionModel.AttachedQumarion == null) return;

            if(e.PropertyName == nameof(setting.UseAccelerometer))
            {
                _modelHandler.QumarionModel.AttachedQumarion.EnableAccelerometer = setting.UseAccelerometer;
            }
            else if(e.PropertyName == nameof(setting.UseAccelerometerFilter))
            {
                _modelHandler.QumarionModel.AccelerometerMode =
                    setting.UseAccelerometerFilter ? AccelerometerMode.Relative : AccelerometerMode.Direct;
            }
            else if(e.PropertyName == nameof(setting.AccelerometerRestrictMode))
            {
                _modelHandler.QumarionModel.AccelerometerRestrictMode = setting.AccelerometerRestrictMode;
            }
            else if(e.PropertyName == nameof(setting.BindFootToGround))
            {
                _modelHandler.BindFootToGround = setting.BindFootToGround;
            }
            else if(e.PropertyName == nameof(setting.ArmAngle))
            {
                _modelHandler.ArmAngleRad = (float)(setting.ArmAngle * Math.PI / 180.0f);
            }
            else if(e.PropertyName == nameof(setting.LegIKScaleFactor))
            {
                _modelHandler.LegIKScaleFactor = setting.LegIKScaleFactor * 0.01f;
            }

            //DEBUG
            //System.Windows.Forms.MessageBox.Show(
            //    $"Use Accel. src:{setting.UseAccelerometer}, dst:(N/A)\n" +
            //    $"Use Accel Filter. src:{setting.UseAccelerometerFilter}, dst:{_modelHandler.QumarionModel.AccelerometerMode}\n" +
            //    $"Accel Restrict. src:{setting.AccelerometerRestrictMode}, dst:{_modelHandler.QumarionModel.AccelerometerRestrictMode}\n" + 
            //    $"FootToGround. src:{setting.BindFootToGround}, dst:{_modelHandler.BindFootToGround}\n" +
            //    $"LegIKScale. src:{setting.LegIKScaleFactor}, dst:{_modelHandler.LegIKScaleFactor}\n" +
            //    $"ArmAngle. src:{setting.ArmAngle:0.000}, dst:{_modelHandler.ArmAngleRad:0.000}\n"
            //    );
        }
    }
}

using System;
using System.ComponentModel;
using Baku.Quma.Pdk;

namespace QumarionForMMM
{
    /// <summary>
    /// QumarionのGUI設定の内容を保持するクラスです。
    /// (プラグインのロジックがGUIに直接触らないためのラッパー的なアレ)
    /// </summary>
    class QumarionSettingHolder : INotifyPropertyChanged
    {
        //NOTE: CallerMemberName属性が.NETバージョンの都合で非サポートっぽい？ちょっと違和感あるが。
        private void SetAndRaisePropertyChanged<T>(ref T target, T value, string pname)
            where T : struct, IEquatable<T>
        {
            if(!target.Equals(value))
            {
                target = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pname));
            }
        }

        private bool _useAccelerometer = false;
        /// <summary>加速度センサを使用するかどうかを取得、設定します。</summary>
        public bool UseAccelerometer
        {
            get { return _useAccelerometer; }
            set { SetAndRaisePropertyChanged(ref _useAccelerometer, value, nameof(UseAccelerometer)); }
        }

        private bool _userAccelerometerFilter = false;
        /// <summary>加速度センサの値にローパスフィルタ的なフィルタを適用するかどうかを取得、設定します。</summary>
        public bool UseAccelerometerFilter
        {
            get { return _userAccelerometerFilter; }
            set { SetAndRaisePropertyChanged(ref _userAccelerometerFilter, value, nameof(UseAccelerometerFilter)); }
        }

        private AccelerometerRestrictMode _acceletometerRestrictMode = AccelerometerRestrictMode.None;
        /// <summary>QUMARIONの回転制限モードを取得、設定します。</summary>
        public AccelerometerRestrictMode AccelerometerRestrictMode
        {
            get { return _acceletometerRestrictMode; }
            set
            {
                if(_acceletometerRestrictMode != value)
                {
                    _acceletometerRestrictMode = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccelerometerRestrictMode)));
                }
            }
        }

        private bool _bindFootToGround = false;
        /// <summary>常時接地を行うかどうかを取得、設定します。</summary>
        public bool BindFootToGround
        {
            get { return _bindFootToGround; }
            set { SetAndRaisePropertyChanged(ref _bindFootToGround, value, nameof(BindFootToGround)); }
        }

        private uint _legIKScaleFactor = 10;
        /// <summary>足IKの計算に用いるスケールファクターです。</summary>
        public uint LegIKScaleFactor
        {
            get { return _legIKScaleFactor; }
            set { SetAndRaisePropertyChanged(ref _legIKScaleFactor, value, nameof(LegIKScaleFactor)); }
        }

        private float _armAngle = 35.0f;
        public float ArmAngle
        {
            get { return _armAngle; }
            set { SetAndRaisePropertyChanged(ref _armAngle, value, nameof(ArmAngle)); }
        }

        /// <summary>GUI側の入力に応じて設定パラメタを更新するようにします。</summary>
        /// <param name="gui">イベントチェック先のGUI</param>
        public void SubscribeToUserControl(QumarionSettingGui gui)
        {
            gui.PropertyChanged += OnGuiPropertyChanged;
        }

        /// <summary>
        /// GUIの設定項目監視を終了します。
        /// MMMプラグインではGUI由来のメモリリークは多分起きないので呼び出さないでも問題ありません。
        /// </summary>
        /// <param name="gui">イベント監視解除する対象となるGUI</param>
        public void UnsubscribeToUserControl(QumarionSettingGui gui)
        {
            //廃棄済みのコントロールは触らない方がいいという一般的なイメージからガード
            if(!gui.IsDisposed)
            {
                gui.PropertyChanged -= OnGuiPropertyChanged;
            }
        }

        /// <summary>GUIの設定を全てコピーします。</summary>
        /// <param name="gui">コピー元のGUI</param>
        public void SyncToGui(QumarionSettingGui gui)
        {
            UseAccelerometer = gui.UseAccelerometer;
            UseAccelerometerFilter = gui.UseAccelerometerFilter;
            AccelerometerRestrictMode = AccelerometerRestrictMode;
            BindFootToGround = gui.BindFootToGround;
            LegIKScaleFactor = gui.LegIKScaleFactor;
            ArmAngle = gui.ArmAngle;
        }

        private void OnGuiPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is QumarionSettingGui)) return;
            var gui = sender as QumarionSettingGui;

            if(e.PropertyName == nameof(gui.UseAccelerometer))
            {
                UseAccelerometer = gui.UseAccelerometer;
            }
            else if(e.PropertyName == nameof(gui.UseAccelerometerFilter))
            {
                UseAccelerometerFilter = gui.UseAccelerometerFilter;
            }
            else if(e.PropertyName == nameof(gui.AccelerometerRestrictMode))
            {
                AccelerometerRestrictMode = AccelerometerRestrictMode;
            }
            else if(e.PropertyName == nameof(gui.BindFootToGround))
            {
                BindFootToGround = gui.BindFootToGround;
            }
            else if(e.PropertyName == nameof(gui.LegIKScaleFactor))
            {
                LegIKScaleFactor = gui.LegIKScaleFactor;
            }
            else if(e.PropertyName == nameof(gui.ArmAngle))
            {
                ArmAngle = gui.ArmAngle;
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;

    }
}

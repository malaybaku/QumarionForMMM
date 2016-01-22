using System;
using System.Drawing;
using System.Windows.Forms;

using Baku.Quma.Pdk;
using Baku.Quma.Pdk.Api;
using MikuMikuPlugin;

namespace QumarionForMMM
{
    public class QumarionModelContolPlugin : IResidentPlugin, IHaveUserControl
    {
        #region IResidentPlugin実装

        public IWin32Window ApplicationForm { get; set; }
        public Scene Scene { get; set; }

        public string Text => "QUMARION";
        public string EnglishText => "QUMARION";
        public string Description => "Qumarion controls active model character.";

        public Guid GUID { get; } = new Guid("25c3bf2b-a71b-4e5a-8284-7d150d2647f3");

        public Image Image => null;
        public Image SmallImage => null;

        public void Initialize()
        {
            //ライブラリ初期化してモデルを一つだけ持つ(MMDのモデル数の分だけ作る戦略も無くはない)
            PdkManager.Initialize();
            _modelHandler = new ModelHandler();
            _setting = new QumarionSettingHolder();
            _modelAdapter = new ModelHandlerSettingAdapter(_modelHandler);
            _modelAdapter.Connect(_setting);
        }

        public void Enabled()
        {
            //この段階でQumarionの接続検証: 失敗したら接続してOFF/ONしろという指針
            _modelHandler.TryAttachQumarionToModel();
            //GUIっていつ初期化されるんだろうね。Initializeの前だったりするとこんなコード書かんで済むのだけど。
            if(_settingGui != null && !_isSettingConnectToGui)
            {
                _setting.SyncToGui(_settingGui);
                _setting.SubscribeToUserControl(_settingGui);
                _isSettingConnectToGui = true;
            }
        }

        public void Update(float Frame, float ElapsedTime)
        {
            //エラーメッセージを乱発しないために。
            if (_errorOccurred) return;

            //適用先があることを検証
            if (Scene?.ActiveModel?.Bones == null) return;

            try
            {
                _modelHandler.Update(Scene.ActiveModel);
            }
            catch(Exception ex)
            {
                _errorOccurred = true;
                MessageBox.Show(
                    $"{ex.Message}, stack trace:{ex.StackTrace}",
                    "QumarionForMMM error in MMM", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                    );
            }
        }

        public void Disabled()
        {

        }

        public void Dispose()
        {
            //アプリケーション終了時なら呼ばないでもいい気するが、一応終了処理
            _modelHandler.Dispose();
            QmPdk.BaseOperation.Final();
        }

        #endregion

        //IHaveUserControl実装
        public UserControl CreateControl()
            => _settingGui ?? (_settingGui = new QumarionSettingGui());



        private ModelHandler _modelHandler;
        private QumarionSettingGui _settingGui;
        private QumarionSettingHolder _setting;
        private ModelHandlerSettingAdapter _modelAdapter;

        private bool _errorOccurred = false;
        private bool _isSettingConnectToGui;

    }
}

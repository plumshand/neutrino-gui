using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using neutrino_gui.Models;

namespace neutrino_gui.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            _executeCommand = new RelayCommand(this.OnExecute);
            _clearCommand = new RelayCommand(this.OnClear);
            _settingCommand = new RelayCommand(this.OnShowSettings);
        }

        public void Init()
        {
            if (string.IsNullOrEmpty(Settings.Instance.Neutrino))
            {
                // NEUTRINOのパスが未設定なので、設定画面表示
                this.ShowInitialSettings();
            }
            else
            {
                // 多々初期化
                this.InitInternal();
            }

            this.Setting.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals(nameof(this.Setting.Visible), StringComparison.Ordinal))
                {
                    this.OnDialogVisibleChanged();
                }
            };
            this.SongSetting.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals(nameof(this.SongSetting.Visible), StringComparison.Ordinal))
                {
                    this.OnDialogVisibleChanged();
                };
            };
        }

        public void AddFiles(string[] files)
        {
            foreach (string f in files)
            {
                if (Path.GetExtension(f).Equals(".musicxml", StringComparison.Ordinal))
                {
                    // musicxmlなら追加
                    this.Songs.Add(new SongItem(f));
                }
            }
        }

        public void ShowSongSetting(SongItem song)
        {
            this.SongSetting.Show(song);
        }

        public ICommand Execute { get { return _executeCommand; } }

        public ICommand Clear { get { return _clearCommand; } }

        public ICommand ShowSetting { get { return _settingCommand; } }

        public bool IsDialogVisible
        {
            get { return _dialogVisible; }
            private set { this.SetProperty(ref _dialogVisible, value); }
        }


        public ObservableCollection<SongItem> Songs { get; } = new ObservableCollection<SongItem>();

        public SettingItem Setting { get; } = new SettingItem();

        public SongSettingItem SongSetting { get; private set; } = new SongSettingItem();


        private bool _dialogVisible = false;

        private readonly RelayCommand _executeCommand;
        private readonly RelayCommand _clearCommand;
        private readonly RelayCommand _settingCommand;

        private void OnExecute()
        {
            // まずは状況をクリア
            foreach (SongItem song in this.Songs)
            {
                song.Clear();
            }

            Task.Run(this.ExecuteInternal);
        }

        private void OnClear()
        {
            this.Songs.Clear();
        }

        private void OnShowSettings()
        {
            this.Setting.Show();
        }

        private void ShowInitialSettings()
        {
            this.Setting.Show();
        }

        private void InitInternal()
        {
            this.SongSetting.Init();
        }

        private void ExecuteInternal()
        {
            foreach (SongItem song in this.Songs)
            {
                try
                {
                    song.Convert();
                    song.Succeeded();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                    song.Failed();
                }
            }
        }

        private void OnDialogVisibleChanged()
        {
            this.IsDialogVisible = this.Setting.Visible || this.SongSetting.Visible;
        }

    }
}

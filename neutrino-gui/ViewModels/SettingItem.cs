using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using neutrino_gui.Models;

namespace neutrino_gui.ViewModels
{
    public class SettingItem : ViewModelBase
    {
        public SettingItem()
        {
            _saveCommand = new RelayCommand(this.OnSave);
            _cancelCommand = new RelayCommand(this.OnCancel);
            _reloadModelsCommand = new RelayCommand(this.OnReloadModels);
        }

        public void Show()
        {
            this.Neutrino = Settings.Instance.Neutrino;

            this.ReloadModelsInternal();
            VoiceModelItem model = this.VoiceModels.FirstOrDefault(p => string.Equals(p.Name, Settings.Instance.DefaultModel, StringComparison.CurrentCultureIgnoreCase));
            this.SelectedModel = model ?? this.VoiceModels.FirstOrDefault();

            this.Visible = true;
        }

        public bool Visible
        {
            get { return _visible; }
            private set { this.SetProperty(ref _visible, value); }
        }

        public ObservableCollection<VoiceModelItem> VoiceModels { get; } = new ObservableCollection<VoiceModelItem>();

        public string Neutrino
        {
            get { return _neutrinoPath; }
            set { this.SetProperty(ref _neutrinoPath, value); }
        }

        public VoiceModelItem SelectedModel
        {
            get { return _selectedModel; }
            set { this.SetProperty(ref _selectedModel, value); }
        }

        public ICommand Save { get { return _saveCommand; } }

        public ICommand Cancel { get { return _cancelCommand; } }

        public ICommand ReloadModels { get { return _reloadModelsCommand; } }


        private bool _visible = false;
        private string _neutrinoPath = string.Empty;
        private VoiceModelItem _selectedModel = null;

        private readonly RelayCommand _saveCommand;
        private readonly RelayCommand _cancelCommand;
        private readonly RelayCommand _reloadModelsCommand;

        private void OnSave()
        {
            Settings.Instance.Neutrino = _neutrinoPath;
            Settings.Instance.DefaultModel = _selectedModel.Name;
            Settings.Instance.Save();
            this.Visible = false;
        }

        private void OnCancel()
        {
            this.Visible = false;
        }

        private void OnReloadModels()
        {
            // 選択中のモデルを保持
            VoiceModelItem selected = this.SelectedModel;

            // リロード
            this.ReloadModelsInternal();
            if (selected == null)
            {
                // 選択済みモデルがまだ未選択の場合は、リスト中の最初の項目を選択
                this.SelectedModel = this.VoiceModels.FirstOrDefault();
            }
            else
            {
                // すでに選択済みで候補一覧にあればそのままキープ
                // なければリスト中の最初の項目を選択肢にする
                VoiceModelItem model = this.VoiceModels.FirstOrDefault(p => string.Equals(p.Name, selected.Name, StringComparison.OrdinalIgnoreCase));
                this.SelectedModel = model ?? this.VoiceModels.FirstOrDefault();
            }
        }

        private void ReloadModelsInternal()
        {
            this.VoiceModels.Clear();
            foreach (VoiceModel m in VoiceModel.Load(this.Neutrino))
            {
                this.VoiceModels.Add(new VoiceModelItem(m));
            }
        }
    }
}

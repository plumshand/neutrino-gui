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
    public class SongSettingItem : ViewModelBase
    {
        public SongSettingItem()
        {
            _saveCommand = new RelayCommand(this.OnSave);
            _cancelCommand = new RelayCommand(this.OnCancel);
        }

        public void Init()
        {
            this.VoiceModels.Clear();
            foreach (VoiceModel m in VoiceModel.Models)
            {
                this.VoiceModels.Add(new VoiceModelItem(m));
            }
        }

        public void Show(SongItem song)
        {
            this.SelectedModel = this.VoiceModels.FirstOrDefault(p => string.Equals(song.Model.Name, p.Name, StringComparison.Ordinal));
            this.FilePath = song.Path;
            _targetSong = song;

            this.Visible = true;
        }

        public bool Visible
        {
            get { return _visible; }
            private set { this.SetProperty(ref _visible, value); }
        }

        public string FilePath
        {
            get { return _filePath; }
            private set { this.SetProperty(ref _filePath, value); }
        }

        public ObservableCollection<VoiceModelItem> VoiceModels { get; } = new ObservableCollection<VoiceModelItem>();

        public VoiceModelItem SelectedModel
        {
            get { return _selectedModel; }
            set { this.SetProperty(ref _selectedModel, value); }
        }

        public ICommand Save { get { return _saveCommand; } }

        public ICommand Cancel { get { return _cancelCommand; } }


        private bool _visible = false;
        private string _filePath = "";
        private VoiceModelItem _selectedModel = null;

        private SongItem _targetSong = null;

        private readonly RelayCommand _saveCommand;
        private readonly RelayCommand _cancelCommand;

        private void OnSave()
        {
            if (_targetSong != null)
            {
                _targetSong.Model = this.SelectedModel;
            }
            this.Visible = false;
        }

        private void OnCancel()
        {
            this.Visible = false;
        }
    }
}

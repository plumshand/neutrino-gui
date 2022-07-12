using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using neutrino_gui.Models;

namespace neutrino_gui.ViewModels
{
    public class SongItem : ViewModelBase, IComparable
    {
        public SongItem(string musicxml)
        {
            this.Path = musicxml;
            this.Model = new VoiceModelItem(VoiceModel.Get(Settings.Instance.DefaultModel));
            this.Status = "";
        }

        public string Path { get; private set; }

        public VoiceModelItem Model
        {
            get { return _model; }
            set { this.SetProperty(ref _model, value); }
        }

        public string Status
        {
            get { return _status; }
            private set { this.SetProperty(ref _status, value); }
        }

        public void Convert()
        {
            this.Status = "▷";
            new SongConverter(this.Path, Settings.Instance.DefaultParameter, this.Model.Model).Convert();
        }


        public void Failed()
        {
            this.Status = "×";
        }

        public void Succeeded()
        {
            this.Status = "〇";
        }

        public void Clear()
        {
            this.Status = "";
        }

        public int CompareTo(object obj)
        {
            return 1;
        }

        private string _status = "";
        private VoiceModelItem _model;
    }
}

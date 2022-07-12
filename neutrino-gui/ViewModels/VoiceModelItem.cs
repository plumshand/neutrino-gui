using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using neutrino_gui.Models;

namespace neutrino_gui.ViewModels
{
    public class VoiceModelItem : ViewModelBase, IComparable
    {
        public VoiceModelItem(VoiceModel model)
        {
            this.Model = model;
            this.Name = model.Name;
        }

        public VoiceModel Model { get; private set; }

        public string Name { get; private set; }

        public int CompareTo(object obj)
        {
            return 1;
        }
    }
}

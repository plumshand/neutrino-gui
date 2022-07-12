using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neutrino_gui.Models
{
    public class VoiceModel
    {
        public string Name { get; private set; }

        public static List<VoiceModel> Models { get; private set; } = Load();

        public static VoiceModel Get(string name)
        {
            foreach (VoiceModel m in Models)
            {
                if (m.Name.Equals(name, StringComparison.Ordinal))
                {
                    return m;
                }
            }
            return null;
        }

        public static List<VoiceModel> Load(string neotrinoPath)
        {
            if (string.IsNullOrEmpty(neotrinoPath))
            {
                return new();
            }

            List<VoiceModel> models = new List<VoiceModel>();

            string modelPath = Path.Combine(neotrinoPath, "model");
            IEnumerable<string> modelDirs = Directory.EnumerateDirectories(modelPath, "*", SearchOption.TopDirectoryOnly);
            foreach (string dir in modelDirs)
            {
                // フォルダ内に*.binが存在すること
                int binCount = Directory.EnumerateFiles(dir, "*.bin", SearchOption.TopDirectoryOnly).Count();
                if (binCount > 0)
                {
                    VoiceModel m = new VoiceModel
                    {
                        Name = Path.GetFileNameWithoutExtension(dir)
                    };
                    models.Add(m);
                }
            }

            return models;
        }

        public static void Reload()
        {
            Models = Load();
        }

        private static List<VoiceModel> Load()
        {
            return Load(Settings.Instance.Neutrino);
        }
    }
}

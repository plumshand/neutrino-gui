using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace neutrino_gui
{
    public class Settings
    {
        /// <summary>
        /// NEUTRINO本体のインストールディレクトリ
        /// </summary>
        public string Neutrino { get; set; }

        /// <summary>
        /// デフォルトのモデル
        /// </summary>
        public string DefaultModel { get; set; }

        /// <summary>
        /// デフォルトの出力パラメータ
        /// </summary>
        public Parameter DefaultParameter { get; set; }


        public static Settings Instance { get; private set; }

        public void Save()
        {
            string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dir, "neutrino-gui-settings.json");

            JsonSerializerSettings s = new()
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
            };

            string json = JsonConvert.SerializeObject(this, Formatting.Indented, s);
            File.WriteAllText(path, json);
        }

        static Settings()
        {
            string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dir, "neutrino-gui-settings.json");

            if (!File.Exists(path))
            {
                Instance = new()
                {
                    Neutrino = string.Empty,
                    DefaultModel = "KIRITAN",
                    DefaultParameter = Parameter.GetDefault(),
                };
                return;
            }

            JsonSerializerSettings s = new()
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
            };

            Instance = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path), s);
            if (Instance.DefaultParameter == null)
            {
                Instance.DefaultParameter = Parameter.GetDefault();
            }
        }

        public class Parameter
        {
            /// <summary>
            /// スレッド数
            /// </summary>
            public int ThreadCount { get; set; }

            /// <summary>
            /// NSFを使用するか
            /// </summary>
            public bool UseNSF { get; set; }

            public static Parameter GetDefault()
            {
                return new()
                {
                    ThreadCount = 3,
                    UseNSF = false
                };
            }
        }
    }
}

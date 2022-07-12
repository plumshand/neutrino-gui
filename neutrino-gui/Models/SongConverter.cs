using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neutrino_gui.Models
{
    public class SongConverter
    {
        private readonly string _musicXml;
        private readonly Settings.Parameter _parameter;
        private readonly VoiceModel _model;
        private readonly string _baseName;

        public SongConverter(string musicXml, Settings.Parameter parameter, VoiceModel model)
        {
            _musicXml = musicXml;
            _parameter = parameter;
            _model = model;
            _baseName = Path.GetFileNameWithoutExtension(musicXml);
        }

        public void Convert()
        {
            using (TempDir tempDir = new())
            {
                // XML to Labelを実行
                FileUtils.CreateDirectory(Path.Combine(tempDir.Path, "label", "full"));
                FileUtils.CreateDirectory(Path.Combine(tempDir.Path, "label", "mono"));
                this.ExecuteMusicXMLtoLabel(tempDir.Path);

                // NEUTRINOを実行
                FileUtils.CreateDirectory(Path.Combine(tempDir.Path, "label", "timing"));
                FileUtils.CreateDirectory(Path.Combine(tempDir.Path, "output"));
                this.ExecuteNeutrino(tempDir.Path);

                // WORLDを実行
                this.ExecuteWorld(tempDir.Path);

                if (_parameter.UseNSF)
                {
                    // NSFありなら実行
                    this.ExecuteNSF(tempDir.Path);
                }
            }
        }

        private void ExecuteMusicXMLtoLabel(string baseDir)
        {
            string bin = Path.Combine(Settings.Instance.Neutrino, "bin", "musicXMLtoLabel.exe");
            string fullLabel = Path.Combine(baseDir, "label", "full", _baseName + ".lab");
            string monoLabel = Path.Combine(baseDir, "label", "mono", _baseName + ".lab");
            CommandExecute musicXmlToLabel = new CommandExecute(bin)
                .SetWorkingDirectory(Settings.Instance.Neutrino)
                .AddArg(_musicXml)
                .AddArg(fullLabel)
                .AddArg(monoLabel)
                .AddOutput(fullLabel)
                .AddOutput(monoLabel);
            musicXmlToLabel.Execute();
        }

        private void ExecuteNeutrino(string baseDir)
        {
            string bin = Path.Combine(Settings.Instance.Neutrino, "bin", "NEUTRINO.exe");
            string fullLabel = Path.Combine(baseDir, "label", "full", _baseName + ".lab");
            string timingLabel = Path.Combine(baseDir, "label", "timing", _baseName + ".lab");
            string outputF0 = Path.Combine(baseDir, "output", _baseName + ".f0");
            string outputMgc = Path.Combine(baseDir, "output", _baseName + ".mgc");
            string outputBap = Path.Combine(baseDir, "output", _baseName + ".bap");
            string modelPath = Path.Combine(Settings.Instance.Neutrino, "model", _model.Name) + Path.DirectorySeparatorChar;
            CommandExecute neutrino = new CommandExecute(bin)
                .SetWorkingDirectory(Settings.Instance.Neutrino)
                .AddArg(fullLabel)
                .AddArg(timingLabel)
                .AddArg(outputF0)
                .AddArg(outputMgc)
                .AddArg(outputBap)
                .AddArg(modelPath)
                .AddArg("-n").AddArg(_parameter.ThreadCount.ToString(System.Globalization.CultureInfo.CurrentCulture))
                .AddArg("-k").AddArg("0")
                .AddArg("-m")
                .AddArg("-t")
                .AddOutputs(outputF0, outputMgc, outputBap);
            neutrino.Execute();
        }

        private void ExecuteWorld(string baseDir)
        {
            string bin = Path.Combine(Settings.Instance.Neutrino, "bin", "WORLD.exe");
            string outputF0 = Path.Combine(baseDir, "output", _baseName + ".f0");
            string outputMgc = Path.Combine(baseDir, "output", _baseName + ".mgc");
            string outputBap = Path.Combine(baseDir, "output", _baseName + ".bap");
            string synWav = Path.Combine(Path.GetDirectoryName(_musicXml), $"{_baseName}_{_model.Name}_syn.wav");
            CommandExecute world = new CommandExecute(bin)
                .SetWorkingDirectory(Settings.Instance.Neutrino)
                .AddArg(outputF0)
                .AddArg(outputMgc)
                .AddArg(outputBap)
                .AddArg("-f").AddArg("1.0")
                .AddArg("-m").AddArg("1.0")
                .AddArg("-p").AddArg("0.0")
                .AddArg("-c").AddArg("0.0")
                .AddArg("-b").AddArg("0.0")
                .AddArg("-o").AddArg(synWav)
                .AddArg("-n").AddArg(_parameter.ThreadCount.ToString(System.Globalization.CultureInfo.CurrentCulture))
                .AddArg("-t")
                .AddOutputs(synWav);
            world.Execute();
        }

        private void ExecuteNSF(string baseDir)
        {
            string bin = Path.Combine(Settings.Instance.Neutrino, "bin", "NSF.exe");
            string fullLabel = Path.Combine(baseDir, "label", "full", _baseName + ".lab");
            string timingLabel = Path.Combine(baseDir, "label", "timing", _baseName + ".lab");
            string outputF0 = Path.Combine(baseDir, "output", _baseName + ".f0");
            string outputMgc = Path.Combine(baseDir, "output", _baseName + ".mgc");
            string outputBap = Path.Combine(baseDir, "output", _baseName + ".bap");
            string nsfWav = Path.Combine(Path.GetDirectoryName(_musicXml), $"{_baseName}_{_model.Name}_nsf.wav");
            CommandExecute nsf = new CommandExecute(bin)
                .SetWorkingDirectory(Settings.Instance.Neutrino)
                .AddArg(fullLabel)
                .AddArg(timingLabel)
                .AddArg(outputF0)
                .AddArg(outputMgc)
                .AddArg(outputBap)
                .AddArg(_model.Name)
                .AddArg(nsfWav)
                .AddOutputs(nsfWav);
            nsf.Execute();
        }
    }
}

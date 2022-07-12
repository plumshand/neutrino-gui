using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neutrino_gui.Models
{
    public class CommandExecute
    {
        private readonly string _exePath;
        private readonly List<string> _arguments = new List<string>();

        private readonly List<string> _outputFiles = new List<string>();

        private string _workingDirectory;

        public CommandExecute(string exePath)
        {
            _exePath = exePath;
            _workingDirectory = Path.GetDirectoryName(exePath);
        }

        public CommandExecute SetWorkingDirectory(string workingDirectory)
        {
            _workingDirectory = workingDirectory;
            return this;
        }

        public CommandExecute AddArg(string arg)
        {
            _arguments.Add(arg);
            return this;
        }

        public CommandExecute AddOutput(string p)
        {
            _outputFiles.Add(p);
            return this;
        }

        public CommandExecute AddOutputs(params string[] p)
        {
            _outputFiles.AddRange(p);
            return this;
        }

        public void Execute()
        {
            ProcessStartInfo psi = new ProcessStartInfo(_exePath)
            {
                WorkingDirectory = _workingDirectory,
                Arguments = string.Join(" ", _arguments),
                UseShellExecute = false,
                CreateNoWindow = false,
            };

            Process process = Process.Start(psi);
            process.WaitForExit();

            // 終了コード確認
            if (process.ExitCode != 0)
            {
            }

            // 出力ファイル確認
        }
    }
}

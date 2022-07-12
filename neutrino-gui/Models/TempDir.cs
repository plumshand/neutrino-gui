using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neutrino_gui.Models
{
    public class TempDir : IDisposable
    {
        public TempDir()
        {
            this.Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            _cleanup = true;
            this.Create();
        }

        public TempDir(bool cleanUp)
        {
            this.Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            _cleanup = cleanUp;
            this.Create();
        }

        public void Dispose()
        {
            if (System.IO.Directory.Exists(this.Path) && _cleanup)
            {
                System.IO.Directory.Delete(this.Path, true);
            }
        }

        public string Path { get; private set; }

        private readonly bool _cleanup;

        private void Create()
        {
            if (!System.IO.Directory.Exists(this.Path))
            {
                System.IO.Directory.CreateDirectory(this.Path);
            }
        }
    }
}

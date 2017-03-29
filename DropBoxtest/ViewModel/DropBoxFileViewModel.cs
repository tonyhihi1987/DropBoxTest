using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropBoxTest.Model
{
   public class DropBoxFileViewModel:ViewModelBase
    {
        public string Name { get; set; }

        public bool IsFolder { get; set; }

        public bool IsFile { get; set; }

        public string Content { get; set; }

        public List<DropBoxFileViewModel> DropBoxFiles { get; set; } = new List<DropBoxFileViewModel>();
    }


}

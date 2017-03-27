using DropBoxtest.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropBoxtest
{
    public class MainPageViewModel : ViewModelBase
    {
        
        public ObservableCollection<DropBoxFile> DropBoxFiles { get; set; }

        public MainPageViewModel()
        {

            var task =   LoadData();            
        }

        private async Task LoadData()
        {
            try
            {
                await DropBoxManager.Connect();                
                await DropBoxManager.ListFiles();

                
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }


                
    }
}

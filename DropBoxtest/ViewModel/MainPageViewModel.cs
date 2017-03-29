using DropBoxTest.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropBoxTest
{
    public class MainPageViewModel : ViewModelBase
    {        
        private DropBoxFileViewModel _rootDropBoxItem;

        public DropBoxFileViewModel RootDropBoxItem
        {
            get { return _rootDropBoxItem; }
            set { _rootDropBoxItem = value;
                OnPropertyChanged(nameof(RootDropBoxItem));
            }
        }


        public MainPageViewModel()
        {
            var task =  LoadData();            
        }

        private async Task LoadData()
        {
            try
            {
                var dropBoxManager = new DropBoxManager();
                await dropBoxManager.Connect();
                RootDropBoxItem =  await dropBoxManager.BuildTreeView();                
            }
            catch (Exception ex)
            {

            }
        }


                
    }
}

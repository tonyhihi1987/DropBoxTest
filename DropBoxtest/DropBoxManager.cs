using Dropbox.Api;
using Dropbox.Api.Files;
using DropBoxTest.Model;
using System;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace DropBoxTest
{
    internal class DropBoxManager
    {
        private  DropboxClient _dbx;
        private  string _token;

        public   async Task Connect()
        {
            var authUri = DropboxOAuth2Helper.GetAuthorizeUri(
                OAuthResponseType.Token,
                "3iye3b8fr5wxt4u",
                new Uri("https://www.dropbox.com/home"));
            var result = await WebAuthenticationBroker.AuthenticateAsync(
                WebAuthenticationOptions.None,
                authUri,
                new Uri("https://www.dropbox.com/home"));

            ProcessResult(result);

            _dbx = new DropboxClient(_token);
        }

        private   void ProcessResult(WebAuthenticationResult result)
        {
            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.Success:
                    var response = DropboxOAuth2Helper.ParseTokenFragment(
                        new Uri(result.ResponseData));
                    _token = response.AccessToken;
                    break;

                case WebAuthenticationStatus.ErrorHttp:
                    throw new Exception();

                case WebAuthenticationStatus.UserCancel:
                default:
                    throw new Exception();
            }
        }


        public  async Task<DropBoxFileViewModel> BuildTreeView()
        {            
            var list = await _dbx.Files.ListFolderAsync(string.Empty);

            return  await GetDropBoxFiles(new DropBoxFileViewModel(),list);
        }

        private  async Task<DropBoxFileViewModel> GetDropBoxFiles(DropBoxFileViewModel rootFile,ListFolderResult childrens)
        {
            foreach (var dropBoxItem in childrens.Entries)
            {
                var newFile = new DropBoxFileViewModel()
                {
                    IsFile = dropBoxItem.IsFile,
                    IsFolder = dropBoxItem.IsFolder,
                    Name = dropBoxItem.Name
                };

                if (dropBoxItem.IsFolder)
                {
                    var newList = await _dbx.Files.ListFolderAsync(new ListFolderArg(dropBoxItem.PathLower));
                    await GetDropBoxFiles(newFile, newList);
                   
                }
                rootFile.DropBoxFiles.Add(newFile);

            }

            return rootFile;
        }

    }
}
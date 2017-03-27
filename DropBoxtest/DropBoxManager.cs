using Dropbox.Api;
using DropBoxtest.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace DropBoxtest
{
    internal class DropBoxManager
    {
        private static DropboxClient _dbx;
        private static string _token;

        public static  async Task Connect()
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

        private static  void ProcessResult(WebAuthenticationResult result)
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


        public static async Task<List<DropBoxFile>> ListFiles()
        {
            List<DropBoxFile> DropBoxFiles = new List<DropBoxFile>();

            var list = await _dbx.Files.ListFolderAsync(string.Empty);
           
            foreach (var folder in list.Entries.Where(i => i.IsFolder))
            {
                list = await _dbx.Files.ListFolderAsync(new Dropbox.Api.Files.ListFolderArg(folder.PathLower));
            }


            return DropBoxFiles;
        }


    }
}
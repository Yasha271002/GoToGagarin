using System.IO;
using System.Net.Http;

namespace GoToGagarin.Utilities
{
    public class ImageLoadingHttpClient
    {
        private readonly HttpClient _httpClient;

        public ImageLoadingHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected virtual string GetUrl(string url)
        {
            return $"{_httpClient.BaseAddress}{url}";
        }

        public async Task<string> DownloadImage(string url, string localPath = "AllImages", UriKind uriKind = UriKind.Absolute)
        {
            var filename = url.Replace('/', '_');
            if (string.IsNullOrEmpty(filename))
                return string.Empty;

            var imageFile = Path.Combine(localPath, filename);

            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);

            if (File.Exists(imageFile)) return Path.GetFullPath(imageFile);

            if (uriKind == UriKind.Relative) url = GetUrl(url);

            var response = await _httpClient.GetAsync(url);
            await using var fs = new FileStream(
                Path.GetFullPath(imageFile),
                FileMode.CreateNew);
            await response.Content.CopyToAsync(fs);
            return imageFile;
        }

        public async Task<string> DownloadVideo(string url, string localPath = "Video", UriKind uriKind = UriKind.Absolute)
        {
            var filename = url.Replace('/', '_');
            if (string.IsNullOrEmpty(filename))
                return string.Empty;

            var imageFile = Path.Combine(localPath, filename);

            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);

            if (File.Exists(imageFile)) return Path.GetFullPath(imageFile);

            if (uriKind == UriKind.Relative) url = GetUrl(url);

            var response = await _httpClient.GetAsync(url);
            await using var fs = new FileStream(
                Path.GetFullPath(imageFile),
                FileMode.CreateNew);
            await response.Content.CopyToAsync(fs);
            return imageFile;
        }
    }
}
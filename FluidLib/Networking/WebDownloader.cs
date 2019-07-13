using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FluidLib.Networking
{
    class WebDownloader
    {
        string url;
        public WebClient client { get; private set; }
        public Stream stream { get; private set; }
        public StreamReader reader { get; private set; }

        public static WebProxy CreateProxy(string url, bool hasCredentials, string username, string password)
        {
            WebProxy proxy = new WebProxy();
            proxy.Address = new Uri(url);
            if (hasCredentials)
            {
                proxy.Credentials = new NetworkCredential(username, password);
                proxy.UseDefaultCredentials = false;
            }
            proxy.BypassProxyOnLocal = false;

            return proxy;
        }
        public static WebProxy CreateProxy(string url)
        {
            return new WebProxy(new Uri(url));
        }

        public WebDownloader(string url, WebProxy proxy = null)
        {
            this.url = url;
            client = new WebClient();

            if (proxy != null)
                client.Proxy = proxy;
        }

        public void SetProxy(WebProxy value)
        {
            client.Proxy = value;

            Close();
            Open();
        }
        public void SetUrl(string url)
        {
            this.url = url;

            Close();
            Open();
        }

        public void Open()
        {
            stream = client.OpenRead(url);
            reader = new StreamReader(stream);
        }
        public void Close()
        {
            stream.Close();
            reader.Close();

            client.Dispose();
            stream.Dispose();
            reader.Dispose();
        }

        public void downloadFile(string file)
        {
            client.DownloadFile(url, file);
        }
        public List<string> getAllText()
        {
            List<string> lines = new List<string>();
            string line;

            while ((line = reader.ReadLine()) != null)
                lines.Add(line);

            return lines;
        }
        public string getString(string value)
        {
            return client.DownloadString(value);
        }
        public object getObject(string value)
        {
            WebRequest objectRequest = WebRequest.Create(value);
            object obj = objectRequest.GetResponse();

            return obj;
        }
    }
}

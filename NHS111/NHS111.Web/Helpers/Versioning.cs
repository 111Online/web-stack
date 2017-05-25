using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace NHS111.Web.Helpers
{
    public static class Versioning
    {

        private static string _version;
        private static readonly Dictionary<string, string> _fileHashes = new Dictionary<string, string>();
        public static IPathProvider _pathProvider = new ServerPathProvider();
        public static IFileIO _fileIO = new FileIO();
        public static string GetWebsiteVersion()
        {
            if (_version == null)
            {
                _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            return _version;
        }

        public static string GetVersionedUriRef(string uri)
        {
            if (_fileHashes.ContainsKey(uri)) return _fileHashes[uri];
            var hashvalue = GenerateChecksum(uri);
            var versionedUriRef = _pathProvider.ToAbsolute(String.Format("{0}?{1}", uri, hashvalue));
             _fileHashes.Add(uri, versionedUriRef);
            return versionedUriRef;
        }

        private static string GenerateChecksum(string uri)
        {
            using (var sha1 = System.Security.Cryptography.SHA1.Create()) {
                using (var reader = _fileIO.OpenRead(_pathProvider.MapPath(uri))) {
                    var hash = sha1.ComputeHash(reader);
                    var hashvalue = string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
                    return hashvalue;
                }
            }
        }
    }

    public interface IPathProvider
    {
        string MapPath(string path);
        string ToAbsolute(string virtualPath);
    }

    public interface IFileIO
    {
        Stream OpenRead(string filePath);
    }

    public class ServerPathProvider : IPathProvider
    {
        public string MapPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }

        public string ToAbsolute(string virtualPath)
        {
            return VirtualPathUtility.ToAbsolute(virtualPath);
        }
    }

    public class FileIO : IFileIO
    {
        public Stream OpenRead(string filePath)
        {
            return File.OpenRead(filePath);
        }
    }


}
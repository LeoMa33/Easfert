using Ho.Droid;
using Ho.Interfaces;
using Android.App;
using System.IO;
using Application = Android.App.Application;
using Path = System.IO.Path;

[assembly: Xamarin.Forms.Dependency(typeof(FileService))]
namespace Ho.Droid
{
    public class FileService:IFileService
    {
        public string GetRootPath()
        {
            return Application.Context.GetExternalFilesDir(null)?.ToString();
        }
        public void CreateFile(string content)
        {
            var filename = "data.json";
            var destination = Path.Combine(GetRootPath(), filename);
            File.WriteAllText(destination, content);
        }

        public string ReadFile()
        {
            var filename = "data.json";
            var destination = Path.Combine(GetRootPath(), filename);
            if (File.Exists(destination))
            {
                return File.ReadAllText(destination);
            }
            return "";
        }
    }
}
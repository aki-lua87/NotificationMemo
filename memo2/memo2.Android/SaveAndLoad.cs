using System.IO;
using Xamarin.Forms;
using memo2.Droid;
using memo2.Models;

[assembly: Dependency(typeof(SaveAndLoad))]

namespace memo2.Droid
{
    /// <summary>
    /// Forms����Android�p�̃f�[�^�̕ۑ��Ɠǂݍ��݂��s��DependencyService
    /// </summary>
    public class SaveAndLoad : ISaveAndLoad
    {
        public void SaveData(string filename, string text)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            File.WriteAllText(filePath, text);
        }
        public string LoadData(string filename)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            return null;
        }
    }
}
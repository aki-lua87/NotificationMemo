using System;
using System.IO;
using Xamarin.Forms;
using memo2.Droid;
using memo2.Models;
using SQLite;

//[assembly: Dependency(typeof(SQLite_Android))]

namespace memo2.Droid
{
    /// <summary>
    /// FormsからAndroid用のDBデータの保存と読み込みを行うDependencyService
    /// Formsから読んでいない？
    /// </summary>
    public class SQLite_Android // : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); 
            var path = Path.Combine(documentsPath, "AllData.SQLite");
            return new SQLiteConnection(path); 
        }
    }
}
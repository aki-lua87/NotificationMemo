using Android.Content;
using memo2.Droid;
using memo2.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(CreateService_Android))]

namespace memo2.Droid
{
    /// <summary>
    /// Formsから一覧画面を呼び出すためのDependencyService
    /// </summary>
    public class CreateService_Android :ICreateService
    {
        private static readonly Context Context = Android.App.Application.Context;
        private readonly Intent _intent = new Intent(Context, typeof(ListService));

        public void ServiceOn()
        {
            Context.StartService(_intent);
        }
        public void ServiceOff()
        {
            Context.StopService(_intent);
        }
    }
}
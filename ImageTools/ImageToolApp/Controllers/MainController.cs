using System.Windows;
using ImageToolApp.Models;

namespace ImageToolApp.Controllers
{
    public class MainController
    {
        public MainController(Window mainWindow)
        {
            var view = mainWindow;
            var encryptController = new EncryptController();
            var decryptController = new DecryptController();
            var viewModel = new MainViewModel(encryptController.GetView(), decryptController.GetView());
            view.DataContext = viewModel;

            view.Show();
        }
    }
}
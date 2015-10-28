﻿using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using FunctionLib.Helper;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;

namespace ImageToolApp.Controllers
{
    public class MainController
    {
        private readonly DecryptController mDecryptController;
        private readonly EncryptController mEncryptController;
        private readonly MainWindow mView;
        private readonly MainViewModel mViewModel;

        public MainController(MainWindow mainWindow)
        {
            mView = mainWindow;
            mEncryptController = new EncryptController();
            mDecryptController = new DecryptController();
            mViewModel = new MainViewModel(mEncryptController.View, mDecryptController.View);
            SetupCommands();
            mView.DataContext = mViewModel;
            mView.Show();
        }

        private IBaseTabController CurrentController
        {
            get
            {
                var view = mViewModel.CurrentElement as EncryptView;
                if (view != null)
                {
                    return mEncryptController;
                }
                return mDecryptController;
            }
        }

        private void SetupCommands()
        {
            mViewModel.CloseAppCommand = UICommand.Regular(CloseApp);
            mViewModel.PreferencesCommand = UICommand.Regular(OpenPreferencesWindow);
            mViewModel.OpenImageCommand = UICommand.Regular(OpenImage);
            mViewModel.SaveImageCommand = UICommand.Regular(SaveImage);
            mViewModel.OpenTxtCommand = UICommand.Regular(OpenTxt);
            mViewModel.SaveTxtCommand = UICommand.Regular(SaveTxt);
            mViewModel.ChangedPixelsCommand = UICommand.Regular(ChangedPixels);
            mViewModel.OpenHelpCommand = UICommand.Regular(OpenHelp);
        }

        private static void OpenHelp()
        {
            var executionPath = MethodHelper.ExecutiongPath;
            System.Diagnostics.Process.Start(Path.Combine(executionPath, "Help.pdf"));
        }

        private void ChangedPixels()
        {
            var controller = CurrentController as EncryptController;
            if (controller != null)
            {
                controller.ChangedPixels();
            }
        }

        private void SaveTxt()
        {
            var controller = CurrentController as DecryptController;
            if (controller != null)
            {
                controller.SaveTxt();
            }
        }

        private void OpenTxt()
        {
            var controller = CurrentController as EncryptController;
            if (controller != null)
            {
                controller.OpenTxt();
            }
        }

        private void SaveImage()
        {
            var controller = CurrentController as EncryptController;
            if (controller != null)
            {
                controller.SaveImage();
            }
        }

        private void OpenPreferencesWindow()
        {
            // TODO
            /*  Open new Window where you can choose which Encryption-Algorithm you want to use.
            *   Define a standard path
            *   Define a standard Password
            */
            var preferncesController = new PreferencesController(mView);
            if (preferncesController.OpenDialog())
            {
                mDecryptController.PreferencesSaved();
                mEncryptController.PreferencesSaved();
            }
        }

        private void OpenImage()
        {
            CurrentController.OpenImage();
        }

        private void CloseApp()
        {
            mView.Close();
        }
    }
}
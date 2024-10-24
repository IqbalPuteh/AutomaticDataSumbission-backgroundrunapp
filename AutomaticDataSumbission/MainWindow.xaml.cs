using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using WinUIEx;

namespace AutomaticDataSumbission
{
    public sealed partial class MainWindow : WindowEx
    {
        private DispatcherTimer _timer;
        private CheckedTime _checkedTime;

        public MainWindow()
        {
            this.InitializeComponent();
            this.HideWindow();
            this.InitializeTrayIcon();
            StartChecking("path_to_your_file.txt");
        }

        private void HideWindow()
        {
            //this.Visible = Visibility.Collapsed;
        }

        private void InitializeTrayIcon()
        {
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText01);
            var toastElement = toastXml.GetElementsByTagName("text").Item(0);
            toastElement.AppendChild(toastXml.CreateTextNode("FTI Data Submission App Running"));
            var toastNotification = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }

        public CheckedTime ReadCheckedTimeFromFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            DateTime dateTime;
            if (DateTime.TryParse(lines[0], out dateTime))
            {
                return new CheckedTime { Time = dateTime };
            }
            else
            {
                throw new Exception("Invalid datetime format in file.");
            }
        }

        public void StartChecking(string filePath)
        {
            _checkedTime = ReadCheckedTimeFromFile(filePath);

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };
            _timer.Tick += CheckTime;
            _timer.Start();
        }

        private void CheckTime(object sender, object e)
        {
            if (DateTime.Now >= _checkedTime.Time)
            {
                // Perform your action when the time is reached
                System.Diagnostics.Debug.WriteLine("The time has come!");
            }
        }
    }

    public class CheckedTime
    {
        public DateTime Time { get; set; }
    }
}

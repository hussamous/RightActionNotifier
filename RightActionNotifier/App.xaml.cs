using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using System.Timers;
using System.Windows.Threading;


namespace RightActionNotifier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon notifyIcon;
        private DispatcherTimer dispatcherTimer;

        public notificationSetting notificationSettings { get; set; }

        private int nextIndextoShow = -1;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            notificationSettings = notificationSettingParser.ParseXMLfile("Settings.xml");
            // for the time span we have to convert into seconds
            this.dispatcherTimer = new DispatcherTimer(new TimeSpan(0, 0, notificationSettings.interval / 1000), DispatcherPriority.Normal, OnTimerTick, Dispatcher);
            this.dispatcherTimer.Start();

        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            DateTime from = notificationSettings.runFrom;
            DateTime until = notificationSettings.runUntil;
            DateTime now = DateTime.Now;

            if (notificationSettings.limitRunningTime &&( TimeSpan.Compare(now.TimeOfDay, until.TimeOfDay) > 0
                || TimeSpan.Compare(now.TimeOfDay, from.TimeOfDay) < 0))
                return;


            if (notificationSettings.randomShow)
            {
                Random rnd = new Random();
                nextIndextoShow = rnd.Next(0, notificationSettings.notificationList.Count - 1);
            }
            else
            {
                if (nextIndextoShow < notificationSettings.notificationList.Count - 1)
                    nextIndextoShow++;
                else
                    nextIndextoShow = 0;
            }
            FancyBalloon balloon = new FancyBalloon();
            balloon.BalloonText = notificationSettings.notificationList.ElementAt(nextIndextoShow).notificationString;

            //show balloon and close it after 4 seconds
            notifyIcon.ShowCustomBalloon(balloon, System.Windows.Controls.Primitives.PopupAnimation.Slide, notificationSettings.showUpTime);
        }


        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner 
            base.OnExit(e);
        }
    }
}

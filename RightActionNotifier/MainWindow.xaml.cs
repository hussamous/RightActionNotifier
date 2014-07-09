using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Hardcodet.Wpf.TaskbarNotification;
using System.Timers;
using System.Windows.Threading;


namespace RightActionNotifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            RefreshView();
        }

        private void RefreshView()
        {
            // Fill the list view with the notifications available
            App thisApp = (App)App.Current;
            lvnotifications.Items.Clear();
            notificationSetting affSetting = thisApp.notificationSettings;
            txtInterval.Text = (affSetting.interval / 1000).ToString();
            txtShowUpTime.Text = (affSetting.showUpTime / 1000).ToString();
            chkLimitRunningTime.IsChecked = affSetting.limitRunningTime;
            chkShowRandomly.IsChecked = affSetting.randomShow;
            timeFrom.Text = affSetting.runFrom.ToShortTimeString();
            timeTo.Text = affSetting.runUntil.ToShortTimeString();

            RefreshList(affSetting);
        }

        private void RefreshList(notificationSetting affSetting)
        {
            foreach (var c in affSetting.notificationList)
            {
                lvnotifications.Items.Add(c);
            }
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            notificationSetting affSetting = new notificationSetting();

            affSetting.interval = int.Parse(txtInterval.Text) * 1000;// convert to ms
            affSetting.showUpTime = int.Parse(txtShowUpTime.Text) * 1000;// convert to ms
            affSetting.limitRunningTime = (bool)chkLimitRunningTime.IsChecked;
            affSetting.runFrom = DateTime.Parse(timeFrom.Text);
            affSetting.runUntil = DateTime.Parse(timeTo.Text);
            affSetting.randomShow = (bool)chkShowRandomly.IsChecked;

            foreach (var c in lvnotifications.Items)
            {

                affSetting.notificationList.Add(new notification(((notification)c).notificationString));
            }

            // Store into file and refresh the app
            notificationSettingParser.SaveToXMLFile(affSetting, "Settings.XML");
            App thisApp = (App)App.Current;
            thisApp.notificationSettings = affSetting;
        }

        private void btnAddToList_Click(object sender, RoutedEventArgs e)
        {
            lvnotifications.Items.Add(new notification(txtNewnotification.Text));
            lvnotifications.SelectedIndex = lvnotifications.Items.Count - 1;
            txtNewnotification.Text = "";
            txtNewnotification.Focus();
        }

        private void btnRemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            if (lvnotifications.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an item before trying this button!");
            }
            else
            {
                int selIndex = lvnotifications.SelectedIndex;
                lvnotifications.Items.Remove(lvnotifications.SelectedItem);
                if (selIndex <= lvnotifications.Items.Count - 1)
                {
                    lvnotifications.SelectedIndex = selIndex;
                }
                else
                {
                    lvnotifications.SelectedIndex = lvnotifications.Items.Count - 1;
                }
            }
        }

        private void btnRestoreDefault_Click(object sender, RoutedEventArgs e)
        {
            App thisApp = (App)App.Current;
            thisApp.notificationSettings = notificationSettingParser.ParseXMLfile("DefaultSettings.xml");
            RefreshView();

        }
    }
}

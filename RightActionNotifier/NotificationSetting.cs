using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RightActionNotifier
{
    public class notification
    {
        public string notificationString { get; set; }

        public notification(string notification)
        {
            notificationString = notification;
        }
    }
    public class notificationSetting
    {

        /// <summary>
        /// Number of milli seconds between two messages
        /// </summary>
        public int interval { get; set;}
        public int showUpTime { get; set;}
        public bool randomShow { get; set;}

        public bool limitRunningTime { get; set; }
        public DateTime runFrom { get; set; }
        public DateTime runUntil { get; set; }

        public List<notification> notificationList { get; set;}

        public notificationSetting()
        {
            interval = 10000;
            showUpTime = 5000;
            randomShow = true;
            limitRunningTime = false;
            notificationList = new List<notification>();
        }
    }

    
}

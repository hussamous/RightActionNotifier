using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RightActionNotifier
{
    public static class notificationSettingParser
    {
        public static notificationSetting ParseXMLfile(string settingsFile)
        {
            var notificationSettings = new notificationSetting();
            XDocument doc = XDocument.Load(settingsFile);
            XElement generalElement = doc
                .Element("notificationSettings");
            notificationSettings.interval = int.Parse(generalElement.Element("interval").Value);
            notificationSettings.showUpTime = int.Parse(generalElement.Element("showUptime").Value);
            notificationSettings.limitRunningTime = bool.Parse(generalElement.Element("limitRunningTime").Value);
            notificationSettings.runFrom = DateTime.Parse(generalElement.Element("limitRunningTime").Attribute("from").Value);
            notificationSettings.runUntil = DateTime.Parse(generalElement.Element("limitRunningTime").Attribute("until").Value);
            notificationSettings.randomShow = bool.Parse(generalElement.Element("randomShow").Value);

            notificationSettings.notificationList = (from c in doc.Descendants("notification")
                                                   select new notification(c.Value)
                                                   //{
                                                     //  notificationString = c.Value
                                                   //}
                                                   ).ToList<notification>();
            return notificationSettings;
        }

        public static void SaveToXMLFile(notificationSetting settings, string settingsFile)
        {
            XDocument xDoc = new XDocument();
            XElement elem = new XElement("notificationSettings");
            elem.Add(new XElement("interval", settings.interval));
            elem.Add(new XElement("showUptime", settings.showUpTime));
            elem.Add(new XElement("randomShow", settings.randomShow));            
            elem.Add(new XElement("limitRunningTime", settings.limitRunningTime, 
                            new XAttribute("from", settings.runFrom.ToShortTimeString()),
                            new XAttribute("until", settings.runUntil.ToShortTimeString())));
                       
            elem.Add (new XElement("notifications",

                from notification in settings.notificationList
                select new XElement("notification", notification.notificationString)
                )
                );

            xDoc.Add(elem);
            xDoc.Save(settingsFile);

        }
    }
}

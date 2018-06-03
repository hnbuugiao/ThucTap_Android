using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ThucTap_Android.Connections
{
    class ConnectString
    {
        public Uri connectstring { get; set; }

        public ConnectString()
        {
            //this.connectstring = new Uri("http://192.168.12.114:29444/BenhNhan");
            this.connectstring = new Uri("http://192.168.56.1:29444/BenhNhan");
        }

        public ConnectString(string link)
        {
            this.connectstring = new Uri(link);
        }
    }
}
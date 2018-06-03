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

namespace ThucTap_Android.Model
{
    class BenhNhan
    {
        public string mabn { get; set; }
        public string holot { get; set; }
        public string ten { get; set; }
        public string ngaysinh { get; set; }
        public int gioitinh { get; set; }

        public BenhNhan()
        {

        }

        public BenhNhan(string MaBenhNhan, string HoLot, string Ten, string NgaySinh, int GioiTinh)
        {
            this.mabn = MaBenhNhan;
            this.holot = HoLot;
            this.ten = Ten;
            this.ngaysinh = NgaySinh;
            this.gioitinh = GioiTinh;
        }
    }
}
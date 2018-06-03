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
using ThucTap_Android.Model;

namespace ThucTap_Android.Adapter
{
    class BenhNhan_Adapter : BaseAdapter<BenhNhan>
    {
        Context context;
        List<BenhNhan> listBenhNhan;
        public BenhNhan_Adapter(Context context, List<BenhNhan> item)
        {
            this.context = context;
            this.listBenhNhan = item;
        }
        public override BenhNhan this[int position]
        {
            get { return listBenhNhan[position]; }
        }

        public override int Count { get { return listBenhNhan.Count; } }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.BenhNhan_Item_Layout, null, false);
            }

            TextView txtMabn = row.FindViewById<TextView>(Resource.Id.txtMaBenhNhan);
            txtMabn.Text = listBenhNhan[position].mabn;


            TextView txtHolot = row.FindViewById<TextView>(Resource.Id.txtHoLot);
            txtHolot.Text = listBenhNhan[position].holot;

            TextView txtTen = row.FindViewById<TextView>(Resource.Id.txtTen);
            txtTen.Text = listBenhNhan[position].ten;

            TextView txtNgaysinh = row.FindViewById<TextView>(Resource.Id.txtNgaySinh);
            txtNgaysinh.Text = listBenhNhan[position].ngaysinh;


            TextView txtGioitinh = row.FindViewById<TextView>(Resource.Id.txtGioiTinh);
            string gioitinh = "";
            int temp = listBenhNhan[position].gioitinh;
            if (temp == 0)
                gioitinh = "Nữ";
            else if (temp == 1)
                gioitinh = "Nam";
            else
                gioitinh = "Không xác định";

            txtGioitinh.Text = gioitinh;

            return row;
        }
    }
}
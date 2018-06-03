using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ThucTap_Android.Model;
using Newtonsoft.Json;
using ThucTap_Android.Connections;
using static Android.Provider.CalendarContract;
using static Android.App.DatePickerDialog;
using System.Globalization;
using System.Drawing;

namespace ThucTap_Android.Activities
{
    [Activity(Label = "BenhNhan_Them_Activity")]
    public class BenhNhan_Them_Activity : Activity,IOnDateSetListener
    {
        Button btnThem,btnHuy;
        ImageButton btnNgaySinh;
        EditText etxMaBN, etxHoLot, etxTen, extNgaySinh;
        CheckBox cbNam, cbNu;
        private const int DATE_DIALOG = 1;
        private int year=2018, month=5, day=28;
        //Uri URL = new Uri("http://192.168.12.114:29444/BenhNhan");
        ConnectString connect = new ConnectString();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.BenhNhan_Them_Layout);


            btnHuy = FindViewById<Button>(Resource.Id.btnHuy);
            btnThem = FindViewById<Button>(Resource.Id.btnThem);
            btnNgaySinh = FindViewById<ImageButton>(Resource.Id.btnNgaySinh);
            etxMaBN = FindViewById<EditText>(Resource.Id.extMaBN);
            etxHoLot = FindViewById<EditText>(Resource.Id.extHoLot);
            etxTen = FindViewById<EditText>(Resource.Id.extTen);
            extNgaySinh = FindViewById<EditText>(Resource.Id.extNgaySinh);
            cbNam = FindViewById<CheckBox>(Resource.Id.cbNam);
            cbNu = FindViewById<CheckBox>(Resource.Id.cbNu);
            

            etxMaBN.Enabled = true;
            extNgaySinh.Enabled = false;

            btnHuy.Click += BtnHuy_Click;
            btnThem.Click += BtnThem_ClickAsync;
            btnNgaySinh.Click += BtnNgaySinh_Click;
        }

        private void BtnNgaySinh_Click(object sender, EventArgs e)
        {
            ShowDialog(DATE_DIALOG);
        }

        public async void BtnThem_ClickAsync(object sender, EventArgs e)
        {
            string MaBN = etxMaBN.Text;
            string HoLot = etxHoLot.Text;
            string Ten = etxTen.Text;
            string NgaySinh = extNgaySinh.Text;

            int GioiTinh = 2;
            int GioiTinhtemp = 2;

            if (cbNu.Checked)
            {
                GioiTinhtemp = 0;
            }

            if (cbNam.Checked)
            {
                if (GioiTinhtemp != 0)
                    GioiTinhtemp = 1;
                else
                    Toast.MakeText(this, "Đã chọn 2 giới tính!", ToastLength.Short).Show();
            }

            GioiTinh = GioiTinhtemp;
            if(!string.IsNullOrEmpty(MaBN) && !string.IsNullOrEmpty(HoLot) && !string.IsNullOrEmpty(Ten) && !string.IsNullOrEmpty(NgaySinh))
            {
                List<BenhNhan> list = new List<BenhNhan>();
                list.Add(new BenhNhan(MaBN, HoLot, Ten, NgaySinh, GioiTinh));


                //var convertedJson = JsonConvert.SerializeObject(list);
                string temp = JsonConvert.SerializeObject(list);
                string temp2 = temp.Replace(@"[", string.Empty);
                string temp3 = temp2.Replace(@"]", string.Empty);
                string convertedJson = temp3;

                Console.WriteLine(convertedJson);
                HttpClient client = new HttpClient();
                HttpContent content = new StringContent(convertedJson, Encoding.UTF8, "application/json");
                Console.WriteLine(content);
                HttpResponseMessage response = await client.PostAsync(connect.connectstring, content);
                string responseMessage = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseMessage);

                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
            }
            else
            {
                Toast.MakeText(this, "Chưa điền đầy đủ thông tin!", ToastLength.Short).Show();

            }



        }
    
            private void BtnHuy_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();

        }

        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case DATE_DIALOG:
                    {
                        return new DatePickerDialog(this, this, year, month, day);
                    }
                    break;
                default:
                    break;
            }
            return null;
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            this.year = year;
            this.month = monthOfYear+1;
            this.day = dayOfMonth;

            string ngay = day.ToString();
            if (ngay.Length == 1) ngay = "0" + ngay;

            string thang = month.ToString();
            if (thang.Length == 1) thang = "0" + thang;
         
            string ngaythang = ngay + "/" + thang + "/" + year;
            extNgaySinh.Text = ngaythang;
        }
    }
}
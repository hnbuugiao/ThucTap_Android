using System;
using System.Collections.Generic;
using System.Linq;
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
using static Android.App.DatePickerDialog;

namespace ThucTap_Android.Dialogs
{
    class BenhNhan_Sua_Dialog : DialogFragment
    {
        string mabn = "";
        string holot = "";
        string ten = "";
        string ngaysinh = "";
        int gioitinh = 0;

        EditText etxMaBN, etxHoLot, etxTen, extNgaySinh;
        CheckBox cbNam, cbNu;
        Button btnCapNhat;
        ImageButton btnNgaySinh;
        private const int DATE_DIALOG = 1;
        private int year = 2018, month = 5, day = 28;
        public BenhNhan_Sua_Dialog(string mabn, string holot, string ten, string ngaysinh, int gioitinh)
        {
            this.mabn = mabn;
            this.holot = holot;
            this.ten = ten;
            this.ngaysinh = ngaysinh;
            this.gioitinh = gioitinh;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.BenhNhan_Sua_Layout, container, false);

            etxMaBN = view.FindViewById<EditText>(Resource.Id.extMaBN_Sua);
            etxHoLot = view.FindViewById<EditText>(Resource.Id.extHoLot_Sua);
            etxTen = view.FindViewById<EditText>(Resource.Id.extTen_Sua);
            extNgaySinh = view.FindViewById<EditText>(Resource.Id.extNgaySinh_Sua);
            cbNam = view.FindViewById<CheckBox>(Resource.Id.cbNam_Sua);
            cbNu = view.FindViewById<CheckBox>(Resource.Id.cbNu_Sua);
            btnCapNhat = view.FindViewById<Button>(Resource.Id.btnCapNhat);
            btnNgaySinh = view.FindViewById<ImageButton>(Resource.Id.btnNgaySinh_Sua);

            etxMaBN.Enabled = false;

            etxMaBN.Text = mabn;
            etxHoLot.Text = holot;
            etxTen.Text = ten;
            extNgaySinh.Text = ngaysinh;

            if (gioitinh == 0)
                cbNu.Checked = true;
            if (gioitinh == 1)
                cbNam.Checked = true;

            btnCapNhat.Click += BtnCapNhat_Click;
            btnNgaySinh.Click += BtnNgaySinh_Click;
            
            return view;
        }

        private void BtnNgaySinh_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnCapNhat_Click(object sender, EventArgs e)
        {

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
                    Toast.MakeText(this.Activity, "Đã chọn 2 giới tính!", ToastLength.Short).Show();
            }

            GioiTinh = GioiTinhtemp;
            if(!string.IsNullOrEmpty(etxMaBN.Text) && !string.IsNullOrEmpty(etxHoLot.Text) && !string.IsNullOrEmpty(etxTen.Text) && !string.IsNullOrEmpty(extNgaySinh.Text))
            {
                string ngaysinh = extNgaySinh.Text;
                string[] temp = ngaysinh.Split('/');

                List<string> temp2 = new List<string>();

                foreach(string a in temp){
                    if (a.Length == 1)
                        temp2.Add("0" + a);
                    else
                        temp2.Add(a);
                }

                string ngaythangfull = "";

                for(int i = 0; i < temp2.Count; i++)
                {
                    ngaythangfull += "/" + temp2[i];
                }

                BenhNhan benhnhan = new BenhNhan(etxMaBN.Text, etxHoLot.Text, etxTen.Text, extNgaySinh.Text, GioiTinh);
                List<BenhNhan> list = new List<BenhNhan>();
                list.Add(benhnhan);
                //var convertedJson = JsonConvert.SerializeObject(list);
                string json = JsonConvert.SerializeObject(list);
                string json2 = json.Replace(@"[", string.Empty);
                string json3 = json2.Replace(@"]", string.Empty);
                string convertedJson = json3;

                EditItem(convertedJson);

                OnDismiss(this.Dialog);
            }

        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }

        public async void EditItem(string json)
        {
            //Uri URL = new Uri("http://192.168.12.114:29444/BenhNhan");
            ConnectString connect = new ConnectString();
            Console.WriteLine(json);
            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            Console.WriteLine(content);
            HttpResponseMessage response = await client.PutAsync(connect.connectstring, content);
            string responseMessage = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseMessage);
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            this.Activity.Recreate();
        }

    }
}
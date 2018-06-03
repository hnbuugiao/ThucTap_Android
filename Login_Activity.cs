using System;
using System.Collections.Generic;
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


namespace ThucTap_Android
{
    //[Activity(Label = "Login_Activity")]
    [Activity(Label = "Login_Activity", MainLauncher = true, Icon = "@drawable/icon")]
    public class Login_Activity : Activity
    {
        EditText edt_TaiKhoan, edt_MatKhau;
        Button btn_DangNhap;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Login_Layout);

            edt_TaiKhoan = FindViewById<EditText>(Resource.Id.edt_TaiKhoan_Login);
            edt_MatKhau = FindViewById<EditText>(Resource.Id.edt_MatKhau_Login);
            btn_DangNhap = FindViewById<Button>(Resource.Id.btn_DangNhap_Login);
            btn_DangNhap.Click += Btn_DangNhap_ClickAsync;            
        }

        private async void Btn_DangNhap_ClickAsync(object sender, EventArgs e)
        {
            string taikhoan = edt_TaiKhoan.Text;
            string matkhau = edt_MatKhau.Text;

            //NhanVien/Check?id={id}&matkhau={matkhau}
            //web.DownloadDataAsync(new Uri("http://192.168.58.1:29444/NhanVien/Check"));
            //Uri uri = new Uri("http://192.168.58.1:29444/NhanVien/Check");
            string temp = "http://192.168.56.1:29444/NhanVien/Check?id=" + taikhoan + "&matkhau=" + matkhau;
            Console.WriteLine("url=" + temp);
            Uri uri = new Uri(temp);
            HttpClient client = new HttpClient();
            HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
            Console.WriteLine(content);
            HttpResponseMessage response = await client.PostAsync(uri, content);
            string responseMessage = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseMessage);
            string accept = responseMessage.ToString();
            if (accept.Equals("true"))
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
            }
            else
            {
                Toast.MakeText(this, "Sai username hoặc password!", ToastLength.Long).Show();
            }
        }

   
    }
}
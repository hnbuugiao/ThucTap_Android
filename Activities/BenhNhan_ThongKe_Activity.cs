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
using Newtonsoft.Json;
using ThucTap_Android.Connections;
using System.Net.Http;
using System.Threading.Tasks;

namespace ThucTap_Android.Activities
{
    [Activity(Label = "BenhNhan_ThongKe_Activity")]
    public class BenhNhan_ThongKe_Activity : Activity
    {
        TextView txtSoLuongNu, txtSoLuongNam, txtSoLuongKXD;
        ConnectString connect = new ConnectString();

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.BenhNhan_ThongKe_Layout);

            List<BenhNhan> benhnhan = new List<BenhNhan>();
            benhnhan = JsonConvert.DeserializeObject<List<BenhNhan>>(Intent.GetStringExtra("ThongKe"));



            int soluongnu = 0;
            int soluongnam = 0;
            int soluongkxd = 0;


            var thongke = await ThongKe();

            string s = thongke.Replace(@"\", string.Empty);
            string final = s.Trim().Substring(1, (s.Length) - 2);
            final = final.Trim().Substring(1, (final.Length) - 2);
            final = final.Replace("\"", string.Empty);
            string[] result = final.Split(',');

            soluongnu = int.Parse(result[0]);
            soluongnam = int.Parse(result[1]);
            soluongkxd = int.Parse(result[2]);


            /*
            foreach(BenhNhan bn in benhnhan)
            {
                if (bn.gioitinh == 0)
                    soluongnu++;
                else if (bn.gioitinh == 1)
                    soluongnam++;
                else
                    soluongkxd++;
            }*/

            txtSoLuongNu = FindViewById<TextView>(Resource.Id.txtSoLuongNu);
            txtSoLuongNam = FindViewById<TextView>(Resource.Id.txtSoLuongNam);
            txtSoLuongKXD = FindViewById<TextView>(Resource.Id.txtSoLuongKXD);

            txtSoLuongNu.Text = soluongnu.ToString();
            txtSoLuongNam.Text = soluongnam.ToString();
            txtSoLuongKXD.Text = soluongkxd.ToString();

            Button btnQuayVe;
            btnQuayVe = FindViewById<Button>(Resource.Id.btnQuayVe);
            btnQuayVe.Click += BtnQuayVe_Click;
        }

        private void BtnQuayVe_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        public async Task<string> ThongKe()
        {
            string url = connect.connectstring + "/ThongKe";
            Console.WriteLine("test " + url);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string responseMessage = await response.Content.ReadAsStringAsync();
            return responseMessage;
        }
    }
}
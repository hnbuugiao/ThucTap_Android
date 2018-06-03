using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using ThucTap_Android.Model;
using ThucTap_Android.Adapter;
using System.Collections.Generic;
using ThucTap_Android.Activities;
using static Android.Widget.AdapterView;
using Android.Media;
using System.Net.Http;
using ThucTap_Android.Dialogs;
using ThucTap_Android.Connections;
using System.Globalization;

namespace ThucTap_Android
{
    //[Activity (Label = "ThucTap_Android", MainLauncher = true, Icon = "@drawable/icon")]
    [Activity(Label = "ThucTap_Android")]
    public class MainActivity : Activity
	{
        List<BenhNhan> listBenhNhan = new List<BenhNhan>();
        BenhNhan_Adapter BenhNhanAdapter;
        ListView listview;
        //Uri URL = new Uri("http://192.168.12.114:29444/BenhNhan");
        ConnectString connect = new ConnectString();

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
            
			var toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
            listview = FindViewById<ListView>(Resource.Id.listBenhNhan);

            //Toolbar will now take on default actionbar characteristics
            SetActionBar (toolbar);

			ActionBar.Title = "Danh Sách Bệnh Nhân";

            WebClient web = new WebClient();

            web.DownloadDataAsync(connect.connectstring);
            web.DownloadDataCompleted += Web_DownloadDataCompleted;

            RegisterForContextMenu(listview);

          //  listview.ItemClick += Listview_ItemClick;
        }
        /*
        private void Listview_ItemClick(object sender, ItemClickEventArgs e)
        {
            var temp = listview.GetItemIdAtPosition(e.Position);
            int so = (int)temp;
            string temp2 = listBenhNhan[so].mabn;
            Toast.MakeText(this, temp2, ToastLength.Long).Show();
        }
        */
        private void Web_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {

                string json = System.Text.Encoding.UTF8.GetString(e.Result);
                string final = json.Trim().Substring(1, (json.Length) - 2);
                string s = final.Replace(@"\", string.Empty);

                listBenhNhan = JsonConvert.DeserializeObject<List<BenhNhan>>(s);
                listBenhNhan.Sort(delegate(BenhNhan benhnhan1,BenhNhan benhnhan2) {
                    return benhnhan1.mabn.CompareTo(benhnhan2.mabn);
                });
                

                BenhNhanAdapter = new BenhNhan_Adapter(this, listBenhNhan);
                foreach(BenhNhan benhnhan in listBenhNhan)
                {
                    string ngaysinh = benhnhan.ngaysinh;
                    DateTime date = DateTime.Parse(ngaysinh);
                    string temp = Convert.ToDateTime(date).ToString("dd/MM/yyyy");
                    benhnhan.ngaysinh = temp;
                }

                listview.Adapter = BenhNhanAdapter;

            });
        }

        public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.home, menu);
			return base.OnCreateOptionsMenu (menu);
		}
		public override bool OnOptionsItemSelected (IMenuItem item)
		{	
			if(item.ItemId == Resource.Id.menu_Them)
            {
                Intent intent = new Intent(this, typeof(BenhNhan_Them_Activity));
                StartActivity(intent);
                Finish();
            }
            else if(item.ItemId == Resource.Id.menu_ThongKe)
            {
                Intent intent = new Intent(this, typeof(BenhNhan_ThongKe_Activity));
                intent.PutExtra("ThongKe", JsonConvert.SerializeObject(listBenhNhan));
                StartActivity(intent);
                Finish();
            }
			return base.OnOptionsItemSelected (item);
		}

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);
            
            menu.SetHeaderTitle("Chức năng");
            menu.Add(0, 1, 1, "Sửa");
            menu.Add(0, 2, 2, "Xoá");

        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            if(item.ItemId == 1)
            {
                var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
                var mabn = listBenhNhan[info.Position].mabn;
                Console.WriteLine("testhere" + mabn);
                BenhNhan temp  = listBenhNhan.Find(e => e.mabn == mabn);

                List<BenhNhan> temp2 = new List<BenhNhan>();
                temp2.Add(temp);

                string json = JsonConvert.SerializeObject(temp2);
                string json2 = json.Replace(@"[", string.Empty);
                string json3 = json2.Replace(@"]", string.Empty);
                string convertedJson = json3;



                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                BenhNhan_Sua_Dialog dialog = new BenhNhan_Sua_Dialog(temp.mabn,temp.holot,temp.ten,temp.ngaysinh,temp.gioitinh);
                dialog.Show(transaction, "Dialog");


            }
            else if(item.ItemId == 2)
            {
                var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
                var mabn = listBenhNhan[info.Position].mabn;

                listBenhNhan.RemoveAll(e => e.mabn == mabn);


                // BenhNhanAdapter = new BenhNhan_Adapter(this, listBenhNhan);
                // listview.Adapter = BenhNhanAdapter;
                BenhNhanAdapter.NotifyDataSetChanged();

                //string link = "http://192.168.12.114:29444/BenhNhan/" + mabn;
                string link = connect.connectstring.ToString() + "/" + mabn;
                DeleteItem(link);
            }
            return true;
        }

        public async void DeleteItem(string link)
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(link);
            HttpResponseMessage response = await client.DeleteAsync(uri);
            string responseMessage = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseMessage);
        }

        public async void EditItem(string json)
        {

            Console.WriteLine(json);
            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            Console.WriteLine(content);
            HttpResponseMessage response = await client.PutAsync(connect.connectstring, content);
            string responseMessage = await response.Content.ReadAsStringAsync();
        
            Console.WriteLine(responseMessage);
        }

    }
}



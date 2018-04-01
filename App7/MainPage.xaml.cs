using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace App7
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
     public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        public class MyList
        {
            public static List<Windows.Storage.StorageFile> FileList = new List<Windows.Storage.StorageFile>();
        }
        private string LoadListBoxData(int j)
        {
            InitializeComponent();
            List<string> songname = new List<string>();
            if (j >= MyList.FileList.Count)
            {
                return "暂无";
            }
            else
            {
                for (int i = 0; i < MyList.FileList.Count; i++)
                {
                    songname.Add(MyList.FileList[i].Name);
                }
                return songname[j];
            }
        }
        private async void MediaEnd(object sender, RoutedEventArgs e) {
                MyList.FileList.RemoveAt(0);
                if (MyList.FileList.Count != 0)
                {
                    if (MyList.FileList[0].ContentType.IndexOf("audio/") == 0)
                    {
                        this.textBlock.Text = "正在播放音频" + MyList.FileList[0].DisplayName;
                    }
                    if (MyList.FileList[0].ContentType.IndexOf("video/") == 0)
                    {
                        this.textBlock.Text = "正在播放视频" + MyList.FileList[0].DisplayName;
                    }
                    var source = await MyList.FileList[0].OpenAsync(Windows.Storage.FileAccessMode.Read);
                    player.SetSource(source, MyList.FileList[0].ContentType);
                    player.Play();
                }
                else {
                    player.Source = null;
                    this.textBlock.Text = "播放结束,请选择文件";
            }
                this.songlist1.Text = LoadListBoxData(0);
                this.songlist2.Text = LoadListBoxData(1);
                this.songlist3.Text = LoadListBoxData(2);
                this.songlist4.Text = LoadListBoxData(3);
                this.songlist5.Text = LoadListBoxData(4);
        }
        private async void btnGetName_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".mp4");
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                bool existFlag = false;
                for (int i = 0; i < MyList.FileList.Count; i++) {
                    if (file.Equals(MyList.FileList[i])) {
                        existFlag = true;
                        break;
                    }
                }
                if (existFlag)
                {
                    this.textBlock.Text = file.DisplayName + "已在播放列表";
                }
                else {
                    MyList.FileList.Add(file);
                    if (MyList.FileList.Count == 1)
                    {
                        if (file.ContentType.IndexOf("audio/") == 0)
                        {
                            this.textBlock.Text = "正在播放音频" + file.DisplayName;
                        }
                        if (file.ContentType.IndexOf("video/") == 0)
                        {
                            this.textBlock.Text = "正在播放视频" + file.DisplayName;
                        }
                        this.songlist1.Text = LoadListBoxData(0);
                        this.songlist2.Text = LoadListBoxData(1);
                        this.songlist3.Text = LoadListBoxData(2);
                        this.songlist4.Text = LoadListBoxData(3);
                        this.songlist5.Text = LoadListBoxData(4);
                        var source = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                        player.SetSource(source, file.ContentType);
                        player.Play();
                    }
                    else {
                        this.songlist1.Text = LoadListBoxData(0);
                        this.songlist2.Text = LoadListBoxData(1);
                        this.songlist3.Text = LoadListBoxData(2);
                        this.songlist4.Text = LoadListBoxData(3);
                        this.songlist5.Text = LoadListBoxData(4);
                    }
                }
            }
            else
            {
                this.textBlock.Text = "操作取消";
            }
        }
    }
 }

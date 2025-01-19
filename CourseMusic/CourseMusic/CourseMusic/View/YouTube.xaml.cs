using MediaToolkit.Model;
using MediaToolkit;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CourseMusic.ViewModel;
using VideoLibrary;
using System.ComponentModel;
using System;
using System.IO;
using System.Collections.ObjectModel;

namespace CourseMusic.View
{
    public partial class YouTube : UserControl
    {
        #region Fields and Properties

        public BackgroundWorker? Worker;
        public ObservableCollection<string>? YoutubeMusicList { get; set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public YouTube()
        {
            InitializeComponent();
            DataContext = this;

            YoutubeMusicList = PlayerVM.Musics;

            Worker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };

            Worker.DoWork += Do_Work!;
            Worker.ProgressChanged += Progress_Back!;
        }

        #region Events
        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                prog.Value = 0;
                string url = txt.Text!;
                txt.IsEnabled = false;

                try
                {
                    if (string.IsNullOrWhiteSpace(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    {
                        throw new ArgumentException("Invalid URL format");
                    }

                    var youtube = VideoLibrary.YouTube.Default;
                    var vid = youtube.GetVideo(url);
                    string musicName = Path.GetFileNameWithoutExtension(vid.FullName);

                    if (!YoutubeMusicList!.Contains($"{musicName}.mp3"))
                    {
                        SaveMP3(vid);
                    }
                    else
                    {
                        messageLabel.Content = "This music has already been added";
                        txt.Text = string.Empty;
                        txt.IsEnabled = true;
                    }
                }
                catch (Exception ex)
                {
                    messageLabel.Content = $"Error: {ex.Message}";
                    txt.IsEnabled = true;
                }
            }
        }

        #region Youtube_Music_File_Downloader_and_Converter

        /// <summary>
        /// Music File Download and Converter
        /// </summary>
        /// <param name="video"></param>
        public void SaveMP3(YouTubeVideo video)
        {
            try
            {
                Dispatcher.Invoke(() => { messageLabel.Content = "Downloading..."; });

                string directoryPath = PlayerVM.Mpath!;
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string videoPath = Path.Combine(directoryPath, video.FullName);
                if (!File.Exists(videoPath))
                {
                    File.WriteAllBytes(videoPath, video.GetBytes());
                }

                string mp3Name = Path.GetFileNameWithoutExtension(video.FullName);
                string mp3Path = Path.Combine(directoryPath, $"{mp3Name}.mp3");

                Dispatcher.Invoke(() => { messageLabel.Content = "Converting to mp3..."; });

                var inputFile = new MediaFile { Filename = videoPath };
                var outputFile = new MediaFile { Filename = mp3Path };

                using (var engine = new Engine())
                {
                    try
                    {
                        engine.GetMetadata(inputFile);

                        if (string.IsNullOrWhiteSpace(inputFile.Metadata?.AudioData?.Format))
                        {
                            throw new InvalidOperationException("The input file has no audio stream.");
                        }

                        engine.Convert(inputFile, outputFile);

                        if (!File.Exists(mp3Path))
                        {
                            throw new FileNotFoundException($"MP3 file {mp3Path} was not created.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            messageLabel.Content = $"Conversion error: {ex.Message}";
                            txt.IsEnabled = true;
                        });
                        return;
                    }
                }

                File.Delete(videoPath);

                Dispatcher.Invoke(() =>
                {
                    txt.IsEnabled = true;
                    if (!YoutubeMusicList!.Contains($"{mp3Name}.mp3"))
                        YoutubeMusicList.Add($"{mp3Name}.mp3");
                    messageLabel.Content = "Download and conversion completed.";
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    messageLabel.Content = $"Error during download or conversion: {ex.Message}";
                    txt.IsEnabled = true;
                });
            }
        }

        /// <summary>
        /// Background progress updater
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Progress_Back(object sender, ProgressChangedEventArgs e)
        {
            Dispatcher.Invoke(() => { prog.Value = e.ProgressPercentage; });
        }

        void Do_Work(object sender, EventArgs e)
        {
            for (int i = 0; i <= 100; i++)
            {
                if (Worker!.CancellationPending)
                {
                    Worker.ReportProgress(100);
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Download was completed");
                        messageLabel.Content = "Download was completed";
                        prog.Value = 0;
                    });
                    break;
                }
                Thread.Sleep(250);
                Worker.ReportProgress(i);
            }
        }

        #endregion

        #region List_Menu_Context_and_File_Deleter

        private void listBox_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MenuItem_Click(sender, e);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuItem = sender as MenuItem;
                string header = menuItem?.Header.ToString()!;
                string? name = listBox.SelectedItem?.ToString();

                if (header == "_delete" && name != null && YoutubeMusicList!.Contains(name))
                {
                    string filePath = Path.Combine(PlayerVM.Mpath!, name);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        YoutubeMusicList.Remove(name);
                    }
                    else
                    {
                        MessageBox.Show("File not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting file: {ex.Message}");
            }
        }

        #endregion

        #endregion
    }
}

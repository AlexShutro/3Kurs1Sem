using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CourseMusic.Helpers;
using CourseMusic.Models;

namespace CourseMusic.View;

public partial class Home : UserControl
{
    public Home()
    {
        InitializeComponent();
       

        Task.Run(async () => await SearchHelper.GetTokenAsync());
    }

    private void txtSearch_KeyUp(object sender, KeyEventArgs e)
    {

        if (txtSearch.Text == string.Empty)
        {
            ListArtist.ItemsSource = null;
            ListArtist.Visibility = Visibility.Visible;
            homevv.Visibility = Visibility.Hidden;
            return;
        }

        ListArtist.Visibility = Visibility.Visible;
        homevv.Visibility = Visibility.Hidden;
        
        var result = SearchHelper.SearchArtistOrSong(txtSearch.Text);

        if (result == null)
        {
            return;
        }

        var listArtist = new List<Artist>();

        foreach (var item in result.Artists!.Items!)
        {
            listArtist.Add(new Artist()
            {
                ID = item.Id,
                Image = item.Images!.Any() ? item.Images![0].Url : "Image.png",
                Name = item.Name,
                Popularity = $"{item.Popularity}% popularidad",
                Followers = $"{item.Followers!.Total.ToString("N")} слушателей"
            });
        }

        ListArtist.ItemsSource = listArtist;
    }
}
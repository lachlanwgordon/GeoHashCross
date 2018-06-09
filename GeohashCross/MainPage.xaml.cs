using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeohashCross.Model.Services;
using Xamarin.Forms;

namespace GeohashCross
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var webclient = new Webclient();
        }
    }
}

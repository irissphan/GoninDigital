﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GoninDigital.Models;

namespace GoninDigital.ViewModels
{
    class HomePageViewModel : BaseViewModel
    {
        private string art;
        public string Art
        {
            get { return art; }
            set { art = value; OnPropertyChanged(); }
        }

        private List<Product> recommnededByEditor;
        public List<Product> RecommendedByEditor
        {
            get { return recommnededByEditor; }
            set { recommnededByEditor = value; OnPropertyChanged(); }
        }
        public List<Product> RecommendedByEditor3
        {
            get { return recommnededByEditor.GetRange(0, 3); }
        }

        private string artGroup1;
        public string ArtGroup1
        {
            get => artGroup1;
        }

        private string artGroup2;
        public string ArtGroup2
        {
            get => artGroup2;
        }

        public HomePageViewModel()
        {
            art = "/Resources/Images/HomeBanner.jpg";
            artGroup1 = "/Resources/Images/HomeProductCardGroupBackground.png";
            artGroup2 = "/Resources/Images/HomeProductCardGroupBackground2.jpg";
            GoninDigitalDBContext db = DataProvider.Instance.Db;
            recommnededByEditor = db.Products.ToList();
        }

    }
}

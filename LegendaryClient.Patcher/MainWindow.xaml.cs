﻿#region

using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Animation;
using LegendaryClient.Patcher.Logic;
using LegendaryClient.Patcher.Pages;

#endregion

namespace LegendaryClient.Patcher
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Visibility = Visibility.Hidden;
            var page = new SplashPage();
            page.Show();
            Client.ExecutingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Client.MainHolder = Container;
            Client.Win = this;
            Player.Volume = Properties.Settings.Default.Volume / 100;
            Client.SoundPlayer = Player;
            var region = Properties.Settings.Default.RegionName;
            if (!string.IsNullOrEmpty(region))
                RegionLabel.Content = "Connected to: " + region;
            else
                RegionLabel.Content = "Not connected to any regions";
            Client.regionLabel = RegionLabel;
            Client.OverlayContainer = OverlayContainer;
            Client.OverlayGrid = OverlayGrid;
            OverlayContainer.Content = new PatcherSettingsPage(Properties.Settings.Default.FirstStart).Content;
            //It is easier to access the menu with this
#if Debug
            Properties.Settings.Default.FirstStart = false;
#endif
            var waitAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
            waitAnimation.Completed += (o, e) => { Container.Content = new PatcherPage().Content; };
            Container.BeginAnimation(OpacityProperty, waitAnimation);
        }


        //Contains a progress for the future
        public void SlideGrid(object sender, RoutedEventArgs e)
        {
            OverlayGrid.Visibility = OverlayGrid.Visibility == Visibility.Hidden
                ? Visibility.Visible
                : Visibility.Hidden;
        }
    }
}
﻿using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Coin_Wave_Lib;

namespace Coin_Wave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CoinWaveWindow : Window
    {
        int levelForGenerator = 0;
        int level = 0;

        private int _windowSize = 56;
        // размер карты 34 на 15
        private int _windowSizeX = 34;
        private int _windowSizeY = 15;
        public CoinWaveWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
        {
            labelLavelForGame.Content = "Уровень: " + level;
            levelEndCanvas.Visibility = Visibility.Collapsed;
        }

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            var nativeWinSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(_windowSizeX * _windowSize, _windowSizeY * _windowSize),
                Location = new Vector2i(5, 30),
                WindowBorder = WindowBorder.Resizable,
                //WindowState = OpenTK.Windowing.Common.WindowState.Fullscreen,


                // Flags = ContextFlags.ForwardCompatible,
                Flags = ContextFlags.Default,
                APIVersion = new Version(3, 3),
                Profile = ContextProfile.Compatability,
                // Profile = ContextProfile.Core,
                API = ContextAPI.OpenGL,
                NumberOfSamples = 0
            };

            string fileFirst = @"..\..\..\..\Coin Wave Lib\data\maps\lvl" + level + @"\first.xml";
            string fileSecond = @"..\..\..\..\Coin Wave Lib\data\maps\lvl" + level + @"\second.xml";
            Coin_Wave_Lib.CoinWaveWindow game = new Coin_Wave_Lib.CoinWaveWindow(GameWindowSettings.Default, nativeWinSettings, fileFirst, fileSecond);
            using (game)
            {
                game.Run();
            }

            levelEndLabel.Content = game.MESSAGE;
            levelEndCanvas.Visibility = Visibility.Visible;

            foreach (var child in ((Grid)Content).Children)
            {
                if (child is UIElement uiElement && uiElement != levelEndCanvas)
                {
                    uiElement.IsEnabled = false;
                }
            }
            levelEndCanvas.IsEnabled = true;

            if (game.levelIsComplieted)
            {
                level++;
                labelLavelForGame.Content = "Уровень: " + level;
            }
        }

        private void MapGenerator_Click(object sender, RoutedEventArgs e)
        {
            var nativeWinSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(Convert.ToInt32(1920/1.1), Convert.ToInt32(1080 / 1.1)),
                Location = new Vector2i(5, 30),
                WindowBorder = WindowBorder.Resizable,
                //WindowState = OpenTK.Windowing.Common.WindowState.Fullscreen,

                Flags = ContextFlags.Default,
                APIVersion = new Version(3, 3),
                Profile = ContextProfile.Compatability,
                API = ContextAPI.OpenGL,
                NumberOfSamples = 0
            };


            string fileFirst = @"..\..\..\..\Coin Wave Lib\data\maps\lvl" + levelForGenerator + @"\first.xml";
            string fileSecond = @"..\..\..\..\Coin Wave Lib\data\maps\lvl" + levelForGenerator + @"\second.xml";
            using (MapGenerateWindow game = new MapGenerateWindow(GameWindowSettings.Default, nativeWinSettings, fileFirst, fileSecond))
            {
                game.Run();
            }
        }

        private void levelUpForGenerateButtonClick_Click(object sender, RoutedEventArgs e)
        {
            levelForGenerator++;
            if (levelForGenerator >= 6) { levelForGenerator = 0; }
            levelLabel.Content = levelForGenerator.ToString();
        }

        private void levelDownForGenerateButtonClick_Click(object sender, RoutedEventArgs e)
        {
            levelForGenerator--;
            if (levelForGenerator < 0) { levelForGenerator = 0; }
            levelLabel.Content = levelForGenerator.ToString();
        }

        private void levelDownButtonClick_Click(object sender, RoutedEventArgs e)
        {
            level--;
            if (level < 0) { level = 0; }
            labelLavelForGame.Content = "Уровень: " + level;
        }

        private void levelUpButtonClick_Click(object sender, RoutedEventArgs e)
        {
            level++;
            if (level >= 6) { level = 0; }
            labelLavelForGame.Content = "Уровень: " + level;
        }

        private void levelEndButton_Click(object sender, RoutedEventArgs e)
        {
            levelEndCanvas.Visibility = Visibility.Collapsed;
            foreach (var child in ((Grid)Content).Children)
            {
                if (child is UIElement uiElement && uiElement != levelEndCanvas)
                {
                    uiElement.IsEnabled = true;
                }
            }
            levelEndCanvas.IsEnabled = false;
        }
    }
}
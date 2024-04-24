using OpenTK.Mathematics;
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
    public partial class MainWindow : Window
    {
        int level = 0;
        private int _windowSize = 50;
        // размер карты 34 на 15
        private int _windowSizeX = 34;
        private int _windowSizeY = 15;
        public MainWindow()
        {
        }

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            var nativeWinSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(_windowSizeX * _windowSize, _windowSizeY * _windowSize),
                Location = new Vector2i(100, 100),
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

            string fileFirst = @"data\maps\lvl" + level + @"\first.xml";
            string fileSecond = @"data\maps\lvl" + level + @"\second.xml";
            CoinWaveWindow game = new CoinWaveWindow(GameWindowSettings.Default, nativeWinSettings, fileFirst, fileSecond);
            using (game)
            {
                game.Run();
            }

            MessageBox.Show(game.MESSAGE);
        }

        private void MapGenerator_Click(object sender, RoutedEventArgs e)
        {
            var nativeWinSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(Convert.ToInt32(1920/1.1), Convert.ToInt32(1080 / 1.1)),
                Location = new Vector2i(10, 10),
                WindowBorder = WindowBorder.Resizable,
                //WindowState = OpenTK.Windowing.Common.WindowState.Fullscreen,

                Flags = ContextFlags.Default,
                APIVersion = new Version(3, 3),
                Profile = ContextProfile.Compatability,
                API = ContextAPI.OpenGL,
                NumberOfSamples = 0
            };


            using (MapGenerateWindow game = new MapGenerateWindow(GameWindowSettings.Default, nativeWinSettings, level))
            {
                game.Run();
            }
        }

        private void levelUpButtonClick_Click(object sender, RoutedEventArgs e)
        {
            level++;
            levelLabel.Content = level.ToString();
        }

        private void levelDownButtonClick_Click(object sender, RoutedEventArgs e)
        {
            level--;
            if (level < 0) { level = 0; }
            levelLabel.Content = level.ToString();
        }
    }
}
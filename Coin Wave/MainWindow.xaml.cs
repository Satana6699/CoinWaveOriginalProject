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
using System.Configuration;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Coin_Wave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWaveWindow : System.Windows.Window
    {
        private Dictionary<string, string> appSettings = new Dictionary<string, string>();

        private int levelForGenerator = 0;
        private int level = 0;

        private int _windowSizeOneSquare;
        private int _countSquaresInWidth;
        private int _countSquaresInHeight;
        private string _filePathForLevels;
        
        private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
        {
            // Чтение параметров из файла конфигурации
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                appSettings.Add(key, ConfigurationManager.AppSettings[key]);
            }

            _countSquaresInWidth = Convert.ToInt32(appSettings["CountSquaresInWidth"]);
            _countSquaresInHeight = Convert.ToInt32(appSettings["CountSquaresInHeight"]);
            _windowSizeOneSquare = Convert.ToInt32(appSettings["WindowSizeOneSquare"]);
            _filePathForLevels = appSettings["FilePathForLevels"];
            labelLavelForGame.Content = "Уровень: " + level;
            levelEndCanvas.Visibility = Visibility.Collapsed;
        }

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            var nativeWinSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(_countSquaresInWidth * _windowSizeOneSquare, _countSquaresInHeight * _windowSizeOneSquare),
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

            string fileFirst = _filePathForLevels + level + @"\first.xml";
            string fileSecond = _filePathForLevels + level + @"\second.xml";
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


            string fileFirst = _filePathForLevels + levelForGenerator + @"\first.xml";
            string fileSecond = _filePathForLevels + levelForGenerator + @"\second.xml";
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

        private void buttonRools_Click(object sender, RoutedEventArgs e)
        {
            WindowRools windowRools = new WindowRools(this);
            windowRools.ShowDialog();
        }
    }
}
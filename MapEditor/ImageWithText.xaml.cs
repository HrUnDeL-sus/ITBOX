using System;
using System.Collections.Generic;
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

namespace MapEditor
{
    /// <summary>
    /// Логика взаимодействия для ImageWithText.xaml
    /// </summary>
    public partial class ImageWithText : UserControl
    {
        public ImageWithText(string text,ImageSource imageSource)
        {
            InitializeComponent();
            ImageTextBlock.Text = text;
            MainImage.Source = imageSource;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VComm.Core.Windows
{
    /// <summary>
    /// Interaction logic for CreateVPack.xaml
    /// </summary>
    public partial class CreateVPack : Window
    {
        public CreateVPack()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += delegate { DraggyWindow(); }; //Draggy windows
        }

        private void DraggyWindow()
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed) DragMove();
        }
    }
}

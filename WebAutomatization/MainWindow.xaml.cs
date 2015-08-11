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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenQA.Selenium.Firefox;
using WebAutomatization.Core;
using WebAutomatization.Core.JS;
using WebAutomatization.Domain;
using WebAutomatization.Domain.Registration;
using WebAutomatization.Registration.View;

namespace WebAutomatization {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            DataContext = new RegistrationViewModel();
        }
    }

   
}

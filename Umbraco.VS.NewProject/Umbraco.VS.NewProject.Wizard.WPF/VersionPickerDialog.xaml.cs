﻿using System;
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

namespace Umbraco.VS.NewProject.Wizard.WPF
{
    /// <summary>
    /// Interaction logic for VersionPickerDialog.xaml
    /// </summary>
    public partial class VersionPickerDialog : UserControl
    {
        public string selectedVersion { get; set; }

        public VersionPickerDialog()
        {
            InitializeComponent();
        }

        private void getUmbracoButtonClick(object sender, RoutedEventArgs e)
        {
            //Set the value from dropdown into selectedVersion variable
            var chosenVersion   = (versionList.SelectedValue as ComboBoxItem).Content.ToString();
            selectedVersion     = chosenVersion;

            //Close dialog
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }
    }
}

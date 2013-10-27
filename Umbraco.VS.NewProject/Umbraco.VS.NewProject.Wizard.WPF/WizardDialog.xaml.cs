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
using System.Xml;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using Path = System.IO.Path;

namespace Umbraco.VS.NewProject.Wizard.WPF
{
    /// <summary>
    /// Interaction logic for WizardDialog.xaml
    /// </summary>
    public partial class WizardDialog : UserControl
    {
        private string _umbracoSitePath;
        public string umbracoSitePath
        {
            get { return _umbracoSitePath; }
            set { _umbracoSitePath = value; }
        }

        public string umbracoVersion
        {
            get { return umbracoVersionStatusBar.Content.ToString(); }
            set { umbracoVersionStatusBar.Content = string.Format("Umbraco Nuget Version: {0}", value); }
        }

        public enum DatabaseType
        {
            SQLCE,
            SQL,
            Azure,
            MySQL,
            Advanced
        }

        public DatabaseContext _db;

        public WizardDialog()
        {
            InitializeComponent();

            //New Up Object
            _db = new DatabaseContext();
        }

        private void CreateProjectClick(object sender, RoutedEventArgs e)
        {
            //Get the selected value from the dropdown
            var selectedEngine = (engineChoice.SelectedValue as ComboBoxItem).Content.ToString();

            //Set rendeing mode dependant on choice
            switch (selectedEngine)
            {
                case "MVC (Recommended)":
                    //Updates config value...
                    UpdateRenderingEngine(RenderingEngine.Mvc);
                    break;

                case "WebForms":
                    //Updates config value...
                    UpdateRenderingEngine(RenderingEngine.WebForms);
                    break;

                default:
                    //Updates config value...
                    UpdateRenderingEngine(RenderingEngine.Mvc);
                    break;
            }

            //Set DB type
            //Get the selected value from the dropdown
            var selectedDB = (databaseType.SelectedValue as ComboBoxItem).Content.ToString();

            switch (selectedDB)
            {
                case "SQL CE File based Database (Recommended)":
                    UpdateDatabase(DatabaseType.SQLCE);
                    break;

                case "SQL Server":
                    //Update Database deals with the intergrated security check
                    UpdateDatabase(DatabaseType.SQL);
                    break;

                case "SQL Azure":
                    UpdateDatabase(DatabaseType.Azure);
                    break;

                case "MySQL":
                    UpdateDatabase(DatabaseType.MySQL);
                    break;

                case "Advanced":
                    UpdateDatabase(DatabaseType.Advanced);
                    break;

                default:
                    //As a default fallback - use SQL CE
                    UpdateDatabase(DatabaseType.SQLCE);
                    break;
            }


            //TODO: Create DB Schema
            //_db.CreateDatabaseSchema();

            //Need to figure a way to close dialog from usercontrol
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private void UpdateRenderingEngine(RenderingEngine engine)
        {
            //Local variables

            // C:\\inetpub\\wwwroot\\UmbracoWebsite5\\UmbracoWebsite5
            var rootFolder = umbracoSitePath;

            // C:\\inetpub\\wwwroot\\UmbracoWebsite5\\UmbracoWebsite5\\config\
            var path = rootFolder + Path.DirectorySeparatorChar + "config" + Path.DirectorySeparatorChar;

            // C:\\inetpub\\wwwroot\\UmbracoWebsite5\\UmbracoWebsite5\\config\umbracoSettings.config
            var filePath = path + "umbracoSettings.config";

            //Template mode (Mvc or WebForms)
            var value = engine;

            //Load the config file as xml
            var xml = new XmlDocument();

            try
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    //Load XML
                    xml.Load(reader);
                }

                //Find the correct node for the 'defaultRenderingEngine' and update the value
                if (xml.DocumentElement != null)
                {
                    //Get the defaultRendering engine XML node, so we can update it's value
                    var node = xml.DocumentElement.SelectSingleNode("/settings/templates/defaultRenderingEngine");

                    if (node != null)
                    {
                        //Update the value in the XML
                        node.InnerText = value.ToString();
                    }
                }

                //Save file
                xml.Save(filePath);

            }
            catch (Exception ex)
            {
                //Otherwise disable the button & show error message
                MessageBox.Show("Exception: " + ex.Message, "Update Rendering Engine Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void databaseTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            //Get the selected value from the dropdown
            if ((databaseType.SelectedValue as ComboBoxItem).Content != null)
            {
                var dbTypeItem = (databaseType.SelectedValue as ComboBoxItem).Content.ToString();

                if (!string.IsNullOrEmpty(dbTypeItem))
                {

                    //Enable/disable controls depending on choice
                    switch (dbTypeItem)
                    {
                        case "SQL CE File based Database (Recommended)":
                            //Disable datbase details group
                            databaseDetailsGroup.IsEnabled = false;
                            break;

                        case "SQL Server":
                            databaseDetailsGroup.IsEnabled  = true;
                            serverLabel.IsEnabled           = true;
                            server.IsEnabled                = true;
                            databaseNameLabel.IsEnabled     = true;
                            databaseName.IsEnabled          = true;
                            usernameLabel.IsEnabled         = true;
                            username.IsEnabled              = true;
                            passwordLabel.IsEnabled         = true;
                            password.IsEnabled              = true;
                            securityLabel.IsEnabled         = true;
                            security.IsEnabled              = true;
                            connectionLabel.IsEnabled       = false;
                            connection.IsEnabled            = false;
                            testConnectionButton.IsEnabled  = true;
                            break;

                        case "SQL Azure":
                            databaseDetailsGroup.IsEnabled  = true;
                            serverLabel.IsEnabled           = true;
                            server.IsEnabled                = true;
                            databaseNameLabel.IsEnabled     = true;
                            databaseName.IsEnabled          = true;
                            usernameLabel.IsEnabled         = true;
                            username.IsEnabled              = true;
                            passwordLabel.IsEnabled         = true;
                            password.IsEnabled              = true;
                            securityLabel.IsEnabled         = false;
                            security.IsEnabled              = false;
                            connectionLabel.IsEnabled       = false;
                            connection.IsEnabled            = false;
                            testConnectionButton.IsEnabled  = true;
                            break;

                        case "MySQL":
                            databaseDetailsGroup.IsEnabled  = true;
                            serverLabel.IsEnabled           = true;
                            server.IsEnabled                = true;
                            databaseNameLabel.IsEnabled     = true;
                            databaseName.IsEnabled          = true;
                            usernameLabel.IsEnabled         = true;
                            username.IsEnabled              = true;
                            passwordLabel.IsEnabled         = true;
                            password.IsEnabled              = true;
                            securityLabel.IsEnabled         = false;
                            security.IsEnabled              = false;
                            connectionLabel.IsEnabled       = false;
                            connection.IsEnabled            = false;
                            testConnectionButton.IsEnabled  = true;
                            break;

                        case "Advanced":
                            databaseDetailsGroup.IsEnabled  = true;
                            serverLabel.IsEnabled           = false;
                            server.IsEnabled                = false;
                            databaseNameLabel.IsEnabled     = false;
                            databaseName.IsEnabled          = false;
                            usernameLabel.IsEnabled         = false;
                            username.IsEnabled              = false;
                            passwordLabel.IsEnabled         = false;
                            password.IsEnabled              = false;
                            securityLabel.IsEnabled         = false;
                            security.IsEnabled              = false;
                            connectionLabel.IsEnabled       = true;
                            connection.IsEnabled            = true;
                            testConnectionButton.IsEnabled  = true;
                            break;

                        default:
                            //Disable datbase details group
                            databaseDetailsGroup.IsEnabled = false;
                            break;
                    }
                }
            }
        }

        private void UpdateDatabase(DatabaseType dbType)
        {
            //Our copy of DatbaseContext from Umbraco.Core
            _db.umbracoSitePath = umbracoSitePath;

            switch (dbType)
            {
                case DatabaseType.SQLCE:
                    //Setup Datbase to use SQL CE
                    _db.ConfigureEmbeddedDatabaseConnection();
                    break;

                case DatabaseType.SQL:
                    //If intergated security checked then
                    if (security.IsChecked == true)
                    {
                        //Intergrated security SQL connection
                        _db.ConfigureIntegratedSecurityDatabaseConnection(server.Text, databaseName.Text);
                        break;
                    }
                    //Normal SQL connection
                    _db.ConfigureDatabaseConnection(server.Text, databaseName.Text, username.Text, password.Text, DatabaseContext.DatabaseType.SQL);
                    break;

                case DatabaseType.Azure:
                    //Normal Azure SQL connection
                    _db.ConfigureDatabaseConnection(server.Text, databaseName.Text, username.Text, password.Text, DatabaseContext.DatabaseType.Azure);
                    break;

                case DatabaseType.MySQL:
                    //Normal MySQL connection
                    _db.ConfigureDatabaseConnection(server.Text, databaseName.Text, username.Text, password.Text, DatabaseContext.DatabaseType.MySQL);
                    break;

                case DatabaseType.Advanced:
                    //Configure using custom DB connection string supplied
                    _db.ConfigureDatabaseConnection(connection.Text);
                    break;

                default:
                    //Setup Datbase to use SQL CE
                    _db.ConfigureEmbeddedDatabaseConnection();
                    break;
            }
        }

        private void intergratedSecurityClick(object sender, RoutedEventArgs e)
        {
            //Security checkbox checked
            if (security.IsChecked == true)
            {
                usernameLabel.IsEnabled = false;
                username.IsEnabled = false;
                passwordLabel.IsEnabled = false;
                password.IsEnabled = false;

            }
            else if (security.IsChecked == false)
            {
                usernameLabel.IsEnabled = true;
                username.IsEnabled = true;
                passwordLabel.IsEnabled = true;
                password.IsEnabled = true;
            }
        }

        
    }
}

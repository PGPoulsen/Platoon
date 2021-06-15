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
using System.Collections.Specialized;

namespace Ohayo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContextChanged += MainWindow_DataContextChanged;
        }

        private void MainWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DataContextChanged -= MainWindow_DataContextChanged;
            DataContextViewModel.LanguageChanged += DataContextViewModel_LanguageChanged;
            DataContextViewModel.Rows.CollectionChanged += Rows_CollectionChanged;
        }

        private void DataContextViewModel_LanguageChanged(object sender, LanguageChangeEventArgs e)
        {
            if(e.Change == LanguageChangeEventArgs.ChangeMode.Add)
            {
                string xaml = "<Style xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" TargetType=\"TextBlock\"><Setter Property=\"TextWrapping\" Value=\"Wrap\"/></Style>";
                Style style = System.Windows.Markup.XamlReader.Parse(xaml) as Style;

                var bindString = string.Format("[{0}]", e.LanguageCode);
                var binding = new Binding(bindString);
                binding.Mode = BindingMode.OneWay;
                var column = new DataGridTextColumn
                {
                    Header = e.LanguageCode,
                    Binding = binding,
                    ElementStyle = style,
                    Width = 300
                };
                LanguageGrid.Columns.Add(column);
            }
            if(e.Change == LanguageChangeEventArgs.ChangeMode.Remove)
            {
                var column = LanguageGrid.Columns.Single(x => x.Header.Equals(e.LanguageCode));
                LanguageGrid.Columns.Remove(column);
            }
        }

        private void Rows_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Reset:
                    throw new NotImplementedException();
            }
        }

        private MainWindowViewModel DataContextViewModel => (MainWindowViewModel)DataContext;

        public void AddDanish(object sender, RoutedEventArgs e)
        {
            DataContextViewModel.LanguageChanged += DataContextViewModel_LanguageChanged;
            var result = DataContextViewModel.AddPoFile(
                "C:\\users\\pgpoulsen\\Downloads\\Locale\\da-DK\\LC_MESSAGES\\ShopFloorManagementSuite.po");
            if (!result)
            {
                var window = Window.GetWindow(this);
                MessageBox.Show(window, "Language already added", "Failed to add language", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            DataContextViewModel.LanguageChanged -= DataContextViewModel_LanguageChanged;

            //var bindString = string.Format("LanguageToTranslationList[0][0]");
        }
        public void AddGerman(object sender, RoutedEventArgs e)
        {
            DataContextViewModel.LanguageChanged += DataContextViewModel_LanguageChanged;
            var result = DataContextViewModel.AddPoFile(
                "C:\\users\\pgpoulsen\\Downloads\\Locale\\de-DE\\LC_MESSAGES\\ShopFloorManagementSuite.po");
            if (!result)
            {
                var window = Window.GetWindow(this);
                MessageBox.Show(window, "Language already added", "Failed to add language", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            DataContextViewModel.LanguageChanged -= DataContextViewModel_LanguageChanged;

            //var bindString = string.Format("LanguageToTranslationList[0][0]");
        }

        private void RemoveLanguage(object sender, RoutedEventArgs e)
        {
            DataContextViewModel.LanguageChanged += DataContextViewModel_LanguageChanged;
            var menuItem = (MenuItem)sender;
            var languageToRemove = (string)menuItem.DataContext;
            DataContextViewModel.RemovePoFile(languageToRemove);

            DataContextViewModel.LanguageChanged -= DataContextViewModel_LanguageChanged;
        }
    }
}

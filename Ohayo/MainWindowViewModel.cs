using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Karambolo.PO;
using System.IO;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace Ohayo
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ICollection<POCatalog> _catalogs = new List<POCatalog>();
        private PoEntryRow _selectedRow;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ObservableCollection<PoEntryRow> Rows { get; } = new();
        public ICollection<string> Languages => _catalogs.Select(x => x.Language).ToList();

        public PoEntryRow SelectedRow
        {
            get => _selectedRow;
            set
            {
                _selectedRow = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedRow)));
            }
        }

        public event EventHandler<LanguageChangeEventArgs> LanguageChanged = delegate { };

        public MainWindowViewModel()
        {
        }


        public bool AddPoFile(string filePath)
        {
            var parser = new POParser(new POParserSettings
            {
                // parser options...
            });

            var reader = File.OpenText(filePath);
            var result = parser.Parse(reader);
            if (_catalogs.Any(x => x.Language == result.Catalog.Language))
            {
                return false;
            }
            _catalogs.Add(result.Catalog);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Languages)));
            foreach (var entry in result.Catalog)
            {
                if (entry.Key.Id == "Cancel Checklist Task")
                {

                }

                var row = Rows.SingleOrDefault(x => x.Key.Equals(entry.Key));
                if (row is null)
                {
                    row = new PoEntryRow(entry.Key, filePath);
                    Rows.Add(row);
                }
                row.LanguageToEntry[result.Catalog.Language] = entry;
            }
            LanguageChanged(this,
                new LanguageChangeEventArgs
                {
                    Change = LanguageChangeEventArgs.ChangeMode.Add,
                    LanguageCode = result.Catalog.Language
                });
            return true;
        }

        public void RemovePoFile(string language)
        {
            var catalog = _catalogs.SingleOrDefault(x => x.Language.Equals(language));
            if (catalog is null)
            {
                return;
            }
            _catalogs.Remove(catalog);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Languages)));

            foreach (var key in catalog.Keys)
            {
                var row = Rows.SingleOrDefault(x => x.Key.Equals(key));
                if (row is not null)
                {
                    row.LanguageToEntry.Remove(language);
                    if (row.LanguageToEntry.Count == 0)
                    {
                        Rows.Remove(row);
                    }
                }
            }
            LanguageChanged(this,
                new LanguageChangeEventArgs
                {
                    Change = LanguageChangeEventArgs.ChangeMode.Remove,
                    LanguageCode = language
                });
        }


        //public void ChangeMessageId(string context, string oldMessageId, string newMessageId)
        //{
        //    var key = new POKey(id: oldMessageId, contextId: context);
        //    foreach (var catalog in _catalogs)
        //    {
        //    }
        //}
    }


    public class LanguageChangeEventArgs
    {
        public enum ChangeMode
        {
            Add,
            Remove
        };
        public ChangeMode Change { get; set; }
        public string LanguageCode { get; set; }
    }
}

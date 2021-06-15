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
using System.Diagnostics.CodeAnalysis;
using System.Collections;

namespace Ohayo
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private HashSet<POKey> _keys = new();
        private ICollection<POCatalog> _catalogs = new List<POCatalog>();

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ObservableCollection<PoEntryRow> Rows { get; } = new();
        public ICollection<string> Languages => _catalogs.Select(x => x.Language).ToList();

        public event EventHandler<LanguageChangeEventArgs> LanguageChanged = delegate { };

        public MainWindowViewModel()
        {
            //   AddPoFile();
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
                var addedKey = _keys.Add(entry.Key);
                if (addedKey)
                {
                    foreach (var e in entry)
                    {

                    }
                    if (entry.Key.Id == "Cancel Checklist Task")
                    {

                    }
                }
                var row = Rows.SingleOrDefault(x => x.Key.Equals(entry.Key));
                if (row is null)
                {
                    row = new PoEntryRow
                    {
                        Key = entry.Key
                    };
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
                    if (row.LanguageToEntry is null || row.LanguageToEntry.Count == 0)
                    {
                        Rows.Remove(row);
                        _keys.Remove(key);
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

    public class PoEntryRow : IDictionary<string, string>
    {
        public POKey Key { get; set; }
        public string Context => Key.ContextId;
        public string MessageId => Key.Id;
        public Dictionary<string, IPOEntry> LanguageToEntry { get; set; } = new();

        public ICollection<string> Keys => LanguageToEntry.Keys;

        public ICollection<string> Values => LanguageToEntry.Values.Select(x => x[0]).ToList();

        public int Count => LanguageToEntry.Count();

        public bool IsReadOnly => true;

        public string this[string key]
        {
            get
            {
                if(LanguageToEntry.TryGetValue(key, out var value))
                {
                    return value[0];
                }
                return null;
            }
            set => throw new NotImplementedException();
        }

        public void Add(string key, string value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            return LanguageToEntry.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
        {
            if (LanguageToEntry.TryGetValue(key, out var entry))
            {
                value = entry[0];
                return true; ;
            }
            value = null;
            return false;
        }

        public void Add(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
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

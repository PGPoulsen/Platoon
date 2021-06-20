using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Karambolo.PO;

namespace Ohayo
{
    public class PoEntryRow : IDictionary<string, string>
    {
        private readonly string _filepath;
        public POKey Key { get; }
        public string Context => Key.ContextId;
        public string MessageId => Key.Id;
        public Dictionary<string, IPOEntry> LanguageToEntry { get; set; } = new();

        public PoEntryRow(POKey key, string filepath)
        {
            _filepath = filepath;
            Key = key;
        }

        public ICollection<SourceEntry> SourceEntries
        {
            get
            {
                var pathBase = Directory.GetParent(_filepath);
                var comments = LanguageToEntry.SelectMany(x => x.Value.Comments.OfType<POReferenceComment>()).ToList();
                var references = comments.SelectMany(x => x.References);
                var uniqueReferences = references.Distinct();
                return uniqueReferences.Select(x => new SourceEntry(x, pathBase)).OrderBy(x => x.ToString()).ToList();
            }
        }

        // Implementation of IDictionary
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
            return LanguageToEntry.GetEnumerator();
        }
    }
}
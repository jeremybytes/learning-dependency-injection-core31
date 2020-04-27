using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonReader.Caching
{
    public class CachingReader : IPersonReader
    {
        private TimeSpan _cacheDuration = new TimeSpan(0, 0, 30);
        private DateTime _dataDateTime;
        private IPersonReader _wrappedReader;
        private IReadOnlyCollection<Person> _cachedItems;

        public CachingReader(IPersonReader wrappedReader)
        {
            _wrappedReader = wrappedReader;
        }

        private bool IsCacheValid
        {
            get
            {
                var _cacheAge = DateTime.Now - _dataDateTime;
                return _cacheAge < _cacheDuration;
            }
        }

        private void ValidateCache()
        {
            if (_cachedItems == null || !IsCacheValid)
            {
                try
                {
                    _cachedItems = _wrappedReader.GetPeople();
                    _dataDateTime = DateTime.Now;
                }
                catch
                {
                    _cachedItems = new List<Person>()
                    {
                        new Person(){ GivenName = "No Data Available", FamilyName = string.Empty, Rating = 0, StartDate = DateTime.Today},
                    };
                }
            }
        }

        private void InvalidateCache()
        {
            _dataDateTime = DateTime.MinValue;
        }

        public IReadOnlyCollection<Person> GetPeople()
        {
            ValidateCache();
            return _cachedItems;
        }

        public Person GetPerson(int id)
        {
            ValidateCache();
            return _cachedItems.FirstOrDefault(p => p.Id == id);
        }
    }
}

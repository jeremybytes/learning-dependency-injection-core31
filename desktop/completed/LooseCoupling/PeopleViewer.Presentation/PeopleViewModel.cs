using Common;
using System.Collections.Generic;
using System.ComponentModel;

namespace PeopleViewer.Presentation
{
    public class PeopleViewModel : INotifyPropertyChanged
    {
        protected IPersonReader Reader;

        private IEnumerable<Person> _people;
        public IEnumerable<Person> People
        {
            get { return _people; }
            set
            {
                if (_people == value)
                    return;
                _people = value;
                RaisePropertyChanged("People");
            }
        }

        public PeopleViewModel(IPersonReader reader)
        {
            Reader = reader;
        }

        public void RefreshPeople()
        {
            People = Reader.GetPeople();
        }

        public void ClearPeople()
        {
            People = new List<Person>();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

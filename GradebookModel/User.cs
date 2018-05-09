using System.Collections.Generic;

namespace GradebookModel
{
    public class User : ModelBase
    {
        #region Fields

        private string username = "";
        private string password = "";
        private string firstName = "";
        private string lastName = "";

        private List<AcademicTerm> terms = new List<AcademicTerm>();

        #endregion

        #region Properties

        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        public IReadOnlyCollection<AcademicTerm> Terms
        {
            get
            {
                return terms.AsReadOnly();
            }
        }

        #endregion

        #region Methods

        public void AddTerm(AcademicTerm term)
        {
            terms.Add(term);
            OnPropertyChanged("Terms");
        }

        public void DeleteTerm(AcademicTerm term)
        {
            terms.Remove(term);
            OnPropertyChanged("Terms");
        }

        #endregion
    }
}

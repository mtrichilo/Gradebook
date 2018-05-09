using GradebookModel;
using System.Collections.ObjectModel;

namespace GradebookViewModel
{
    public class UserViewModel : ViewModelBase
    {
        #region Fields

        private User user;
        private ObservableCollection<AcademicTermViewModel> terms;

        private AcademicTermViewModel selected;

        #endregion

        #region Constructors

        public UserViewModel() : this(new User())
        {

        }

        public UserViewModel(User user)
        {
            this.user = user;

            // TODO: DELETE
            var term1 = new AcademicTerm()
            {
                School = "Northeastern University",
                Term = "Fall",
                Year = 2016
            };
            user.AddTerm(term1);

            terms = new ObservableCollection<AcademicTermViewModel>();
            foreach (var term in user.Terms)
            {
                terms.Add(new AcademicTermViewModel(term));
            }
        }

        #endregion

        #region Properties

        public string Username
        {
            get => user.Username;
            set
            {
                user.Username = value;
                OnPropertyChanged("Username");
            }
        }

        public string Password
        {
            get => user.Password;
        }

        public string FirstName
        {
            get => user.FirstName;
            set
            {
                user.FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get => user.LastName;
            set
            {
                user.LastName = value;
                OnPropertyChanged("LastName");
            }
        }

        public ObservableCollection<AcademicTermViewModel> Terms
        {
            get => terms;
        }

        public AcademicTermViewModel Selected
        {
            get => selected;
            set
            {
                selected = value;
                OnPropertyChanged("Selected");
            }
        }

        public void AddTerm(AcademicTermViewModel term)
        {
            terms.Add(term);
            user.AddTerm(term.AcademicTerm);
            OnPropertyChanged("Terms");
        }

        public void DeleteTerm(AcademicTermViewModel term)
        {
            terms.Remove(term);
            user.DeleteTerm(term.AcademicTerm);
            OnPropertyChanged("Terms");
        }

        #endregion
    }
}

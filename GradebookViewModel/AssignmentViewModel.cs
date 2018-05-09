using GradebookModel;

namespace GradebookViewModel
{
    public class AssignmentViewModel
    {
        #region Fields

        private Assignment assignment;

        #endregion

        #region Constructors 

        public AssignmentViewModel()
        {
            assignment = new Assignment();
        }

        public AssignmentViewModel(Assignment assignment)
        {
            this.assignment = assignment;
        }

        #endregion

        #region Properties

        internal Assignment Assignment
        {
            get => assignment;
        }

        public string Name
        {
            get => assignment.Name;
            set => assignment.Name = value;
        }

        public double Earned
        {
            get => assignment.Earned;
            set => assignment.Earned = value;
        }

        public double Worth
        {
            get => assignment.Worth;
            set => assignment.Worth = value;
        }

        public bool Drop
        {
            get => assignment.Drop;
            set => assignment.Drop = value;
        }

        #endregion
    }
}

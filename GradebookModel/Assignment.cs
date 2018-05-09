using System;

namespace GradebookModel
{
    public class Assignment : GoalBase
    {
        #region Fields

        private int id;
        private string name = "";
        private double earned;
        private double worth;
        private Difficulty difficulty;
        private bool drop;

        #endregion

        #region Constructors

        public Assignment() : base() { }

        public Assignment(int id) : this()
        {
            this.id = id;
        }

        #endregion

        #region Properties

        internal int Id
        {
            get => id;
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public double Earned
        {
            get => earned;
            set
            {
                if (value >= 0)
                {
                    earned = value;
                    OnGradeChanged();
                    OnPropertyChanged("Earned");
                }
            }
        }

        public double Worth
        {
            get => worth;
            set
            {
                if (value > 0)
                {
                    worth = value;
                    OnGradeChanged();
                    OnPropertyChanged("Worth");
                }
            }
        }

        public Difficulty Difficulty
        {
            get => difficulty;
            set => difficulty = value;
        }

        public bool Drop
        {
            get => drop;
            set
            {
                drop = value;
                OnPropertyChanged("Drop");
            }
        }

        #endregion

        #region Events

        public event EventHandler GradeChanged;

        private void OnGradeChanged()
        {
            GradeChanged?.Invoke(this, new EventArgs());
        }

        #endregion

        #region Methods

        protected override void OnGoalModeChanged(object sender, EventArgs e)
        {
            GoalEarned = Earned;
        }

        protected override void OnGoalSelected()
        {
            GoalEarned = Worth;
        }

        #endregion
    }
}

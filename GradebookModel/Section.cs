using System;
using System.Collections.Generic;
using System.Linq;

namespace GradebookModel
{
    public class Section : GoalBase
    {
        #region Fields

        private string name = "";
        private double earned;
        private double weight;

        private List<Assignment> assignments = new List<Assignment>();
        private int dropLowest;

        #endregion

        #region Constructors

        public Section() : base()
        {

        }

        #endregion

        #region Properties

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public double Earned
        {
            get
            {
                return earned;
            }
            protected set
            {
                if (value >= 0)
                {
                    earned = value;
                    OnEarnedChanged();
                    OnPropertyChanged("Earned");
                }
            }
        }

        public double Worth
        {
            get
            {
                return weight * 100;
            }
        }

        public double Weight
        {
            get
            {
                return weight;
            }
            set
            {
                if (value >= 0 && value <= 1)
                {
                    weight = value;
                    OnPropertyChanged("Weight");
                }
            }
        }

        public IReadOnlyCollection<Assignment> Assignments
        {
            get
            {
                return assignments.AsReadOnly();
            }
        }

        public int DropLowest
        {
            get
            {
                return dropLowest;
            }
            set
            {
                if (value >= 0)
                {
                    dropLowest = value;
                    OnPropertyChanged("DropLowest");
                }
            }
        }

        #endregion

        #region Events

        public event EventHandler EarnedChanged;

        private void OnEarnedChanged()
        {
            EarnedChanged?.Invoke(this, new EventArgs());
        }

        #endregion

        #region Methods

        public void AddAssignment(Assignment assignment)
        {
            assignments.Add(assignment);
            assignment.GradeChanged += AssignmentGradeChanged;
            OnPropertyChanged("Assignments");
        }

        public void DeleteAssignment(Assignment assignment)
        {
            assignments.Remove(assignment);
            assignment.GradeChanged -= AssignmentGradeChanged;
            OnPropertyChanged("Assignments");
        }

        private void AssignmentGradeChanged(object sender, EventArgs e)
        {
            var counted = assignments.Where(assignment => !assignment.Drop);
            counted = counted.OrderBy(assignment => assignment.Earned / assignment.Worth);
            counted = counted.Skip(DropLowest);
            Earned = counted.Sum(assignment => assignment.Earned) /
                counted.Sum(assignment => assignment.Worth) * Weight * 100;
        }

        protected override void OnGoalModeChanged(object sender, EventArgs e)
        {
            GoalSelected = true;
            GoalEarned = Earned;
        }

        #endregion
    }
}

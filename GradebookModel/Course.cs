using Simplex;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GradebookModel
{
    public class Course : GoalBase
    {
        #region Fields

        private string deptNumber = "";
        private string name = "";
        private string instructor = "";
        private double earned;

        private List<Section> sections = new List<Section>();

        #endregion

        #region Constructors

        public Course() : base()
        {

        }

        #endregion

        #region Properties

        public string DeptNumber
        {
            get
            {
                return deptNumber;
            }
            set
            {
                deptNumber = value;
                OnPropertyChanged("DeptNumber");
            }
        }

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

        public string Instructor
        {
            get
            {
                return instructor;
            }
            set
            {
                instructor = value;
                OnPropertyChanged("Instructor");
            }
        }

        public double Earned
        {
            get
            {
                return earned;
            }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    earned = value;
                    OnPropertyChanged("Earned");
                }
            }
        }

        public IReadOnlyCollection<Section> Sections
        {
            get
            {
                return sections.AsReadOnly();
            }
        }

        #endregion

        #region Methods

        public void AddSection(Section section)
        {
            if (sections.Sum(s => s.Weight) + section.Weight > 1)
            {
                throw new Exception("Total section weight cannot be greater than 1.");
            }

            sections.Add(section);
            section.EarnedChanged += SectionEarnedChanged;
            OnPropertyChanged("Sections");
        }

        public void DeleteSection(Section section)
        {
            sections.Remove(section);
            section.EarnedChanged -= SectionEarnedChanged;
            OnPropertyChanged("Sections");
        }

        private void SectionEarnedChanged(object sender, EventArgs e)
        {
            Earned = sections.Sum(section => section.Earned);
        }

        protected override void OnGoalModeChanged(object sender, EventArgs e)
        {
            GoalSelected = false;
            GoalEarned = Earned;
        }

        public void CalculateGoals(ObjectiveType objective, params ConstraintType[] constraints)
        {
            var simplex = new SimplexSolver();

            var goalEarned = GoalEarned;
            var objCoefficients = new Dictionary<object, double>();
            var consCoefficients = new Dictionary<object, double>();

            foreach (var section in sections)
            {
                var staticEarned = section.Assignments.Where(a => !a.GoalSelected).Sum(a => a.Earned);
                var totalWorth = section.Assignments.Sum(a => a.Worth);
                goalEarned -= section.Weight * 100 * (staticEarned / totalWorth);

                foreach (var assignment in section.Assignments)
                {
                    if (assignment.GoalSelected)
                    {
                        var objCoefficient = objective == ObjectiveType.Equal ? 1 : section.Weight;
                        objCoefficients.Add(assignment.Id, objCoefficient);

                        var consCoefficient = section.Weight * 100 / totalWorth;
                        consCoefficients.Add(assignment.Id, consCoefficient);

                        var maxEarnedCoeff = new Dictionary<object, double>();
                        maxEarnedCoeff.Add(assignment.Id, 1);
                        simplex.AddConstraint(maxEarnedCoeff, Relationship.LessThanOrEqual, assignment.GoalEarned);
                    }
                }
            }
            simplex.SetObjective(Optimization.Min, objCoefficients);
            simplex.AddConstraint(consCoefficients, Relationship.GreaterThanOrEqual, goalEarned);

            simplex.Solve(out IDictionary<object, double> solution); // 100, 100, 36.923
        }

        #endregion
    }
}

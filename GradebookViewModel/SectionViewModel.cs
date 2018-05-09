using GradebookModel;
using System.Collections.ObjectModel;

namespace GradebookViewModel
{
    public class SectionViewModel
    {
        #region Fields

        private Section section;
        private ObservableCollection<AssignmentViewModel> assignments;

        #endregion

        #region Constructors

        public SectionViewModel(Section section)
        {
            this.section = section;

            // TODO: DELETE
            var a1 = new Assignment(1)
            {
                Name = "Midterm 1",
                Earned = 65,
                Worth = 100
            };
            section.AddAssignment(a1);

            var a2 = new Assignment(2)
            {
                Name = "Midterm 2",
                Earned = 90,
                Worth = 100
            };
            section.AddAssignment(a2);

            var a3 = new Assignment(3)
            {
                Name = "Midterm 3",
                Earned = 0,
                Worth = 100,
                GoalSelected = true
            };
            section.AddAssignment(a3);

            var a4 = new Assignment(4)
            {
                Name = "Final 1",
                Earned = 95,
                Worth = 100
            };
            section.AddAssignment(a4);

            var a5 = new Assignment(5)
            {
                Name = "Final 2",
                Earned = 0,
                Worth = 100,
                GoalSelected = true
            };
            section.AddAssignment(a5);

            var a6 = new Assignment(6)
            {
                Name = "Final 3",
                Earned = 0,
                Worth = 100,
                GoalSelected = true
            };
            section.AddAssignment(a6);

            assignments = new ObservableCollection<AssignmentViewModel>();
            foreach (var assignment in section.Assignments)
            {
                assignments.Add(new AssignmentViewModel(assignment));
            }
        }

        #endregion

        #region Properties

        internal Section Section
        {
            get => section;
        }

        public string Name
        {
            get => section.Name;
            set => section.Name = value;
        }

        public double Earned
        {
            get => section.Earned;
        }

        public double Worth
        {
            get => section.Worth;
        }

        public double Weight
        {
            get => section.Weight;
            set => section.Weight = value;
        }

        public ObservableCollection<AssignmentViewModel> Assignments
        {
            get => assignments;
        }

        public void AddAssignment(AssignmentViewModel assignment)
        {
            assignments.Add(assignment);
            section.AddAssignment(assignment.Assignment);
        }

        public void DeleteAssignment(AssignmentViewModel assignment)
        {
            assignments.Remove(assignment);
            section.DeleteAssignment(assignment.Assignment);
        }

        #endregion
    }
}

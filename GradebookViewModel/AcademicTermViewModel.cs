using GradebookModel;
using System.Collections.ObjectModel;

namespace GradebookViewModel
{
    public class AcademicTermViewModel
    {
        #region Fields

        private AcademicTerm term;
        private ObservableCollection<CourseViewModel> courses;

        #endregion

        #region Constructors

        public AcademicTermViewModel() { }

        public AcademicTermViewModel(AcademicTerm term)
        {
            this.term = term;

            // TODO: DELETE
            var course1 = new Course()
            {
                DeptNumber = "CS 5800",
                Name = "Algorithms and Data",
                Instructor = "Rajmohan Rajaraman",
                GoalModeEnabled = true,
                GoalEarned = 80
            };
            term.AddCourse(course1);

            courses = new ObservableCollection<CourseViewModel>();
            foreach (var course in term.Courses)
            {
                courses.Add(new CourseViewModel(course));
            }
        }

        #endregion

        #region Properties

        internal AcademicTerm AcademicTerm
        {
            get => term;
        }

        public string School
        {
            get => term.School;
            set => term.School = value;
        }

        public string Term
        {
            get => term.Term;
            set => term.Term = value;
        }

        public int Year
        {
            get => term.Year;
            set => term.Year = value;
        }

        public ObservableCollection<CourseViewModel> Courses
        {
            get => courses;
        }

        public void AddCourse(CourseViewModel course)
        {
            courses.Add(course);
            term.AddCourse(course.Course);
        }

        public void DeleteCourse(CourseViewModel course)
        {
            courses.Remove(course);
            term.DeleteCourse(course.Course);
        }

        #endregion
    }
}

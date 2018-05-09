using GradebookModel;
using System.Collections.ObjectModel;

namespace GradebookViewModel
{
    public class CourseViewModel
    {
        #region Fields

        private Course course;
        private ObservableCollection<SectionViewModel> sections;

        #endregion

        #region Constructors

        public CourseViewModel() { }

        public CourseViewModel(Course course)
        {
            this.course = course;

            // TODO: DELETE
            var midterms = new Section()
            {
                Name = "Midterms",
                Weight = .35
            };
            course.AddSection(midterms);

            var final = new Section()
            {
                Name = "Final",
                Weight = .65
            };
            course.AddSection(final);

            sections = new ObservableCollection<SectionViewModel>();
            foreach (var section in course.Sections)
            {
                sections.Add(new SectionViewModel(section));
            }
        }

        #endregion

        #region Properties

        internal Course Course
        {
            get => course;
        }

        public string DeptNumber
        {
            get => course.DeptNumber;
            set => course.DeptNumber = value;
        }

        public string Name
        {
            get => course.Name;
            set => course.Name = value;
        }

        public string Instructor
        {
            get => course.Instructor;
            set => course.Instructor = value;
        }

        public double Earned
        {
            get => course.Earned;
            set => course.Earned = value;
        }

        public ObservableCollection<SectionViewModel> Sections
        {
            get => sections;
        }

        public void AddSection(SectionViewModel section)
        {
            sections.Add(section);
            course.AddSection(section.Section);
        }

        public void DeleteSection(SectionViewModel section)
        {
            sections.Remove(section);
            course.DeleteSection(section.Section);
        }

        #endregion
    }
}

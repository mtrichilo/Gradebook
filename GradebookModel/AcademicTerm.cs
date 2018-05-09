using System;
using System.Collections.Generic;

namespace GradebookModel
{
    public class AcademicTerm : ModelBase
    {
        #region Fields

        private string school = "";
        private string term = "";
        private int year;

        private List<Course> courses = new List<Course>();

        #endregion

        #region Properties

        public string School
        {
            get
            {
                return school;
            }
            set
            {
                school = value;
                OnPropertyChanged("School");
            }
        }

        public string Term
        {
            get
            {
                return term;
            }
            set
            {
                term = value;
                OnPropertyChanged("Term");
            }
        }

        public int Year
        {
            get
            {
                return year;
            }
            set
            {
                if (value >= 1900 && value <= DateTime.Now.Year)
                {
                    year = value;
                    OnPropertyChanged("Year");
                }
            }
        }

        public IReadOnlyCollection<Course> Courses
        {
            get
            {
                return courses.AsReadOnly();
            }
        }

        #endregion

        #region Methods

        public void AddCourse(Course course)
        {
            courses.Add(course);
            OnPropertyChanged("Courses");
        }

        public void DeleteCourse(Course course)
        {
            courses.Remove(course);
            OnPropertyChanged("Courses");
        }

        #endregion
    }
}

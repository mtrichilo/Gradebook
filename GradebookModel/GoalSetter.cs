using Simplex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradebookModel
{
    class GoalSetter : ModelBase
    {
        #region Fields

        private Course course;
        private double goalGrade;

        private IList<Section> calculated = new List<Section>();
        private IDictionary<Section, bool> uncalculated = new Dictionary<Section, bool>();

        #endregion

        #region Constructors

        public GoalSetter(Course course, double goalGrade)
        {
            this.course = course;
            this.goalGrade = goalGrade;
        }

        #endregion

        #region Methods
        
        public void Calculate()
        {
            var staticSections = course.Sections.Where(s => s.Assignments.All(a => !a.GoalSelected)).ToList();
            var dynamicSections = course.Sections.Where(s => s.Assignments.Any(a => a.GoalSelected)).ToList();

            var earned = staticSections.Sum(s => s.Earned);
            var needed = goalGrade - earned;


            var dynamicWeight = dynamicSections.Sum(s => s.Weight);
            if (dynamicWeight < needed)
            {
                // Not possible
            }

            var goalSectionGrade = needed / dynamicWeight * 100;
            foreach (var section in dynamicSections.ToArray())
            {
                var maxedOut = SetGoalGrades(section, goalSectionGrade);
                if (maxedOut)
                {
                    dynamicSections.Remove(section);
                    staticSections.Add(section);
                }
            }
        }

        private bool SetGoalGrades(Section section, double goalGrade)
        {

            var staticPercent = 1;//staticEarned / staticWorth * 100;

            var dynamicAssigments = section.Assignments.Where(a => a.GoalSelected);
            var dynamicWorth = dynamicAssigments.Sum(a => a.Worth);
            var dynamicEarned = (goalGrade - staticPercent) * dynamicWorth / (dynamicAssigments.Count() * 100);

            bool maxedOut;
            dynamicEarned = (maxedOut = dynamicEarned > 100) ? 100 : dynamicEarned;

            foreach (var assignment in dynamicAssigments)
            {
                //assignment.GoalGrade = dynamicEarned;
            }

            return maxedOut;
        }

        private double RecalculateGoalGrade()
        {
            var missing = goalGrade - calculated.Sum(section => section.Earned);
            //goalGrade = missing
            return 0;
        }

        #endregion
    }
}

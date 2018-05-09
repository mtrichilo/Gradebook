using GradebookModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplex;
using System.Collections.Generic;

namespace Test
{
    [TestClass]
    public class Class1
    {
        [TestMethod]
        public void Test1()
        {
            var s = new SimplexSolver();

            var coeff = new Dictionary<object, double>
            {
                { 1, .35 },
                { 2, .65 },
                { 3, .65 }
            };
            s.SetObjective(Optimization.Min, coeff);

            coeff = new Dictionary<object, double>
            {
                { 1, .116667 },
                { 2, .216667 },
                { 3, .216667 }
            };
            s.AddConstraint(coeff, Relationship.GreaterThanOrEqual, 43.5);

            coeff = new Dictionary<object, double> { { 1, 1 } };
            s.AddConstraint(coeff, Relationship.LessThanOrEqual, 95);

            coeff = new Dictionary<object, double> { { 2, 1 } };
            s.AddConstraint(coeff, Relationship.LessThanOrEqual, 95);

            coeff = new Dictionary<object, double> { { 3, 1 } };
            s.AddConstraint(coeff, Relationship.LessThanOrEqual, 90);

            coeff = new Dictionary<object, double>
            {
                { 2, 1 },
                { 3, -1 }
            };
            s.AddConstraint(coeff, Relationship.Equal, 0);

            s.Solve(out IDictionary<object, double> solution);
        }

        [TestMethod]
        public void Test2()
        {
            //var s = new SimplexSolver();
            //s.SetObjective(Optimization.Min, new[]
            //{
            //    1.0, 1, 1
            //});

            //s.AddConstraint(new[] { .116667, .216667, .216667 }, Relationship.GreaterThanOrEqual, 43.5);
            //s.AddConstraint(new[] { 1.0, 0, 0 }, Relationship.LessThanOrEqual, 70);
            //s.AddConstraint(new[] { 0, 1.0, 0 }, Relationship.LessThanOrEqual, 70);
            //s.AddConstraint(new[] { 0, 0, 1.0 }, Relationship.LessThanOrEqual, 70);

            //s.Solve(out IList<double> solution);
        }

        [TestMethod]
        public void Test3()
        {
            var term = new AcademicTerm()
            {
                School = "Northeastern University",
                Term = "Fall",
                Year = 2016
            };

            var course = new Course()
            {
                DeptNumber = "CS 5800",
                Name = "Algorithms and Data",
                Instructor = "Rajmohan Rajaraman",
                GoalModeEnabled = true,
                GoalEarned = 80
            };
            term.AddCourse(course);

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

            var a1 = new Assignment(1)
            {
                Name = "Midterm 1",
                Earned = 65,
                Worth = 100
            };
            midterms.AddAssignment(a1);

            var a2 = new Assignment(2)
            {
                Name = "Midterm 2",
                Earned = 90,
                Worth = 100
            };
            midterms.AddAssignment(a2);

            var a3 = new Assignment(3)
            {
                Name = "Midterm 3",
                Earned = 0,
                Worth = 100,
                GoalSelected = true
            };
            midterms.AddAssignment(a3);

            var a4 = new Assignment(4)
            {
                Name = "Final 1",
                Earned = 95,
                Worth = 100
            };
            final.AddAssignment(a4);

            var a5 = new Assignment(5)
            {
                Name = "Final 2",
                Earned = 0,
                Worth = 100,
                GoalSelected = true
            };
            final.AddAssignment(a5);

            var a6 = new Assignment(6)
            {
                Name = "Final 3",
                Earned = 0,
                Worth = 100,
                GoalSelected = true
            };
            final.AddAssignment(a6);

            course.CalculateGoals(ObjectiveType.Weighted);
        }
    }
}

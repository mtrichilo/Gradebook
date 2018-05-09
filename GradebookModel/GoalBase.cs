using System;

namespace GradebookModel
{
    public abstract class GoalBase : ModelBase
    {
        #region Fields

        private static bool goalModeEnabled;
        private bool goalSelected;
        private double goalEarned;

        #endregion

        #region Constructors

        public GoalBase()
        {
            GoalModeChanged += OnGoalModeChanged;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Whether or not goal mode is enabled. 
        /// This is a static value for all derived classes.
        /// </summary>
        public bool GoalModeEnabled
        {
            get
            {
                return goalModeEnabled;
            }
            set
            {
                goalModeEnabled = value;
                OnGoalModeChanged();
                OnPropertyChanged("GoalModeEnabled");
            }
        }

        /// <summary>
        /// Whether or not this grade is selected to be overriden.
        /// </summary>
        public bool GoalSelected
        {
            get
            {
                return goalSelected;
            }
            set
            {
                goalSelected = value;
                OnGoalSelected();
                OnPropertyChanged("GoalSelected");
            }
        }

        /// <summary>
        /// The number of points earned.
        /// Will be overriden if GoalSelected is true.
        /// </summary>
        public double GoalEarned
        {
            get
            {
                return goalEarned;
            }
            set
            {
                if (value >= 0)
                {
                    goalEarned = value;
                    OnPropertyChanged("GoalEarned");
                }
            }
        }

        #endregion

        #region Events

        private static event EventHandler GoalModeChanged;

        private void OnGoalModeChanged()
        {
            GoalModeChanged?.Invoke(this, new EventArgs());
        }

        #endregion

        #region Methods

        protected abstract void OnGoalModeChanged(object sender, EventArgs e);

        protected virtual void OnGoalSelected() { }

        #endregion
    }
}

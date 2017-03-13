﻿namespace System.Waf.Applications
{
    /// <summary>
    /// Abstract base class for a ViewModel implementation.
    /// </summary>
    public abstract class ViewModel : ViewModel<IView>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class and attaches itself as DataContext to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        protected ViewModel(IView view) : base(view)
        {    
        }
    }
}

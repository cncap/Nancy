﻿namespace Nancy.Testing
{
    using Nancy.ViewEngines;

    /// <summary>
    /// A view factory decorator, aimed for test,
    /// that exposes some interesting properties about the generated view
    /// </summary>
    public class TestingViewFactory : IViewFactory
    {
        private readonly DefaultViewFactory decoratedViewFactory;


        /// <summary>
        /// Creates the view based on the view factory sent to the constructor
        /// </summary>
        /// <param name="viewFactory">the view factory that is decorated</param>
        public TestingViewFactory(DefaultViewFactory viewFactory)
        {
            this.decoratedViewFactory = viewFactory;
        }

        /// <summary>
        /// Renders the view and then call into the viewfactory 
        /// that the TestingViewFactory is decorating
        /// </summary>
        /// <param name="viewName">The name of the view to render.</param>
        /// <param name="model">The module path of the module that is rendering the view.</param>
        /// <param name="viewLocationContext">A <see cref="ViewLocationContext"/> instance, containing information about the context for which the view is being rendered.</param>
        /// <returns>A response.</returns>
        public Response RenderView(string viewName, dynamic model, ViewLocationContext viewLocationContext)
        {
            // Intercept and store interesting stuff
            viewLocationContext.Context.Items[TestingViewContextKeys.VIEWMODEL] = model;
            viewLocationContext.Context.Items[TestingViewContextKeys.VIEWNAME] = viewName;
            viewLocationContext.Context.Items[TestingViewContextKeys.MODULENAME] = viewLocationContext.ModuleName;
            viewLocationContext.Context.Items[TestingViewContextKeys.MODULEPATH] = viewLocationContext.ModulePath;

            return this.decoratedViewFactory.RenderView(viewName, model, viewLocationContext);
        }
    }

    /// <summary>
    /// The key names for where the testing view context data is stored
    /// </summary>
    public static class TestingViewContextKeys
    {
        /// <summary>
        ///  The key in ViewLocationContext.Item for the view model
        /// </summary>
        public const string VIEWMODEL = "__Nancy_Testing_ViewModel";
        /// <summary>
        ///  The key in ViewLocationContext.Item for the view name
        /// </summary>
        public const string VIEWNAME = "__Nancy_Testing_ViewName";
        /// <summary>
        ///  The key in ViewLocationContext.Item for the model name
        /// </summary>
        public const string MODULENAME = "__Nancy_Testing_ModuleName";
        /// <summary>
        ///  The key in ViewLocationContext.Item for the module path        
        /// </summary>
        public const string MODULEPATH = "__Nancy_Testing_ModulePath";
    }
}

using ARMDesktopUI.ViewModels;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ARMDesktopUI
{
    public class Bootstrapper : BootstrapperBase
    {
        /// <summary>
        /// (dependency injecton) handle instantiation of all classses
        /// </summary>
        private SimpleContainer _container = new SimpleContainer();
        public Bootstrapper()
        {
            Initialize();

        }

        #region 
        /// <summary>
        /// when you ask for container instance, it returns the instance to you
        /// it holds an isntance of itself
        /// </summary>
        protected override void Configure()
        {
            _container.Instance(_container);

            //window manager: brings windows in and out
            //event aggregator: handle event throughout application
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();

            //looks for all classes that end in ViewModel, create a list
            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));
        }

        /// <summary>
        /// on startup looks for the shellviewmodel which looks for the shellview.xaml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewForAsync<ShellViewModel>();
        }

        /// <summary>
        /// pass in type and name and get that instance using _container for instantiation
        /// </summary>
        /// <param name="service"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        /// <summary>
        /// get all instances of the _container
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        #endregion
    }
}

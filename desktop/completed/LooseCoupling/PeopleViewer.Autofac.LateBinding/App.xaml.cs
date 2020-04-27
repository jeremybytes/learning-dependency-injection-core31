﻿using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using PeopleViewer.Presentation;
using System;
using System.Runtime.Loader;
using System.Windows;

namespace PeopleViewer.Autofac.LateBinding
{
    public partial class App : Application
    {
        IContainer Container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            Application.Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("autofac.json");

            var configuration = configBuilder.Build();
            LoadAssembly(configuration);

            var module = new ConfigurationModule(configuration);
            var builder = new ContainerBuilder();
            builder.RegisterModule(module);

            builder.RegisterType<MainWindow>().InstancePerDependency();
            builder.RegisterType<PeopleViewModel>().InstancePerDependency();

            Container = builder.Build();
        }

        private void ComposeObjects()
        {
            Application.Current.MainWindow = Container.Resolve<MainWindow>();
        }

        private static void LoadAssembly(IConfigurationRoot configuration)
        {
            // This is a helper method to load an assembly from the file system.
            // With .NET Core, if the assembly is not loaded, Autofac cannot find
            // it (not sure why). The same is true when using Type.GetType with
            // a fully-qualified assembly name.
            var section = configuration.GetSection("defaultAssembly");
            var assemblyName = section.Value + ".dll";
            var assemblyPath = AppDomain.CurrentDomain.BaseDirectory + assemblyName;
            AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
        }
    }
}

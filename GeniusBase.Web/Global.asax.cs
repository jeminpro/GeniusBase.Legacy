﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using GeniusBase.Dal;
using GeniusBase.Dal.Repository;
using GeniusBase.Web.Business.ApplicationSettings;
using GeniusBase.Web.Business.Articles;
using GeniusBase.Web.Business.Categories;
using GeniusBase.Web.ViewEngines;
using Microsoft.AspNet.SignalR;
using AutofacDependencyResolver = Autofac.Integration.Mvc.AutofacDependencyResolver;

namespace GeniusBase.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();            
            builder.RegisterType<CategoryFactory>().As<ICategoryFactory>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<TagRepository>().As<ITagRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<ArticleRepository>().As<IArticleRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<ArticleFactory>().As<IArticleFactory>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<UserRepository>().As<IUserRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<SettingsRepository>().As<ISettingsRepository>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<SettingsFactory>().As<ISettingsFactory>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<SettingsService>().As<ISettingsService>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();            
            var container = builder.Build();                  
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            System.Web.Mvc.ViewEngines.Engines.Clear();
            System.Web.Mvc.ViewEngines.Engines.Add(new GeniusBaseViewEngine());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
    }
}
using Autofac;
using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.MobileDomain;
using BudgetUnderControl.Mobile.Extensions;
using BudgetUnderControl.Mobile.Keys;
using BudgetUnderControl.Mobile.PlatformSpecific;
using BudgetUnderControl.Mobile.Services;
using BudgetUnderControl.Mobile.ViewModels;
using BudgetUnderControl.ViewModel;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using BudgetUnderControl.CommonInfrastructure.Settings;
using BudgetUnderControl.CommonInfrastructure.Commands;
using System.Reflection;
using BudgetUnderControl.CommonInfrastructure;
using BudgetUnderControl.MobileDomain.Repositiories;
using BudgetUnderControl.Mobile.Repositories;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace BudgetUnderControl.Mobile.IoC
{
    public class MobileModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            var settings = this.GetSettings<GeneralSettings>("generalsettings.json");
            builder.RegisterInstance(settings)
              .SingleInstance();

            var dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath(settings.BUC_DB_Name);

            var assembly = typeof(MobileModule)
            .GetTypeInfo()
            .Assembly;

            var commonInfrastructureAssembly = typeof(AddAccount)
            .GetTypeInfo()
            .Assembly;

            builder.RegisterAssemblyTypes(commonInfrastructureAssembly)
                  .AsClosedTypesOf(typeof(ICommandHandler<>))
                  .InstancePerLifetimeScope();

            builder.RegisterType<CommandDispatcher>()
                .As<ICommandDispatcher>()
                .InstancePerLifetimeScope();

            builder.Register<Func<IUserIdentityContext>>(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return () =>
                {
                    var service = context.Resolve<IUserService>();
                    var identity = service.CreateUserIdentityContext();
                    return identity;
                };
            });

            builder.Register<IUserIdentityContext>(c =>
            {
                return c.Resolve<Func<IUserIdentityContext>>()();
            });

            builder.RegisterInstance(new ContextConfig() { DbName = settings.BUC_DB_Name, DbPath = dbPath, Application = ApplicationType.Mobile , ConnectionString = "Filename=" }).As<IContextConfig>();
            builder.RegisterInstance(new MemoryCache(new MemoryCacheOptions() {  })).As<IMemoryCache>();
            builder.RegisterType<BaseModel>().As<IBaseModel>().InstancePerLifetimeScope();
            builder.RegisterType<AccountMobileService>().As<IAccountMobileService>().InstancePerLifetimeScope();
            builder.RegisterType<AccountMobileService>().As<IAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyService>().As<ICurrencyService>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<AccountGroupService>().As<IAccountGroupService>().InstancePerLifetimeScope();
            builder.RegisterType<TransactionService>().As<ITransactionService>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyRepository>().As<ICurrencyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AccountMobileRepository>().As<IAccountMobileRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AccountGroupRepository>().As<IAccountGroupRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TransactionRepository>().As<ITransactionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TagRepository>().As<ITagRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FileRepository>().As<IFileRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SyncService>().As<ISyncService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<TagService>().As<ITagService>().InstancePerLifetimeScope();
            builder.RegisterType<TestDataSeeder>().As<ITestDataSeeder>().InstancePerLifetimeScope();
            builder.RegisterType<SynchronizationRepository>().As<ISynchronizationRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MobileSynchroniser>().As<ISynchroniser>().InstancePerLifetimeScope();
            builder.RegisterType<SyncRequestMobileBuilder>().As<ISyncRequestBuilder>().InstancePerLifetimeScope();

            builder.RegisterInstance<IFileHelper>(DependencyService.Get<IFileHelper>());
            builder.RegisterInstance<ILogManager>(DependencyService.Get<ILogManager>());
            builder.RegisterInstance<ILogger>(DependencyService.Get<ILogManager>().GetLog());
            builder.RegisterInstance<ILocalNotificationService>(DependencyService.Get<ILocalNotificationService>());
            builder.RegisterType<ContextFacade>().As<IContextFacade>().SingleInstance();
            builder.RegisterType<EditAccountViewModel>().As<IEditAccountViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<AddAccountViewModel>().As<IAddAccountViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<AccountsViewModel>().As<IAccountsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<AccountDetailsViewModel>().As<IAccountDetailsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<TransactionsViewModel>().As<ITransactionsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<AddTransactionViewModel>().As<IAddTransactionViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<EditTransactionViewModel>().As<IEditTransactionViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<SettingsViewModel>().As<ISettingsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<OverviewViewModel>().As<IOverviewViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<SyncMobileService>().As<ISyncMobileService>().InstancePerLifetimeScope();
            builder.RegisterType<LoginMobileService>().As<ILoginMobileService>().InstancePerLifetimeScope();
            builder.RegisterType<LoginViewModel>().As<ILoginViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<TagViewModel>().As<ITagViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyViewModel>().As<ICurrencyViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<IconService>().As<IIconService>().InstancePerLifetimeScope();
            builder.RegisterType<FiltersViewModel>().As<IFiltersViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<BalanceService>().As<IBalanceService>().InstancePerLifetimeScope();
            builder.RegisterType<ChartsViewModel>().As<IChartsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryReportService>().As<ICategoryReportService>().InstancePerLifetimeScope();
            builder.RegisterType<ExpensesReportService>().As<IExpensesReportService>().InstancePerLifetimeScope();

            var url = Preferences.Get(PreferencesKeys.APIURL, string.Empty);
            var apiUrl = string.IsNullOrEmpty(url) || string.IsNullOrWhiteSpace(url) ? settings.ApiBaseUri : url;
            builder.Register(ctx => new HttpClient() { BaseAddress = new Uri(apiUrl), Timeout = TimeSpan.FromMinutes(5)
        })
            .Named<HttpClient>("api")
            .SingleInstance();

        }

    }
}

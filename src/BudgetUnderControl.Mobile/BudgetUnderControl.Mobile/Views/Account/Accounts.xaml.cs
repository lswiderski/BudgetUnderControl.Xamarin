﻿using Autofac;
using BudgetUnderControl.Infrastructure;
using BudgetUnderControl.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Accounts : ContentPage
    {
        private FloatingActionButtonView fab;

        IAccountsViewModel vm;
        public Accounts()
        {
            InitializeComponent();
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<IAccountsViewModel>();
            }

            InitFAB();


        }

        private void InitFAB()
        {
            fab = new FloatingActionButtonView()
            {
                ImageName = "ic_add.png",
                ColorNormal = Color.FromHex("ff3498db"),
                ColorPressed = Color.Black,
                ColorRipple = Color.FromHex("ff3498db"),
                Clicked = OnAddButtonClicked
            };
         
            // Overlay the FAB in the bottom-right corner
            AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(fab, new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            absoluteLayout.Children.Add(fab);
        }

        protected async void OnAddButtonClicked(object sender, EventArgs args)
        {
            var addAccount = new AddAccount();
            await Navigation.PushModalAsync(addAccount);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm.LoadAccounts();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Guid accountId = vm.SelectedAccount.ExternalId;
            App.MasterPage.NavigateTo(typeof(AccountDetails), accountId);
        }

    }
}
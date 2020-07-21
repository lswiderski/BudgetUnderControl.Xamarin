using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.Mobile.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.CommonInfrastructure;

namespace BudgetUnderControl.ViewModel
{
    public class AddAccountViewModel : IAddAccountViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        IAccountService accountService;
        ICurrencyService currencyService;
        IAccountGroupService accountGroupService;
        ICommandDispatcher commandDispatcher;
        IIconService iconService;

        List<CurrencyDTO> currencies;
        List<AccountGroupItemDTO> accountGroups;
        
        public List<AccountGroupItemDTO> AccountGroups => accountGroups;
        public List<CurrencyDTO> Currencies => currencies;
        
        List<AccountTypeDTO> accountTypes;
        public List<AccountTypeDTO> AccountTypes => accountTypes;
        List<AccountListItemDTO> accounts;
        public List<AccountListItemDTO> Accounts => accounts;

        private IconDto currentIcon;
        List<SelectIconDto> icons;
        public List<SelectIconDto> Icons => icons;

        public SelectIconDto selectedIcon;
        public SelectIconDto SelectedIcon
        {
            get
            {
                return selectedIcon;
            }
            set
            {
                if (selectedIcon != value)
                {
                    selectedIcon = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIcon)));
                }

            }
        }

        int selectedCurrencyIndex;
        public int SelectedCurrencyIndex
        {
            get
            {
                return selectedCurrencyIndex;
            }
            set
            {
                if (selectedCurrencyIndex != value)
                {
                    selectedCurrencyIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCurrencyIndex)));
                }

            }
        }

        int selectedAccountGroupIndex;

        public int SelectedAccountGroupIndex
        {
            get
            {
                return selectedAccountGroupIndex;
            }
            set
            {
                if (selectedAccountGroupIndex != value)
                {
                    selectedAccountGroupIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAccountGroupIndex)));
                }

            }
        }

        int selectedAccountTypeIndex;
        public int SelectedAccountTypeIndex
        {
            get
            {
                return selectedAccountTypeIndex;
            }
            set
            {
                if (selectedAccountTypeIndex != value)
                {
                    selectedAccountTypeIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAccountTypeIndex)));
                }

            }
        }

        int selectedAccountIndex;
        public int SelectedAccountIndex
        {
            get
            {
                return selectedAccountIndex;
            }
            set
            {
                if (selectedAccountIndex != value)
                {
                    selectedAccountIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAccountIndex)));
                }

            }
        }


        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        private string number;
        public string Number
        {
            get => number;
            set
            {
                if (number != value)
                {
                    number = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Number)));
                }
            }
        }

        private string comment;
        public string Comment
        {
            get => comment;
            set
            {
                if (comment != value)
                {
                    comment = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Comment)));
                }
            }
        }

        private bool isInTotal;
        public bool IsInTotal
        {
            get => isInTotal;
            set
            {
                if (isInTotal != value)
                {
                    isInTotal = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsInTotal)));
                }
            }
        }

        private string amount;
        public string Amount
        {
            get => amount;
            set
            {
                if (amount != value)
                {
                    amount = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
                }
            }
        }

        private string order;
        public string Order
        {
            get => order;
            set
            {
                if (order != value)
                {
                    order = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Order)));
                }
            }
        }

        public AddAccountViewModel(IAccountService accountService, ICurrencyService currencyService, 
            IAccountGroupService accountGroupService, IIconService iconService,
            ICommandDispatcher commandDispatcher)
        {
            this.accountService = accountService;
            this.currencyService = currencyService;
            this.accountGroupService = accountGroupService;
            this.iconService = iconService;
            this.commandDispatcher = commandDispatcher;
            GetDropdowns();

            selectedAccountIndex = -1;
        }

        async void GetDropdowns()
        {
            currencies = (await currencyService.GetCurriencesAsync()).OrderBy(x => x.Code).ToList();
            accountGroups = (await accountGroupService.GetAccountGroupsAsync()).ToList();
            accounts = (await accountService.GetAccountsWithBalanceAsync()).ToList();
            accountTypes = this.GetAccountTypes().ToList();
            icons = this.iconService.GetAvailableAccountIcons();

        }

        public async Task AddAccount()
        {
            decimal value;
            int _order = 0;
            if(!int.TryParse(order, out _order))
            {
                _order = 0;
            }
            decimal.TryParse(amount.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value);
            var command = new AddAccount
            {
                Name = Name,
                Number = Number,
                Comment = Comment,
                Amount = value,
                Order = _order,
                AccountGroupId = AccountGroups[SelectedAccountGroupIndex].Id,
                CurrencyId = Currencies[SelectedCurrencyIndex].Id,
                IsIncludedInTotal = IsInTotal,
                Type = (AccountType)AccountTypes[SelectedAccountTypeIndex].Id,
                ParentAccountId = selectedAccountIndex > -1 ? Accounts[SelectedAccountIndex].Id : (int?)null,
                Icon = selectedIcon?.Id,
            };

            using (var scope = App.Container.BeginLifetimeScope())
            {
                await commandDispatcher.DispatchAsync(command, scope);
            }
        }

        public void ClearParentAccountCombo()
        {
            SelectedAccountIndex = -1;
        }

        private ICollection<AccountTypeDTO> GetAccountTypes()
        {
            var collection = new List<AccountTypeDTO>();
            collection.Add(new AccountTypeDTO { Id = (int)AccountType.Account, Name = AccountType.Account.ToString() });
            collection.Add(new AccountTypeDTO { Id = (int)AccountType.Wallet, Name = AccountType.Wallet.ToString() });
            collection.Add(new AccountTypeDTO { Id = (int)AccountType.Card, Name = AccountType.Card.ToString() });
            return collection;
        }
    }
}

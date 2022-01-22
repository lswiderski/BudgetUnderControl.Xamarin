using BudgetUnderControl.Common.Contracts;
using Syncfusion.XForms.Border;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BudgetUnderControl.Views
{
    class GroupedTransactionListItem : ViewCell
    {

        public static readonly BindableProperty TransactionProperty = BindableProperty.Create("GroupedTransaction", typeof(TransactionListItemDTO), typeof(GroupedTransactionListItem));

        Label nameLabel, accountLabel, valueLabel, tagsLabel, categoryLabel, dayLabel, monthLabel, categoryIconLabel, accountIconLabel;

        public TransactionListItemDTO Transaction
        {
            get { return (TransactionListItemDTO)GetValue(TransactionProperty); }
            set { SetValue(TransactionProperty, value); }
        }

        public GroupedTransactionListItem()
        {
            var frame = new Frame
            {
                Margin = new Thickness(10, 5),
                Padding = new Thickness(5),
                Style = (Style)Application.Current.Resources["BaseFrame"]
            };
            var grid = new Grid { Padding = new Thickness(1), VerticalOptions = LayoutOptions.FillAndExpand };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.7, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            nameLabel = new Label
            {
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Style = (Style)Application.Current.Resources["BaseLabel"]
            };
            accountLabel = new Label
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = 10,
                Style = (Style)Application.Current.Resources["BaseLabel"]
            };
            valueLabel = new Label
            {
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Style = (Style)Application.Current.Resources["BaseLabel"]
            };
            tagsLabel = new Label
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = 10,
                Style = (Style)Application.Current.Resources["BaseLabel"]
            };
            categoryLabel = new Label
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = 10,
                Style = (Style)Application.Current.Resources["BaseLabel"]
            };

            dayLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 16,
                VerticalTextAlignment = TextAlignment.End,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontAttributes = FontAttributes.Bold,
                Style = (Style)Application.Current.Resources["BaseLabel"]
            };
            monthLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 10,
                VerticalTextAlignment = TextAlignment.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Style = (Style)Application.Current.Resources["BaseLabel"]
            };

            var datestack = new StackLayout { Orientation = StackOrientation.Vertical, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
            datestack.Children.Add(dayLabel);
            datestack.Children.Add(monthLabel);

            var boxView = new BoxView { WidthRequest = 1, BackgroundColor = Color.LightGray, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.End };
            grid.Children.Add(datestack, 0, 0);
            grid.Children.Add(boxView, 0, 0);
            Grid.SetRowSpan(datestack, 2);
            Grid.SetRowSpan(boxView, 2);

            grid.Children.Add(nameLabel, 1, 0);

            categoryIconLabel = new Label
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = 10,
                Style = (Style)Application.Current.Resources["BaseLabel"]
            };
            accountIconLabel = new Label
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = 10,
                Style = (Style)Application.Current.Resources["BaseLabel"]
            };
            var accountCategoryStack = new StackLayout { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Start };
            accountCategoryStack.Children.Add(accountIconLabel);
            accountCategoryStack.Children.Add(accountLabel);
            accountCategoryStack.Children.Add(new Label
            {
                Text = "/",
                Style = (Style)Application.Current.Resources["BaseLabel"]
            });
            accountCategoryStack.Children.Add(categoryIconLabel);
            accountCategoryStack.Children.Add(categoryLabel);

            var middleStack = new StackLayout { Orientation = StackOrientation.Vertical, VerticalOptions = LayoutOptions.Start };
            middleStack.Children.Add(accountCategoryStack);
            middleStack.Children.Add(tagsLabel);
            grid.Children.Add(middleStack, 1, 1);

            grid.Children.Add(valueLabel, 2, 0);
            Grid.SetRowSpan(valueLabel, 2);
            frame.Content = grid;
            View = frame;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                var value = Transaction.ValueWithCurrency;
                if (Transaction.Value < 0)
                {
                    value = value.Remove(0, 1);
                }

                categoryIconLabel.Text = Transaction.CategoryIcon?.Glyph;
                categoryIconLabel.FontFamily = Transaction.CategoryIcon?.FontFamily;
                accountIconLabel.Text = Transaction.AccountIcon?.Glyph;
                accountIconLabel.FontFamily = Transaction.AccountIcon?.FontFamily;
                dayLabel.Text = Transaction.Date.Day.ToString("00");
                monthLabel.Text = Transaction.Date.ToString("MMM");
                nameLabel.Text = Transaction.Name;
                accountLabel.Text = Transaction.Account;
                valueLabel.Text = value;
                valueLabel.TextColor = Transaction.Type == Common.Enums.TransactionType.Income ? Color.Green : Color.Red;
                categoryLabel.Text = Transaction.Category;
                tagsLabel.Text = string.Join(", ", Transaction.Tags.Select(x => "#" + x.Name));
                if (string.IsNullOrWhiteSpace(tagsLabel.Text))
                {
                    tagsLabel.IsVisible = false;
                }
            }
        }
    }
}

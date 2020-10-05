using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Goals : ContentPage
    {
        public Goals()
        {
            InitializeComponent();
        }

        protected async void OnAddButtonClicked(object sender, EventArgs args)
        {
            var addGoal = new AddGoal();
            await Navigation.PushModalAsync(addGoal);
        }
    }
}
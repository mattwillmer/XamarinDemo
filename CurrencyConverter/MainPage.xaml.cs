using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CurrencyConverter
{
    public partial class MainPage : ContentPage
    {
        List<ConversionRate> rates;

        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            decimal amount;
            decimal.TryParse(txtAmount.Text, out amount);

            ConversionRate inputRate = rates.Where(x => x.Currency == cboInputCurrency.SelectedItem.ToString()).Single();
            ConversionRate outputRate = rates.Where(x => x.Currency == cboOutputCurrency.SelectedItem.ToString()).Single();

            decimal convertedAmount = (amount * inputRate.Rate) / outputRate.Rate;
            lblResult.Text = convertedAmount.ToString("0.00");
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            HttpClient client = new HttpClient();
            string response = await client.GetStringAsync("https://mattxamarindemo.azurewebsites.net/ConversionRates");
            rates = JsonConvert.DeserializeObject<List<ConversionRate>>(response);
            foreach(ConversionRate rate in rates)
            {
                cboInputCurrency.Items.Add(rate.Currency);
                cboOutputCurrency.Items.Add(rate.Currency);
            }
        }
    }

    public class ConversionRate {
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        }
}

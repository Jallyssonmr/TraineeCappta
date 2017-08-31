﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VendeBemVeiculos_Patterns.Controlers;
using VendeBemVeiculos_Patterns.Domain;
using VendeBemVeiculos_Patterns.Interfaces;
using VendeBemVeiculos_Patterns.Reporters;

namespace VendeBemVeiculos_Patterns
{
    public partial class FMain : Form
    {
        private const string INCOMPLETE_FIELDS_MESSAGE = "Preencha todos os campos que contém asterísco (*)";
        private const string INVALID_DATA = "Dados invalidos.";

        private const int VEHICLE_SEARCH = 0;
        private const int BACKSPACE_ASCII = 8;

        private const string BRAND = "brand";
        private const string MODEL = "model";
        private const string YEAR = "year";
        private const string COLOR = "color";
        private const string QUANTITY = "quantity";

        private const string NAME = "name";
        private const string RG_DOCUMENT = "rgDocument";
        private const string CPF_DOCUMENT = "cpfDocument";
        private const string ADDRESS = "address";
        private const string PHONE_NUMBER = "phoneNumber";
        private const string SALARY = "salary";

        private ClientsControl clientsControl;
        private SellersControl sellersControl;
        private StockControl stockControl;
        private SalesControl salesControl;

        private IReport reporter;

        public FMain()
        {
            this.clientsControl = new ClientsControl();
            this.sellersControl = new SellersControl();
            this.stockControl = new StockControl();
            this.salesControl = new SalesControl();

            InitializeComponent();
        }
        #region vehicles
        private void RuttonRegisterVehicleToStockClick(object sender, EventArgs e)
        {
            if (this.AnyNewVehicleDataFiedIsEmpty())
            {
                this.MessageMissingData();
                return;
            }
            var valuesVehicle = this.GetValuesFieldsNewVevhicle();
            var vehicle = new Vehicle(valuesVehicle[BRAND], valuesVehicle[MODEL], int.Parse(valuesVehicle[YEAR]), valuesVehicle[COLOR]);
            this.stockControl.Add(vehicle, int.Parse(valuesVehicle[QUANTITY]));
            this.CleanNewVehicleDataFields();
        }

        private bool AnyNewVehicleDataFiedIsEmpty()
        {
            return string.IsNullOrEmpty(this.textBoxModelNewVehicle.Text) ||
                string.IsNullOrEmpty(this.textBoxQuantityNewVehicle.Text) ||
                string.IsNullOrEmpty(this.comboBoxBrandNewVehicle.Text) ||
                string.IsNullOrEmpty(this.comboBoxYearNewVehicle.Text) ||
                string.IsNullOrEmpty(this.comboBoxColorNewVehicle.Text);
        }

        private IDictionary<string, string> GetValuesFieldsNewVevhicle()
        {
            var valuesVehicles = new Dictionary<string, string>();

            valuesVehicles.Add(BRAND, this.comboBoxBrandNewVehicle.Text.ToUpper());
            valuesVehicles.Add(MODEL, this.textBoxModelNewVehicle.Text.ToUpper());
            valuesVehicles.Add(YEAR, this.comboBoxYearNewVehicle.Text);
            valuesVehicles.Add(COLOR, this.comboBoxColorNewVehicle.Text.ToUpper());
            valuesVehicles.Add(QUANTITY, this.textBoxQuantityNewVehicle.Text);
            return valuesVehicles;
        }

        private void ButtonCancelNewVehicleClick(object sender, EventArgs e)
        {
            this.CleanNewVehicleDataFields();
        }

        private void CleanNewVehicleDataFields()
        {
            this.textBoxModelNewVehicle.Clear();
            this.textBoxQuantityNewVehicle.Clear();

            this.comboBoxBrandNewVehicle.Text = string.Empty;
            this.comboBoxYearNewVehicle.Text = string.Empty;
            this.comboBoxColorNewVehicle.Text = string.Empty;
        }

        private void ButtonStockReportClick(object sender, EventArgs e)
        {
            this.reporter = new StockReport(this.stockControl);
            this.reporter.Report();
        }

        private void ComboBoxYearNewVehicleKeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnlyNumbers(e);
        }

        private void TextBoxQuantityNewVehicleKeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnlyNumbers(e);
        }
        #endregion

        #region sales

        #endregion

        #region clients
        private void ButtonRegisterClientClick(object sender, EventArgs e)
        {
            if (this.AnyNewClientDataFieldIsEmpty())
            {
                this.MessageMissingData();
                return;
            }
            var valuesClient = this.GetValuesFieldsNewClient();
            var client = new Person(valuesClient[NAME], valuesClient[CPF_DOCUMENT], valuesClient[PHONE_NUMBER])
            {
                RGDocument = valuesClient[RG_DOCUMENT],
                Address = valuesClient[ADDRESS]
            };
            this.clientsControl.Add(client);
            this.CleanFieldsInputClient();
        }

        private bool AnyNewClientDataFieldIsEmpty()
        {
            return string.IsNullOrEmpty(this.textBoxInputNameClients.Text) ||
                string.IsNullOrEmpty(this.textBoxInputCpfDocumentClients.Text) ||
                string.IsNullOrEmpty(this.textBoxInputPhoneNumberClients.Text);
        }

        private IDictionary<string, string> GetValuesFieldsNewClient()
        {
            var valuesClients = new Dictionary<string, string>();
            valuesClients.Add(NAME, this.textBoxInputNameClients.Text.ToUpper());
            valuesClients.Add(RG_DOCUMENT, this.textBoxInputRgDocumentClients.Text.ToUpper());
            valuesClients.Add(CPF_DOCUMENT, this.textBoxInputCpfDocumentClients.Text);
            valuesClients.Add(ADDRESS, this.textBoxInputAddressClients.Text.ToUpper());
            valuesClients.Add(PHONE_NUMBER, this.textBoxInputPhoneNumberClients.Text);
            return valuesClients;
        }

        private void ButtonCancelarClientClick(object sender, EventArgs e)
        {
            this.CleanFieldsInputClient();
        }

        private void CleanFieldsInputClient()
        {
            this.textBoxInputNameClients.Text = string.Empty;
            this.textBoxInputRgDocumentClients.Text = string.Empty;
            this.textBoxInputCpfDocumentClients.Text = string.Empty;
            this.textBoxInputAddressClients.Text = string.Empty;
            this.textBoxInputPhoneNumberClients.Text = string.Empty;
        }

        private void ComboBoxCpfDocumentCustomerSaleKeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnlyNumbers(e);
        }

        private void ComboBoxCpfDocumentSellerSaleKeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnlyNumbers(e);
        }
        #endregion

        #region sellers
        private void ButtonCadastarSellerClick(object sender, EventArgs e)
        {
            if (this.AnyNewSellerDataFieldIsEmpty())
            {
                this.MessageMissingData();
                return;
            }
            var valuesSeller = this.GetValuesFieldsNewClient();
            var seller = new Seller(valuesSeller[NAME], valuesSeller[CPF_DOCUMENT], valuesSeller[PHONE_NUMBER],
                Convert.ToDouble(valuesSeller[SALARY]))
            {
                RGDocument = valuesSeller[RG_DOCUMENT],
                Address = valuesSeller[ADDRESS]
            };
            this.sellersControl.Add(seller);
            this.CleanFieldsInputSeller();
        }

        private bool AnyNewSellerDataFieldIsEmpty()
        {
            return string.IsNullOrEmpty(this.textBoxInputNameSeller.Text) ||
               string.IsNullOrEmpty(this.textBoxInputCpfDocumentSeller.Text) ||
               string.IsNullOrEmpty(this.textBoxInputPhoneNumberSeller.Text) ||
               string.IsNullOrEmpty(this.textBoxInputSalarySeller.Text);
        }

        private IDictionary<string, string> GetValuesFieldsNewSeller()
        {
            var valuesSellers = new Dictionary<string, string>();
            valuesSellers.Add(NAME, this.textBoxInputNameSeller.Text.ToUpper());
            valuesSellers.Add(RG_DOCUMENT, this.textBoxInputRgDocumentSeller.Text.ToUpper());
            valuesSellers.Add(CPF_DOCUMENT, this.textBoxInputCpfDocumentSeller.Text);
            valuesSellers.Add(ADDRESS, this.textBoxInputAddressSeller.Text.ToUpper());
            valuesSellers.Add(PHONE_NUMBER, this.textBoxInputPhoneNumberSeller.Text);
            valuesSellers.Add(SALARY, this.textBoxInputSalarySeller.Text);
            return valuesSellers;
        }

        private void CleanFieldsInputSeller()
        {
            this.textBoxInputNameSeller.Text = string.Empty;
            this.textBoxInputRgDocumentSeller.Text = string.Empty;
            this.textBoxInputCpfDocumentSeller.Text = string.Empty;
            this.textBoxInputAddressSeller.Text = string.Empty;
            this.textBoxInputPhoneNumberSeller.Text = string.Empty;
            this.textBoxInputSalarySeller.Text = string.Empty;
        }

        private void TextBoxInputRgDocumentSellerKeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnlyNumbers(e);
        }

        private void TextBoxInputCpfDocumentSellerKeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnlyNumbers(e);
        }

        private void TextBoxInputPhoneNumberSellerKeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnlyNumbers(e);
        }
        #endregion

        private void OnlyNumbers(KeyPressEventArgs eventKey)
        {
            if (this.IsNotDecimalOrBackSpace(eventKey.KeyChar)) { this.DisableEventKey(eventKey); }
        }

        private void DisableEventKey(KeyPressEventArgs eventKey)
        {
            eventKey.Handled = true;
        }

        private bool IsNotDecimalOrBackSpace(char keyChar)
        {
            return Char.IsDigit(keyChar) == false && keyChar != (BACKSPACE_ASCII);
        }

        private void MessageMissingData()
        {
            MessageBox.Show(INCOMPLETE_FIELDS_MESSAGE);
        }
    }
}
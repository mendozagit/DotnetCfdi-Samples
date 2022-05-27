using Invoicing.Base;
using Invoicing.Common;

namespace InvoicingSamples
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CfdiIngresoButton_Click(object sender, EventArgs e)
        {
            var xml = BuildXmlCfdiIngreso();
        }

        private string BuildXmlCfdiIngreso()
        {
            var xml = string.Empty;

            // emisor
            var issuer = new InvoiceIssuer
            {
                Tin = "MEJJ940824C61",
                LegalName = "JESUS MENDOZA JUAREZ",
                TaxRegime = "621" //Incorporación Fiscal

            };

            //receptor
            var recipient = new InvoiceRecipient
            {
                Tin = "DGE131017IP1",
                LegalName = "DYM GENERICOS",
                InvoiceUse = "G01"
            };

            //comprobante
            var invoice = new Invoice
            {
                InvoiceType = InvoiceType.I,
                InvoiceDate = DateTime.Now.ToSatFormat(),
                InvoiceVersion = InvoiceVersion.V40,
                InvoiceSerie = InvoiceSeries.Ingreso,
                InvoiceNuber = "1234",
                SignatureValue = "abcd123",
                PaymentForm = "PUE",
                CertificateNumber = "123456789abcd",
                PaymentConditions = null,
                Subtotal = 200,
                Discount = 0,
                Currency = InvoiceCurrency.MXN,
                ExchangeRate = 1,
                Total = 232,
                PaymentMethod = "01",
                ExpeditionZipCode = "38034",
                InvoiceRelateds = null,
                InvoiceIssuer = null,
                InvoiceRecipient = null,
                InvoiceItems = null
            };


            return xml;
        }
    }
}
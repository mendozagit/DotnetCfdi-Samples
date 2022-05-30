using System.Text;
using Invoicing.Base;
using Invoicing.Common;
using Invoicing.Common.Constants;
using Invoicing.Common.Enums;
using Invoicing.Common.Extensions;
using Invoicing.Common.Serializing;

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
                TaxRegimeId = "621", //Incorporación Fiscal,
                OperationNumber = null
            };

            //receptor
            var recipient = new InvoiceRecipient
            {
                Tin = "DGE131017IP1",
                LegalName = "DYM GENERICOS",
                ZipCode = null,
                ForeignCountryId = null,
                ForeignTin = null,
                TaxRegimeId = "601", //General de Ley Personas Morales
                CfdiUseId = "G01" //Adquisición de mercancías.
            };

            var itemTaxes = new InvoiceItemTaxesWrapper
            {
                TransferredTaxes = new List<InvoiceItemTax>
                {
                    new InvoiceItemTax
                    {
                        Base = 100,
                        TaxId = "002",
                        TaxTypeId = "Exento"
                    },
                    new InvoiceItemTax
                    {
                        Base = 100,
                        TaxId = "002",
                        TaxTypeId = "Tasa",
                        TaxRate = 0.160000m,
                        Amount = 16
                    },
                    new InvoiceItemTax
                    {
                        Base = 100,
                        TaxId = "002",
                        TaxTypeId = "Tasa",
                        TaxRate = 0.160000m,
                        Amount = 16
                    },
                    new InvoiceItemTax
                    {
                        Base = 100,
                        TaxId = "003",
                        TaxTypeId = "Tasa",
                        TaxRate = 0.080000m,
                        Amount = 8
                    }
                },
                WithholdingTaxes = new List<InvoiceItemTax>
                {
                    new InvoiceItemTax
                    {
                        Base = 100,
                        TaxId = "002",
                        TaxTypeId = "Tasa",
                        TaxRate = 0.060000m,
                        Amount = 6
                    }
                }
            };


            //conceptos
            var ivoiceItems = new List<InvoiceItem>()
            {
                new InvoiceItem
                {
                    SatItemId = InvoiceConstants.SatItemId,
                    ItemId = "1801",
                    Quantity = 1,
                    UnitOfMeasureId = InvoiceConstants.UnitOfMeasureId,
                    UnitOfMeasure = "PZA",
                    Description = "Product description 1",
                    UnitCost = 200,
                    Amount = 200,
                    Discount = 0,
                    TaxObjectId = InvoiceConstants.TaxObjectId,
                    ItemTaxex = itemTaxes
                },
                new InvoiceItem
                {
                    SatItemId = InvoiceConstants.SatItemId,
                    ItemId = "1802",
                    Quantity = 1,
                    UnitOfMeasureId = InvoiceConstants.UnitOfMeasureId,
                    UnitOfMeasure = "PZA",
                    Description = "Product description 2",
                    UnitCost = 200,
                    Amount = 200,
                    Discount = 0,
                    TaxObjectId = InvoiceConstants.TaxObjectId,
                    ItemTaxex = itemTaxes
                },
                new InvoiceItem
                {
                    SatItemId = InvoiceConstants.SatItemId,
                    ItemId = "1803",
                    Quantity = 1,
                    UnitOfMeasureId = InvoiceConstants.UnitOfMeasureId,
                    UnitOfMeasure = "PZA",
                    Description = "Product description 3",
                    UnitCost = 200,
                    Amount = 200,
                    Discount = 0,
                    TaxObjectId = InvoiceConstants.TaxObjectId,
                    ItemTaxex = itemTaxes
                },
                new InvoiceItem
                {
                    SatItemId = InvoiceConstants.SatItemId,
                    ItemId = "1804",
                    Quantity = 1,
                    UnitOfMeasureId = InvoiceConstants.UnitOfMeasureId,
                    UnitOfMeasure = "PZA",
                    Description = "Product description 4",
                    UnitCost = 200,
                    Amount = 200,
                    Discount = 0,
                    TaxObjectId = InvoiceConstants.TaxObjectId,
                    ItemTaxex = itemTaxes
                }
            };


            //comprobante
            var invoice = new Invoice
            {
                InvoiceTypeId = InvoiceType.Ingreso,
                InvoiceDate = DateTime.Now.ToSatFormat(),
                InvoiceVersion = InvoiceVersion.V40,
                InvoiceSerie = InvoiceSerie.Ingreso.ToValue(),
                InvoiceNuber = "1234",
                SignatureValue = "abcd123",
                PaymentForm = "PUE",
                CertificateNumber = "123456789abcd",
                PaymentConditions = null,
                Subtotal = 200,
                Discount = 0,
                Currency = InvoiceCurrency.MXN.ToValue(),
                ExchangeRate = 1,
                Total = 232,
                PaymentMethodId = "01",
                ExpeditionZipCode = "38034",
                InvoiceRelateds = null,
                InvoiceIssuer = issuer,
                InvoiceRecipient = recipient,
                SchemaLocation = SerializerHelper.SchemaLocationIE40,
                InvoiceItems = ivoiceItems
            };

            invoice.ComputeInvoice();
            
            xml = Serializer<Invoice>.Serialize(invoice, SerializerHelper.NamespacesIE40,
                SerializerHelper.DefaultXmlWriterSettings);
            File.WriteAllText("invoice.xml", xml);


            return xml;
        }
    }
}
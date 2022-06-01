using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using Credencials.Common;
using Credencials.Core;
using Invoicing.Base;
using Invoicing.Common.Constants;
using Invoicing.Common.Enums;
using Invoicing.Common.Extensions;
using Invoicing.Common.Serializing;

namespace InvoicingSamples
{
    public partial class Form1 : Form
    {
        private ICredential credential;

        public Form1()
        {
            InitializeComponent();

            //credential = GetCredenctialInProduction(); //Descomente esta linea y comente la prxima.
            credential = GetCredenctialInDev();
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
                TaxRegimeId = "621", //RIF
                OperationNumber = null,
            };

            //receptor
            var recipient = new InvoiceRecipient
            {
                Tin = "DGE131017IP1",
                LegalName = "DYM GENERICOS",
                ZipCode = "38050",
                ForeignCountryId = null,
                ForeignTin = null,
                TaxRegimeId = "601", //General de Ley Personas Morales
                CfdiUseId = "G03" //Adquisición de mercancías.
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
                InvoiceVersion = InvoiceVersion.V40,
                InvoiceSerie = InvoiceSerie.Ingreso.ToValue(),
                InvoiceNuber = "1234",
                InvoiceDate = DateTime.Now.ToSatFormat(),
                PaymentForm = "01",
                CertificateNumber = credential.Certificate.CertificateNumber,
                CertificateB64 = credential.Certificate.PlainBase64,
                PaymentConditions = null,
                Subtotal = 0,
                Discount = 0,
                Currency = InvoiceCurrency.MXN.ToValue(),
                ExchangeRate = 1,
                Total = 0,
                InvoiceTypeId = InvoiceType.Ingreso,
                ExportId = "01",
                PaymentMethodId = "PUE",
                ExpeditionZipCode = "38034",
                PacConfirmation = null,
                SchemaLocation = SerializerHelper.SchemaLocationIE40,
                GlobalInformation = null,
                InvoiceRelateds = null,
                InvoiceIssuer = issuer,
                InvoiceRecipient = recipient,
                InvoiceItems = ivoiceItems,
            };

            invoice.ComputeInvoice();

            xml = Serializer<Invoice>.Serialize(invoice, SerializerHelper.NamespacesIE40,
                SerializerHelper.DefaultXmlWriterSettings);


            var originalStr = credential.GetOriginalStringByXmlString(xml);
            var signature = credential.SignData(originalStr);
            invoice.SignatureValue = signature.ToBase64String();

            xml = Serializer<Invoice>.Serialize(invoice, SerializerHelper.NamespacesIE40,
                SerializerHelper.DefaultXmlWriterSettings);

            File.WriteAllText("invoice.xml", xml);


            return xml;
        }


        /// <summary>
        /// Este metodo construye el objeto de la manera tradicional, simplemente cada vez que requiera construir
        /// un objeto credential, deberá leer los archivos .cer/.key como se muestra abajo.
        /// Vea el metodo GetCredenctialInDev() para conocer otra alternativa.
        /// vea: https://github.com/dotnetcfdi/credentials para mas detalles.
        /// </summary>
        /// <returns>credential</returns>
        private ICredential GetCredenctialInProduction()
        {
            //Creating a certificate instance
            var cerPath = @"CSD\EKU9003173C9.cer";
            var cerBytes = File.ReadAllBytes(cerPath);
            var cerBase64 = Convert.ToBase64String(cerBytes);
            var certificate =
                new Certificate(
                    cerBase64); //puedes guardar cerBase64 en la db, entonces omite las lineas anteriores y crea el objeto recuperando cerBase64 de la db


            //Creating a private key instance
            var keyPath = @"CSD\EKU9003173C9.key";
            var keyBytes = File.ReadAllBytes(keyPath);
            var keyBase64 = Convert.ToBase64String(keyBytes);
            var privateKey =
                new PrivateKey(keyBase64,
                    "12345678a"); //puedes guardar keyBase64 en la db, entonces omite las lineas anteriores y crea el objeto recuperandolo db

            //Create a credential instance, certificate and privatekey previously created.
            var credential = new Credential(certificate, privateKey);

            //Configure el algoritmo de firmado usado en las facturas por el SAT
            credential.ConfigureAlgorithmForInvoicing();

            //Establezca la ruta del archivo de transformaciones de cadena original.
            CredentialSettings.OriginalStringPath = @"CadenaOriginal40\cadenaoriginal40.xslt";


            //CredentialSettings.Algorithm
            return credential;
        }


        /// <summary>
        /// Este metodo es solo para las pruebas durante la etapa de desarrollo.
        /// La idea es simple, almacenar data sensible localmente en un archivo SecretInfo.json (simulando mi base de datos)
        ///
        /// Cuando necesite construir un objeto credenctial, simplemente leo mi base de datos SecretInfo.json y
        /// recupero los certificados. Recuerde que cualquier archivo tiene una representacion en texto y ese texto se puede almacenar
        /// como un string en un archivo / base de datos.
        /// 
        /// Estp demuestra que los certificados pueden ser almacenados como texto y ese
        /// texto puede ser almacenado en cualquier base de datos (en este caso un simple json)
        /// pero el principio es exactamente el mismo si almacena y recupera de MSSQL,MySql,Oracle, PostgreSQL.
        /// Sientase libre de implementar su capa de acceso a datos y aplicar este principio de construir el
        /// objeto credential sin leer directamente los archivos .cer /.key
        /// En su lugar recuperar la representacion de esos archivos en texto y construir e objeto credential.
        /// </summary>
        /// vea: https://github.com/dotnetcfdi/credentials para mas detalles.
        /// <returns>credential</returns>
        private ICredential GetCredenctialInDev()
        {
            var fileName = @"C:\dotnetcfdi\SecretInfo.json";
            var jsonString = File.ReadAllText(fileName);
            var secretInfo = JsonSerializer.Deserialize<SecretInfo>(jsonString)!;


            //Creating a certificate instance
            //var cerPath = @"CSD\MEJJ940824C61.cer";
            //var cerBytes = File.ReadAllBytes(cerPath);
            //var cerBase64 = Convert.ToBase64String(cerBytes);

            var cerBase64 = secretInfo.CsdCer; // Comenta esta linea y descomenta las 3 lineas inmediamente anteriores
            var certificate =
                new Certificate(
                    cerBase64); //puedes guardar cerBase64 en la db, entonces omite las lineas anteriores y crea el objeto recuperando cerBase64 de la db


            //Creating a private key instance
            //var keyPath = @"CSD\MEJJ940824C61.key";
            //var keyBytes = File.ReadAllBytes(keyPath);
            //var keyBase64 = Convert.ToBase64String(keyBytes);

            var keyBase64 = secretInfo.CsdKey; // Comenta esta linea y descomenta las 3 lineas inmediamente anteriores
            var privateKey =
                new PrivateKey(keyBase64,
                    secretInfo
                        .Password); //puedes guardar keyBase64 en la db, entonces omite las lineas anteriores y crea el objeto recuperandolo db

            //Create a credential instance, certificate and privatekey previously created.
            var credential = new Credential(certificate, privateKey);

            //Configure el algoritmo de firmado usado en las facturas por el SAT
            credential.ConfigureAlgorithmForInvoicing();

            //Establezca la ruta del archivo de transformaciones de cadena original.
            CredentialSettings.OriginalStringPath = @"CadenaOriginal40\cadenaoriginal40.xslt";

            //CredentialSettings.Algorithm
            return credential;
        }
    }


    /// <summary>
    /// Esta clase es unica y exclusivamente para no exponer data sensible en gitgub.
    /// La idea es bastante simple, tengo un directorio con un json y todos los datos certificados en texto
    /// para no harcodear y exponer, de esta forma, ese archivo secretInfo.json, es desconsiderado por el
    /// sistema de control de versiones y la data sensible nunca será expuesta publicamente.
    /// en la vida real usted no tendrea que utilizar esta clase, solo descomente las lineas del metodo GetCredenctial().
    /// </summary>
    public class SecretInfo
    {
        [JsonPropertyName("csdcer")] public string? CsdCer { get; set; }

        [JsonPropertyName("csdkey")] public string? CsdKey { get; set; }

        [JsonPropertyName("fielcer")] public string? FielCer { get; set; }

        [JsonPropertyName("fielkey")] public string? FielKey { get; set; }
        [JsonPropertyName("password")] public string? Password { get; set; }
    }
}
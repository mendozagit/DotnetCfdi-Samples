using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using Credencials.Common;
using Credencials.Core;
using Invoicing.Base;
using Invoicing.Common.Constants;
using Invoicing.Common.Enums;
using Invoicing.Common.Extensions;
using Invoicing.Common.Serializing;
using Invoicing.Complements.Payments;
using Invoicing.Servicies;
using Invoice = Invoicing.Base.Invoice;

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
            var xml = string.Empty;
            SerializerHelper.ConfigureSettingsForInvoice();

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
                    SatItemId = InvoiceConstants.SatInvoiceItemId,
                    ItemId = "1801",
                    Quantity = 1,
                    UnitOfMeasureId = InvoiceConstants.SatInvoiceUnitOfMeasureId,
                    UnitOfMeasure = "PZA",
                    Description = "Product description 1",
                    UnitCost = 200,
                    Amount = 200,
                    Discount = 0,
                    TaxObjectId = InvoiceConstants.SatInvoiceObjectId,
                    ItemTaxex = itemTaxes
                },
                new InvoiceItem
                {
                    SatItemId = InvoiceConstants.SatInvoiceItemId,
                    ItemId = "1802",
                    Quantity = 1,
                    UnitOfMeasureId = InvoiceConstants.SatInvoiceUnitOfMeasureId,
                    UnitOfMeasure = "PZA",
                    Description = "Product description 2",
                    UnitCost = 200,
                    Amount = 200,
                    Discount = 0,
                    TaxObjectId = InvoiceConstants.SatInvoiceObjectId,
                    ItemTaxex = itemTaxes
                },
                new InvoiceItem
                {
                    SatItemId = InvoiceConstants.SatInvoiceItemId,
                    ItemId = "1803",
                    Quantity = 1,
                    UnitOfMeasureId = InvoiceConstants.SatInvoiceUnitOfMeasureId,
                    UnitOfMeasure = "PZA",
                    Description = "Product description 3",
                    UnitCost = 200,
                    Amount = 200,
                    Discount = 0,
                    TaxObjectId = InvoiceConstants.SatInvoiceObjectId,
                    ItemTaxex = itemTaxes
                },
                new InvoiceItem
                {
                    SatItemId = InvoiceConstants.SatInvoiceItemId,
                    ItemId = "1804",
                    Quantity = 1,
                    UnitOfMeasureId = InvoiceConstants.SatInvoiceUnitOfMeasureId,
                    UnitOfMeasure = "PZA",
                    Description = "Product description 4",
                    UnitCost = 200,
                    Amount = 200,
                    Discount = 0,
                    TaxObjectId = InvoiceConstants.SatInvoiceObjectId,
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
                ExchangeRate = 0,
                Total = 0,
                InvoiceTypeId = InvoiceType.Ingreso,
                ExportId = "01",
                PaymentMethodId = "PUE",
                ExpeditionZipCode = "38034",
                PacConfirmation = null,
                SchemaLocation = SerializerHelper.SchemaLocation,
                GlobalInformation = null,
                RelatedInvoiceWrapper = null,
                InvoiceIssuer = issuer,
                InvoiceRecipient = recipient,
                InvoiceItems = ivoiceItems,
            };


            invoice.Compute();

            xml = Serializer<Invoice>.Serialize(invoice, SerializerHelper.Namespaces, new XmlWriterSettings());


            var originalStr = credential.GetOriginalStringByXmlString(xml);
            var signature = credential.SignData(originalStr);
            invoice.SignatureValue = signature.ToBase64String();

            xml = Serializer<Invoice>.Serialize(invoice, SerializerHelper.Namespaces, new XmlWriterSettings());

            File.WriteAllText("invoice.xml", xml);

            MessageBox.Show(@"Ok");
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
            CredentialSettings.OriginalStringPath = @"C:\dotnetcfdi\CadenaOriginal40\cadenaoriginal40.xslt";

            //CredentialSettings.Algorithm
            return credential;
        }

        private void PaymentButton_Click(object sender, EventArgs e)
        {
            var xml = string.Empty;

            SerializerHelper.ConfigureSettingsForPayment();

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
                CfdiUseId = "CP01" //Pagos.
            };


            //conceptos: En este nodo se debe expresar un solo concepto descrito en el comprobante fiscal.
            var ivoiceItems = new List<InvoiceItem>()
            {
                new InvoiceItem
                {
                    SatItemId = InvoiceConstants.SatPaymentItemId,
                    //ItemId = "1801", Este campo no debe existir.
                    Quantity = 1,
                    UnitOfMeasureId = InvoiceConstants.SatPaymentUnitOfMeasureId,
                    //UnitOfMeasure = "PZA", Este campo no debe existir.
                    Description = InvoiceConstants.SatPaymentItemDescriptionId,
                    UnitCost = 0,
                    Amount = 0,
                    Discount = 0,
                    TaxObjectId = InvoiceConstants.SatPaymentObjectId,
                }
            };

            //comprobante
            var invoice = new Invoice
            {
                InvoiceVersion = InvoiceVersion.V40,
                InvoiceSerie = InvoiceSerie.Pago.ToValue(),
                InvoiceNuber = "1235",
                InvoiceDate = DateTime.Now.ToSatFormat(),
                //PaymentForm = "01", Este campo no debe existir.
                CertificateNumber = credential.Certificate.CertificateNumber,
                CertificateB64 = credential.Certificate.PlainBase64,
                //PaymentConditions = null, Este campo no debe existir.
                Subtotal = 0,
                //Discount = 0, Este campo no debe existir.
                Currency = InvoiceCurrency.XXX.ToValue(),
                //ExchangeRate = 1, Este campo no debe existir.
                Total = 0,
                InvoiceTypeId = InvoiceType.Pago,
                ExportId = "01", //Se debe registrar la clave “01” (No aplica).
                //PaymentMethodId = "PUE",Este campo no debe existir.
                ExpeditionZipCode = "38034",
                PacConfirmation = null,
                SchemaLocation = SerializerHelper.SchemaLocation,
                GlobalInformation = null,
                RelatedInvoiceWrapper = null,
                InvoiceIssuer = issuer,
                InvoiceRecipient = recipient,
                InvoiceItems = ivoiceItems,
                // InvoiceTaxes = null
            };


            var paymentComplement = new PaymentComplement
            {
                PaymentSummary = null,
                Version = "2.0",
                Payments = new List<Payment>()
                {
                    new Payment
                    {
                        PaymentDate = DateTime.Now.ToSatFormat(),
                        PaymentFormId = "28",
                        CurrencyId = InvoiceCurrency.MXN.ToValue(),
                        ExchangeRate = 1,
                        Amount = 90000,
                        OperationNumber = null,
                        OriginBankTin = "BSM970519DU8",
                        OriginBankAccountNumber = "1234567891012131",
                        DestinationBankTin = "BBA830831LJ2",
                        DestinationAccountNumber = "1234567890",
                        ForeignBankName = null,
                        ElectronicPaymentSystemId = null,
                        Base64PaymetCertificate = null,
                        PaymentOriginalString = null,
                        SignatureValue = null,
                        Invoices = new List<PaymentInvoice>()
                        {
                            new PaymentInvoice
                            {
                                InvoiceUuid = "5C7B0622-01B4-4EB8-96D0-E0DEBD89FF0F",
                                InvoiceSeries = "F",
                                InvoiceNumber = "1127",
                                InvoiceCurrencyId = InvoiceCurrency.MXN.ToValue(),
                                InvoiceExchangeRate = 1,
                                PartialityNumber = 1,
                                PreviousBalanceAmount = 98618.388800m,
                                PaymentAmount = 90000m,
                                RemainingBalance = 8618.3888m,
                                TaxObjectId = TaxationObject.YesSubjectToTax.ToValue(),

                                InvoiceTaxesWrapper = new PaymentInvoiceTaxesWrapper()
                                {
                                    TransferredTaxes = new List<PaymentInvoiceTransferredTax>
                                    {
                                        new PaymentInvoiceTransferredTax
                                        {
                                            Base = 90000m,
                                            TaxId = "002",
                                            TaxTypeId = TaxType.Exento.ToValue()
                                        },
                                        new PaymentInvoiceTransferredTax
                                        {
                                            Base = 90000m,
                                            TaxId = "002",
                                            TaxTypeId = TaxType.Tasa.ToValue(),
                                            TaxRate = 0.000000m,
                                        },
                                        new PaymentInvoiceTransferredTax
                                        {
                                            Base = 90000m,
                                            TaxId = "002",
                                            TaxTypeId = TaxType.Tasa.ToValue(),
                                            TaxRate = 0.160000m,
                                        },
                                        new PaymentInvoiceTransferredTax
                                        {
                                            Base = 90000m,
                                            TaxId = "002",
                                            TaxTypeId = TaxType.Tasa.ToValue(),
                                            TaxRate = 0.080000m,
                                        },
                                        new PaymentInvoiceTransferredTax
                                        {
                                            Base = 90000m,
                                            TaxId = "003",
                                            TaxTypeId = TaxType.Tasa.ToValue(),
                                            TaxRate = 0.530000m,
                                        }
                                    },
                                    WithholdingTaxes = new List<PaymentInvoiceWithholdingTax>()
                                    {
                                        new PaymentInvoiceWithholdingTax()
                                        {
                                            Base = 90000m,
                                            TaxId = "002",
                                            TaxTypeId = TaxType.Tasa.ToValue(),
                                            TaxRate = 0.106666m,
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            paymentComplement.HeaderDecimals = invoice.HeaderDecimals;
            paymentComplement.ItemsDecimals = invoice.ItemsDecimals;
            paymentComplement.RoundingStrategy = invoice.RoundingStrategy;
            paymentComplement.Compute();


            //insert complement into invoice

            var paymentComplementXml =
                Serializer<Invoice>.SerializeElement(paymentComplement,
                    InvoiceConstants.SatPayment20Namespace.GetSerializerNamespace("pago20"));
            invoice.AddComplement(paymentComplementXml);


            invoice.Compute();

            xml = Serializer<Invoice>.Serialize(invoice, SerializerHelper.Namespaces, new XmlWriterSettings());


            var originalStr = credential.GetOriginalStringByXmlString(xml);
            var signature = credential.SignData(originalStr);
            invoice.SignatureValue = signature.ToBase64String();

            xml = Serializer<Invoice>.Serialize(invoice, SerializerHelper.Namespaces, new XmlWriterSettings());

            File.WriteAllText("payment-invoice.xml", xml);


            MessageBox.Show(@"Ok");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void CreditNoteButton_Click(object sender, EventArgs e)
        {
            var xml = string.Empty;

            SerializerHelper.ConfigureSettingsForInvoice();

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
                    SatItemId = InvoiceConstants.SatInvoiceItemId,
                    ItemId = "1801",
                    Quantity = 1,
                    UnitOfMeasureId = InvoiceConstants.SatInvoiceUnitOfMeasureId,
                    UnitOfMeasure = "PZA",
                    Description = "Product description 1",
                    UnitCost = 200,
                    Amount = 200,
                    Discount = 0,
                    TaxObjectId = InvoiceConstants.SatInvoiceObjectId,
                    ItemTaxex = itemTaxes
                },
                new InvoiceItem
                {
                    SatItemId = InvoiceConstants.SatInvoiceItemId,
                    ItemId = "1802",
                    Quantity = 1,
                    UnitOfMeasureId = InvoiceConstants.SatInvoiceUnitOfMeasureId,
                    UnitOfMeasure = "PZA",
                    Description = "Product description 2",
                    UnitCost = 200,
                    Amount = 200,
                    Discount = 0,
                    TaxObjectId = InvoiceConstants.SatInvoiceObjectId,
                    ItemTaxex = itemTaxes
                },
                new InvoiceItem
                {
                    SatItemId = InvoiceConstants.SatInvoiceItemId,
                    ItemId = "1803",
                    Quantity = 1,
                    UnitOfMeasureId = InvoiceConstants.SatInvoiceUnitOfMeasureId,
                    UnitOfMeasure = "PZA",
                    Description = "Product description 3",
                    UnitCost = 200,
                    Amount = 200,
                    Discount = 0,
                    TaxObjectId = InvoiceConstants.SatInvoiceObjectId,
                    ItemTaxex = itemTaxes
                },
                new InvoiceItem
                {
                    SatItemId = InvoiceConstants.SatInvoiceItemId,
                    ItemId = "1804",
                    Quantity = 1,
                    UnitOfMeasureId = InvoiceConstants.SatInvoiceUnitOfMeasureId,
                    UnitOfMeasure = "PZA",
                    Description = "Product description 4",
                    UnitCost = 200,
                    Amount = 200,
                    Discount = 0,
                    TaxObjectId = InvoiceConstants.SatInvoiceObjectId,
                    ItemTaxex = itemTaxes
                }
            };

            //Relationshipt with original invoice for a credit note (factura de egreso)
            //Where original invoice uuid is: 1fac4464-1111-0000-1111-cd37179db12e
            var relatedInvoiceWrapper = new InvoiceRelatedWrapper
            {
                RelatedInvoices = new List<InvoiceRelated>()
                {
                    new InvoiceRelated
                    {
                        InvoiceUuid = "1fac4464-1111-0000-1111-cd37179db12e"
                    }
                },
                RelationshipTypeId = InvoiceRelationshipType.CreditNoteOfRelatedDocuments.ToValue()
            };

            //comprobante
            var invoice = new Invoice
            {
                InvoiceVersion = InvoiceVersion.V40,
                InvoiceSerie = InvoiceSerie.Egreso.ToValue(),
                InvoiceNuber = "1234",
                InvoiceDate = DateTime.Now.ToSatFormat(),
                PaymentForm = "01",
                CertificateNumber = credential.Certificate.CertificateNumber,
                CertificateB64 = credential.Certificate.PlainBase64,
                PaymentConditions = null,
                Subtotal = 0,
                Discount = 0,
                Currency = InvoiceCurrency.MXN.ToValue(),
                ExchangeRate = 0,
                Total = 0,
                InvoiceTypeId = InvoiceType.Egreso,
                ExportId = "01",
                PaymentMethodId = "PUE",
                ExpeditionZipCode = "38034",
                PacConfirmation = null,
                SchemaLocation = SerializerHelper.SchemaLocation,
                GlobalInformation = null,
                RelatedInvoiceWrapper = relatedInvoiceWrapper,
                InvoiceIssuer = issuer,
                InvoiceRecipient = recipient,
                InvoiceItems = ivoiceItems,
            };

            invoice.Compute();

            xml = Serializer<Invoice>.Serialize(invoice, SerializerHelper.Namespaces, new XmlWriterSettings());


            var originalStr = credential.GetOriginalStringByXmlString(xml);
            var signature = credential.SignData(originalStr);
            invoice.SignatureValue = signature.ToBase64String();

            xml = Serializer<Invoice>.Serialize(invoice, SerializerHelper.Namespaces, new XmlWriterSettings());

            File.WriteAllText("credit-note-invoice.xml", xml);
            MessageBox.Show(@"Ok");
        }

        private async void IngresoServicebutton_Click(object sender, EventArgs e)
        {
            var invoiceService = new InvoiceService
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
                ExchangeRate = 0,
                Total = 0,
                InvoiceTypeId = InvoiceType.Ingreso,
                ExportId = "01",
                PaymentMethodId = "PUE",
                ExpeditionZipCode = "38034",
                PacConfirmation = null,
                Credential = credential
            };


            // emisor
            var issuer = new InvoiceIssuer
            {
                Tin = "MEJJ940824C61",
                LegalName = "JESUS MENDOZA JUAREZ",
                TaxRegimeId = "621", //RIF
                OperationNumber = null,
            };
            invoiceService.AddIssuer(issuer);


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
            invoiceService.AddRecipient(recipient);


            var invoiceItem1 = new InvoiceItem
            {
                SatItemId = InvoiceConstants.SatInvoiceItemId,
                ItemId = "1801",
                Quantity = 1,
                UnitOfMeasureId = InvoiceConstants.SatInvoiceUnitOfMeasureId,
                UnitOfMeasure = "PZA",
                Description = "Product description 1",
                UnitCost = 200,
                Amount = 200,
                Discount = 0,
                TaxObjectId = InvoiceConstants.SatInvoiceObjectId
            };
            invoiceItem1.AddTransferredTax("002", "Exento");
            invoiceItem1.AddTransferredTax("002", "Tasa", 0.160000m);
            invoiceItem1.AddTransferredTax("002", "Tasa", 0.160000m);
            invoiceItem1.AddTransferredTax("003", "Tasa", 0.080000m);
            invoiceItem1.AddWithholdingTax("003", "Tasa", 0.060000m);

            var invoiceItem2 = new InvoiceItem
            {
                SatItemId = InvoiceConstants.SatInvoiceItemId,
                ItemId = "1802",
                Quantity = 1,
                UnitOfMeasureId = InvoiceConstants.SatInvoiceUnitOfMeasureId,
                UnitOfMeasure = "PZA",
                Description = "Product description 2",
                UnitCost = 200,
                Amount = 200,
                Discount = 0,
                TaxObjectId = InvoiceConstants.SatInvoiceObjectId,
            };
            invoiceItem2.AddTransferredTax("002", "Exento");
            invoiceItem2.AddTransferredTax("002", "Tasa", 0.160000m);
            invoiceItem2.AddTransferredTax("002", "Tasa", 0.160000m);
            invoiceItem2.AddTransferredTax("003", "Tasa", 0.080000m);
            invoiceItem2.AddWithholdingTax("003", "Tasa", 0.060000m);

            var invoiceItem3 = new InvoiceItem
            {
                SatItemId = InvoiceConstants.SatInvoiceItemId,
                ItemId = "1803",
                Quantity = 1,
                UnitOfMeasureId = InvoiceConstants.SatInvoiceUnitOfMeasureId,
                UnitOfMeasure = "PZA",
                Description = "Product description 3",
                UnitCost = 200,
                Amount = 200,
                Discount = 0,
                TaxObjectId = InvoiceConstants.SatInvoiceObjectId
            };
            invoiceItem3.AddTransferredTax("002", "Exento");
            invoiceItem3.AddTransferredTax("002", "Tasa", 0.160000m);
            invoiceItem3.AddTransferredTax("002", "Tasa", 0.160000m);
            invoiceItem3.AddTransferredTax("003", "Tasa", 0.080000m);
            invoiceItem3.AddWithholdingTax("003", "Tasa", 0.060000m);

            var invoiceItem4 = new InvoiceItem
            {
                SatItemId = InvoiceConstants.SatInvoiceItemId,
                ItemId = "1804",
                Quantity = 1,
                UnitOfMeasureId = InvoiceConstants.SatInvoiceUnitOfMeasureId,
                UnitOfMeasure = "PZA",
                Description = "Product description 4",
                UnitCost = 200,
                Amount = 200,
                Discount = 0,
                TaxObjectId = InvoiceConstants.SatInvoiceObjectId
            };
            invoiceItem4.AddTransferredTax("002", "Exento");
            invoiceItem4.AddTransferredTax("002", "Tasa", 0.160000m);
            invoiceItem4.AddTransferredTax("002", "Tasa", 0.160000m);
            invoiceItem4.AddTransferredTax("003", "Tasa", 0.080000m);
            invoiceItem4.AddWithholdingTax("003", "Tasa", 0.060000m);


            invoiceService.AddInvoiceItem(invoiceItem1);
            invoiceService.AddInvoiceItem(invoiceItem2);
            invoiceService.AddInvoiceItem(invoiceItem3);
            invoiceService.AddInvoiceItem(invoiceItem4);
            //invoiceService.AddInvoiceItems()


            invoiceService.SignInvoice();
            var xml = invoiceService.SerializeToString();
            var base64Xml = xml.EncodeToBase64();
            await File.WriteAllTextAsync("invoice-by-service.xml", xml);

            MessageBox.Show(@"Ok");
        }

        private async void PaymentServiceButton_Click(object sender, EventArgs e)
        {
            var service = new PaymentService()
            {
                InvoiceNuber = "1235",
                InvoiceDate = DateTime.Now.ToSatFormat(),
                CertificateNumber = credential.Certificate.CertificateNumber,
                CertificateB64 = credential.Certificate.PlainBase64,
                Subtotal = 0,
                Currency = InvoiceCurrency.XXX.ToValue(),
                Total = 0,
                ExportId = "01", //Se debe registrar la clave “01” (No aplica).
                ExpeditionZipCode = "38034",
                Credential = credential

                //PaymentMethodId = "PUE",Este campo no debe existir.
                //ExchangeRate = 1, Este campo no debe existir.
                //Discount = 0, Este campo no debe existir.
                //PaymentForm = "01", Este campo no debe existir.
                //PaymentConditions = null, Este campo no debe existir.
            };


            // emisor
            service.AddIssuer("MEJJ940824C61", "JESUS MENDOZA JUAREZ", "621");


            //receptor
            service.AddRecipient("DGE131017IP1", "DYM GENERICOS", "38050", "601", "CP01");


            //Payment
            var payment = new Payment
            {
                PaymentDate = DateTime.Now.ToSatFormat(),
                PaymentFormId = "28",
                CurrencyId = InvoiceCurrency.MXN.ToValue(),
                ExchangeRate = 1,
                Amount = 90000,
                OperationNumber = null,
                OriginBankTin = "BSM970519DU8",
                OriginBankAccountNumber = "1234567891012131",
                DestinationBankTin = "BBA830831LJ2",
                DestinationAccountNumber = "1234567890",
            };


            //Payment invoice
            var paymentInvoice = new PaymentInvoice
            {
                InvoiceUuid = "5C7B0622-01B4-4EB8-96D0-E0DEBD89FF0F",
                InvoiceSeries = "F",
                InvoiceNumber = "1127",
                InvoiceCurrencyId = InvoiceCurrency.MXN.ToValue(),
                InvoiceExchangeRate = 1,
                PartialityNumber = 1,
                PreviousBalanceAmount = 98618.388800m,
                PaymentAmount = 90000m,
                RemainingBalance = 8618.3888m,
                TaxObjectId = TaxationObject.YesSubjectToTax.ToValue(),
            };

            //Paymet invoice Taxes
            var invoiceTransferredTaxes = new List<PaymentInvoiceTransferredTax>
            {
                new PaymentInvoiceTransferredTax
                {
                    Base = 90000m,
                    TaxId = "002",
                    TaxTypeId = TaxType.Exento.ToValue()
                },
                new PaymentInvoiceTransferredTax
                {
                    Base = 90000m,
                    TaxId = "002",
                    TaxTypeId = TaxType.Tasa.ToValue(),
                    TaxRate = 0.000000m,
                },
                new PaymentInvoiceTransferredTax
                {
                    Base = 90000m,
                    TaxId = "002",
                    TaxTypeId = TaxType.Tasa.ToValue(),
                    TaxRate = 0.160000m,
                },
                new PaymentInvoiceTransferredTax
                {
                    Base = 90000m,
                    TaxId = "002",
                    TaxTypeId = TaxType.Tasa.ToValue(),
                    TaxRate = 0.080000m,
                },
                new PaymentInvoiceTransferredTax
                {
                    Base = 90000m,
                    TaxId = "003",
                    TaxTypeId = TaxType.Tasa.ToValue(),
                    TaxRate = 0.530000m,
                }
            };
            var withholdingTaxes = new List<PaymentInvoiceWithholdingTax>()
            {
                new PaymentInvoiceWithholdingTax()
                {
                    Base = 90000m,
                    TaxId = "002",
                    TaxTypeId = TaxType.Tasa.ToValue(),
                    TaxRate = 0.106666m,
                }
            };

            //Addind taxex to payment invoice
            paymentInvoice.AddTransferredTaxes(invoiceTransferredTaxes);
            paymentInvoice.AddWithholdingTaxes(withholdingTaxes);

            //add invoice to payment
            payment.AddInvoice(paymentInvoice);

            //add payment to payment service
            service.AddPayment(payment);

            service.SignInvoice();
            var xml = service.SerializeToString();
            await File.WriteAllTextAsync("payment-invoice-service.xml", xml);

            MessageBox.Show(@"Ok");
        }

        private void CreditNoteServiceButton_Click(object sender, EventArgs e)
        {
            //credit note service
            var creditNoteService = new CreditNoteService()
            {
                InvoiceSerie = InvoiceSerie.Egreso.ToValue(),
                InvoiceNuber = "1234",
                InvoiceDate = DateTime.Now.ToSatFormat(),
                PaymentForm = "01",
                CertificateNumber = credential.Certificate.CertificateNumber,
                CertificateB64 = credential.Certificate.PlainBase64,
                Currency = InvoiceCurrency.MXN.ToValue(),
                PaymentMethodId = "PUE",
                ExpeditionZipCode = "38034",
                Credential = credential
            };

            // emisor
            creditNoteService.AddIssuer("MEJJ940824C61", "JESUS MENDOZA JUAREZ", "621");


            //receptor
            creditNoteService.AddRecipient("DGE131017IP1", "DYM GENERICOS", "38050", "601", "G03");


            var invoiceItem1 = new InvoiceItem
            {
                SatItemId = InvoiceConstants.SatInvoiceItemId,
                ItemId = "1801",
                Quantity = 1,
                UnitOfMeasureId = InvoiceConstants.SatInvoiceUnitOfMeasureId,
                UnitOfMeasure = "PZA",
                Description = "Product description 1",
                UnitCost = 200,
                Amount = 200,
                Discount = 0,
                TaxObjectId = InvoiceConstants.SatInvoiceObjectId
            };
            invoiceItem1.AddTransferredTax("002", "Exento");
            invoiceItem1.AddTransferredTax("002", "Tasa", 0.160000m);
            invoiceItem1.AddTransferredTax("002", "Tasa", 0.160000m);
            invoiceItem1.AddTransferredTax("003", "Tasa", 0.080000m);
            invoiceItem1.AddWithholdingTax("003", "Tasa", 0.060000m);

            var invoiceItem2 = new InvoiceItem
            {
                SatItemId = InvoiceConstants.SatInvoiceItemId,
                ItemId = "1802",
                Quantity = 1,
                UnitOfMeasureId = InvoiceConstants.SatInvoiceUnitOfMeasureId,
                UnitOfMeasure = "PZA",
                Description = "Product description 2",
                UnitCost = 200,
                Amount = 200,
                Discount = 0,
                TaxObjectId = InvoiceConstants.SatInvoiceObjectId,
            };
            invoiceItem2.AddTransferredTax("002", "Exento");
            invoiceItem2.AddTransferredTax("002", "Tasa", 0.160000m);
            invoiceItem2.AddTransferredTax("002", "Tasa", 0.160000m);
            invoiceItem2.AddTransferredTax("003", "Tasa", 0.080000m);
            invoiceItem2.AddWithholdingTax("003", "Tasa", 0.060000m);

            var invoiceItem3 = new InvoiceItem
            {
                SatItemId = InvoiceConstants.SatInvoiceItemId,
                ItemId = "1803",
                Quantity = 1,
                UnitOfMeasureId = InvoiceConstants.SatInvoiceUnitOfMeasureId,
                UnitOfMeasure = "PZA",
                Description = "Product description 3",
                UnitCost = 200,
                Amount = 200,
                Discount = 0,
                TaxObjectId = InvoiceConstants.SatInvoiceObjectId
            };
            invoiceItem3.AddTransferredTax("002", "Exento");
            invoiceItem3.AddTransferredTax("002", "Tasa", 0.160000m);
            invoiceItem3.AddTransferredTax("002", "Tasa", 0.160000m);
            invoiceItem3.AddTransferredTax("003", "Tasa", 0.080000m);
            invoiceItem3.AddWithholdingTax("003", "Tasa", 0.060000m);

            var invoiceItem4 = new InvoiceItem
            {
                SatItemId = InvoiceConstants.SatInvoiceItemId,
                ItemId = "1804",
                Quantity = 1,
                UnitOfMeasureId = InvoiceConstants.SatInvoiceUnitOfMeasureId,
                UnitOfMeasure = "PZA",
                Description = "Product description 4",
                UnitCost = 200,
                Amount = 200,
                Discount = 0,
                TaxObjectId = InvoiceConstants.SatInvoiceObjectId
            };
            invoiceItem4.AddTransferredTax("002", "Exento");
            invoiceItem4.AddTransferredTax("002", "Tasa", 0.160000m);
            invoiceItem4.AddTransferredTax("002", "Tasa", 0.160000m);
            invoiceItem4.AddTransferredTax("003", "Tasa", 0.080000m);
            invoiceItem4.AddWithholdingTax("003", "Tasa", 0.060000m);


            creditNoteService.AddInvoiceItem(invoiceItem1);
            creditNoteService.AddInvoiceItem(invoiceItem2);
            creditNoteService.AddInvoiceItem(invoiceItem3);
            creditNoteService.AddInvoiceItem(invoiceItem4);


            creditNoteService.AddRelatedCfdi("1fac4464-1111-0000-1111-cd37179db12e");

            creditNoteService.SignInvoice();

            creditNoteService.SerializeToFile("credit-note-service.xml");

            MessageBox.Show(@"OK");
        }


        private async void PaymentServiceButton2_Click(object sender, EventArgs e)
        {
            var service = new PaymentService()
            {
                InvoiceNuber = "1235",
                InvoiceDate = DateTime.Now.ToSatFormat(),
                CertificateNumber = credential.Certificate.CertificateNumber,
                CertificateB64 = credential.Certificate.PlainBase64,
                Subtotal = 0,
                Currency = InvoiceCurrency.XXX.ToValue(),
                Total = 0,
                ExportId = "01", //Se debe registrar la clave “01” (No aplica).
                ExpeditionZipCode = "38034",
                Credential = credential

                //PaymentMethodId = "PUE",Este campo no debe existir.
                //ExchangeRate = 1, Este campo no debe existir.
                //Discount = 0, Este campo no debe existir.
                //PaymentForm = "01", Este campo no debe existir.
                //PaymentConditions = null, Este campo no debe existir.
            };


            // emisor
            service.AddIssuer("MEJJ940824C61", "JESUS MENDOZA JUAREZ", "621");


            //receptor
            service.AddRecipient("DGE131017IP1", "DYM GENERICOS", "38050", "601", "CP01");


            //Payment
            var payment = new Payment
            {
                PaymentDate = DateTime.Now.ToSatFormat(),
                PaymentFormId = "28",
                CurrencyId = InvoiceCurrency.MXN.ToValue(),
                ExchangeRate = 1,
                Amount = 90000,
                OperationNumber = null,
                OriginBankTin = "BSM970519DU8",
                OriginBankAccountNumber = "1234567891012131",
                DestinationBankTin = "BBA830831LJ2",
                DestinationAccountNumber = "1234567890",
            };


            //Payment invoice
            var paymentInvoice = new PaymentInvoice
            {
                InvoiceUuid = "5C7B0622-01B4-4EB8-96D0-E0DEBD89FF0F",
                InvoiceSeries = "F",
                InvoiceNumber = "1127",
                InvoiceCurrencyId = InvoiceCurrency.MXN.ToValue(),
                InvoiceExchangeRate = 1,
                PartialityNumber = 1,
                PreviousBalanceAmount = 98618.388800m,
                PaymentAmount = 90000m,
                RemainingBalance = 8618.3888m,
                TaxObjectId = TaxationObject.YesSubjectToTax.ToValue(),
            };

            //Add payment and invoices (related documents) before adding taxes,
            //because the taxes methods use the *LAST* payment or invoice added to the service. 
            service.AddPayment(payment);
            service.AddPaymentInvoice(paymentInvoice);

            //Add taxes to the LAST invoice of the LAST payment. (withholding or/ and transferred taxes)
            service.AddTransferredTax("002", "Exento");
            service.AddTransferredTax("002", "Tasa", 0.000000m);
            service.AddTransferredTax("002", "Tasa", 0.160000m);
            service.AddTransferredTax("002", "Tasa", 0.080000m);
            service.AddTransferredTax("003", "Tasa", 0.530000m);
            service.AddWithholdingTax("002", "Tasa", 0.106666m);


            service.SignInvoice();
            var xml = service.SerializeToString();
            await File.WriteAllTextAsync("payment-invoice-service2.xml", xml);

            MessageBox.Show(@"Ok");
        }
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
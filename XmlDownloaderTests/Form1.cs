using Credencials.Core;
using XmlDownloader.Core.Helpers;
using XmlDownloader.Core.Services;
using XmlDownloader.Core.Services.Common;

namespace XmlDownloaderTests
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            #region dotnetcfi/credentials

            var cerbase64 = "TuCertificadoEnBase64";
            var keybase64 = "TuClavePrivadaEnBase64";
            var passPhrase = "LaContraseñaDeTuClavePrivada";

            var certificate = new Certificate(cerbase64);
            var privateKey = new PrivateKey(keybase64, passPhrase);

            var credential = new Credential(certificate, privateKey);

            #endregion


            #region dotnetcfdi/xml-downloader

            var xmlService = new XmlService(credential);


            //Configuracion de directorios
            Settings.LogsDirectory = @"C:\DescargaXml\Logs";  //Directorio temporal de trabajo, este directorio es borrado en cada nueva descarga
            Settings.WorkDirectory = @"C:\DescargaXml\Temp";   //Donde se guardan tus paquetes cfdi o Metadata (depende de la propiedad EnableRedundantWriting)
            Settings.PackagesDirectory = @"C:\DescargaXml\Paquetes"; //Directorio de paquetes
            Settings.EnableRedundantWriting = true; //Escribe el paquete en  Settings.PackagesDirectory, y despues en Settings.WorkDirectory para deserializar (su valor por defecto es true, recuerda que el SAT solo permite descargar 2 veces el mismo paquete, si no lo guardas, entonces no lo solo tienes 1 oportunidad más, despues se agotan de por vida la descarga de dicho paquete) 



            //Primer servicio
            var authenticateResult = await xmlService.AuthenticateAsync();

            //Segundo servicio
            var queryParameters = new QueryParameters
            {
                StartDate = new DateTime(2022, 1, 18, 0, 0, 0),
                EndDate = new DateTime(2022, 1, 19, 23, 59, 59),
                EmitterRfc = null, //Si te interesa CFDI o Metadata emitida, entonces informa este parametro
                ReceiverRfc = "TuRFC", //Si te interesa CFDI o Metadata recibida, entonces informa este parametro
                RequestType = RequestType.Metadata, // CFDI | Metadata 
                DownloadType = DownloadType.Received //  Emitidos | Recibidos
            };
            var queryResult = await xmlService.Query(queryParameters);


            //Tercer servicio
            var verifyResul = await xmlService.Verify(queryResult.RequestUuid);
            while (!verifyResul.IsSuccess)
            {
                verifyResul = await xmlService.Verify(queryResult.RequestUuid);
                Thread.Sleep(30_000);
            }

            //Cuarto servicio
            foreach (var packageId in verifyResul.PackagesIds)
            {
                var downloadResul = await xmlService.Download(packageId);


                //Use el este metodo si está descargando cfdi
                //var cfdis = await xmlService.GetCfdisAsync(downloadResul);

                //Use el este metodo si está descargando metadata
                var metadata = await xmlService.GetMetadataAsync(downloadResul);

                //Haga algo con cfdis o metadata.....


            }

            #endregion
        }
    }
}
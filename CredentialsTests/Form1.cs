using System.Text;
using Credencials.Core;

namespace CredentialsTests
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Creating a certificate instance

            var cerbase64 = "TuCertificadoEnBase64";
            var keybase64 = "TuClavePrivadaEnBase64";
            var passPhrase = "LaContraseñaDeTuClavePrivada";

            var certificate = new Certificate(cerbase64);
            var privateKey = new PrivateKey(keybase64, passPhrase);

            var credential = new Credential(certificate, privateKey);



            //Certificate 
            //show certificate basic information
            MessageBox.Show($@"PlainBase64 {certificate.PlainBase64}");
            MessageBox.Show($@"Rfc {certificate.Rfc}");
            MessageBox.Show($@"Razón social {certificate.Organization}");
            MessageBox.Show($@"SerialNumber {certificate.SerialNumber}");
            MessageBox.Show($@"CertificateNumber {certificate.CertificateNumber}");
            MessageBox.Show($@"ValidFrom {certificate.ValidFrom}");
            MessageBox.Show($@"ValidTo {certificate.ValidTo}");
            MessageBox.Show($@"IsFiel { certificate.IsFiel()}");
            MessageBox.Show($@"IsValid { certificate.IsValid()}"); // ValidTo > Today

            //Converts X.509 DER base64 or X.509 DER to X.509 PEM
            var pemCertificate = certificate.GetPemRepresentation();
            File.WriteAllText("MyPemCertificate.pem", pemCertificate);


            //PrivateKey
            //Converts PKCS#8 DER private key to PKCS#8 PEM
            var PemPrivateKey = privateKey.GetPemRepresentation();
            File.WriteAllText("MyPemPrivateKey.pem", PemPrivateKey);



            //Credential
            //Create a credential instance, certificate and privatekey previously created.
            var fiel = new Credential(certificate, privateKey);

            var dataToSign = "Hello world"; //replace with cadena original

            //SignData
            var signedBytes = fiel.SignData(dataToSign);

            //Verify signature
            var originalDataBytes = Encoding.UTF8.GetBytes(dataToSign);
            var isValid = fiel.VerifyData(originalDataBytes, signedBytes);

            //Create pfx file
            var pxfBytes = fiel.CreatePFX();
            File.WriteAllBytes("MyPFX.pfx", pxfBytes);


            //compute and verify hash/digest
            var dataToHash = "xml to get hash"; //replace with canonical representation (c14n) xml. (Descarga masiva xml SAT)
            var hash = fiel.CreateHash(dataToHash); // hash bytes converted into base64 encode.
            var isValidHash = fiel.VerifyHash(dataToHash, hash); // True | False


            //basic info
            MessageBox.Show($@"CredentialType { fiel.CredentialType}");  // Enum: Fiel || Csd
            MessageBox.Show($@"IsValidFiel { fiel.IsValidFiel()}");      // True when (certificate.ValidTo > Today and  CredentialType == Fiel)

        }
    }
}
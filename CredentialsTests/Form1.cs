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

            var cerbase64 =
                "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";
            var keybase64 =
                "MIIFDjBABgkqhkiG9w0BBQ0wMzAbBgkqhkiG9w0BBQwwDgQIAgEAAoIBAQACAggAMBQGCCqGSIb3DQMHBAgwggS+AgEAMASCBMh9+jQFIsJGmRB8Ne0DDLPLROR1hBR2Zzcie0kjyWAyvr4Rj3WSTuGyloRP+vtHyKvApoMQd1QiNAxWSlBpJbyYNUd6U2kcfGZsjmmwQGbqCxDgiLrhUO9VxSxC4WBJrgY2uR8MNMBtkihTfAmoQWM/zifW7BwZyYPyf5DQF/IJjadu13F0VeHDNMDdU2jio9KlQOwB8bDN/3FsMqEurX4y/UHP7wTujFHn0SVujX4dGsrQf5dz/tzVaedtQ3SnloT2IkhwTwNKKeeE5ZuI5fvOT7wQ1eIMbJ4O5sC7p7eieyjPbk8rMnKEm/829WbKvN5Arl6H13DzvSxnBkUtXOxREfIEAMzAg11nRzxyXsxBHL55AOrAhQ3WiJdEerlVqoikYobM3NWbOz4GnVFZogYOJsdCdh1ehgyFrxjfN8ZhSRPwJ4REds2Y8Z93wMK3U7xdRzZ1wCNOW8adgTj3zGNTZ14dKZRp02K33giIgjjXG7gUYYKQgOf0WgTSj1agsfrAjHhR7t0Zico2RjSzSpyGeJn4EfvescgztXq35IvrS42+znclWO0aIBSZYNewSdq9lP7pEsvqgpNbIdeDZgipIesu/pG81z2V34LfcT1lznXInF/49qnVynCYNVJXdsXHDLOC4t5CH6DbTVVkekaUlynAkmdqh88y1sL1a6UBk3ejexnLAGLATyl/yFpSi4Q40XZ3egqjYvStpWGU0/RSnGygnTgjXDzuBYXI4R3KWIVhNYUAoFeaE3BfhrTulLtOPIFAxv7REM+gh68ofI0vFh2eLgejTAZ4h0KNPOOcAYdGOXPXSOu6T6IeTIB0vK0g0izdmHhGJNbrlTOl8zjHmIsU/gatsa1Q/D5lJG3kI6O/68MTFcV9GLEJBTjEvJ8FWEUoPskt8mf3mw4PC60ReiW0diO9aLSoXKNP6K1H+tYIOD+oROJp2UE8cOBixBX1QEXxAusTADOXy788QtGTQv8Dp0i/E+baX75pVio9TSMHaZTCrYHb5bOj3Dm7dDkpqccBLDgdsHQv082iuJsTMu5pZjGUKJqGSYjTLdNV/D6gMyG21tGJpEErBnE4YODEc24d6V1xMJcNZwD9ZGwcXNg1w6M5qAzhzE0mT7STYybhKFqAXb9gfvUmtTXyfZGVWdfMoJH9O71ohAotxrNvPfuTpA4u7k3SLGWaIrrlTuL3ukfIH8ge22bHArUSPmPhQLZqfhUBi+YNwzFbV7GoEsono6Vy2QL/v5fIiubMJbCLDf2FNdD+wKNCm21ie9SULENBe4JEybdAUCR4awUetCvR+WIP4tbJFFZBN1SASXZZxZH7Kh7vh6CEAMoEbnn3jkoQd3Gto/aDl56Tp8/hESyG17LYlQWuCAsQ0wsTnlKgKH+FaYvcbcUqgFCGorfTsaf3UuTRrkxMjLyzjeBru1LVIjz9M8nFkRwTyhtxin3R/cUtrEU9iqM1zNO8sT80CRVK6JHem7CpzT7aZ47WOWB9Zb9xFV8Z0r0o4wlfEWvwoEDhCtnUMjSbRCmyWKxquMFMOBtr+iiDXVHa0shSP96XvMBEFpC5629VQb1ajL9BifLRhtZN235mx+Ol2nXUUEMpM1Tsp0GrrHlwTafLncAMDOcdcWo=";
            var passPhrase = "DGE131017";

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
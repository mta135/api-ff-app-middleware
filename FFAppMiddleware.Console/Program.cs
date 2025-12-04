



using FFappMiddleware.DataBase.EncryptionService;

string SpherusPharma = "Data Source=192.168.1.252;Initial Catalog=Medica_CRM_Loiality;Persist Security Info=True;User ID=sa;Password=ZoobItch3z;TrustServerCertificate=True;MultipleActiveResultSets=true";
string publichKey = "k65gR0Q3E0nKLxNk8A1Ceg==";
string encrypted_SpherusPharma = AesEncryptionHelper.Encrypt(SpherusPharma, publichKey);


string SpherusMain = "Data Source=192.168.1.252;Initial Catalog=Medica_CRM_Loiality;Persist Security Info=True;User ID=sa;Password=ZoobItch3z;TrustServerCertificate=True;MultipleActiveResultSets=true";

string encrypted_SpherusMain = AesEncryptionHelper.Encrypt(SpherusMain, publichKey);


string SpherusPharmaFF = "Data Source=192.168.1.22;Initial Catalog=SPHERUS_PHARMA_FF;Persist Security Info=True;User ID=sa;Password=ZoobItch3z;TrustServerCertificate=True;MultipleActiveResultSets=true";
string encrypted_SpherusPharmaFF = AesEncryptionHelper.Encrypt(SpherusPharmaFF, publichKey);

string connectionString = encrypted_SpherusPharmaFF; //AesEncryptionHelper.Decrypt(x, publichKey);
var x = encrypted_SpherusPharmaFF;






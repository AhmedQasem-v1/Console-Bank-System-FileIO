namespace BankProject
{
    public static class ErrorHandler
    {

        public static void HandlerError(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"\n[CRITICAL ERROR -{DateTime.Now}]");
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            System.Diagnostics.Debug.WriteLine("----------------------------------------------");

            if (ex is FormatException)
            {
                UIHelper.PrintError(" Input Error: Please enter valid numbers only.");

            }
            else if (ex is InvalidOperationException)
            {
                UIHelper.PrintError($" Operation Error: {ex.Message}");

            }
            else if (ex is ArgumentException || ex is ArgumentNullException)
            {
                UIHelper.PrintError($" Data Error: {ex.Message}");

            }
            // 🛡️ درع دالة الـ Load (الاسترنق العائلي اللي فيه الأسطر الخربانة)
            else if (ex is InvalidDataException)
            {

                UIHelper.PrintWarning(" Data Load Warning: Some data was corrupted.");
                Console.WriteLine(ex.Message); // بيطبع الاسترنق العائلي هنا
                                               //الاسترنق العائلي باي لون يكون حلو لانه الاصفر بيخلي المبرمج يصير احول لو الاخطاء كثيره 
            }

            // 🛡️ درع دالة الـ Save (حماية مايكروسوفت: الملف مقفول، مسار غلط، هارد مليان)
            else if (ex is IOException || ex is UnauthorizedAccessException)
            {
                UIHelper.PrintError(" Storage Error: System cannot access or save the data file.");
                UIHelper.PrintError("   Check if the file is open in another program or if the disk is full.");
            }
            else
            {
                UIHelper.PrintError(" Unexpected System Error.");
                UIHelper.PrintError($"   Please contact support with Code: {ex.GetType().Name}");
                //{ex.GetType().Name}هذه تطبع الاسم حق الخطاء حسب ما فهمت في اي داله واين كان 
            }

        }


    }
}
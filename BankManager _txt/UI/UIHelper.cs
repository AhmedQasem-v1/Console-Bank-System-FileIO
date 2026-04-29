namespace BankProject
{
    public static class UIHelper
    {
        // تعريف الديليجيت
        public delegate bool BankOperation(out string message);

        // 1. دالة التأكيد (بدون كتابة اسم الكلاس قبلها!)
        public static bool GetConfirmation(string message)
        {
            while (true)
            {
                PrintWarning($"{message} (Press Enter to confirm, Spacebar to cancel)");
                var key = Console.ReadKey(intercept: true).Key;

                if (key == ConsoleKey.Enter) return true;
                if (key == ConsoleKey.Spacebar) return false;
            }
        }

        // 2. دالة تنفيذ العمليات باستخدام Func
        public static void ExecuteTransaction(Func<bool> operation, string successMsg, string errorMsg)
        {
            try
            {
                bool result = operation();
                if (result)
                {
                    PrintSuccess($"{successMsg ?? "Completed successfully."}");
                }
                else
                {
                    PrintError($"{errorMsg ?? "Error occurred."}");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandlerError(ex);
                Forprass("\nPress any key to exit...");
                Console.ReadLine();
            }
        }

        // 3. دالة تنفيذ العمليات باستخدام Delegate (مع رسالة Out)
        public static bool BankOpr(BankOperation opr)
        {
            try
            {
                string resultMessage;
                bool isSuccess = opr(out resultMessage);
                if (isSuccess)
                {
                    PrintSuccess(resultMessage);
                    return true;
                }

                PrintError(resultMessage);
                return false;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandlerError(ex);
                return false;
            }
        }

        // --- دوال الألوان والطباعة ---

        public static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✅ {message}");
            Console.ResetColor();
        }

        public static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ {message}");
            Console.ResetColor();
        }

        public static void PrintWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠️ {message}");
            Console.ResetColor();
        }

        public static void Forprass(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"⚠️ {message}");
            Console.ResetColor();
        }

        public static void PrintHeader(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n==================================================");
            Console.WriteLine($"\t\t{title}");
            Console.WriteLine("==================================================\n");
            Console.ResetColor();
        }
    }
}
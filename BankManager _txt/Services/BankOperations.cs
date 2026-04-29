namespace BankProject
{ 
        public static class BankOperations
        {
            public static string PrintMenuAndGetChoice()
            {
                Console.Clear();
                UIHelper.PrintHeader("🏦 Main Menu - Bank System");
                Console.WriteLine("1. Add New Client");
                Console.WriteLine("2. Delete Client");
                Console.WriteLine("3. Update Client");
                Console.WriteLine("4. Find Client");
                Console.WriteLine("5. Print All Clients");
                Console.WriteLine("6. Transactions (Transfer)");
                Console.WriteLine("7. Find Richest Client"); 
                Console.WriteLine("8. Sort Clients");        
                Console.WriteLine("0. Exit");
                Console.WriteLine(new string('-', 30));
                Console.Write("👉 Choose an option (0-8): "); 

                return Console.ReadLine()!;
            }

            public static void AddNewClient(BankManager manager)
            {
                while (true) 
                {
                    bool flag = false; 
                    string? name = null;
                    string? id = null;
                    decimal balance = 0;

                    // المحطة 1: إدخال الاسم
                    Console.Clear();
                    UIHelper.PrintHeader("Add New Client Screen");
                    while (!flag) 
                    {
                        Console.WriteLine("Enter Client Name:");
                        UIHelper.PrintWarning("Note: Name should not contain numbers.");
                        name = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(name))
                        {
                            // خيار 1: إعادة محاولة نفس السؤال (الاسم)
                            if (UIHelper.GetConfirmation(" Name cannot be empty. Do you want to try again?")) continue;

                            // خيار 2: الهروب للمين منيو
                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;

                            // خيار 3: إعادة النموذج من الصفر
                            flag = true; // 🚩 نرفع العلم الأحمر
                            break;       // نكسر هذا اللوب الصغير
                        }

                        // فحص الأرقام
                        if (name.Any(char.IsDigit))
                        {
                            if (UIHelper.GetConfirmation(" Name cannot contain numbers. Do you want to try again?")) continue;
                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                            flag = true;
                            break;
                        }

                        break; // ✅ الاسم سليم، ننتقل للمحطة التالية
                    }

                    // =================================================
                    // المحطة 2: إدخال الآيدي
                    // (ملاحظة ذكية: لو flag=true، الشرط while(!flag) يصير خطأ، فيقفز الكود فوراً ويتجاوز هذه المحطة)
                    // =================================================
                    while (!flag)
                    {
                        Console.WriteLine("Enter Client ID (6 digits):");
                        UIHelper.PrintWarning("Note: ID must be exactly 6 digits and contain only numbers.");
                        id = Console.ReadLine();

                        // فحص الطول
                        if (string.IsNullOrWhiteSpace(id) || id.Length != 6)
                        {
                            if (UIHelper.GetConfirmation(" ID must be exactly 6 digits. Do you want to try again?")) continue; // يعيد سؤال الآيدي
                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                            flag = true; // نرفع العلم لإلغاء العملية كاملة
                            break;
                        }

                        if (manager.searchClient(id) != null) // إذا لم يرجع null، يعني العميل موجود!
                        {
                            UIHelper.PrintError($" Security Alert: The ID '{id}' is already registered to another client!");

                            // نفس نظامك الجميل في التعامل مع المستخدم
                            if (UIHelper.GetConfirmation(" Do you want to try a different ID?")) continue;
                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                            flag = true;
                            break;
                        }
                        // فحص الأرقام فقط
                        if (!id.All(char.IsDigit))
                        {
                            if (UIHelper.GetConfirmation(" ID must contain only digits. Do you want to try again?")) continue;
                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                            flag = true;
                            break;
                        }

                        break; // ✅ الآيدي سليم، ننتقل للمحطة التالية
                    }

                    // =================================================
                    // المحطة 3: إدخال الرصيد
                    // (أيضاً سيتم القفز عنها لو flag=true)
                    // =================================================
                    while (!flag)
                    {
                        Console.WriteLine("Enter Initial Balance:");
                        UIHelper.PrintWarning("Note: Balance cannot be negative.");

                        // فحص التحويل لرقم
                        if (!decimal.TryParse(Console.ReadLine(), out balance))
                        {
                            if (UIHelper.GetConfirmation(" Invalid balance format. Please enter a valid decimal number. Do you want to try again?")) continue;
                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                            flag = true; break;
                        }

                        // فحص السالب
                        if (balance < 0)
                        {
                            if (UIHelper.GetConfirmation(" The number must not be negative ")) continue;
                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                            flag = true; break;
                        }

                        break; // ✅ الرصيد سليم
                    }

                    // =================================================
                    // نقطة التفتيش الرئيسية
                    // =================================================
                    // هنا نسأل: هل مررنا بجميع المحطات بسلام؟ أم رفعنا العلم الأحمر في الطريق؟
                    if (flag == true) continue; // 🛑 ارجع لبداية "الدائرة الكبيرة" (اسم جديد)

                    // =================================================
                    // التنفيذ والحفظ
                    // =================================================
                    var newClient = new Client(name!, id!, balance);

                    // تنفيذ العملية باستخدام اللامبدا (شغل نظيف)
                    UIHelper.ExecuteTransaction(() => manager.AddClient(newClient),
                        " Client added successfully.",
                        " Failed to add client. Please try again.");

                    // السؤال الختامي: هل تريد إضافة عميل آخر؟
                    if (!UIHelper.GetConfirmation("Do you want to try adding a client again?"))
                    {
                        break; // 🚪 خروج نهائي من الدالة
                    }
                }
            }

            public static void AmendmentClient(BankManager manager)
            {
                while (true) 
                {
                    bool flag = false;     
                    bool menuOpen = false;  
                    string? searchId =null;
                    string? newName = null;
                    string? newId = null;
                    decimal newBalance = 0;
                    Client? client = null;
                    try
                    {
                        UIHelper.PrintHeader("Amendment Client Method Called");

                        while (!flag)
                        {

                            Console.Write("Enter Client ID to amend:  ");
                            searchId = Console.ReadLine();
                            client = manager.searchClient(searchId!);
                            if (string.IsNullOrEmpty(searchId)) {
                                if (UIHelper.GetConfirmation(" The ID should not be blank.")) continue;

                                if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                            }

                            if (client == null)
                            {
                                if (UIHelper.GetConfirmation(" Client not found in our records.")) continue;

                                if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;

                                flag = true; 
                                break;
                            }
                            break;
                        }

                        if (flag == true) continue;

                        while (!menuOpen)
                        {
                            Console.WriteLine("Choose field to amend:");
                            Console.WriteLine("1. Name");
                            Console.WriteLine("2. ID");
                            Console.WriteLine("3. Balance");
                            string? chooser = Console.ReadLine();
                            switch (chooser)
                            {
                                case "1": 
                                    while (!flag)
                                    {
                                        Console.Write("Enter new Name:");
                                        newName = Console.ReadLine();
                                        if (string.IsNullOrEmpty(newName))
                                        {
                                            if (UIHelper.GetConfirmation(" The name should not be blank.")) continue;
                                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                                            flag = true; 
                                            break;
                                        }
                                        if (newName.Any(char.IsDigit))
                                        {
                                            if (UIHelper.GetConfirmation(" The name must not contain numbers.")) continue;
                                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                                            flag = true; 
                                            break;
                                        }
                                        break;
                                    }

                                    if (flag == true)
                                    {
                                        menuOpen = true; 
                                        break;           
                                    }

                                    manager.AmendmentName(searchId!, newName!);
                                    UIHelper.PrintSuccess(" Name updated successfully.");
                                    Console.ReadLine();
                                    menuOpen = true;
                                    break;

                                case "2":
                                    while (!flag)
                                    {
                                        Console.Write("Enter new ID (6 digits):  ");
                                        newId = Console.ReadLine();
                                        if (string.IsNullOrWhiteSpace(newId) || newId.Length != 6)
                                        {
                                            if (UIHelper.GetConfirmation(" The ID must contain 6 digits")) continue;
                                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                                            flag = true;
                                            break;
                                        }
                                        if (manager.searchClient(newId) != null) 
                                        {
                                            UIHelper.PrintError($" Security Alert: The ID '{newId}' is already registered to another client!");
                                            if (UIHelper.GetConfirmation(" Do you want to try a different ID?")) continue;
                                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                                            flag = true;
                                            break;
                                        }
                                        if (!newId.All(char.IsDigit))
                                        {
                                            if (UIHelper.GetConfirmation(" The ID must not contain letters.")) continue;
                                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                                            flag = true;
                                            break;
                                        }
                                        break;
                                    }

                                    if (flag == true)
                                    {
                                        menuOpen = true; 
                                        break;
                                    }

                                    manager.AmendmentId(searchId!, newId!);
                                    UIHelper.PrintSuccess(" ID updated successfully.");
                                    Console.ReadLine();
                                    menuOpen = true; 
                                    break;

                                case "3": 
                                    while (!flag)
                                    {
                                        Console.Write("Enter new Balance:  ");
                                        if (!decimal.TryParse(Console.ReadLine(), out newBalance))
                                        {
                                            if (UIHelper.GetConfirmation(" Invalid balance format. Please enter a valid decimal number.")) continue;
                                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                                            flag = true;
                                            break;
                                        }
                                        if (newBalance < 0)
                                        {
                                            if (UIHelper.GetConfirmation(" The reading should not be negative.")) continue;
                                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                                            flag = true;
                                            break;
                                        }
                                        break;
                                    }

                                    if (flag == true)
                                    {
                                        menuOpen = true; 
                                        break;
                                    }

                                    
                                    manager.AmendmentBalance(searchId!, newBalance!);
                                    UIHelper.PrintSuccess(" Balance updated successfully.");
                                    Console.ReadLine();
                                    menuOpen = true;
                                    break;

                                default: 

                                    if (UIHelper.GetConfirmation("Invalid choice. Please select 1, 2, or 3.")) continue; // يعيد القائمة
                                    if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return; // خروج نهائي

                                    menuOpen = true; 
                                    flag = true;     
                                    break;
                            }


                            if (menuOpen == true) break; 
                        }

                        if (flag == true) continue;
                    }

                    catch (Exception ex)
                    {
                        ErrorHandler.HandlerError(ex);
                        UIHelper.Forprass("\nPress any key to exit...");
                        Console.ReadLine();
                    }

                    if (!UIHelper.GetConfirmation("Do you want to have another operation?")) break;
                }
            }

            public static void DeleteClient(BankManager manager)
            {
                while (true)
                {
                    bool flag = false;
                    Client? client = null;
                    try
                    {
                        UIHelper.PrintHeader("Delete Client Screen");
                        while (!flag)
                        {
                            Console.Write("Enter ID to delete : ");
                            string? deleteId = Console.ReadLine();
                            client = manager.searchClient(deleteId!);
                            if (string.IsNullOrEmpty(deleteId))
                            {
                                if (UIHelper.GetConfirmation(" The ID should not be blank.")) continue;

                                if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                            }
                            if (client == null)
                            {

                                if (UIHelper.GetConfirmation(" Sorry, no client found with this ID.")) continue;
                                if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Change Client")) return;
                                flag = true;
                                break;

                            }
                            break;
                        }
                        while (!flag)
                        {
                            Console.WriteLine($"Client Found: {client?.Name} | Balance: {client?.Balance:C}");


                            if (!UIHelper.GetConfirmation("Are you sure you want to delete this client?"))
                            {

                                if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Change Client")) return;

                                flag = true;
                                break;

                            }
                            UIHelper.ExecuteTransaction(() => manager.RemoveClient(client!), " Delete is completed successfully",
                               "The operation failed. Please try again.");
                            break;
                        }
                        if (flag == true) continue;
                        while (!flag)
                        {
                            if (!UIHelper.GetConfirmation("Do you want to undo the deletion?  "))
                            {

                                break;

                            }
                            UIHelper.ExecuteTransaction(() => manager.UndoDelete(client!), " Client restored successfully",
                                "The operation failed. Please try again.");
                            break;
                        }





                    }


                    catch (Exception ex)
                    {
                        ErrorHandler.HandlerError(ex);
                        UIHelper.Forprass("\nPress any key to exit...");
                        Console.ReadLine(); ;
                    }

                    if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Change Client again")) break;







                }
            }

            public static void HandleSearch(BankManager manager)
            {
                Client? client = null;
                while (true)
                {
                    try
                    {
                        UIHelper.PrintHeader("Search Client Screen");
                        Console.Write("Enter ID to search : ");
                        string? searchId = Console.ReadLine();
                        client = manager.searchClient(searchId!);
                        if (string.IsNullOrEmpty(searchId))
                        {
                            if (UIHelper.GetConfirmation(" The ID should not be blank.")) continue;

                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                        }
                        if (client == null)
                        {
                            if (!UIHelper.GetConfirmation(" Sorry, no client found with this ID. ")) return;

                            continue;


                        }
                        UIHelper.PrintSuccess(" Client Found: ");
                        Console.WriteLine(client.ToString());
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.HandlerError(ex);
                        UIHelper.Forprass("\nPress any key to exit...");
                        Console.ReadLine();
                    }
                    if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Search again")) break;
                }
            }

            public static void ShowRichestClients(BankManager manager)
            {

                UIHelper.PrintHeader("--- 💰 Top Richest Clients 💰 ---");
                try
                {
                    List<Client> SRC = manager.GetRichestClient();
                    Console.WriteLine($"Found {SRC.Count} client(s) with the highest balance:");
                    foreach (Client c in SRC)
                    {
                        Console.WriteLine($"{c.ToString()}");
                        Console.WriteLine("------------------------------------------------");
                    }
                }

                catch (Exception ex)
                {
                    ErrorHandler.HandlerError(ex);
                    UIHelper.Forprass("\nPress any key to exit...");
                    Console.ReadLine();
                }
                UIHelper.Forprass("List is end Press any key for exit");
                Console.ReadLine();
            }

            public static void PrintAllClients(BankManager manager)
            {
                try
                {
                    UIHelper.PrintHeader("All Bank Clients");
                    var AllClients = manager.PrintAllClients();
                    foreach (Client c in AllClients)
                    {
                        Console.WriteLine(c.ToString());
                        Console.WriteLine(new string('-', 30));


                    }
                }
                catch (Exception ex)
                {
                    UIHelper.PrintError(ex.Message);
                }
                UIHelper.Forprass("\nPress any key to return to Main Menu...");
                Console.ReadLine();
            }
            public static void Sorting(BankManager manager)
            {
                Console.Clear();
                Console.WriteLine("----sorting Client from Big Balance---");
                try
                {
                    List<Client> sort = manager.SortClients();
                    Console.WriteLine($"Count is {sort.Count}");
                    foreach (Client c in sort)
                    {
                        Console.WriteLine($"{c.ToString()}");
                        Console.WriteLine("---------------------------------------------------");

                    }
                }


                catch (Exception ex)
                {
                    ErrorHandler.HandlerError(ex);
                    Console.WriteLine("\nPress any key to exit...");
                    Console.ReadLine();
                }
                Console.WriteLine("End of list. Press any key to return to Main Menu..");
                Console.ReadLine();

            }
            public static void Withdrawal(BankManager manager)
            {
                if (manager == null)
                {
                    UIHelper.PrintError("Bank manager instance is null.");
                    return;
                }
                while (true)
                {

                    try
                    {
                        UIHelper.PrintHeader("Withdrawal Screen");
                        bool flag = false;
                        Console.Write("Enter ID : ");
                        string? searchId = Console.ReadLine();
                        if (string.IsNullOrEmpty(searchId))
                        {
                            if (UIHelper.GetConfirmation(" The ID should not be blank.")) continue;

                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                        }
                        var client = manager.searchClient(searchId!);
                        if (client == null)
                        {

                            if (UIHelper.GetConfirmation("Sorry, no client found. Do you want to try again?")) continue;

                            break;

                        }
                        while (true)
                        {
                            UIHelper.PrintHeader("Withdrawal Screen");
                            Console.WriteLine($"\nClient: {client.Name} | Current Balance: {client.Balance:C}");
                            Console.Write("Enter Amount: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                            {


                                UIHelper.PrintError("Error: Invalid amount, or insufficient balance. ");
                                if (UIHelper.GetConfirmation("Try again?")) continue;

                                if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Change Client")) return;
                                flag = true;

                                break;
                            }
                            if (amount <= 0)
                            {
                                if (UIHelper.GetConfirmation(" Invalid amount. Please enter a positive number.")) continue;
                                if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Change Client")) return;

                                flag = true;

                                break;

                            }
                            if (amount > client.Balance)
                            {
                                UIHelper.PrintError(" Insufficient funds for withdrawal.");
                                if (UIHelper.GetConfirmation("Try again?")) continue;
                                if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Change Client")) return;
                                flag = true;

                                break;

                            }


                            UIHelper.BankOpr((out string msg) => manager.WithdrawManager(searchId!, amount, out msg));



                            break;

                        }

                        if (flag == true) continue;
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.HandlerError(ex);
                        UIHelper.Forprass("\nPress any key to exit...");
                        Console.ReadLine();
                    }
                    if (!UIHelper.GetConfirmation("Do you want to perform another operation?"))
                    {
                        break; 
                    }
                }
            }
            public static void Deposit(BankManager manager)
            {
                if (manager == null)
                {
                    UIHelper.PrintError("Bank manager instance is null.");
                    return;

                }

                while (true)
                {

                    try
                    {
                        UIHelper.PrintHeader("Deposit Screen");
                        bool flag = false;
                        Console.Write("Enter ID :");
                        string? searchId = Console.ReadLine();
                        if (string.IsNullOrEmpty(searchId))
                        {
                            if (UIHelper.GetConfirmation(" The ID should not be blank.")) continue;

                            // سيناريو 2: الخروج للقائمة الرئيسية
                            if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                        }
                        var client = manager.searchClient(searchId!);
                        if (client == null)
                        {


                            if (UIHelper.GetConfirmation(" Sorry, no client found with this ID. Try again?")) continue;

                            break;
                        }


                        while (true)
                        {
                            UIHelper.PrintHeader("Deposit Screen");

                            Console.WriteLine($"\nClient: {client.Name} | Current Balance: {client.Balance:C}");
                            Console.Write("Enter Amount: ");

                            //string input;
                            //input = Console.ReadLine();
                            //decimal amount;
                            //if (!decimal.TryParse(input, out amount))
                            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                            {
                                if (UIHelper.GetConfirmation("Error: Invalid amount.please enter a positive number. ")) continue;


                                if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Change Client")) return;

                                flag = true;
                                break;
                            }
                            if (amount <= 0)
                            {
                                if (UIHelper.GetConfirmation(" Invalid amount . Please enter a positive number.")) continue;

                                if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Change Client")) return;
                                flag = true;
                                break;
                            }

                            UIHelper.BankOpr((out string msg) => manager.DepositManager(searchId!, amount, out msg));




                            break;
                        }
                        if (flag == true) continue;
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.HandlerError(ex);
                        UIHelper.Forprass("\nPress any key to exit...");
                        Console.ReadLine();

                    }

                    if (!UIHelper.GetConfirmation("Do you want to perform another operation?"))
                    {
                        break; // خروج نهائي
                    }
                }





            }

            public static void HandleTransfer(BankManager manager)
            {
                while (true)
                {
                    bool flag = false;
                    Client? sender = null;
                    string? senderId = null;
                    Client? recipient = null;
                    string? recipientId = null;
                    decimal amount = 0;
                    string? message = null;

                    try
                    {


                        while (true)
                        {
                            UIHelper.PrintHeader("HandleTransfer Screen");
                            Console.Write("Enter Sender ID : ");
                            senderId = Console.ReadLine();
                            if (string.IsNullOrEmpty(senderId))
                            {
                                if (UIHelper.GetConfirmation(" The ID should not be blank.")) continue;

                                if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                            }
                            sender = manager.searchClient(senderId!);
                            if (sender == null)
                            {


                                if (!UIHelper.GetConfirmation(" Sender not found.\n Sender not found. Do you want to try again? ")) return;
                                continue;

                            }
                            break;
                        }

                        while (!flag)
                        {
                            Console.WriteLine($"Sender Found: {sender.Name} | Balance: {sender.Balance:C}");

                            Console.Write("Enter Recipient ID : ");
                            recipientId = Console.ReadLine();
                            if (string.IsNullOrEmpty(recipientId))
                            {
                                if (UIHelper.GetConfirmation(" The ID should not be blank.")) continue;

                                if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Try Again")) return;
                            }
                            recipient = manager.searchClient(recipientId!);
                            if (recipient == null)
                            {
                                if (UIHelper.GetConfirmation(" Recipient not found.")) continue;
                                if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Change Client")) return;
                                flag = true;
                                break;
                            }
                            if (senderId == recipientId)
                            {
                                if (UIHelper.GetConfirmation(" Sender and recipient cannot be the same.")) continue;
                                if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Change Client")) return;
                                flag = true;
                                break;

                            }
                            break;
                        }
                        while (!flag)
                        {
                            Console.WriteLine($"Recipient Found: {recipient?.Name}");
                            Console.WriteLine("Enter Amount to Transfer : ");
                            if (!decimal.TryParse(Console.ReadLine(), out amount))
                            {
                                if (UIHelper.GetConfirmation(" Error: Invalid amount.")) continue;
                                if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Change Client")) return;
                                flag = true;
                                break;

                            }
                            if (amount <= 0)
                            {
                                if (UIHelper.GetConfirmation(" Error: Write a number greater than zero")) continue;
                                if (!UIHelper.GetConfirmation("[Space] = Main Menu | [Enter] = Change Client")) return;
                                flag = true;
                                break;

                            }
                            break;
                        }
                        if (flag == true) continue;



                        bool success = manager.Transfer(senderId!, recipientId!, amount, out message);
                        if (!success)
                        {
                            UIHelper.PrintError(message);
                            if (!UIHelper.GetConfirmation("Transfer failed. Do you want to try again?")) return;
                            flag = true;
                            continue;

                        }
                        if (success)
                        {
                            UIHelper.PrintSuccess(message);
                            UIHelper.Forprass("Press any key to continue...");
                            Console.ReadKey();
                        }
                        if (flag == true) continue;
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.HandlerError(ex);
                        UIHelper.Forprass("\nPress any key to exit...");
                        Console.ReadLine();
                    }
                    if (!UIHelper.GetConfirmation("Do you want another operation?")) break;
                }


            }

        }
}



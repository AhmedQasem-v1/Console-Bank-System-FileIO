namespace BankProject
{

    class Program
    {

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; 
            BankManager? first = null;
            try
            {

                first = new BankManager("alyaseen", "alyaseen.txt");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandlerError(ex);
                UIHelper.Forprass("\nPress any key to exit. Please fix the file and restart...");
                Console.ReadLine();
                return; 
            }


            while (true)
            {
                try
                {

                    switch (BankOperations.PrintMenuAndGetChoice())
                    {
                        case "1":
                            BankOperations.AddNewClient(first);

                            break;

                        case "2":
                            BankOperations.DeleteClient(first);
                            break;

                        case "3":
                            BankOperations.AmendmentClient(first);
                            break;

                        case "4":
                            BankOperations.HandleSearch(first);
                            break;

                        case "5":
                            BankOperations.PrintAllClients(first);
                            break;
                        case "6":
                            BankOperations.HandleTransfer(first);
                            break;
                        case "7":
                            BankOperations.ShowRichestClients(first);
                            break;

                        case "8":
                            BankOperations.Sorting(first);
                            break;
                        case "0": return;

                        default:

                            UIHelper.PrintError("Error: Invalid option. Please choose a valid number from the menu (0-8).");
                            UIHelper.Forprass("\nPress any key to try again...");
                            Console.ReadLine();

                            continue;
                    }





                }
                catch (Exception ex)
                {
                    ErrorHandler.HandlerError(ex);
                    UIHelper.Forprass("\nPress any key to exit...");
                    Console.ReadLine();

                }

                if (UIHelper.GetConfirmation("Do you need another transaction from the bank?")) continue;
                UIHelper.PrintSuccess("Thank you for using Alyaseen Bank. Goodbye!");
                break;
            }






        }




    }




}// first

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace LibraryTerminal
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Item> itemsLoaded = new List<Item>();            
            StreamReader reader = new StreamReader("../../../SavedItems.txt");
            string line = reader.ReadLine();
            while (line != null)
            {
                string[] itemEntryInfo = line.Split("|"); // Record line
                Item newItem = GenerateItem(itemEntryInfo);
                itemsLoaded.Add(newItem);
                line = reader.ReadLine();
            }
            reader.Close();

            Library L = new Library(itemsLoaded);

            bool userContinue = true;

            Console.WriteLine("Welcome to the Library!");
            while (userContinue)
            {
                LibraryMenu();
                string input = CnslFormatter.PromptForInput($"What would you like to do? ");

                if(input == "1")
                {
                    L.PrintItems();
                    bool proceed = CnslFormatter.AskYesOrNo($"Would you like to check out an item?");
                    if(proceed)
                    {
                        L.Checkout(L.Catalog);
                    }
                }
                else if (input == "2")
                {
                    input = CnslFormatter.PromptForInput("Please enter name to search: ");
                    List<Item> resultsAuthor = L.SearchByAuthor(input);

                    if (resultsAuthor.Count == 0)
                    {
                        Console.WriteLine("No items found.");
                        CnslFormatter.PauseByAnyKey();
                    }
                    else if (resultsAuthor.Count >= 1)
                    {
                        foreach (Item result in resultsAuthor)
                        {
                            result.PrintInfo();
                        }
                        bool proceed = CnslFormatter.AskYesOrNo($"Would you like to check out an item?");
                        if (proceed)
                        {
                            L.Checkout(resultsAuthor);
                        }
                    }
                }
                else if (input == "3")
                {
                    input = CnslFormatter.PromptForInput("Please enter title to search: ");
                    List<Item> resultsTitle = L.SearchByTitle(input);

                    if (resultsTitle.Count == 0)
                    {
                        Console.WriteLine("No items found.");
                        CnslFormatter.PauseByAnyKey();
                    }
                    else if (resultsTitle.Count >= 1)
                    {
                        foreach (Item result in resultsTitle)
                        {
                            result.PrintInfo();
                        }
                        bool proceed = CnslFormatter.AskYesOrNo($"Would you like to check out an item?");
                        if (proceed)
                        {
                            L.Checkout(resultsTitle);
                        }
                    }
                }
                else if (input == "4")
                {
                    List<Item> results = new List<Item>();
                    foreach (Item itemMatch in L.Catalog)
                    {
                        if (itemMatch.Status == ItemStatus.CheckedOut)
                        {
                            results.Add(itemMatch);
                        }
                    }
                    if (results.Count <= 0)
                    {
                        Console.WriteLine("No matches found");
                        CnslFormatter.PauseByAnyKey();
                    }
                    else if (results.Count >= 1)
                    foreach (Item result in results)
                    {
                        result.PrintInfo();
                    }
                CnslFormatter.PauseByAnyKey();
                }
                else if (input == "5")
                {
                    bool proceed = CnslFormatter.AskYesOrNo($"Would you like to check in an item?");
                    if (proceed)
                    {
                        L.CheckIn(L.Catalog);
                    }
                }
                else if (input == "6")
                {
                    userContinue = false;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again. ");
                    CnslFormatter.PauseByAnyKey();
                }
            }
            Console.WriteLine("Saving Library Content!");
            
            File.WriteAllText("../../../SavedItems.txt", string.Empty); // Clear the File
            StreamWriter writer = new StreamWriter("../../../SavedItems.txt"); // Should generate new file if deleted
            List<Item> itemsSaved = L.Catalog;
            foreach (Item item in itemsSaved)
            {
                string itemEntry = GenerateEntry(item);
                writer.WriteLine(itemEntry);
            }
            writer.Close();


            Console.WriteLine("Goodbye!");
        }
        public static void LibraryMenu()
        {
            Console.WriteLine($"\t1. Display items.");
            Console.WriteLine($"\t2. Search for a book by author.");
            Console.WriteLine($"\t3. Search for a book by title.");
            Console.WriteLine($"\t4. Show items you have checked out.");
            Console.WriteLine($"\t5. Return a book.");
            Console.WriteLine($"\t6. Quit.");
        }
        public static bool UserContinue(string message)
        {
            Console.WriteLine(message);
            string proceed = Console.ReadLine().Trim().ToLower();

            while (proceed != "n" && proceed != "y")
            {
                Console.WriteLine("Invalid input. Please enter 'y' to continue or 'n' to exit.");
                proceed = Console.ReadLine().Trim().ToLower();
            }

            if (proceed == "y")
            {
                return true;
            }
            else
                return false;
        }
        public static Item GenerateItem(string[] itemInfo)
        {
            string itemType = itemInfo[0].ToLower();
            if (itemType.Equals("book"))
            {
                return new Book(itemInfo[1], itemInfo[2], (ItemStatus)Enum.Parse(typeof(ItemStatus), itemInfo[3], true), DateTime.Parse(itemInfo[4]), int.Parse(itemInfo[5]), int.Parse(itemInfo[6]));
            }
            else if (itemType.Equals("cd"))
            {
                return new CD(itemInfo[1], itemInfo[2], (ItemStatus)Enum.Parse(typeof(ItemStatus), itemInfo[3], true), DateTime.Parse(itemInfo[4]), int.Parse(itemInfo[5]), itemInfo[6]);
            }
            else if (itemType.Equals("dvd"))
            {
                return new DVD(itemInfo[1], itemInfo[2], (ItemStatus)Enum.Parse(typeof(ItemStatus), itemInfo[3], true), DateTime.Parse(itemInfo[4]), int.Parse(itemInfo[5]), int.Parse(itemInfo[6]));
            }
            else if (itemType.Equals("magazine"))
            {
                return new Magazine(itemInfo[1], itemInfo[2], (ItemStatus)Enum.Parse(typeof(ItemStatus), itemInfo[3], true), DateTime.Parse(itemInfo[4]), int.Parse(itemInfo[5]), int.Parse(itemInfo[6]));
            }
            return null;
        }

        public static string GenerateEntry(Item item)
        {
            string itemEntry = "";
            string itemType = item.GetType().Name;

            itemEntry += itemType + "|";
            itemEntry += item.Title + "|";
            itemEntry += item.Author + "|";
            itemEntry += (int)item.Status + "|";
            itemEntry += item.DueDate + "|";
            itemEntry += item.ReleaseYear + "|";

            if (itemType.Equals("Book"))
            {
                itemEntry += ((Book)item).PageCount;
            }
            else if (itemType.Equals("CD"))
            {
                itemEntry += ((CD)item).Tracks;
            }
            else if (itemType.Equals("DVD"))
            {
                itemEntry += ((DVD)item).RunTime;
            }
            else if (itemType.Equals("Magazine"))
            {
                itemEntry += ((Magazine)item).PublishMonth;
            }
            return itemEntry;
        }
    }
}

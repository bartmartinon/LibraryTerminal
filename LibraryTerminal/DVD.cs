﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LibraryTerminal
{
    class DVD : Item
    {
        public int RunTime { get; set;}

        public DVD(string Title, string Author, int ReleaseYear, int RunTime) : base(Title, Author, ReleaseYear)
        {
            this.RunTime = RunTime;
        }
        public override void PrintInfo()
        {
            Console.WriteLine($"\nTitle: {Title}");
            Console.WriteLine($"Director: {Author}");
            Console.WriteLine($"Year Released: {ReleaseYear}");
            Console.WriteLine($"RunTime: {RunTime} minutes");
            if (ItemStatus.Equals("CheckedOut"))
            {
                Console.WriteLine($"Return Date: {DueDate}");
            }
            else
            {
                Console.WriteLine($"Status: {ItemStatus}");
            }
        }
    }
}

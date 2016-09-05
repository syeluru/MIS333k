//Author: Sai Yeluru
//Date: September 6, 2016
//Assignment: Homework 1
//Description: 

using System;

namespace Yeluru_Sai_HW1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Intro to the application
            Console.WriteLine("Welcome to the UT Football Ticket Checkout.");

            // Gather and validate
            int intGenAdmTix = gatherAndValidateGenAdmTix();
            int intPremAdmTix = gatherAndValidatePremAdmTix();

            // Now validate that both are not zero or negative
            validateBothTix(intGenAdmTix, intPremAdmTix);

            // Now display all the required outputs
            calculateAndDisplayOutputs(intGenAdmTix, intPremAdmTix);

            //include code to pause
            Console.WriteLine("Press any key to close...");
            Console.ReadLine();

        }

        public static int gatherAndValidateGenAdmTix()
        {
            // Gather number of general admission tickets
            Console.WriteLine("Please enter how many general admission tickets you would like: ");
            String strGenAdmTix;
            strGenAdmTix = Console.ReadLine();
            // Quick validation on general admission tickets
            int intGenAdmTix;
            intGenAdmTix = QuickCheckTix(strGenAdmTix, "General Admission");
            return intGenAdmTix;
        }

        public static int gatherAndValidatePremAdmTix()
        {
            // Gather number of premium admission tickets
            Console.WriteLine("Now please enter how many premium admissions tickets you would like: ");
            String strPremAdmTix;
            strPremAdmTix = Console.ReadLine();
            // Quick validation on premium admission tickets
            int intPremAdmTix;
            intPremAdmTix = QuickCheckTix(strPremAdmTix, "Premium Admission");
            return intPremAdmTix;
        }

        public static int QuickCheckTix(String strNumTix, String strTixType)
        {

            // Declare the variables
            int intNumTix;
            String strTixOnReInput;

            // Try to convert and ask for number of tickets again if negative
            try
            {
                intNumTix = Convert.ToInt16(strNumTix);
            }
            catch
            {
                // If this code works, it means they didn't put in a number
                Console.WriteLine("Looks like you didn't put in a whole number. Please put in a whole number that is zero or greater: ");
                strTixOnReInput = Console.ReadLine();
                intNumTix = QuickCheckTix(strTixOnReInput, strTixType);

            }

            // Now we need to make sure that the value is not negative.
            if (intNumTix < 0)
            {
                Console.WriteLine("Looks like you are trying to order a negative number of tickets. If you don't want any " + strTixType + " tickets, please type zero.");
                strTixOnReInput = Console.ReadLine();
                intNumTix = QuickCheckTix(strTixOnReInput, strTixType);
            }

            // Assuming it's all good, return the int version of the ticket number
            return intNumTix;
        }

        public static void validateBothTix(int intGenAdmTix, int intPremAdmTix)
        {
            if (intGenAdmTix == 0 && intPremAdmTix == 0)
            {
                Console.WriteLine("Looks like you tried to purchase zero general admission and zero premium admission tickets. Please purchase at least some of one or the other (or some of both)!");
                intGenAdmTix = gatherAndValidateGenAdmTix();
                intPremAdmTix = gatherAndValidatePremAdmTix();
                validateBothTix(intGenAdmTix, intPremAdmTix);
            }
        }

        public static void calculateAndDisplayOutputs(int intGenAdmTix, int intPremAdmTix)
        {
            // Declare constants
            const int GenAdmTixPrice = 50, PremAdmTixPrice = 75;
            const decimal AustinTaxRate = 0.0875m;

            // Start the outputs
            Console.WriteLine("Great! Let's review your order.");
            int intTotalNumTix = intGenAdmTix + intPremAdmTix;
            Console.WriteLine("You've purchased " + intGenAdmTix + " General Admission tickets and " + intPremAdmTix + " Premium Admission tickets, so in total you have " + intTotalNumTix + " tickets.");
            // Display premium total
            int intPremiumTotal = intPremAdmTix * PremAdmTixPrice;
            String strPremiumTotalFormatted = string.Format("{0:0.00}", intPremiumTotal);
            Console.WriteLine("Your " + intPremAdmTix + " Premium Admission tickets at a rate of $" + PremAdmTixPrice + ", so your total comes out to $" + strPremiumTotalFormatted + ".");

            // Display general total
            int intGeneralTotal = intGenAdmTix * GenAdmTixPrice;
            String strGeneralTotalFormatted = string.Format("{0:0.00}", intGeneralTotal);
            Console.WriteLine("Your " + intGenAdmTix + " General Admission tickets at a rate of $" + GenAdmTixPrice + ", so your total comes out to $" + strGeneralTotalFormatted + ".");

            // Display subtotal
            int intSubTotal = intPremiumTotal + intGeneralTotal;
            String strSubTotalFormatted = string.Format("{0:0.00}", intSubTotal);
            Console.WriteLine("Your subtotal comes out to $" + strSubTotalFormatted + ".");

            // Sales tax
            Decimal decTotalTax = intSubTotal * AustinTaxRate;
            Decimal decTotalTaxRounded = Math.Round(decTotalTax, 2);
            Console.WriteLine("Your tax comes out to $" + decTotalTaxRounded + ".");

            // Grand total with sales tax included
            Decimal decGrandTotal = intSubTotal + decTotalTaxRounded;
            Console.WriteLine("Your grand total comes out to $" + decGrandTotal + ".");

            // Premium percentage
            Decimal decPremiumPercentage = (Convert.ToDecimal(intPremAdmTix) / Convert.ToDecimal(intTotalNumTix)) * 100;
            int intPremiumPercentageRounded = Convert.ToInt32(Math.Round(decPremiumPercentage));
            Console.WriteLine("Your premium percentage comes out to " + intPremiumPercentageRounded + "%.");


        }
    }
}

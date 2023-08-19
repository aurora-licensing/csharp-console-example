using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of the Aurora API class
            Aurora aurora = new Aurora("app_name", "app_secret", "app_hash", "app_version", "https://aurora-licensing.pro/api/");

            // Initialize the API by sending an initialization request
            aurora.connectApi();

            // Check if the initialization was successful
            if (!aurora.info.valid)
            {
                // If not successful, display the error message and exit
                Console.WriteLine(aurora.info.response);
                System.Threading.Thread.Sleep(1500); // Wait for 1500 milliseconds before exiting
                return;
            }

            // Prompt the user to enter a license
            Console.Write("Enter license: ");
            string license = Console.ReadLine();

            // Check the validity of the license
            aurora.CheckLicense(license);
            if (!aurora.info.valid)
            {
                // If the license check was unsuccessful, display the error message and exit
                Console.WriteLine(aurora.info.response);
                System.Threading.Thread.Sleep(1500); // Wait for 1500 milliseconds before exiting
                return;
            }

            // Return when the license was used on
            aurora.usedDate(license);
            if (aurora.info.valid)
            {
                // If the request was successful, return the date the license was used
                Console.WriteLine("License used on the: " + aurora.info.response);
            }

            // Check the expiry date of the license
            aurora.CheckLicenseExpiry(license);
            if (aurora.info.valid)
            {
                // If the license expiry check was successful, display the expiry date
                Console.WriteLine("License expires on the: " + aurora.info.response);
            }

            // Check the subscription level of the license
            aurora.CheckLicenseSub(license);
            if (aurora.info.valid)
            {
                // If the subscription check was successful, display the subscription level
                Console.WriteLine("Subscription: " + aurora.info.response);
            }

            // Return the note for current license
            aurora.licenseNote(license);
            if (aurora.info.valid)
            {
                // If the request was successful, the license note
                Console.WriteLine("Note: " + aurora.info.response);
            }

            // Return the hwid for current license
            aurora.getHwid(license);
            if (aurora.info.valid)
            {
                // If the license hwid request was successful, display the information
                Console.WriteLine("HWID: " + aurora.info.response);
            }

            string varMessage = aurora.GetVarMessage("CTwJEeCRpx");
            if (aurora.info.valid)
            {
                // If the variable message retrieval was successful, display the message
                Console.WriteLine(varMessage);
            }

            // Call the downloadFileAsync function of the 'aurora' object to download the file with "qweqwesdf" as the file secret.
            // The downloaded file content will be stored in the 'fileData' byte array.
            byte[] fileData = aurora.DownloadFile("A2jEskjX5y");

            // Check if the file download was successful (if the 'data.success' flag is true).
            if (aurora.info.valid)
            {
                // Use fileData to do something with the downloaded file bytes

                string filePath = "C:\\Users\\Ash\\Desktop\\file.exe"; // Change the file path as desired

                try
                {
                    File.WriteAllBytes(filePath, fileData);
                    Console.WriteLine("File saved successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while saving the file: " + ex.Message);
                }
            }
            else
            {
                // If the file download was not successful, output the error message.
                Console.WriteLine(aurora.info.response);
            }

            // Send a webhook with provided information
            aurora.SendWebhook("123", "https://uixdesign.xyz/data/assets/logo/favicon.png", "123", "123");
            if (aurora.info.valid)
            {
                // If the webhook was successfully sent, display the response message
                Console.WriteLine(aurora.info.response);
            }

            // Ban a users license - example for debugging ect...
            //aurora.banLicnese(license);
        }
    }
}

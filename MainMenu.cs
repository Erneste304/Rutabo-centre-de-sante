using System;
using System.Threading.Tasks;
using HospitalManagementSystem.Core.Services;
using HospitalManagementSystem.Core.Models;

namespace HospitalManagementSystem.ConsoleApp.Dashboard
{
    public class MainMenu
    {
        private readonly PatientService _patientService;

        public MainMenu(PatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task ShowAsync()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== Hospital Management System ===");
                Console.WriteLine("1. List All Patients");
                Console.WriteLine("2. Add New Patient");
                Console.WriteLine("3. Update Patient Details");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        await ListPatientsAsync();
                        break;
                    case "2":
                        await AddPatientAsync();
                        break;
                    case "3":
                        await UpdatePatientAsync();
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task ListPatientsAsync()
        {
            Console.WriteLine("\n--- Patient List ---");
            var patients = await _patientService.GetAllPatientsAsync();
            foreach (var patient in patients)
            {
                // Ensure these properties (Id, FirstName, LastName) exist in your Patient model
                Console.WriteLine($"ID: {patient.Id} - Name: {patient.FirstName} {patient.LastName} - DOB: {patient.DateOfBirth.ToShortDateString()}");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task AddPatientAsync()
        {
            Console.WriteLine("\n--- Add New Patient ---");
            var firstName = ReadNonEmptyInput("Enter First Name: ");
            var lastName = ReadNonEmptyInput("Enter Last Name: ");
            var dateOfBirth = ReadDateOfBirth("Enter Date of Birth (YYYY-MM-DD): ");

            var newPatient = new Patient 
            { 
                FirstName = firstName, 
                LastName = lastName,
                DateOfBirth = dateOfBirth
            };

            await _patientService.AddPatientAsync(newPatient);
            Console.WriteLine("Patient added successfully! Press any key to continue...");
            Console.ReadKey();
        }

        private async Task UpdatePatientAsync()
        {
            Console.WriteLine("\n--- Update Patient Details ---");
            Console.Write("Enter the ID of the patient to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format. Press any key to return to the menu.");
                Console.ReadKey();
                return;
            }

            try
            {
                var patient = await _patientService.GetPatientByIdAsync(id);

                if (patient == null)
                {
                    Console.WriteLine($"Patient with ID {id} not found. Press any key to return to the menu.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine($"\nUpdating details for: {patient.FirstName} {patient.LastName}");

                var newFirstName = ReadLineWithDefault($"Enter new First Name (current: {patient.FirstName}): ", patient.FirstName);
                var newLastName = ReadLineWithDefault($"Enter new Last Name (current: {patient.LastName}): ", patient.LastName);
                var newDateOfBirth = ReadDateOfBirthWithDefault($"Enter new Date of Birth (current: {patient.DateOfBirth.ToShortDateString()}): ", patient.DateOfBirth);

                patient.FirstName = newFirstName;
                patient.LastName = newLastName;
                patient.DateOfBirth = newDateOfBirth;

                await _patientService.UpdatePatientAsync(patient);
                Console.WriteLine("Patient details updated successfully! Press any key to continue...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during update: {ex.Message}");
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Prompts the user for input but returns a default value if the input is empty.
        /// </summary>
        private string ReadLineWithDefault(string prompt, string defaultValue)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? defaultValue : input;
        }

        private string ReadNonEmptyInput(string prompt)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty. Please try again.");
                }
            } while (string.IsNullOrWhiteSpace(input));
            return input;
        }

        private DateTime ReadDateOfBirth(string prompt)
        {
            DateTime date;
            do
            {
                Console.Write(prompt);
                if (!DateTime.TryParse(Console.ReadLine(), out date))
                {
                    Console.WriteLine("Invalid date format. Please use YYYY-MM-DD.");
                }
            } while (date == default);
            return date;
        }

        private DateTime ReadDateOfBirthWithDefault(string prompt, DateTime defaultValue)
        {
            DateTime date;
            do
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    return defaultValue;
                }
                if (DateTime.TryParse(input, out date))
                {
                    return date;
                }
                Console.WriteLine("Invalid date format. Please use YYYY-MM-DD or leave blank to keep the current value.");
            } while (true);
        }
    }
}